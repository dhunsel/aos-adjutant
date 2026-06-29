import { createAbilitySchema } from "@/features/dashboard/ability.schemas";
import { AbilityForm, type AbilityFormErrors, type AbilityFormFields } from "./ability-form";
import { ApiError } from "@/lib/api-client";
import { useUpdateAbility } from "../../ability.queries";
import type { Ability } from "@/types/api.types";

export function ChangeAbility({
  ability,
  onUpdate,
  onSuccess,
}: {
  ability: Ability;
  onUpdate: (updatedAbility: Ability) => Promise<void>;
  onSuccess?: () => void;
}) {
  const changeAbility = useUpdateAbility({
    onSuccess: (updatedAbility) => onUpdate(updatedAbility),
  });

  const onSubmitAsync = async ({
    value,
  }: {
    value: AbilityFormFields;
  }): Promise<AbilityFormErrors | null> => {
    try {
      const parsed = createAbilitySchema.parse(value);
      await changeAbility.mutateAsync({
        abilityId: ability.abilityId,
        data: { ...parsed, version: ability.version },
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
    <AbilityForm
      defaultValues={{
        name: ability.name,
        reaction: ability.reaction ?? "",
        declaration: ability.declaration ?? "",
        effect: ability.effect,
        phase: ability.phase,
        restriction: ability.restriction ?? "",
        turn: ability.turn ?? "",
      }}
      submitLabel="Save"
      onSubmitAsync={onSubmitAsync}
      {...(onSuccess ? { onSuccess: onSuccess } : {})}
    />
  );
}
