export interface Faction {
  factionId: number;
  name: string;
  version: number;
}

// Currently using Zod schemas in the feature api and inferring types from the schema
//export interface CreateFactionInput {
//  name: string;
//}
//
//export interface UpdateFactionInput {
//  name: string;
//  version: number;
//}
