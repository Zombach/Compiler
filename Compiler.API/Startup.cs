using Compiler.API.Controllers.Extensions;
using Compiler.Core;
using Compiler.Models.Settings;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using NSwag.Generation.Processors.Security;
using System.Net;
using System.Text.Json.Serialization;

namespace Compiler.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        string currentEnvironment = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile($"appsettings{currentEnvironment}.json", true);
        Configuration = builder.Build();
        Instance.App.Config = builder.Build().Get<AppConfig>();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container. 
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
        services.AddAppConfiguration(Configuration);
        services.AddAutoMapper(typeof(Startup));
        services.AddConnectors();
        services.AddCustomServices();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsApi", builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials());
        });
        services.AddControllers();
        
        services
        .AddMvc().AddXmlSerializerFormatters()
        .AddXmlDataContractSerializerFormatters()
        .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });


        services.AddSwaggerDocument(document =>
        {
            document.DocumentName = "Endpoints for ZDE";
            document.Title = "ZLoo Development Environment API";
            document.Version = "v8";
            document.Description = "An interface for ZDE";
            document.DocumentProcessors.Add(
                new SecurityDefinitionAppender("JWT token", new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Copy 'Bearer ' + valid JWT token into field",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header
                }));
            document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));
        });
        services.AddOptions();
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
        app.UseOpenApi();
        app.UseSwaggerUi3(s => ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true);
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("CorsApi");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}