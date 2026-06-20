import { useParams } from "react-router";
import { useBattleTraits } from "../battle-traits.queries";
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

export function BattleTraitListPage() {
  const params = useParams();
  const battleTraits = useBattleTraits(Number(params["factionId"]));
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const isAdmin = useIsAdmin();

  return (
    <div className="flex flex-col gap-2">
      <div className="flex justify-between">
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
        <div className="grid grid-cols-[repeat(auto-fill,minmax(250px,1fr))] gap-3">
          {battleTraits.data?.items.map((bt) => (
            <Button
              className="flex h-auto flex-col gap-2 p-2 text-nowrap"
              key={bt.abilityId}
              variant="secondary"
              nativeButton={false}
            >
              <span className="font-heading text-xl">{bt.name}</span>
            </Button>
          ))}
        </div>
      )}
    </div>
  );
}
