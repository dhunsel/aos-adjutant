import { useEffect } from "react";
import { Outlet } from "react-router";
import { Spinner } from "@/components/ui/spinner";
import { ApiError } from "@/lib/api-client";
import { useCurrentUser } from "./auth.queries";
import { login } from "./auth.actions";

export function RequireAuth() {
  const { isPending, error } = useCurrentUser();
  const isUnauthorized = error instanceof ApiError && error.status === 401;

  useEffect(() => {
    if (isUnauthorized) login();
  }, [isUnauthorized]);

  if (isPending || isUnauthorized) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <Spinner className="size-8" />
      </div>
    );
  }

  // Non-401 (network failure, 5xx): surface via the route ErrorBoundary instead
  // of bouncing to login, which would loop.
  if (error) throw error;

  return <Outlet />;
}
