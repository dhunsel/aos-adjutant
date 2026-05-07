import { Spinner } from "@/components/ui/spinner";
import { useFactions } from "../faction.queries";
import type { GrandAlliance } from "@/types/api.types";
import { Badge } from "@/components/ui/badge";
import { type ComponentProps } from "react";
import { cn } from "@/lib/utils";
import { ToggleGroup, ToggleGroupItem } from "@/components/ui/toggle-group";
import { useSearchParams } from "react-router";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";

const GrandAllianceBadge = ({
  grandAlliance,
  className,
  ...props
}: { grandAlliance: GrandAlliance } & ComponentProps<typeof Badge>) => {
  let color;
  switch (grandAlliance) {
    case "Order": {
      color = "bg-alliance-order text-foreground";
      break;
    }
    case "Chaos": {
      color = "bg-alliance-chaos text-foreground";
      break;
    }
    case "Death": {
      color = "bg-alliance-death text-foreground";
      break;
    }
    case "Destruction": {
      color = "bg-alliance-destruction text-foreground";
      break;
    }
  }

  return (
    <Badge className={cn(color, className)} {...props}>
      {grandAlliance}
    </Badge>
  );
};

export function FactionListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const grandAlliance = searchParams.get("grandAlliance") as GrandAlliance;
  const factions = useFactions({ GrandAlliance: grandAlliance });

  const onFilterClick = (value: string[]) => {
    const newVal = value[0] ? { grandAlliance: value[0] } : undefined;
    setSearchParams(newVal);
  };

  return (
    <div className="flex flex-col gap-2">
      <div className="flex justify-between">
        <ToggleGroup value={[grandAlliance]} onValueChange={onFilterClick}>
          <ToggleGroupItem variant="outline" value="Order">
            Order
          </ToggleGroupItem>
          <ToggleGroupItem variant="outline" value="Chaos">
            Chaos
          </ToggleGroupItem>
          <ToggleGroupItem variant="outline" value="Death">
            Death
          </ToggleGroupItem>
          <ToggleGroupItem variant="outline" value="Destruction">
            Destruction
          </ToggleGroupItem>
        </ToggleGroup>
        <Button variant="outline">
          <Plus />
          Add Faction
        </Button>
      </div>
      {factions.isLoading ? (
        <div className="pt-10">
          <Spinner className="mx-auto" />
        </div>
      ) : (
        <div className="grid grid-cols-[repeat(auto-fill,minmax(250px,1fr))] gap-3">
          {factions.data?.items.map((f) => (
            <div
              key={f.factionId}
              className="flex flex-col gap-2 rounded-lg border border-border bg-card p-2 text-nowrap text-card-foreground"
            >
              <span className="font-heading text-xl">{f.name}</span>
              <GrandAllianceBadge grandAlliance={f.grandAlliance} />
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
