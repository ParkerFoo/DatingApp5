using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions    {

        public static IServiceCollection AddIdentityServiceExtensions(this IServiceCollection services,IConfiguration _config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,  // important line. means api validate the key set to true
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),  // important line. get the signing key
                    ValidateIssuer = false, //api server
                    ValidateAudience = false //angular application
                };
            });

            return services;
        }        
    }
}