using System.Security.Claims;
using TechRental.Application.Abstractions.Identity;
using TechRental.Presentation.WebAPI.Configuration;
using TechRental.Presentation.WebAPI.Extensions;
using TechRental.Presentation.WebAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogForAppLogs(builder.Configuration);

var webApiConfiguration = new WebApiConfiguration(builder.Configuration);
var identityConfigurationSection = builder.Configuration.GetSection("Identity:IdentityConfiguration");

builder.Services.ConfigureServices(
    webApiConfiguration,
    identityConfigurationSection,
    builder.Configuration);

var app = builder.Build().Configure();

using (var scope = app.Services.CreateScope())
{
    var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.Role, TechRentalIdentityRoleNames.AdminRoleName),
        new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
    }));

    var currentUserManager = scope.ServiceProvider.GetRequiredService<ICurrentUserManager>();
    currentUserManager.Authenticate(principal);

    await SeedingHelper.SeedRoles(scope.ServiceProvider);
    await SeedingHelper.SeedAdmins(scope.ServiceProvider, app.Configuration);
}

app.Logger.LogInformation("Initializing IronPDF renderer...");
Installation.Initialize();
app.Run();
