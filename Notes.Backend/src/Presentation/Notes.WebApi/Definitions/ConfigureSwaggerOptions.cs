using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Notes.WebApi.Definitions
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider) => 
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider; 

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions) 
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName,
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Version = apiVersion,
                        Title = $"Notes API {apiVersion}",
                        Description = "Simple app",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Denis"
                        }
                    });

                options.AddSecurityDefinition($"AuthToken {apiVersion}",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Name = "Authorization",
                        Description = "Authorization token"
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"AuthToken {apiVersion}"
                            }
                        },
                        new string[] { }
                    }
                });

                options.CustomOperationIds(apiDescription =>
                    apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                        ? methodInfo.Name
                        : null);
            }
        }
    }
}
