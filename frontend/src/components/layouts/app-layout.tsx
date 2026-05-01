import { cn } from "@/lib/utils";
import { Link, Outlet, useMatch } from "react-router";
import { Anvil, Database, Play, ListPlus, Settings, Search, ChevronDown } from "lucide-react";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarProvider,
  SidebarSeparator,
  SidebarTrigger,
  useSidebar,
} from "../ui/sidebar";
import { InputGroup, InputGroupAddon, InputGroupInput } from "../ui/input-group";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../ui/dialog";
import GithubLogo from "@/assets/GitHub_Invertocat_White.svg?react";

type SidebarNavButtonProps = {
  to: string;
  disabled?: boolean;
  closeSidebarOnClick?: boolean;
  showActive?: boolean;
} & Omit<React.ComponentProps<typeof SidebarMenuButton>, "render" | "isActive">;

const SidebarNavButton = ({
  to,
  disabled,
  closeSidebarOnClick = true,
  showActive = true,
  className,
  onClick,
  children,
  ...props
}: SidebarNavButtonProps) => {
  const isActive = !!useMatch({ path: to, end: false });
  const { isMobile, openMobile, toggleSidebar, open } = useSidebar();

  if (disabled)
    return (
      <SidebarMenuButton
        size="xl"
        aria-disabled={true}
        className={cn(
          "cursor-not-allowed font-heading aria-disabled:pointer-events-auto",
          className,
        )}
        onClick={onClick}
        {...props}
      >
        {children}
      </SidebarMenuButton>
    );

  return (
    <SidebarMenuButton
      size="xl"
      render={<Link to={to} aria-current={isActive ? "page" : undefined} />}
      isActive={showActive && isActive}
      className={cn("font-heading", className)}
      onClick={(e) => {
        onClick?.(e);
        if (e.defaultPrevented) return;
        if (closeSidebarOnClick && ((isMobile && openMobile) || (!isMobile && open)))
          toggleSidebar();
      }}
      {...props}
    >
      {children}
    </SidebarMenuButton>
  );
};

export function AppLayout() {
  return (
    // Always show the collapsed icon sidebar
    <SidebarProvider open={false}>
      <Sidebar collapsible="icon">
        <SidebarHeader>
          <SidebarMenu>
            <SidebarMenuItem>
              <SidebarNavButton
                showActive={false}
                className="bg-primary font-semibold text-nowrap text-primary-foreground hover:bg-primary hover:text-primary-foreground active:bg-primary active:text-primary-foreground"
                to="/"
              >
                <Anvil />
                AoS Adjutant
              </SidebarNavButton>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarHeader>
        <SidebarSeparator />
        <SidebarContent>
          <SidebarGroup>
            <SidebarMenu>
              <SidebarMenuItem>
                <SidebarNavButton className="text-nowrap" tooltip="Dashboard" to="/factions">
                  <Database />
                  Dashboard
                </SidebarNavButton>
              </SidebarMenuItem>
              <SidebarMenuItem>
                <SidebarNavButton
                  className="text-nowrap"
                  disabled
                  tooltip="List Builder (TBD)"
                  to="/list-builder"
                >
                  <ListPlus />
                  List Builder
                </SidebarNavButton>
              </SidebarMenuItem>
              <SidebarMenuItem>
                <SidebarNavButton
                  className="text-nowrap"
                  disabled
                  tooltip="Play Mode (TBD)"
                  to="/battle"
                >
                  <Play />
                  Battle Mode
                </SidebarNavButton>
              </SidebarMenuItem>
            </SidebarMenu>
          </SidebarGroup>
        </SidebarContent>
        <SidebarFooter>
          <SidebarMenu>
            <SidebarMenuItem>
              <SidebarMenuButton
                className="font-heading text-nowrap"
                size={"xl"}
                tooltip={"Settings"}
              >
                <Settings />
                Settings
              </SidebarMenuButton>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarFooter>
      </Sidebar>
      <div className="flex min-h-screen flex-1 flex-col">
        <header className="flex w-full items-center justify-between gap-1 border-b border-border bg-sidebar py-2">
          <div className="flex items-center justify-start gap-1">
            <SidebarTrigger className="size-4 md:hidden" />
            <InputGroup className="max-w-xs">
              <InputGroupAddon>
                <Search />
              </InputGroupAddon>
              <InputGroupInput
                className="hidden md:block"
                type="search"
                placeholder="Search factions, units, abilities, ..."
              />
            </InputGroup>
          </div>
          <div className="flex max-w-xs items-center gap-3 pr-2">
            <span className="inline-flex size-10 shrink-0 items-center justify-center rounded-3xl border-2 border-sidebar-border bg-sidebar-ring font-bold text-primary-foreground">
              PU
            </span>
            <span className="hidden truncate text-foreground md:block">Placeholder User</span>
            <ChevronDown className="hidden size-4 shrink-0 md:block" />
          </div>
        </header>
        <main className="flex-1 px-3 py-2 md:px-5 md:py-6">
          <div className="mx-auto max-w-7xl">
            <Outlet />
          </div>
        </main>
        <footer className="flex items-center justify-center gap-3 border-t border-border bg-sidebar py-1 pr-3 text-muted-foreground md:justify-end md:text-xs">
          <Dialog>
            <DialogTrigger className="cursor-pointer hover:text-sidebar-foreground">
              Disclaimer
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>Disclaimer</DialogTitle>
                <DialogDescription>
                  AoS Adjutant is an unofficial application and not affiliated with Games Workshop.
                  Warhammer, Age of Sigmar, and associated brands are {"\u00A9"} Games Workshop Ltd.
                </DialogDescription>
              </DialogHeader>
            </DialogContent>
          </Dialog>
          <span aria-hidden="true">{"\u00B7"}</span>
          <a
            className="shrink-0"
            href="https://github.com/dhunsel/aos-adjutant"
            target="_blank"
            rel="noopener noreferrer"
            aria-label="Github Repository"
          >
            <GithubLogo className="size-6 md:size-4" />
          </a>
          <span aria-hidden="true">{"\u00B7"}</span>
          <span className="font-mono">v{__APP_VERSION__}</span>
        </footer>
      </div>
    </SidebarProvider>
  );
}
