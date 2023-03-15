using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Interfaces;
using Notes.Application.Mappings;
using Notes.WebApi.Definitions.DbContext;
using System.Reflection;
using Notes.Application;
using Notes.Persistence;

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

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseEndpoints(endpoints => endpoints.MapControllers());


app.UseDbInitializer();
app.Run();
