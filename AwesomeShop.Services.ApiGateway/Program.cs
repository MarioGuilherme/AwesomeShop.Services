using Ocelot.DependencyInjection;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddJsonFile("configuration.json")
    .AddEnvironmentVariables();

builder.Services.AddOcelot();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseOcelot();

app.UseAuthorization();

app.MapControllers();

app.Run();