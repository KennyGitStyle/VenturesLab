using API.Endpoints;
using API.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddConfigurationExtension();
builder.Services.AddDbConnExtension(builder.Configuration);
builder.Services.AddRedisServiceExtension(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<CachingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.AddSwaggerExtension();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

await app.UseDbInitializer();

app.MapUserTasksEndpoint();

app.MapHealthChecks("/health");

await app.RunAsync();
