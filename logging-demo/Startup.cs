using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog.Extensions.Logging;

namespace logging_demo
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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("LoggingExamples", new OpenApiInfo()
                {
                    Title = "Logging Examples",
                    Description = "API used to demonstrate different types of logging",
                    Version = "v1"
                });
                options.CustomSchemaIds(x => x.FullName);

            });

            //services.AddSingleton<ILoggerFactory>(sp => new SerilogLoggerFactory(null, true));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.DocumentTitle = "Logging API";
                    options.SwaggerEndpoint("LoggingExamples/swagger.json", "Logging Examples");
                });
        }
    }
}
