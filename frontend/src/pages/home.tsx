export function Home() {
  return (
    <>
      <div className="flex items-center gap-3 py-2 pr-15 pl-6 leading-none">
        <h1>Welcome to </h1>
        <div className="item-start flex flex-col font-heading leading-none tracking-wide text-primary uppercase">
          <span className="text-2xl">AoS</span>
          <span className="text-xs tracking-widest text-muted-foreground">Adjutant</span>
        </div>
      </div>
    </>
  );
}
