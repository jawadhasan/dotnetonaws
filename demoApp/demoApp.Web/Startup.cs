using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using demoApp.Data;
using demoApp.Web.Dynamo;
using demoApp.Web.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace demoApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetSection("DefaultConnection").Value;

            var productRepo = new ProductsRepository(connString);
            services.Add(new ServiceDescriptor(typeof(ProductsRepository), productRepo));

            var userRepo = new UserRepository(connString);
            services.Add(new ServiceDescriptor(typeof(UserRepository), userRepo));

            services.AddControllers();

            //AWS Services
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName("eu-central-1")//can read from appsetting.json
            });
            services.AddSingleton<TruckSensorRepo>();

        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandler>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
            });
        }
    }
}
