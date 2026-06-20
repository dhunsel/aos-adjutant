import { createAbilitySchema } from "@/features/dashboard/ability.schemas";
import {
  AbilityForm,
  type AbilityFormErrors,
  type AbilityFormFields,
} from "../../../components/ui/ability-form";
import { ApiError } from "@/lib/api-client";
import { useCreateBattleTrait } from "../battle-traits.queries";
import { useParams } from "react-router";

export function CreateBattleTrait({ onSuccess }: { onSuccess?: () => void }) {
  const params = useParams();
  const createBattleTrait = useCreateBattleTrait(Number(params["factionId"]));

  const onSubmitAsync = async ({
    value,
  }: {
    value: AbilityFormFields;
  }): Promise<AbilityFormErrors | null> => {
    try {
      const parsed = createAbilitySchema.parse(value);
      await createBattleTrait.mutateAsync(parsed);
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
    <AbilityForm
      defaultValues={{
        name: "",
        reaction: "",
        declaration: "",
        effect: "",
        phase: "",
        restriction: "",
        turn: "",
      }}
      submitLabel="Create"
      onSubmitAsync={onSubmitAsync}
      {...(onSuccess ? { onSuccess: onSuccess } : {})}
    />
  );
}
