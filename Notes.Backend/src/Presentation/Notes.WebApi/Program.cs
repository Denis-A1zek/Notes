using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Notes.Application;
using Notes.Application.Interfaces;
using Notes.Application.Mappings;
using Notes.Persistence;
using Notes.WebApi.Common.Extensions;
using Notes.WebApi.Definitions;
using Notes.WebApi.Definitions.DbContext;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(options =>
{
    options.AddProfile(new AssemblyMappingProfile(assembly: Assembly.GetExecutingAssembly()));
    options.AddProfile(new AssemblyMappingProfile(assembly: typeof(IApplicationDbContext).Assembly));
});

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddVersionedApiExplorer(options =>
                options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
        ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();


builder.Services.AddApiVersioning();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:7172/";
    options.Audience = "NotesWebAPI";
    options.RequireHttpsMetadata = false;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    foreach (var description in app.Services.GetService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
    {
        config.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
        config.RoutePrefix = string.Empty;
    }
});
app.UseCustomExceptionHandler();  

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();
app.UseEndpoints(endpoints => endpoints.MapControllers());


app.UseDbInitializer();
app.Run();
