import { Badge } from "@/components/ui/badge";
import { cn } from "@/lib/utils";
import type { Phase, Turn } from "@/types/api.types";
import type { ComponentProps } from "react";

export function PhaseBadge({
  phase,
  turn,
  className,
  ...props
}: { phase: Phase; turn: Turn } & ComponentProps<typeof Badge>) {
  let phaseLabel;
  let turnLabel;
  let color;

  // Temporary hardcoded mapping

  switch (phase) {
    case "deployment": {
      color = "bg-phase-deployment";
      phaseLabel = "Deployment Phase";
      break;
    }
    case "start": {
      color = "bg-phase-start";
      phaseLabel = "Start Phase";
      break;
    }
    case "hero": {
      color = "bg-phase-hero";
      phaseLabel = "Hero Phase";
      break;
    }
    case "movement": {
      color = "bg-phase-movement";
      phaseLabel = "Movement Phase";
      break;
    }
    case "shooting": {
      color = "bg-phase-shooting";
      phaseLabel = "Shooting Phase";
      break;
    }
    case "charge": {
      color = "bg-phase-charge";
      phaseLabel = "Charge Phase";
      break;
    }
    case "combat": {
      color = "bg-phase-combat";
      phaseLabel = "Combat Phase";
      break;
    }
    case "end": {
      color = "bg-phase-end";
      phaseLabel = "End Phase";
      break;
    }
    case "passive": {
      color = "bg-phase-passive";
      phaseLabel = "Passive";
      break;
    }
  }

  switch (turn) {
    case "yourTurn": {
      turnLabel = "Your ";
      break;
    }
    case "enemyTurn": {
      turnLabel = "Enemy ";
      break;
    }
    case "anyTurn": {
      turnLabel = "Any ";
      break;
    }
  }

  return (
    <Badge className={cn(color, className)} {...props}>
      {(turnLabel ?? "") + phaseLabel}
    </Badge>
  );
}
