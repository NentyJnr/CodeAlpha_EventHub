using EventHub.Context;
using EventHub.Contracts;
using EventHub.Entities;
using EventHub.Extension;
using EventHub.Services;
using Microsoft.EntityFrameworkCore;

namespace EventHub
{
    public static class ServiceRegistry
    {
        public static IServiceCollection ServiceRegister(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("EventHubConnection")));

            services.Configure<ApplicationSettings>(config.GetSection("ApplicationSettings"));

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IDashboardService, DashBoardService>();
            services.AddScoped<IEventUploadService, EventUploadService>();
            services.AddScoped<IGuestSpeakerService, GuestSpeakerService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUploadHelper, UploadHelper>();


            return services;
        }
    }
}
