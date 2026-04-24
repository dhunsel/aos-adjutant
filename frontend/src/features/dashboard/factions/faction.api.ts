import { api } from "@/lib/api";
import type { Faction } from "@/types/api";
import { queryOptions, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { z } from "zod";

// Create faction
export const createFactionInputSchema = z.object({
  name: z.string().min(1).max(100),
});

export type CreateFactionInput = z.infer<typeof createFactionInputSchema>;

export const createFaction = (data: CreateFactionInput): Promise<Faction> =>
  api.post("/factions", data);

export const useCreateFaction = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateFactionInput) => createFaction(data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: factionsQueryOptions().queryKey }),
  });
};

// Get faction list
export const getFactions = (): Promise<Faction[]> => api.get("/factions");

export const factionsQueryOptions = () =>
  queryOptions({ queryKey: ["factions"], queryFn: getFactions });

export const useFactions = () => useQuery(factionsQueryOptions());

// Get specific faction
export const getFaction = (factionId: number): Promise<Faction> =>
  api.get(`/factions/${factionId.toString()}`);

export const factionQueryOptions = (factionId: number) =>
  queryOptions({ queryKey: ["factions", factionId], queryFn: () => getFaction(factionId) });

export const useFaction = (factionId: number) => useQuery(factionQueryOptions(factionId));

// Update a faction
export const updateFactionInputSchema = z.object({
  name: z.string().min(1).max(100),
  version: z.number(),
});

export type UpdateFactionInput = z.infer<typeof updateFactionInputSchema>;

export const updateFaction = (factionId: number, data: UpdateFactionInput): Promise<Faction> =>
  api.put(`/factions/${factionId.toString()}`, data);

export const useUpdateFaction = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ factionId, data }: { factionId: number; data: UpdateFactionInput }) =>
      updateFaction(factionId, data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: factionsQueryOptions().queryKey }),
  });
};

// Delete a faction
export const deleteFaction = (factionId: number): Promise<void> =>
  api.delete(`/factions/${factionId.toString()}`);

export const useDeleteFaction = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (factionId: number) => deleteFaction(factionId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: factionsQueryOptions().queryKey }),
  });
};
