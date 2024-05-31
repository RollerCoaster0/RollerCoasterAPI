using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using RollerCoaster;
using RollerCoaster.DataBase;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;
using RollerCoaster.Services.Abstractions.LongPoll;
using RollerCoaster.Services.Abstractions.Sessions;
using RollerCoaster.Services.Abstractions.Users;
using RollerCoaster.Services.Implementations.Common;
using RollerCoaster.Services.Implementations.Game;
using RollerCoaster.Services.Implementations.LongPoll;
using RollerCoaster.Services.Implementations.Session;
using RollerCoaster.Services.Implementations.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SiteConfiguration>(builder.Configuration);
builder.Services.Configure<SiteConfiguration.JWTConfiguration>(builder.Configuration.GetSection("JWT"));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RollerCoaster", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCors();

builder.Services.AddSingleton<ILongPollService, SimpleQueueLongPollService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IPasswordHashService, PasswordHashService>();
builder.Services.AddSingleton<IRollService, RollService>();
builder.Services.AddSingleton<IFileTypeValidator, FileTypeValidator>();

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IActiveNonPlayableCharactersService, ActiveNonPlayableCharactersService>();
builder.Services.AddScoped<IQuestStatusService, QuestStatusService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IQuestService, QuestService>();
builder.Services.AddScoped<INonPlayableCharacterService, NonPlayableCharacterService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ICharacterClassService, CharacterClassService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(builder.Configuration["ObjectStorage:Endpoint"])
    .WithSSL(false)
    .WithCredentials(
        builder.Configuration["ObjectStorage:AccessKey"],
        builder.Configuration["ObjectStorage:SecretKey"]));

builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DataBase:ConnString"]);
});

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("DockerTesting"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policyBuilder => policyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
}

app.UseAuthorization();

app.MapControllers();

app.Run();