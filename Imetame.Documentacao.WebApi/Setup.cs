using AutoMapper;

using Imetame.Documentacao.WebApi.Filters;
using Imetame.Documentacao.WebApi.Helpers;


using Imetame.Documentacao.WebApi.Options;

using Imetame.Documentacao.Domain.Core;
using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Core.Models;
using Imetame.Documentacao.Domain.Helpers;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Domain.Services;
using Imetame.Documentacao.Infra.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.OpenApi.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Infra.Data.Repositories;

namespace Imetame.Documentacao.WebApi
{
    public static class Setup
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(ValidateModelStateFilter));

            })
              
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                });


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;

        }


        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Add framework services.
            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DeParaConnection"),
                     options => options.EnableRetryOnFailure());


            });

            return services;
        }


        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PodeGerenciar", policy => policy.RequireClaim("servidor", "gerenciar"));
                

            });

            return services;
        }


        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "M2M",
                    Version = "v1",
                    Description = "M2M API",
                    TermsOfService = new Uri("https://www.imetame.com.br/TermsOfService"), // exemplo, não é obrigatorio
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {

                    Type = SecuritySchemeType.OpenIdConnect,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    OpenIdConnectUrl = new Uri("https://minha.imetame.com.br/"),


                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

            });

            return services;
        }
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program).GetTypeInfo().Assembly, typeof(Entity).GetTypeInfo().Assembly);


            return services;
        }

        public static IServiceCollection RegisterCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            //api

            

            //Options
            services.Configure<EmailSenderOptions>(configuration.GetSection("Email"));
            services.Configure<ReportOption>(configuration.GetSection("Report"));
            services.Configure<MqttOptions>(configuration.GetSection("Mqtt"));
            services.Configure<ConfigDestra>(configuration.GetSection("Destra"));


            //email
            //services.AddPostal();





            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


            // Domain
            services.AddScoped<IUser, AspNetUser>();
            
            services.AddScoped<ICredenciadoraDeParaService, CredenciadoraDeParaService>();
            services.AddScoped<ICadastroService, CadastroService>();
            services.AddScoped<ICadastroDestraService, CadastroDestraService>();
            services.AddScoped<IPedidoService, PedidoService>();  
            
            services.AddScoped<IProcessamentoService, ProcessamentoService>();
            
            services.AddScoped<ConsoleHelper>();

            // Infra - Data      


            services.AddTransient<ApplicationDbContext>();



            services.AddScoped<Domain.Repositories.IColaboradorRepository, Infra.Data.Repositories.ColaboradorRepository>();
            services.AddScoped<Domain.Repositories.ICredenciadoraDeParaRepository, Infra.Data.Repositories.CredenciadoraDeParaRepository>();
            services.AddScoped<Domain.Repositories.IPedidoRepository, Infra.Data.Repositories.PedidoRepository>();
            services.AddScoped<Domain.Repositories.IProcessamentoRepository, Infra.Data.Repositories.ProcessamentoRepository>();
            services.AddScoped<Domain.Repositories.ILogProcessamentoRepository, Infra.Data.Repositories.LogProcessamentoRepository>();
            services.AddScoped<Domain.Repositories.IResultadoCadastroRepository, Infra.Data.Repositories.ResultadoCadastroRepository>();            
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //CrossCutting
            services.AddTransient<CrossCutting.Services.Destra.IDestraService, CrossCutting.Services.Destra.DestraService>();

            //Selenium
            services.AddScoped<IWebDriver, ChromeDriver>();
            services.AddSingleton<ConfigDestra>();






            return services;
        }

       
        public static IApplicationBuilder UseCustomStaticFiles(this IApplicationBuilder app)
        {
            app.UseStaticFiles();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Startup.Configuration["PdfsStaticFiles:Path"]),
            //    RequestPath = "/pdf"
            //});



            return app;
        }

    }
}
