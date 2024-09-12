using Business_Logic_Layer.AppConstants;
using Business_Logic_Layer.IAuditModule;
using Business_Logic_Layer.IUpdateScripts;
using Business_Logic_Layer.LogsModule;
using Business_Logic_Layer.UpdateScripts;
using Data_Access_Layer;
using Data_Access_Layer.Repositories;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo.Wmi;
using SafeScriptDb_BE.AppConstants;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

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

// Register services
//builder.Services.AddScoped<IDataAccessLayer, DataAccessLayer>(); // Example: register your data access layer
builder.Services.AddScoped<IServerService, ServerService>();
builder.Services.AddSingleton<IDatabaseSettings, Business_Logic_Layer.AppConstants.DatabaseSettings>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(AppDatabaseSettings.GetApplicationConnectionString()));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
