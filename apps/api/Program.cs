using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Set up CORS policy to allow requests from SPA during development
builder.Services.AddCors(options =>
{
    options.AddPolicy("spa", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var domain = builder.Configuration["Auth0:Domain"];
        var audience = builder.Configuration["Auth0:Audience"];

        if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("Missing Auth0:Domain or Auth0:Audience config.");

        options.Authority = $"https://{domain}/";
        options.Audience = audience;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Add CORS middleware
app.UseCors("spa");

// Add authentication & authorization middlewares
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "GoTogether API v1");
    });
}

app.UseHttpsRedirection();

// Use api variable for versioning and holding prefix
var api = app.MapGroup("/api/v1");

api.MapGet("/", () =>
{
    return "Home!";
})
    .WithName("Hello")
    .WithSummary("Health check / hello")
    .WithDescription("Simple endpoint used to confirm the API is reachable.");

api.MapGet("/secret", () => Results.Ok(new { message = "Authenticated ✅" }))
   .RequireAuthorization()
   .WithTags("Auth")
   .WithSummary("Protected test endpoint");

app.Run();
