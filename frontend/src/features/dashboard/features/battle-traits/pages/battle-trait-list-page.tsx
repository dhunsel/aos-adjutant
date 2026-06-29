import { useParams } from "react-router";
import { battleTraitsKeys, useBattleTraits } from "../battle-traits.queries";
import { Spinner } from "@/components/ui/spinner";
import { useState } from "react";
import { useIsAdmin } from "@/features/auth/auth.queries";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { CreateBattleTrait } from "../components/create-battle-trait";
import { AbilityCard } from "@/features/dashboard/components/ui/ability-card";
import { useQueryClient } from "@tanstack/react-query";

export function BattleTraitListPage() {
  const params = useParams();
  const factionId = Number(params["factionId"]);

  const battleTraits = useBattleTraits(factionId);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const isAdmin = useIsAdmin();
  const queryClient = useQueryClient();

  return (
    <div className="flex flex-col gap-2">
      <div className="flex justify-between">
        <h1 className="font-heading text-2xl">Battle Traits</h1>
        {isAdmin && (
          <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
            <DialogTrigger
              render={
                <Button variant="outline">
                  <Plus />
                  <span className="hidden md:inline">Add Battle Trait</span>
                </Button>
              }
            />
            <DialogContent>
              <DialogHeader>
                <DialogTitle>Add Battle Trait</DialogTitle>
              </DialogHeader>
              <CreateBattleTrait
                onSuccess={() => {
                  setIsCreateDialogOpen(false);
                }}
              />
            </DialogContent>
          </Dialog>
        )}
      </div>
      {battleTraits.isLoading ? (
        <div className="pt-10">
          <Spinner className="mx-auto" />
        </div>
      ) : (
        <div className="grid grid-cols-[repeat(auto-fill,minmax(350px,1fr))] gap-3">
          {battleTraits.data?.items.map((bt) => (
            <AbilityCard
              ability={bt}
              key={bt.abilityId}
              onDelete={() =>
                queryClient.invalidateQueries({
                  queryKey: battleTraitsKeys.lists(factionId),
                })
              }
              onUpdate={(updatedAbility) => {
                queryClient.setQueryData(
                  battleTraitsKeys.detail(factionId, bt.abilityId),
                  updatedAbility,
                );
                return queryClient.invalidateQueries({
                  queryKey: battleTraitsKeys.lists(factionId),
                });
              }}
            />
          ))}
        </div>
      )}
    </div>
  );
}
