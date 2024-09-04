using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Models;
using HelteOgHulerServer.Services;
using HelteOgHulerServer.Utilities;
using HelteOgHulerServer;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

var unauthorizedError = HHJsonSerializer.Serialize(new HHError
{
    Message = "Your Innkeeper license could not be verified."
});

// Allow all mongodb serialization
BsonSerializer.RegisterSerializer(new ObjectSerializer(ObjectSerializer.AllAllowedTypes));
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
BsonSerializer.RegisterSerializer(new EnumSerializer<ActionName>(BsonType.String));
BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;

// Register mongodb event types
BsonClassMap.RegisterClassMap<Inn>();
BsonClassMap.RegisterClassMap<Hero>();
BsonClassMap.RegisterClassMap<Monster>();
BsonClassMap.RegisterClassMap<AdventureEvent_V1>();
BsonClassMap.RegisterClassMap<NewPlayerEvent_V1>();
BsonClassMap.RegisterClassMap<RecruitHeroEvent_V1>();

var builder = WebApplication.CreateBuilder(args);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
}
);

// builder.Services.AddHttpsRedirection(options =>
// {
//     options.HttpsPort = 443;
// });

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

// Configure server
// builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });
builder.WebHost.ConfigureKestrel(options => { options.AddServerHeader = false; });

var app = builder.Build();

// app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// Setup Swagger if Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Instantiate critical services on startup
app.Services.GetService<GameStateLogic>();
var userLogic = app.Services.GetService<UserLogic>();

// Setup auth middleware
app.Use(async (context, next) =>
{
    context.Items["User"] = userLogic?.GetUser(context.Request.Headers["HHLoginName"]);

    if (context.Items["User"] == null)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(unauthorizedError);
        return;
    }

    await next();
});

app.MapControllers();

// app.UseResponseCompression();

app.Run();

if (app.Environment.IsProduction())
{
    HHSentry.Initialize(builder.Configuration.GetSection("Sentry"));
}

// TODO: 
// Evaluate if compression is worth the response delay