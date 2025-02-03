using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy(
                    "CorsPolicy",
                    policy =>
                    {   
                        policy
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithOrigins(["http://localhost:5173", "https://imagekit.io"]);

                    }
                );
            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(List).Assembly));

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<Create>();

            services.AddHttpContextAccessor();

            services.AddScoped<IUserAccessor, UserAccessor>();

            services.AddScoped<IPhotoAccessor, PhotoAccessor>();

            services.Configure<CloudSettings>(config.GetSection("Cloud"));

            services.AddSignalR();

            return services;
        }
    }
}
