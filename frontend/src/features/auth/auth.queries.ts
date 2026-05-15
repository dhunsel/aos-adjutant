import { api } from "@/lib/api-client";
import type { User } from "@/types/api.types";
import { queryOptions, useQuery } from "@tanstack/react-query";

const authKeys = {
  all: ["auth"] as const,
  me: () => [...authKeys.all, "me"] as const,
};

export const currentUserQueryOptions = () =>
  queryOptions({
    queryKey: authKeys.me(),
    queryFn: () => api.get<User>("/auth/me"),
  });

export const useCurrentUser = () => useQuery(currentUserQueryOptions());

export const useIsAdmin = (): boolean =>
  useQuery({ ...currentUserQueryOptions(), select: (user) => user.isAdmin }).data ?? false;
