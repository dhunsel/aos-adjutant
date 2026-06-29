import { api } from "@/lib/api-client";
import {
  type PaginatedResponse,
  type Ability,
  type AbilityQuery,
  type CreateAbilityRequest,
} from "@/types/api.types";
import { queryOptions, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export const battleTraitsKeys = {
  all: (factionId: number) => ["battleTraits", factionId] as const,
  lists: (factionId: number) => [...battleTraitsKeys.all(factionId), "list"] as const,
  list: (factionId: number, filters: AbilityQuery) =>
    [...battleTraitsKeys.lists(factionId), filters] as const,
  details: (factionId: number) => [...battleTraitsKeys.all(factionId), "detail"] as const,
  detail: (factionId: number, abilityId: number) =>
    [...battleTraitsKeys.details(factionId), abilityId] as const,
};

export const battleTraitsQueryOptions = (factionId: number, params?: AbilityQuery) =>
  queryOptions({
    queryKey: battleTraitsKeys.list(factionId, params ?? {}),
    queryFn: () =>
      api.get<PaginatedResponse<Ability>>(`/factions/${factionId.toString()}/abilities`),
  });

export const useBattleTraits = (factionId: number, params?: AbilityQuery) =>
  useQuery(battleTraitsQueryOptions(factionId, params));

export const useCreateBattleTrait = (factionId: number) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateAbilityRequest) =>
      api.post<Ability>(`/factions/${factionId.toString()}/abilities`, data),
    onSuccess: (createdAbility) => {
      queryClient.setQueryData(
        battleTraitsKeys.detail(factionId, createdAbility.abilityId),
        createdAbility,
      );
      return queryClient.invalidateQueries({ queryKey: battleTraitsKeys.lists(factionId) });
    },
  });
};
