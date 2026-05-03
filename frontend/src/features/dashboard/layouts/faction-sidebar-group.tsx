import {
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar";
import { Flag } from "lucide-react";
import { useParams } from "react-router";
import { useFaction } from "../factions/faction.queries";

export function FactionSidebarGroup() {
  const params = useParams();
  const factionId = params["factionId"];
  const faction = useFaction(Number(factionId));

  return factionId ? (
    <SidebarGroup>
      <SidebarGroupLabel className="font-heading">{faction.data?.name}</SidebarGroupLabel>
      <SidebarMenu>
        <SidebarMenuItem>
          <SidebarMenuButton>
            <Flag />
            Battle Traits
          </SidebarMenuButton>
        </SidebarMenuItem>
        <SidebarMenuItem>
          <SidebarMenuButton>
            <Flag />
            Battle Formations
          </SidebarMenuButton>
        </SidebarMenuItem>
        <SidebarMenuItem>
          <SidebarMenuButton>
            <Flag />
            Units
          </SidebarMenuButton>
        </SidebarMenuItem>
        <SidebarMenuItem>
          <SidebarMenuButton>
            <Flag />
            Heroic Traits
          </SidebarMenuButton>
        </SidebarMenuItem>
        <SidebarMenuItem>
          <SidebarMenuButton>
            <Flag />
            Artefacts of Power
          </SidebarMenuButton>
        </SidebarMenuItem>
        <SidebarMenuItem>
          <SidebarMenuButton>
            <Flag />
            Spell Lores
          </SidebarMenuButton>
        </SidebarMenuItem>
        <SidebarMenuItem>
          <SidebarMenuButton>
            <Flag />
            Prayer Lores
          </SidebarMenuButton>
        </SidebarMenuItem>
        <SidebarMenuItem>
          <SidebarMenuButton>
            <Flag />
            Manifestation Lores
          </SidebarMenuButton>
        </SidebarMenuItem>
      </SidebarMenu>
    </SidebarGroup>
  ) : null;
}
