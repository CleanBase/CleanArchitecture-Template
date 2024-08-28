using CleanBase.Core.Api;
using CleanBase.Core.Data.UnitOfWorks;
using CleanBase.Core.Infrastructure.Jobs.Hosting;
using CleanBase.Core.Services.Jobs;
using Domain.Validators;
using Domain;
using Core.ViewModels.Profiles;
using Core;
using Infrastructure;
using Infrastructure.UnitOfWorks;
using Infrastructure.Azure.KeyVault;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Authorization;
using CleanBase.Core.Api.Swagger;
using CleanBase.Core.Security;
using CleanBase.Core.Services.Core.Base;
using CleanBase.Core.Api.Authorization;
using Infrastructure.Jobs;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using CleanBase.Core.Services.Core;
using CleanBase.Core.Data.Policies.Base;
using CleanBase.Core.Infrastructure.Policies;
using CleanBase.Core.Services.Storage;
using CleanBase.Core.Api.Middlewares;

// Create builder
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Configure logging
Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

builder.Services.AddSingleton(Log.Logger);

// Configure DbContext with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(config.GetConnectionString("DefaultConnection")));


// Configure configuration sources
var vaultName = config["KeyVault:VaultName"];

// Uncomment if Key Vault is used
// builder.Configuration.AddAzureKeyVault(
//     $"https://{vaultName}.vault.azure.net/",
//     config["KeyVault:ClientId"],
//     config["KeyVault:ClientSecret"],
//     new PrefixKeyVaultManager(config["KeyVault:Prefix"])
// );

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = config["App:AppId"] + " API",
        Version = "v1"
    });
    if (!string.IsNullOrEmpty(config["AppSettings:PathBase"]))
    {
        c.DocumentFilter<BasePathFilter>(new object[] { config["AppSettings:PathBase"] });
    }
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer' followed by a space and JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure authentication and authorization
IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = config["Auth:Authority"],
			ValidateIssuer = true,
			ValidateAudience = !string.IsNullOrEmpty(config["Auth:Audience"]),
			ValidAudience = config["Auth:Audience"],
			RequireSignedTokens = !string.IsNullOrEmpty(config["Auth:Modulus"]),
			ValidateIssuerSigningKey = !string.IsNullOrEmpty(config["Auth:Modulus"]),
			IssuerSigningKey = !string.IsNullOrEmpty(config["Auth:Modulus"])
				? CryptoGraphyHelper.SigningKey(config["Auth:Modulus"], config["Auth:Exponent"])
				: null
		};
		options.Events = new JwtBearerEvents
		{
			OnTokenValidated = async context =>
			{
				if (context.Principal.Identity is ClaimsIdentity claimsIdentity && claimsIdentity.IsAuthenticated)
				{
					var provider = context.HttpContext.RequestServices.GetService<IIdentityProvider>();
					var logger = context.HttpContext.RequestServices.GetService<ISmartLogger>();
					await provider.UpdateIdentity(context.Principal, (context.SecurityToken as JwtSecurityToken)?.RawData);
					logger.UserName = provider.Identity.UserName;
				}
			}
		};
	});
builder.Services.AddSingleton<IAuthorizationHandler, RolePermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();


// Add Hangfire
var hangfireOptions = new PostgreSqlStorageOptions
{
    QueuePollInterval = TimeSpan.FromSeconds(5),
    PrepareSchemaIfNecessary = true,
    SchemaName = "public"
};
var storage = new PostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"), hangfireOptions);
builder.Services.AddHangfire(config => config.UseStorage(storage));
builder.Services.AddHangfireServer(options => options.WorkerCount = 5 * Environment.ProcessorCount);


// Register additional services
builder.Services.AddScoped<ISmartLogger, SmartLogger>();
builder.Services.AddScoped<IIdentityProvider, IdentityProvider>();
builder.Services.AddScoped<IPolicyFactory, PolicyFactory>();
builder.Services.AddScoped<ICoreProvider, CoreProvider>();
builder.Services.AddScoped(typeof(IStorageService<>), typeof(StorageService<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkGeneral>();
builder.Services.RegisterInfrastructureService();
builder.Services.RegisterDomainService();
builder.Services.AddAutoMapperProfiles();
builder.Services.RegisterValidators();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Init middleware
app.UseMiddleware<HttpInjectMiddleware>(Array.Empty<object>());
app.UseMiddleware<RequestLoggingMiddleware>(Array.Empty<object>());

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors(policy => policy
    .WithOrigins(config["Cors:AllowedHosts"]?.Split(',') ?? new[] { "*" })
    .AllowAnyHeader()
    .AllowAnyMethod());

app.MapControllers();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new DashboardAuthorizationFilter() }
});

// Seed data
//using (var scope = app.Services.CreateScope())
//{
//    var contextSeed = scope.ServiceProvider.GetRequiredService<ContextSeed>();
//    await contextSeed.SeedData();
//}

app.Run();
