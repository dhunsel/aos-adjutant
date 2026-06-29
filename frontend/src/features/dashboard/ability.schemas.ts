import type { Phase, Restriction, Turn } from "@/types/api.types";
import { z } from "zod";

const phases = [
  "deployment",
  "start",
  "hero",
  "movement",
  "shooting",
  "charge",
  "combat",
  "end",
  "passive",
] as const; //satisfies Phase[];

const restrictions = [
  "onceTurnArmy",
  "onceRoundArmy",
  "onceBattleArmy",
  "onceRound",
  "onceBattle",
] as const; //satisfies Restriction[];

const turns = ["yourTurn", "enemyTurn", "anyTurn"] as const; //satisfies Turn[];

export const phaseSchema = z.enum(phases, "Invalid Phase");
export const restrictionSchema = z.enum(restrictions, "Invalid Restriction");
export const turnSchema = z.enum(turns, "Invalid Turn");

export const createAbilitySchema = z.object({
  name: z.string().min(1, "Name is required").max(100, "Name too long"),
  reaction: z
    .string()
    .trim()
    .max(100, "Reaction field too long")
    .transform((r) => (r === "" ? null : r)),
  declaration: z
    .string()
    .trim()
    .max(100, "Declaration field too long")
    .transform((d) => (d === "" ? null : d)),
  effect: z.string().min(1, "Effect is required").max(100, "Effect field too long"),
  phase: phaseSchema,
  restriction: restrictionSchema,
  turn: turnSchema,
});
