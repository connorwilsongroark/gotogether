using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// OPTIONAL for debugging auth issues (turn off once working)
// IdentityModelEventSource.ShowPII = true;
// IdentityModelEventSource.LogCompleteSecurityArtifact = true;

// ---- CORS (React dev server) ----
builder.Services.AddCors(options =>
{
    options.AddPolicy("spa", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ---- OpenAPI + Swagger UI ----
builder.Services.AddOpenApi();

// ---- Auth0 JWT Bearer ----
var domain = builder.Configuration["Auth0:Domain"];
var audience = builder.Configuration["Auth0:Audience"];

if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(audience))
{
    throw new InvalidOperationException(
        "Missing Auth0 configuration. Ensure user-secrets contain 'Auth0:Domain' and 'Auth0:Audience'.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{domain}/";
        options.Audience = audience;

        // Helpful while troubleshooting:
        options.IncludeErrorDetails = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ---- Middleware order matters ----
app.UseCors("spa");

app.UseAuthentication();
app.UseAuthorization();

// ---- OpenAPI endpoints (Development only) ----
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "GoTogether API v1");
        options.RoutePrefix = "swagger";
    });
}

// ---- Routes ----
var api = app.MapGroup("/api/v1");

api.MapGet("/", () => Results.Ok(new { message = "Home ✅" }))
   .AllowAnonymous();

api.MapGet("/secret", () => Results.Ok(new { message = "Authenticated ✅" }))
   .RequireAuthorization();

app.Run();
