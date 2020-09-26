using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyExpManAPI.Context;
using MyExpManAPI.Helpers;
using MyExpManAPI.Models;
using MyExpManAPI.Services;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Swashbuckle.AspNetCore.Swagger;

namespace MyExpManAPI
{
    public class Startup
    {
    
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<ImageLocalStorage>();

            services.AddHttpContextAccessor();

            services.AddScoped<ActionFilters>();

            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
        
            services.AddSingleton(provider =>
            
                new MapperConfiguration(config =>
                {
                    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                    config.AddProfile(new AutoMapperProfiles(geometryFactory));
                }).CreateMapper()
            );

            services.AddTransient<ILogManager, LogManager>();
            //Conexión a base de datos
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            //Para poder usar views desde un modelo es necesario usar AutoMapper y crearlo como un servicio
            //de Inyección de dependencias entre modelo y entidad.  La idea es solamente pasar los valores
            //que realmente usará el cliente y no tener que pasar todo el cotexto de datos.
           // services.AddAutoMapper(configuration =>
            //{
                //Es mandatorio siempre agregar el mapping de cada DTO con su correspondiente entidad.
                //Sea de la entidad al DTO o viceversa
                //configuration.CreateMap<Autor, AutorDTO>();
               // configuration.CreateMap<Libro, LibroDTO>();
               // configuration.CreateMap<AutorCreacionDTO, Autor>().ReverseMap();
                //usamos ReverseMap para hacer que el mapeo sea ne doble vía
           // }, typeof(Startup));
            //FIn de servicio AutoMapper


            //agregamos el servicio para manejar el identity de nuestra seguridad y 
            //token de confirmación con politicas de password y tiempo de vigencia
            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
                {
                    opt.Password.RequiredLength = 4;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireUppercase = false;
                    opt.User.RequireUniqueEmail = true;
                    opt.SignIn.RequireConfirmedEmail = true;
                    opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailconfirmation");

                services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromDays(3));
            
                services.AddControllers(options =>
                {
                options.Filters.Add(new ExceptionFilters());
                options.Filters.Add(new RequireHttpsAttribute ());
                // Si hubiese Inyección de dependencias en el filtro
                //options.Filters.Add(typeof(ExceptionFilters)); 
            });

               //Servicio de autenticacion para validar Token enviando contra llave de aplicacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                    ClockSkew = TimeSpan.Zero
                });

            //Swagger
            services.AddSwaggerGen(config =>
                {
                    config.SwaggerDoc("v1", new OpenApiInfo
                    {
                    Version = "v1",
                    Title = "Mi Expense Manager Web API",
                    Description = "Esta es una descripción del Web API",
                    TermsOfService = new Uri("https://www.ecuriel.com"),
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("http://bfy.tw/4nqh")
                    },
                
                    });
                });

            var emailConfig = Configuration
            .GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddScoped<IEmailSender, EmailSender>();

        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //Middleware de autenticacion
            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
            //Para dejar configurado Swagger
            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "My Expense Manager API V1");
                config.RoutePrefix= string.Empty;
            });

        }
    }
}
