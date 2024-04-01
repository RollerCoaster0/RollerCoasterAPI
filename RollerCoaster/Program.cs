using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RollerCoaster;
using RollerCoaster.DataBase;
using RollerCoaster.Services.Abstractions.Game;
using RollerCoaster.Services.Abstractions.Users;
using RollerCoaster.Services.Realisations.Game;
using RollerCoaster.Services.Realisations.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SiteConfiguration>(builder.Configuration);
builder.Services.Configure<SiteConfiguration.JWTConfiguration>(builder.Configuration.GetSection("JWT"));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IPasswordHashService, PasswordHashService>();

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