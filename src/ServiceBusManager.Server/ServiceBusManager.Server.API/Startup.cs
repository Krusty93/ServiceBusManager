using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

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
            SetupServiceBus(services);

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

        private void SetupServiceBus(IServiceCollection services)
        {
            services.AddAzureClients(builder =>
            {
                var connectionString = _configuration.GetConnectionString("ServiceBus");
                builder.AddServiceBusAdministrationClient(connectionString);
            });
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

                var version = _configuration["SwaggerApiInfo:Version"];

                options.SwaggerDoc(
                    version,
                    new OpenApiInfo
                    {
                        Title = $"{_configuration["SwaggerApiInfo:Title"]}",
                        Version = version,
                        Contact = contact
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
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
            //    , typeof(Startup));
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

        private static void SetupInternalServices(IServiceCollection services)
        {
            Infrastructure.Bootstrapper.Initialize(services);
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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    options.InjectStylesheet("/swagger-custom/swaggerstyle.css");
                    options.InjectJavascript("/swagger-custom/swaggerstyle.js");
                    options.DisplayRequestDuration();
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
