import { useParams } from "react-router";
import { useDeleteFaction, useFaction } from "../faction.queries";
import { NotFound } from "@/pages/not-found";
import { GrandAllianceBadge } from "../components/grand-alliance-badge";
import { Button } from "@/components/ui/button";
import { SquarePen, Trash2 } from "lucide-react";
import { useState } from "react";
import { useIsAdmin } from "@/features/auth/auth.queries";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { ChangeFaction } from "../components/change-faction";
import { Spinner } from "@/components/ui/spinner";

export function FactionDetailPage() {
  const params = useParams();
  const faction = useFaction(Number(params["factionId"]));
  const deleteFaction = useDeleteFaction();
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const isAdmin = useIsAdmin();

  if (faction.isLoading)
    return (
      <div className="pt-10">
        <Spinner className="mx-auto" />
      </div>
    );

  if (faction.isError || !faction.data) return NotFound();

  return (
    <div className="flex items-center justify-between gap-3">
      <div className="flex items-center gap-3">
        <h1 className="font-heading text-2xl">{faction.data.name}</h1>
        <GrandAllianceBadge grandAlliance={faction.data.grandAlliance} />
      </div>
      <div className="flex gap-2">
        {isAdmin && (
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
                <DialogTitle>Add Faction</DialogTitle>
              </DialogHeader>
              <ChangeFaction
                onSuccess={() => {
                  setIsEditDialogOpen(false);
                }}
              />
            </DialogContent>
          </Dialog>
        )}
        <Button
          variant="destructive"
          onClick={() => {
            deleteFaction.mutate(faction.data.factionId);
          }}
        >
          <Trash2 />
          <span>Delete</span>
        </Button>
      </div>
    </div>
  );
}
