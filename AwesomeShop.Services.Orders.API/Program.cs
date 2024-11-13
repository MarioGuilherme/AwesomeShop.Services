using AwesomeShop.Services.Orders.Application;
using AwesomeShop.Services.Orders.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRedisCache()
    .AddMessageBus()
    .AddMongo()
    .AddRepositories()
    .AddHandlers()
    .AddSubscribers()
    .AddServiceDiscoveryConfig(builder.Configuration);

builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseConsul();

app.MapControllers();

app.Run();