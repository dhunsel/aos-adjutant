import { createBrowserRouter } from "react-router";
import App from "./App";
import { AppLayout } from "./components/layouts/app-layout";
import { Home } from "./pages/home";
import { NotFound } from "./pages/not-found";
import { UnexpectedError } from "./pages/unexpected-error";
import { DashBoardLayout } from "./features/dashboard/layouts/dashboard-layout";

export const router = createBrowserRouter([
  {
    path: "/",
    ErrorBoundary: UnexpectedError,
    Component: AppLayout,
    children: [
      { index: true, Component: Home },
      {
        path: "dashboard",
        Component: DashBoardLayout,
        children: [
          { path: "factions", Component: App, children: [{ path: ":factionId", Component: App }] },
        ],
      },
    ],
  },
  {
    path: "*",
    Component: NotFound,
  },
]);
