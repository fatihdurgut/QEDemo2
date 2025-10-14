using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Authors.Domain.Repositories;
using Authors.Infrastructure.Data;
using Authors.Infrastructure.Repositories;
using Authors.Application.Mappings;

namespace Authors.API.Extensions;

/// <summary>
/// Service collection extensions for dependency injection configuration
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(AuthorMappingProfile).Assembly);
        });

        // Add AutoMapper
        services.AddAutoMapper(typeof(AuthorMappingProfile).Assembly);

        // Add FluentValidation
        services.AddValidatorsFromAssembly(typeof(AuthorMappingProfile).Assembly);

        // Add DbContext
        services.AddDbContext<AuthorsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("AuthorsDb") 
                ?? "Server=localhost;Database=AuthorsDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True";
            options.UseSqlServer(connectionString);
        });

        // Add Repositories
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Authors API",
                Version = "v1",
                Description = "Authors microservice API for managing author information",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Development Team",
                    Email = "dev@example.com"
                }
            });

            // Include XML comments if available
            var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }
}
