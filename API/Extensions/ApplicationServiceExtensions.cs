using API.Data;
using API.Helpers;
using API.interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration _config)
        {            

            //added by FRS
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ITokenService, TokenService>(); //added by FRS

            services.AddScoped<IUserRepository,UserRepository>(); //added by FRS

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly); //added by FRS. This will find the mapping we crate in the AutoMapperProfiles

            services.Configure<CloudinarySettings>(_config.GetSection("CloudinarySettings")); //added by FRS

            services.AddScoped<IPhotoService,PhotoService>(); //added by FRS

            return services;
        }
    }
}