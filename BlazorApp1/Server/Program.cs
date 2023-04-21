using BlazorApp1.Server.GrpcServices;
using BlazorApp1.Server.ModelBinders;
using DbRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using winus.Server.Configuration;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews( x=> {
    x.ValueProviderFactories.Add(new UserIdValueProviderFactory());
});

var DbConfig = config.GetSection(DatabaseConfig.SectionName).Get<DatabaseConfig>();

var googleConfig = config.GetSection(GoogleAuthConfig.SectionName).Get<GoogleAuthConfig>();

builder.Services
    .AddOptions<DatabaseConfig>()
    .Bind(config.GetSection(DatabaseConfig.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<GoogleAuthConfig>()
    .Bind(config.GetSection(GoogleAuthConfig.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddAuthentication(
            CookieAuthenticationDefaults.AuthenticationScheme
    )
    .AddCookie()
    .AddGoogle(g =>
    {
        g.ClientId = googleConfig.ClientId;
        g.ClientSecret = googleConfig.ClientSecret;
        g.Events.OnCreatingTicket = async ctx =>
        {
            
            //var id = ctx.Identity.Claims.Single(x => x.Issuer == "Google" && x.Type == ClaimTypes.NameIdentifier).Value;
            //var db = ctx.HttpContext.RequestServices.GetRequiredService<MyDbContext>();
            //var user = await db.GoogleAccounts.SingleOrDefaultAsync(x => x.Id == id);
            //if (user == null)
            //{
            //    db.GoogleAccounts.Add(new GoogleAccount(ctx.Identity.Claims));
            //    await db.SaveChangesAsync();
            //}
            return;
        };

    }
    );

builder.Services.AddAuthorization();


builder.Services.AddDbContext<MyDbContext>(o => o.UseNpgsql(DbConfig.ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseGrpcWeb( new GrpcWebOptions { DefaultEnabled=true});
app.MapGrpcService<WeatherService>().RequireAuthorization();
app.MapRazorPages();
app.MapControllers().RequireAuthorization();
app.MapFallbackToFile("index.html");

app.Run();
