using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GoTogether.Data;
using Microsoft.Extensions.Options;

namespace GoTogether.ApiTests;

public sealed class TestAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
           // Replace the real DbContext with SQLite in-memory
            var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Datasource=:memory:");
            });

            // Build the provider, open the connection, and create the schema
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.OpenConnection();
            db.Database.EnsureCreated();
        });
    }
}