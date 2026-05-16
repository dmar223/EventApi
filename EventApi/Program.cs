using EventApi.Objects;
using EventApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Dependency Injection
builder.Services.AddScoped<IPublisher<EventEnvelope>, EventPublisher>();
builder.Services.AddScoped<IValidator<EventEnvelope>, EventValidator>();
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

app.MapControllers();

app.Run();