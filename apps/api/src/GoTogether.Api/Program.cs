using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GoTogether.Data;
using GoTogether.Features.Places;
using GoTogether.Features.Profile;
using GoTogether.Storage;

var builder = WebApplication.CreateBuilder(args);

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

// ---- Register DbContext
var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(cs));

// ---- OpenAPI + Swagger UI ----
// Add 'Authorize' button to the Swagger UI for adding bearer token for testing
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??=
            new Dictionary<string, OpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] =
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization"
            };

        document.SecurityRequirements ??=
            new List<OpenApiSecurityRequirement>();

        document.SecurityRequirements.Add(
            new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }
                ] = Array.Empty<string>()
            });

        return Task.CompletedTask;
    });
});


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
        
        options.IncludeErrorDetails = true;
    });

// ---- File Storage ----
builder.Services.Configure<LocalStorageOptions>(
    builder.Configuration.GetSection("Storage")
);

builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();

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

    var uploadRoot = builder.Configuration["Storage:UploadRoot"] ?? "uploads";
    var uploadPath = Path.Combine(app.Environment.ContentRootPath, uploadRoot);
    // Ensure the folder exists so PhysicalFileProvider doesn't throw an error
    Directory.CreateDirectory(uploadPath);

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadPath),
        RequestPath = "/" + uploadRoot
    });
}

// ---- Routes ----
var api = app.MapGroup("/api/v1");

api.MapGroup("/places").MapPlacesEndpoints();

// Map profile routes
var me = api.MapGroup("/me")
    .RequireAuthorization();

me.MapMeEndpoints();
me.MapMeAvatarEndpoints();


app.Run();

// Required for WebApplicationFactory<Program>
public partial class Program { }
