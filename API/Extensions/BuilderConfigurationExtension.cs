namespace API.Extensions;

/// <summary>
/// Extension class for IApplicationBuilder to configure Swagger.
/// </summary>
public static class BuilderConfigurationExtension
{
    public static IApplicationBuilder AddSwaggerExtension(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
            
        return app;
    }
}
