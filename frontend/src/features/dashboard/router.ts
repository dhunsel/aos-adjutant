import { DashBoardLayout } from "./layouts/dashboard-layout";
import { FactionSidebarGroup } from "./layouts/faction-sidebar-group";
import type { DasboardRouteHandle } from "./route-handle";
import type { RouteObject } from "react-router";
import { FactionListPage } from "./factions/pages/faction-list-page";
import { FactionDetailPage } from "./factions/pages/faction-detail-page";

export const dashboardRouter: RouteObject = {
  path: "/dashboard",
  Component: DashBoardLayout,
  children: [
    {
      path: "factions",
      children: [
        {
          index: true,
          Component: FactionListPage,
        },
        {
          path: ":factionId",
          Component: FactionDetailPage,
          handle: { sidebar: FactionSidebarGroup } satisfies DasboardRouteHandle,
        },
      ],
    },
  ],
};
