var builder = WebApplication.CreateBuilder(args);
await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/sleep", (CancellationToken cancellationToken) =>
{
    while (!cancellationToken.IsCancellationRequested)
    {
        Enumerable.Range(1, 100).Select(x => x).ToList().ForEach(x =>
        {
            //WARNING: BAD CODE
            Task.Run(() => Thread.Sleep(3 * 60 * 1_000), cancellationToken);
        });
        Thread.Sleep(2 * 60 * 1_000);
    }
    return "Done.";
});

app.MapGet("/delay", async (CancellationToken cancellationToken) =>
{
    while (!cancellationToken.IsCancellationRequested)
    {
        Enumerable.Range(1, 100).Select(x => x).ToList().ForEach(x =>
        {
            //WARNING: BAD CODE
            Task.Run(async () => await Task.Delay(3 * 60 * 1_000, cancellationToken), cancellationToken);
        });
        await Task.Delay(2 * 60 * 1_000, cancellationToken);
    }
    return "Done.";
});

await app.RunAsync();