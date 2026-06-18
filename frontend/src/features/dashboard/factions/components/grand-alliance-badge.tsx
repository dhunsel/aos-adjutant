import { Badge } from "@/components/ui/badge";
import { type ComponentProps } from "react";
import type { GrandAlliance } from "@/types/api.types";
import { cn } from "@/lib/utils";

export function GrandAllianceBadge({
  grandAlliance,
  className,
  ...props
}: { grandAlliance: GrandAlliance } & ComponentProps<typeof Badge>) {
  let color;
  switch (grandAlliance) {
    case "order": {
      color = "bg-alliance-order text-foreground";
      break;
    }
    case "chaos": {
      color = "bg-alliance-chaos text-foreground";
      break;
    }
    case "death": {
      color = "bg-alliance-death text-foreground";
      break;
    }
    case "destruction": {
      color = "bg-alliance-destruction text-foreground";
      break;
    }
  }

  return (
    <Badge className={cn(color, className)} {...props}>
      {grandAlliance.charAt(0).toUpperCase() + grandAlliance.slice(1)}
    </Badge>
  );
}
