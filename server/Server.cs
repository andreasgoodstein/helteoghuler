using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Models;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using HelteOgHulerServer.Utilities;

var unauthorizedError = new HHError
{
    Message = "Your Innkeeper license could not be verified"
};

// Allow all mongodb serialization
ObjectSerializer objectSerializer = new ObjectSerializer(ObjectSerializer.AllAllowedTypes);
BsonSerializer.RegisterSerializer(objectSerializer);
BsonClassMap.RegisterClassMap<AdventureEvent_V1>();
BsonClassMap.RegisterClassMap<NewPlayerEvent_V1>();
BsonClassMap.RegisterClassMap<RecruitHeroEvent_V1>();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));

// Register services
builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<UserService>();

// Register logic
builder.Services.AddSingleton<AdventureLogic>();
builder.Services.AddSingleton<GameStateLogic>();
builder.Services.AddSingleton<HeroLogic>();
builder.Services.AddSingleton<InnLogic>();
builder.Services.AddSingleton<PlayerLogic>();
builder.Services.AddSingleton<UserLogic>();

// Register utilities
builder.Services.AddSingleton<NameUtility>();

var app = builder.Build();

// Instantiate critical services on startup
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
    context.Items["User"] = userLogic?.GetUser(context.Request.Headers["HHLoginName"]);

    if (context.Items["User"] == null)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(HHJsonSerializer.Serialize(unauthorizedError));
        return;
    }

    await next();
});

app.MapControllers();

app.Run();
