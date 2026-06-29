import { Separator } from "@/components/ui/separator";
import type { Ability } from "@/types/api.types";
import { PhaseBadge } from "./phase-badge";
import { useIsAdmin } from "@/features/auth/auth.queries";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { SquarePen, Trash2 } from "lucide-react";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { useDeleteAbility } from "../../ability.queries";
import { useState } from "react";
import { ChangeAbility } from "./change-ability";

const restrictions = [
  { label: "Once per turn (army)", value: "onceTurnArmy" },
  { label: "Once per battle round (army)", value: "onceRoundArmy" },
  { label: "Once per battle (army)", value: "onceBattleArmy" },
  { label: "Once per battle round", value: "onceRound" },
  { label: "Once per battle", value: "onceBattle" },
];

export function AbilityCard({
  ability,
  onDelete,
  onUpdate,
  label = "ability",
}: {
  ability: Ability;
  onDelete: () => Promise<void>;
  onUpdate: (updatedAbility: Ability) => Promise<void>;
  label?: string;
}) {
  const restrictionLabel = restrictions.find((r) => r.value === ability.restriction)?.label;
  const deleteAbility = useDeleteAbility({ onSuccess: onDelete });
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const isAdmin = useIsAdmin();

  return (
    <div className="flex flex-col justify-between gap-2 rounded-lg border border-border bg-card p-2 text-card-foreground">
      <div className="flex flex-col gap-2">
        <div className="flex items-center justify-between">
          <span className="font-heading text-lg">{ability.name}</span>
          <div className="flex flex-col items-end gap-2 text-sm">
            <PhaseBadge
              phase={ability.phase}
              turn={ability.turn}
              className="text-card-foreground"
            />
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
      {isAdmin && (
        <div className="flex gap-2 self-end">
          <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
            <DialogTrigger
              render={
                <Button variant="outline">
                  <SquarePen />
                  <span className="hidden md:inline">Edit</span>
                </Button>
              }
            />
            <DialogContent>
              <DialogHeader>
                <DialogTitle>Edit {label}</DialogTitle>
              </DialogHeader>
              <ChangeAbility
                onSuccess={() => {
                  setIsEditDialogOpen(false);
                }}
                onUpdate={onUpdate}
                ability={ability}
              />
            </DialogContent>
          </Dialog>
          <AlertDialog>
            <AlertDialogTrigger
              render={
                <Button variant="destructive">
                  <Trash2 />
                  <span className="hidden md:inline">Delete</span>
                </Button>
              }
            />
            <AlertDialogContent>
              <AlertDialogHeader>
                <AlertDialogTitle>Are you sure you want to delete this {label}?</AlertDialogTitle>
              </AlertDialogHeader>
              <AlertDialogFooter>
                <AlertDialogCancel>Cancel</AlertDialogCancel>
                <AlertDialogAction
                  onClick={() => {
                    deleteAbility.mutate(ability.abilityId, {});
                  }}
                >
                  Confirm
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        </div>
      )}
    </div>
  );
}
