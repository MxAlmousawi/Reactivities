using Application.Activities;
using Application.Core;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:5173");
                });
            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(List).Assembly));

            services.AddAutoMapper(typeof(MappingProfiles));

            return services;
        }
    }
}
