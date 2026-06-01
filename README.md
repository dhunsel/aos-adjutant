# AoS Adjutant

[![backend](https://github.com/dhunsel/aos-adjutant/actions/workflows/backend.yml/badge.svg)](https://github.com/dhunsel/aos-adjutant/actions/workflows/backend.yml)
[![frontend](https://github.com/dhunsel/aos-adjutant/actions/workflows/frontend.yml/badge.svg)](https://github.com/dhunsel/aos-adjutant/actions/workflows/frontend.yml)
[![codecov](https://codecov.io/github/dhunsel/aos-adjutant/graph/badge.svg?token=Y9Y5NO8O1Z)](https://codecov.io/github/dhunsel/aos-adjutant)

A companion web app for **Warhammer: Age of Sigmar (4th edition)**. The goal is a fully-featured application
to manage game data, build army lists, and provide assistance during a game. Built as a full-stack project with a
.NET 10 API, a React 19 SPA, and a self-hosted, observable, single-VPS deployment.

> **Status: active development.** The data-management foundation and the
> production platform (auth, observability, deployment) are in place. The
> player-facing features (list builder, play mode) are in progress.
> [Roadmap](#roadmap).

![Dashboard Faction List](docs/screenshot.png)

## Features

| Area | Description | Status |
|------|-------------|--------|
| **Admin dashboard** | Manage factions, units, attack profiles, weapon effects, abilities, and battle formations | 🟡 In progress |
| **List builder** | Create, save, and validate army lists | ⚪ Planned |
| **Play mode** | Run a game session from a saved list, with data reshaped for fast in-game reference | ⚪ Planned |

## Tech stack

**Backend** — .NET 10 / ASP.NET Core · Entity Framework Core 10 · PostgreSQL 18 ·
OpenTelemetry · OIDC Authentication

**Frontend** — React 19 · TypeScript · Vite · TanStack Query · React Router 7 ·
Tailwind CSS 4 · shadcn/ui (on Base UI) · Zod · types generated from the API's
OpenAPI schema

**Infrastructure** — Docker Compose · Caddy (automatic TLS via Cloudflare
DNS-01) · Pocket ID (OIDC SSO) · Grafana + Loki + Tempo + Prometheus · GitHub
Actions

## Architecture

```
                          ┌───────────────────────────────────────────┐
   Browser ──HTTPS──────▶ │      Caddy  (reverse proxy + auto-TLS)    │
                          └────┬───────────────┬───────────────┬──────┘
                               │               │               │
                    ┌──────────▼───┐     ┌─────▼──────┐   ┌────▼──────┐
                    │   Web app    │◀───▶│  Pocket ID │◀─▶│  Grafana  │
                    │ (API + SPA)  │     │ OIDC / SSO │   │           │
                    └─┬─────────┬──┘     └────────────┘   └─────┬─────┘
                      │         │ EF Core                       │
        OTLP telemetry│         ▼                               │ queries
       (traces /      │   ┌─────────────┐                       │
        metrics /     │   │ PostgreSQL  │                       │
        logs)         │   │ (least-priv)│                       │
                      │   └─────────────┘                       │
                      ▼                                         │
             ┌──────────────────┐                               │
             │  OTel Collector  │                               │
             └────────┬─────────┘                               │
                      ▼                                         │
            ┌──────────────────────────────┐                    │
            │   Tempo · Prometheus · Loki  │◀───────────────────┘
            └──────────────────────────────┘
```

- **Backend For Frontend (BFF)**: The backend is currently designed only for the browser frontend SPA.
  In production the SPA static files are served though the ASP.NET application.
  For authentication the OIDC authorization code flow is handled by the backend to avoid the issue of
  storing tokens in the browser. After successful authentication, an encrypted session cookie is set in the
  browser which is validated by the backend. A future change might be to extract the resouce API out of the
  backend and keep the current backend as a thin proxy forwarding requests to that API.
- **Feature-based structure**: In both backend and frontend, the code is grouped by domain feature
  rather than by technical layer. The structure is kept relatively flat and will be
  refactored as real needs appear.
- **Typed end to end**: The frontend's API types are generated from the
  backend's OpenAPI document, so contract drift surfaces at build time.

## Technical Features

- **Observability**: Every request is traced (ASP.NET Core, EF Core,
  HttpClient), with metrics and structured logs exported over OTLP to a
  Grafana stack. Traces, metrics, and logs are correlated via exemplars and a
  per-request trace ID.
- **Authentication**: OpenID Connect with PKCE, `__Host-`-prefixed
  session cookies, `SameSite=Strict`, HttpOnly, and an admin-gated fallback
  authorization policy.
- **Least-privilege database**: Separate PostgreSQL roles for the superuser,
  schema migrations, and the application runtime. The app never connects with
  rights it doesn't need.
- **Versioned SQL migrations**: Applied by a dedicated, single-shot migrator
  container. Schema changes are reproducible and decoupled from the running app.
- **Production deployment**: single-VPS Docker Compose with network
  segmentation (an internal-only network for data services, a proxy network for
  edge), automatic TLS, and SSO shared across the app and Grafana.
- **Quality gates**: Unit tests and Testcontainers-backed integration tests, static
  analysis (Meziantou, SonarAnalyzer), code coverage, and CI on every push/PR.

## Getting started (local development)

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS)
- [Docker](https://www.docker.com/) — for PostgreSQL and the observability stack

### Run it
Step 1-4 are only necessary for first-time setup.

```bash
# 1. Add local .env used by dev docker compose
cp .env.example .env

# 2. Initialize Pocket ID
docker compose run --rm pocket-id-init  # Use the generated link to add a passkey for auth

# 3. Open created file at .secrets/pocket-id.env and set app vars via dotnet user-secrets, copy Grafana vars into .env
dotnet user-secrets set "Authentication:ClientId" "<WEBAPP_CLIENT_ID>" --project backend/src/AosAdjutant.Api
dotnet user-secrets set "Authentication:ClientSecret" "<WEBAPP_CLIENT_SECRET>" --project backend/src/AosAdjutant.Api

# 4. Run DB migrations
docker compose run --rm migrator

# 5. Deploy services
docker compose up -d

# 6. Backend
cd backend
dotnet run --project src/AosAdjutant.Api
# API → http://localhost:5280   ·   API reference (Scalar) → http://localhost:5280/scalar

# 7. Frontend (second terminal)
cd frontend
npm install
npm run dev                   # → http://localhost:5173, proxied to the API
```

## Deployment

Production runs on a single VPS via [`docker-compose.prod.yml`](docker-compose.prod.yml):
Caddy terminates TLS (Cloudflare DNS-01) and reverse-proxies the app, Pocket ID,
and Grafana on separate subdomains; data services sit on an internal-only
network; schema is applied by the one-shot `migrator` profile. First-run auth
client provisioning is bootstrapped by the `init` profile (see
[`infra/pocket-id`](infra/pocket-id)).

## Roadmap

- [ ] Complete dashboard CRUD UI for all game-data entities + Finish data API
- [ ] List builder with army-composition validation
- [ ] Play mode (game-optimised data view + session state)

## License

[MIT](LICENSE)
