import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarRail,
} from "@/components/ui/sidebar";
import { Flag, List, Megaphone } from "lucide-react";
import type { CSSProperties } from "react";
import { FactionSidebarGroup } from "./faction-sidebar-group";

export function DashboardSidebar() {
  return (
    <Sidebar
      collapsible="none"
      className="hidden md:flex"
      style={{ "--sidebar-width": "12rem" } as CSSProperties}
    >
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel className="font-heading">General</SidebarGroupLabel>
          <SidebarMenu>
            <SidebarMenuItem>
              <SidebarMenuButton>
                <Flag />
                Factions
              </SidebarMenuButton>
            </SidebarMenuItem>
            <SidebarMenuItem>
              <SidebarMenuButton>
                <Megaphone />
                Commands
              </SidebarMenuButton>
            </SidebarMenuItem>
            <SidebarMenuItem>
              <SidebarMenuButton>
                <List />
                Keywords
              </SidebarMenuButton>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarGroup>
        <FactionSidebarGroup />
      </SidebarContent>
      <SidebarRail />
    </Sidebar>
  );
}
