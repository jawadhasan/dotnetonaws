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
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Net;
using demoApp.Web.Services;
using demoApp.Web.Settings;
using Npgsql;

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

            // Add IDbConnection for using with Dapper
            services.AddTransient<IDbConnection>(option =>
            new NpgsqlConnection(Configuration.GetConnectionString("DefaultConnection")));


            services.AddTransient<ProductsRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<NotesRepository>();
            services.AddTransient<VehicleRepository>();

           // var connString = Configuration.GetSection("DefaultConnection").Value;

            //var productRepo = new ProductsRepository(connString);
            //services.Add(new ServiceDescriptor(typeof(ProductsRepository), productRepo));

         
            //AWS Services
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName("eu-central-1")//can read from appsetting.json
            });
            services.AddSingleton<TruckSensorRepo>();

            services.Configure<SQSSettings>(Configuration.GetSection("Queueing"));
            services.AddSingleton<IEventPublisher, SQSEventPublisher>();

            services.AddCors();
            services.AddControllers();

            services.AddAuthorization();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>{
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = GetCognitoTokenValidationParams();
                });

            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", options =>
            //    {
            //        options.IncludeErrorDetails = true;
            //        options.Authority = "https://localhost:5001";
            //        options.Audience = "api1";// The API resource scope issued in authorization server
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = false,
            //            ValidIssuer = "https://localhost:5001",
            //        };
            //    });


            //services.AddSwaggerGen(setupAction =>
            //{
            //    setupAction.SwaggerDoc("dotnetonaws", new()
            //    {
            //        Title = ".NET on AWS Web API",
            //        Version = "v1",
            //    });
            //});

            services.AddSwaggerGen();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandler>();

            app.UseHttpsRedirection();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();  
            });

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        private TokenValidationParameters GetCognitoTokenValidationParams()
        {
            var region = Configuration.GetSection("AppConfig:Region").Value;
            var userPoolId = Configuration.GetSection("AppConfig:UserPoolId").Value;
            var appClientId = Configuration.GetSection("AppConfig:AppClientId").Value;

            var cognitoIssuer = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
            var jwtKeySetUrl = $"{cognitoIssuer}/.well-known/jwks.json";
            var cognitoAudience = appClientId;

            return new TokenValidationParameters
            {
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    // get JsonWebKeySet from AWS
                    var json = new WebClient().DownloadString(jwtKeySetUrl);

                    // serialize the result
                    var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;

                    // cast the result to be the type expected by IssuerSigningKeyResolver
                    return (IEnumerable<SecurityKey>)keys;
                },
                ValidIssuer = cognitoIssuer,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = false
                //ValidAudience = cognitoAudience
            };
        }
    }
}
