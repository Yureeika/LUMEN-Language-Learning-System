namespace LUMEN.Extensions;

using LUMEN.Filters;
using LUMEN.Services;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBadgeService, BadgeService>();
        services.AddScoped<IXPService, XPService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<IReportService, ReportService>();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<AuditLogFilter>();
        });

        return services;
    }
}