import { useHideOnScroll } from "@/hooks/use-hide-on-scroll";
import { cn } from "@/lib/utils";
import { SidebarTrigger } from "../ui/sidebar";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "../ui/dialog";
import { Button } from "../ui/button";
import { LogOut, Search } from "lucide-react";
import { SearchInput } from "../ui/search-input";
import { useCurrentUser } from "@/features/auth/auth.queries";
import { logout } from "@/features/auth/auth.actions";

const searchPlaceholder = "Search factions, units, abilities, ...";

export function AppHeader() {
  const isHeaderHidden = useHideOnScroll();
  const { data: user } = useCurrentUser();
  const initials = (user?.username ?? "")
    .split(" ")
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part[0])
    .join("")
    .toUpperCase();

  return (
    <header
      className={cn(
        isHeaderHidden ? "-translate-y-full" : "translate-y-0",
        "sticky top-0 z-40 flex w-full items-center justify-between gap-1 border-b border-border bg-sidebar py-2 pl-2 transition-transform duration-200 md:pl-4",
      )}
    >
      <div className="flex items-center gap-5">
        <SidebarTrigger size="icon-lg" className="md:hidden" />
        <Dialog>
          <DialogTrigger
            className="cursor-pointer md:hidden"
            render={
              <Button variant="ghost" size="icon-lg">
                <Search />
              </Button>
            }
          />
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Search</DialogTitle>
              <SearchInput placeholder={searchPlaceholder} />
            </DialogHeader>
          </DialogContent>
        </Dialog>
        <SearchInput className="hidden max-w-xs md:flex" placeholder={searchPlaceholder} />
      </div>
      <div className="flex max-w-xs items-center gap-3 pr-2">
        <span className="inline-flex size-10 shrink-0 items-center justify-center rounded-3xl border-2 border-sidebar-border bg-sidebar-ring font-bold text-primary-foreground">
          {initials}
        </span>
        <span className="hidden truncate text-foreground md:block">{user?.username}</span>
        <Button variant="ghost" size="icon-lg" aria-label="Log out" onClick={logout}>
          <LogOut />
        </Button>
      </div>
    </header>
  );
}
