using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceBusManager.Server.API.Controllers;
using ServiceBusManager.Server.Infrastructure;
using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            SetupSettings(services);

            SetupSwagger(services);

            SetupMediatR(services);

            SetupAutomapper(services);

            SetupGZipCompression(services);

            SetupHealthChecks(services);

            SetupInternalServices(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            UseSwagger(app);

            UseGZipCompression(app);

            UseEndpoints(app);
        }

        #region Setup

        private void SetupSettings(IServiceCollection services)
        {
            services.Configure<ProviderOption>(_configuration.GetSection(nameof(ProviderOption)));
        }

        private void SetupSwagger(IServiceCollection services)
        {
            if (!_environment.IsDevelopment())
                return;

            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                var contact = new OpenApiContact
                {
                    Name = _configuration["SwaggerApiInfo:Name"],
                    Email = _configuration["SwaggerApiInfo:Email"],
                    Url = new Uri(_configuration["SwaggerApiInfo:Uri"])
                };

                options.SwaggerDoc(
                    SwaggerDocumentation.AZ_GROUP,
                    new OpenApiInfo
                    {
                        Title = SwaggerDocumentation.AZ_TITLE,
                        Version = SwaggerDocumentation.AZ_VERSION,
                        Description = SwaggerDocumentation.AZ_DESCRIPTION,
                        Contact = contact
                    });

                options.SwaggerDoc(
                    SwaggerDocumentation.AWS_GROUP,
                    new OpenApiInfo
                    {
                        Title = SwaggerDocumentation.AWS_TITLE,
                        Version = SwaggerDocumentation.AWS_VERSION,
                        Description = SwaggerDocumentation.AWS_DESCRIPTION,
                        Contact = contact
                    });

                // Set the comments path for the Swagger JSON and UI.
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        private static void SetupMediatR(IServiceCollection services)
        {
            services.AddMediatR(typeof(Application.Bootstrapper));
        }

        private static void SetupAutomapper(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(Application.Bootstrapper));
        }

        private static void SetupGZipCompression(IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        private static void SetupHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        private void SetupInternalServices(IServiceCollection services)
        {
            services.InitializeInfrastructure(_configuration);
            Application.Bootstrapper.Initialize(services);
        }

        #endregion

        #region Use

        private void UseSwagger(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DisplayRequestDuration();

                    using IServiceScope scope = app.ApplicationServices.CreateScope();

                    IOptionsSnapshot<ProviderOption> opt = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ProviderOption>>();

                    if (opt.Value.Type == ProviderType.Az)
                    {
                        options.SwaggerEndpoint($"./{SwaggerDocumentation.AZ_GROUP}/swagger.json", SwaggerDocumentation.AZ_TITLE);

                    }
                    else if (opt.Value.Type == ProviderType.Aws)
                    {
                        options.SwaggerEndpoint($"./{SwaggerDocumentation.AWS_GROUP}/swagger.json", SwaggerDocumentation.AWS_TITLE);
                    }
                });
            }
        }

        private static void UseGZipCompression(IApplicationBuilder app)
        {
            app.UseResponseCompression();
        }

        private static void UseEndpoints(IApplicationBuilder app)
        {
            // required by Swagger
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                UseHealthCheks(endpoints);
            });
        }

        private static void UseHealthCheks(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapHealthChecks("alive");
        }

        #endregion
    }
}
