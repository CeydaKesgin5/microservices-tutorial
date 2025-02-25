var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.MapGet("/api", () => "API 3");

app.Run();
