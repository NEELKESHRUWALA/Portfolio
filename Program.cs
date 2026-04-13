using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Services;
using Portfolio.Components;
using Portfolio.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("!!! APP STARTING WITH SQL SERVER !!!");
Console.WriteLine("========================================");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Response caching for static assets
builder.Services.AddResponseCaching();

// Configure Infrastructure: DB Context with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Application: Register Portfolio Service (Dependency Injection)
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

var app = builder.Build();

// Ensure Database is initialized and seeded from appsettings.json
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    await DbInitializer.SeedData(context, config);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// Response caching for static assets
app.UseResponseCaching();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
