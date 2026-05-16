using EventApi.Objects;
using EventApi.Services;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Dependency Injection
builder.Services.AddScoped<IPublisher<EventEnvelope>, EventPublisher>();
builder.Services.AddScoped<IValidator<EventEnvelope>, EventValidator>();

builder.Services.AddSingleton(
    Channel.CreateBounded<EventEnvelope>(new BoundedChannelOptions(1000)
    {
        FullMode = BoundedChannelFullMode.Wait,
        SingleReader = true,
        SingleWriter = false
    }));
builder.Services.AddSingleton<IMessageQueueService, InMemoryMessageQueue>();

builder.Services.AddHostedService<EventProcessorJob>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var configuredApiKey = builder.Configuration["ApiKey"];

    if (!context.Request.Headers.TryGetValue("X-API-Key", out var providedApiKey) ||
        providedApiKey != configuredApiKey)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsJsonAsync(new { error = "Invalid or missing API key" });
        return;
    }

    await next();
});

app.MapControllers();

app.Run();