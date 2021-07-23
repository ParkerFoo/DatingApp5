using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Extensions;

namespace API
{
    public class Startup
    {

        //using better way. Dependeny injection way
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;  //configuration being injected to startup class 
        }
        //old way
        // public Startup(IConfiguration configuration)
        // {
        //     Configuration = configuration; //configuration being injected to startup class 
        // }

       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //commented because already moved to ApplicationServicesExtensions.cs
            // //added by FRS
            // services.AddDbContext<DataContext>(options =>
            // {
            //     options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            // });           

            // services.AddScoped<ITokenService, TokenService>(); //added by FRS
            services.AddApplicationServices(_config); //added by FRS


            services.AddCors();  //added by FRS


            services.AddControllers();
         

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            //commented because already moved to IdentityServiceExtensions.cs
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=>
            // {
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //         ValidateIssuerSigningKey=true,  // important line. means api validate the key set to true
            //         IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),  // important line. get the signing key
            //        ValidateIssuer=false, //api server
            //         ValidateAudience=false //angular application
            //    };
            // });
            services.AddIdentityServiceExtensions(_config); //added by FRS

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //FRS: ordering here really matters. so be careful
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection(); //if http, redirect to https endpoint 

            app.UseRouting(); //for us to navigate through url 

            //added FRS
            //allow any header- allow headers such as authentication coming from angular
            //allow any method - allow any methods such as PUT, GET POST
            //the url is the angular url
            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));          
            //
            
            app.UseAuthentication(); //added by FRS

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
