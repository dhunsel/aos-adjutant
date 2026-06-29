import { DashBoardLayout } from "./components/layouts/dashboard-layout";
import { FactionSidebarGroup } from "./components/layouts/faction-sidebar-group";
import type { DasboardRouteHandle } from "./route-handle";
import type { RouteObject } from "react-router";
import { FactionListPage } from "./features/factions/pages/faction-list-page";
import { FactionDetailPage } from "./features/factions/pages/faction-detail-page";
import { BattleTraitListPage } from "./features/battle-traits/pages/battle-trait-list-page";

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
          children: [
            {
              index: true,
              Component: FactionDetailPage,
            },
            {
              path: "battle-traits",
              Component: BattleTraitListPage,
            },
          ],
          handle: { sidebar: FactionSidebarGroup } satisfies DasboardRouteHandle,
        },
      ],
    },
  ],
};
