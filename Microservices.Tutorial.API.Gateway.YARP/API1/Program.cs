var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.MapGet("/api1", () => "API 1");

app.Run();
