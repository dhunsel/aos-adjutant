// OIDC login/logout are server-driven full-page redirects, not in-app navigations.
export function login() {
  window.location.href = "/api/auth/login";
}

export function logout() {
  window.location.href = "/api/auth/logout";
}
