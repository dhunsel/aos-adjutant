import type { FactionQuery } from "@/types/api.types";
import z from "zod";

// CURRENTLY NOT USED

const grandAlliances = ["order", "chaos", "death", "destruction"] as const;

const sortByValues = ["name", "grandAlliance"] as const;

const sortDirections = ["asc", "desc"] as const;

export const grandAllianceSchema = z.enum(grandAlliances, "Invalid Grand Alliance");

export const factionListParamsSchema = z
  .object({
    grandAlliance: grandAllianceSchema.optional().catch(undefined),
    page: z.coerce.number().int().min(1).optional().catch(undefined),
    pageSize: z.coerce.number().int().min(1).max(100).optional().catch(undefined),
    sortBy: z.enum(sortByValues).optional().catch(undefined),
    sortDirection: z.enum(sortDirections).optional().catch(undefined),
  })
  .transform((s) => {
    const out: FactionQuery = {};
    if (s.grandAlliance) out.grandAlliance = s.grandAlliance;
    if (s.page) out.page = s.page;
    if (s.pageSize) out.pageSize = s.pageSize;
    if (s.sortBy) out.sortBy = s.sortBy;
    if (s.sortDirection) out.sortDirection = s.sortDirection;
    return out;
  });

export const createFactionSchema = z.object({
  name: z.string().min(1, "Name is required").max(50, "Name too long"),
  grandAlliance: grandAllianceSchema,
});
