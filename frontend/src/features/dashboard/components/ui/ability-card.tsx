import { Separator } from "@/components/ui/separator";
import type { Ability } from "@/types/api.types";
import { PhaseBadge } from "./phase-badge";

const restrictions = [
  { label: "Once per turn (army)", value: "onceTurnArmy" },
  { label: "Once per battle round (army)", value: "onceRoundArmy" },
  { label: "Once per battle (army)", value: "onceBattleArmy" },
  { label: "Once per battle round", value: "onceRound" },
  { label: "Once per battle", value: "onceBattle" },
];

export function AbilityCard({ ability }: { ability: Ability }) {
  const restrictionLabel = restrictions.find((r) => r.value === ability.restriction)?.label;

  return (
    <div className="flex flex-col gap-2 rounded-lg border border-border bg-card p-2 text-card-foreground">
      <div className="flex items-center justify-between">
        <span className="font-heading text-lg">{ability.name}</span>
        <div className="flex flex-col items-end gap-2 text-sm">
          <PhaseBadge phase={ability.phase} turn={ability.turn} className="text-card-foreground" />
          <span>{restrictionLabel}</span>
        </div>
      </div>
      <Separator />
      <div className="flex flex-col gap-2">
        {ability.reaction && (
          <div>
            <span className="font-bold text-primary/80">Reaction: </span>
            <span className="">{ability.reaction}</span>
          </div>
        )}
        <div>
          <span className="font-bold text-primary/80">Declare: </span>
          <span>{ability.declaration}</span>
        </div>
        <div>
          <span className="font-bold text-primary/80">Effect: </span>
          <span>{ability.effect}</span>
        </div>
      </div>
    </div>
  );
}
