using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;
using QuestionBank.Components;
using QuestionBank.Components.Model;
using QuestionBank.Components.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Console sink
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

// add auth 

//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// builder.Services.AddAuthorization(options =>
// {
//     options.FallbackPolicy = options.DefaultPolicy;
// });
builder.Services.AddHttpContextAccessor();  
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddTransient<QuestionSampler>();
builder.Services.AddTransient<ResultsManager>();
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Register DbContext
builder.Services.AddDbContext<QuestionBankContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("QuestionBank")));

// Register your service
builder.Services.AddScoped<ConfigService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor 
                               | ForwardedHeaders.XForwardedProto 
                               | ForwardedHeaders.XForwardedHost;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.Configure<List<FormulaItem>>(
    builder.Configuration.GetSection("Formulas"));
builder.Services.AddSingleton<FormulaStore>();

var app = builder.Build();

// Add before UseAuthentication
app.UseForwardedHeaders();

//app.UseAuthentication();
//app.UseAuthorization();
app.UseHttpsRedirection();

app.UseStaticFiles();

// create the db
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuestionBankContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Use(async (context, next) =>
{
    Log.Information("Request scheme: " + context.Request.Scheme);
    await next();
});
try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}