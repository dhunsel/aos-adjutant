import { useNavigate, useParams } from "react-router";
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
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";

export function FactionDetailPage() {
  const params = useParams();
  const faction = useFaction(Number(params["factionId"]));
  const deleteFaction = useDeleteFaction();
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const isAdmin = useIsAdmin();
  const navigate = useNavigate();

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
      {isAdmin && (
        <div className="flex gap-2">
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
                <DialogTitle>Edit Faction</DialogTitle>
              </DialogHeader>
              <ChangeFaction
                onSuccess={() => {
                  setIsEditDialogOpen(false);
                }}
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
                <AlertDialogTitle>Are you sure you want to delete this faction?</AlertDialogTitle>
                <AlertDialogDescription>
                  This action will permanently delete this faction and all the related data.
                </AlertDialogDescription>
              </AlertDialogHeader>
              <AlertDialogFooter>
                <AlertDialogCancel>Cancel</AlertDialogCancel>
                <AlertDialogAction
                  onClick={() => {
                    deleteFaction.mutate(faction.data.factionId, {
                      onSuccess: () => {
                        void navigate("/dashboard/factions");
                      },
                    });
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
