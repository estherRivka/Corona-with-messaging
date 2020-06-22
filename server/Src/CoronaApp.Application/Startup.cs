using AutoMapper;
using CoronaApp.Dal;
using CoronaApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NServiceBus;
using NServiceBus.Routing;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
//[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace CoronaApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {

     /*  var endpointConfiguration = new EndpointConfiguration("CoronaApplication");

             var transport = endpointConfiguration.UseTransport<LearningTransport>();


            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration);

            var endpointInstance = services.BuildServiceProvider().GetService<IMessageSession>()
            .ConfigureAwait(false);

            services.AddScoped(typeof(IEndpointInstance), x => endpointInstance);*/

            services.AddScoped(typeof(IPatientRepository), typeof(PatientRepository));
            services.AddScoped(typeof(IPathRepository), typeof(PathRepository));
            services.AddScoped(typeof(IPathService), typeof(PathService));
            services.AddScoped(typeof(IPatientService), typeof(PatientService));

            //services.AddDbContext<CoronaContext>(options => options.UseSqlServer
            //(Configuration.GetConnectionString("CoronaDBConnectionStringTzippy")));

            services.AddDbContext<CoronaContext>(options => options.UseSqlServer
            (Configuration.GetConnectionString("CoronaDBConnectionString")));

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("Policy1",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080");
                        builder.AllowCredentials();
                        builder.AllowAnyHeader();
                        builder.WithMethods("GET", "POST", "PUT");

                    });

            });


            // services.AddIEndpointInstance();
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:Secret"));
            //var appSettingsSection = Configuration.GetSection("AppSettings");
            //services.Configure<AppSettings>(appSettingsSection);
            ////Configuration.GetSection<string>
            //// configure jwt authentication
            //var appSettings = appSettingsSection.Get<AppSettings>();
            //var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
                // setupAction.ApiVersionReader = new HeaderApiVersionReader("version-api");
            });

            services.AddVersionedApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = "'v'VV";
            });

            var versionDescriptionProvider =
                   services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            //  apiVersionDescriptionProvider.ApiVersionDescriptions
            services.AddSwaggerGen(setupAction =>
            {
                foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerDoc(
                                     $"CoronaAppOpenApiSpecification{description.GroupName}", new Microsoft.OpenApi.Models.OpenApiInfo()
                                     {
                                         Title = "Corona Api",
                                         Version = description.ApiVersion.ToString(),
                                         Description = "this app provides information about The Corona virus cases",
                                         Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                                         {
                                             Name = "Tzippy Freedman",
                                             Email = "tzippyfreedman1@gmail.com",

                                         }
                                     });

                    var xmlCommentsFile = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlCommentsFullPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                    setupAction.IncludeXmlComments(xmlCommentsFullPath);
                }
                setupAction.DocInclusionPredicate((documentName, apiDescription) =>
                       {
                           var docApiVersionModel = apiDescription.ActionDescriptor.GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);
                           if (docApiVersionModel == null)
                           {
                               return true;
                           }
                           if (docApiVersionModel.DeclaredApiVersions.Any())
                           {
                               return docApiVersionModel.DeclaredApiVersions.Any(v =>
                               $"CoronaAppOpenApiSpecificationv{v.ToString()}" == documentName);
                           }
                           return docApiVersionModel.DeclaredApiVersions.Any(v =>
                             $"CoronaAppOpenApiSpecificationv{v.ToString()}" == documentName);

                       });


            });

            services.AddMvc(setupAction =>
            {
                setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                setupAction.ReturnHttpNotAcceptable = true;

                //setupAction.Filters.Add(

                //    )
                //    new ProducesResponseTypeAttribute(StatusCodes.Status200OK));
                //setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status404NotFound));

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseErrorHandlingMiddleware();
            app.UseStatusCodePages();
            app.UseApiVersioning();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerEndpoint($"/swagger/CoronaAppOpenApiSpecification{description.GroupName}/swagger.json", $"CoronaApp Api {description.GroupName}");

                }
            });
            app.UseRouting();
            app.UseCors("Policy1"
                );
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies[".AspNetCore.Application.Id"];
                if (!string.IsNullOrEmpty(token))
                {
                    //context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    //context.Response.Headers.Add("X-Xss-Protection", "1");
                    //context.Response.Headers.Add("X-Frame-Options", "DENY");
                    context.Request.Headers.Add("Authorization", "Bearer " + token);

                    //   seDefaultCredentials = true

                }
                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
