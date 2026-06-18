import { ApiError } from "@/lib/api-client";
import { useFaction, useUpdateFaction } from "../faction.queries";
import { createFactionSchema } from "../faction.schemas";
import { FactionForm, type FactionFormErrors } from "./faction-form";
import { useParams } from "react-router";

export function ChangeFaction({ onSuccess }: { onSuccess?: () => void }) {
  const changeFaction = useUpdateFaction();
  const params = useParams();
  const faction = useFaction(Number(params["factionId"]));

  if (faction.isError || !faction.data) return <div>Empty</div>;

  const onSubmitAsync = async ({
    value,
  }: {
    value: { name: string; grandAlliance: string };
  }): Promise<FactionFormErrors | null> => {
    try {
      const parsed = createFactionSchema.parse(value);
      await changeFaction.mutateAsync({
        factionId: faction.data.factionId,
        data: { ...parsed, version: faction.data.version },
      });
      return null;
    } catch (err) {
      if (err instanceof ApiError && err.status === 409) {
        return {
          fields: {
            name: { message: "Name already used by another faction" },
          },
        };
      }
      if (err instanceof ApiError && err.status === 403) {
        return { form: { message: "You don't have permission to do this." } };
      }
      return { form: { message: "Unexpected error" } };
    }
  };

  return (
    <FactionForm
      name={faction.data.name}
      grandAlliance={faction.data.grandAlliance}
      submitLabel="Save"
      onSubmitAsync={onSubmitAsync}
      {...(onSuccess ? { onSuccess: onSuccess } : {})}
    />
  );
}
