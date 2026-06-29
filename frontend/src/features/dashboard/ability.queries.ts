import { api } from "@/lib/api-client";
import { type Ability, type AbilityQuery, type ChangeAbilityRequest } from "@/types/api.types";
import { queryOptions, useMutation, useQuery } from "@tanstack/react-query";

const abilityKeys = {
  all: ["abilities"] as const,
  lists: () => [...abilityKeys.all, "list"] as const,
  list: (filters: AbilityQuery) => [...abilityKeys.lists(), filters] as const,
  details: () => [...abilityKeys.all, "detail"] as const,
  detail: (abilityId: number) => [...abilityKeys.details(), abilityId] as const,
};

export const abilityQueryOptions = (abilityId: number) =>
  queryOptions({
    queryKey: abilityKeys.detail(abilityId),
    queryFn: () => api.get<Ability>(`/abilities/${abilityId.toString()}`),
  });

export const useAbility = (abilityId: number) => useQuery(abilityQueryOptions(abilityId));

export const useUpdateAbility = ({
  onSuccess,
}: {
  onSuccess: (updatedAbility: Ability) => Promise<void>;
}) => {
  return useMutation({
    mutationFn: ({ abilityId, data }: { abilityId: number; data: ChangeAbilityRequest }) =>
      api.put<Ability>(`/abilities/${abilityId.toString()}`, data),
    onSuccess: (updatedAbility) => onSuccess(updatedAbility),
  });
};

export const useDeleteAbility = ({ onSuccess }: { onSuccess: () => Promise<void> }) => {
  return useMutation({
    mutationFn: (abilityId: number) => api.delete(`/abilities/${abilityId.toString()}`),
    onSuccess: onSuccess,
  });
};
