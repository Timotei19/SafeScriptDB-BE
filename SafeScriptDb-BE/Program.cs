using Business_Logic_Layer.AppConstants;
using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.IUpdateScripts;
using Business_Logic_Layer.LogsModule;
using Business_Logic_Layer.Services;
using Business_Logic_Layer.UpdateScripts;
using Data_Access_Layer;
using Data_Access_Layer.ContextInterfaces;
using Data_Access_Layer.Contexts;
using Data_Access_Layer.Repositories;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Management.Smo.Wmi;
using Models.Models;
using SafeScriptDb_BE.AppConstants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200",
                                              "https://localhost:7165")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});


var connectionString = AppDatabaseSettings.GetApplicationConnectionString();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<Data_Access_Layer.Contexts.IdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 15;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<Data_Access_Layer.Contexts.IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Lockout.AllowedForNewUsers = true;
    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    opts.Lockout.MaxFailedAccessAttempts = 3;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
     opt.TokenLifespan = TimeSpan.FromSeconds(3600));

// IdentityDbContext should be resolved before inject  UserManager<ApplicationUser> or SignInManager<ApplicationUser>
// Please see RefreshToken or SignIn actions from IdentityController


builder.Services.AddAuthorization();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// TODO: consolidate token validation parameters with the ones in the token generator
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "SafeScriptDB",
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notificationhub")))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });*/



// Register services
//builder.Services.AddScoped<IDataAccessLayer, DataAccessLayer>(); // Example: register your data access layer
builder.Services.AddScoped<IDatabaseSettings, Business_Logic_Layer.AppConstants.DatabaseSettings>();
// Correctly registered as Scoped
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IServerService, ServerService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ISignInService, SignInService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddSingleton<IUserContext, UserContext>();

/*builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    // Configure password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();*/

builder.Services.AddScoped<IAuditRepository, AuditRepository>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
