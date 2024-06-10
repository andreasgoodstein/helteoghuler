using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Models;
using HelteOgHulerServer.Services;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

// Allow all mongodb serialization
ObjectSerializer objectSerializer = new ObjectSerializer(ObjectSerializer.AllAllowedTypes);
BsonSerializer.RegisterSerializer(objectSerializer);
BsonClassMap.RegisterClassMap<AdventureEvent>();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<AdventureLogic>();
builder.Services.AddSingleton<GameStateLogic>();
builder.Services.AddSingleton<UserLogic>();

var app = builder.Build();

app.Services.GetService<GameStateLogic>();
var userLogic = app.Services.GetService<UserLogic>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Items["User"] = userLogic?.GetUser(context.Request.Headers["HHPlayerName"]);

    await next();
});

app.MapControllers();

app.Run();
