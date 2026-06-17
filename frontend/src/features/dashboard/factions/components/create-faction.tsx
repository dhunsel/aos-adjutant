import { createFactionSchema } from "../faction.schemas";
import { useCreateFaction } from "../faction.queries";
import { ApiError } from "@/lib/api-client";
import { FactionForm, type FactionFormErrors } from "./faction-form";

export function CreateFaction({ onSuccess }: { onSuccess?: () => void }) {
  const createFaction = useCreateFaction();

  const onSubmitAsync = async ({
    value,
  }: {
    value: { name: string; grandAlliance: string };
  }): Promise<FactionFormErrors | null> => {
    try {
      const parsed = createFactionSchema.parse(value);
      await createFaction.mutateAsync(parsed);
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
      name=""
      grandAlliance=""
      submitLabel="Create"
      onSubmitAsync={onSubmitAsync}
      {...(onSuccess ? { onSuccess: onSuccess } : {})}
    />
  );
}
