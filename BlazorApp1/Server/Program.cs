using BlazorApp1.Server;
using BlazorApp1.Server.GrpcServices;
using BlazorApp1.Server.ModelBinders;
using DbRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
var discordConfig = config.GetSection(DiscordAuthConfig.SectionName).Get<DiscordAuthConfig>();

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
    .AddOptions<GoogleAuthConfig>()
    .Bind(config.GetSection(DiscordAuthConfig.SectionName))
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


            var db = ctx.HttpContext.RequestServices.GetRequiredService<MyDbContext>();

            await tmpClass.AddExternalAccountToDb<GoogleAccount>(db, ctx);
            //var getUser = db.DiscordAccounts.SingleOrDefaultAsync(x => x.UserId == discordId.Value);
            //string userId;
            //if (getUser == null)
            //{
            //    var newUser = new User()
            //    {
            //        DiscordAccount = new DiscordAccount(true, ctx.Identity.Claims)
            //    };
            //    db.Users.Add(newUser);
            //    await db.SaveChangesAsync();
            //    userId = newUser.Id.ToString();
            //}
            //else
            //{
            //    userId = getUser.Id.ToString();
            //}
            //var claims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, userId)
            //    };

            //var claimsIdentity = new ClaimsIdentity(
            //    claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            //ctx.Principal.AddIdentity(claimsIdentity);
            return;
        };

    }
    ).AddOAuth("discord",
     x =>
     {
         x.ClientId = discordConfig.ClientId;
         x.ClientSecret = discordConfig.ClientSecret;
         x.Scope.Add("identify");
         x.Scope.Add("email");
         x.CallbackPath = new PathString("/auth/discordCallback");

         x.AuthorizationEndpoint = "https://discord.com/oauth2/authorize";
         x.TokenEndpoint = "https://discord.com/api/oauth2/token";
         x.UserInformationEndpoint = "https://discord.com/api/users/@me";

         x.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
         x.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
         x.ClaimActions.MapJsonKey("discriminator", "discriminator");
         x.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
         x.ClaimActions.MapJsonKey("verified", "verified");



         x.Events.OnCreatingTicket = async ctx =>
         {
             var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
             request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
             var response = await ctx.Backchannel.SendAsync(request,
                                HttpCompletionOption.ResponseHeadersRead,
                                ctx.HttpContext.RequestAborted);
             response.EnsureSuccessStatusCode();
             var user = await response.Content.ReadFromJsonAsync<JsonDocument>();
             ctx.RunClaimActions(user.RootElement);

             var db = ctx.HttpContext.RequestServices.GetRequiredService<MyDbContext>();

             await tmpClass.AddExternalAccountToDb<DiscordAccount>(db, ctx);

             //var discordId = ctx.Identity.FindFirst(ClaimTypes.NameIdentifier);

             //var db = ctx.HttpContext.RequestServices.GetRequiredService<MyDbContext>();
             //var getUser = db.DiscordAccounts.SingleOrDefaultAsync(x => x.ExternalId == discordId.Value);
             //string userId;
             //if(getUser == null)
             //{
             //    var newUser = new User()
             //    {
             //        DiscordAccount = new DiscordAccount(true, ctx.Identity.Claims)
             //    };
             //    db.Users.Add(newUser);
             //    await db.SaveChangesAsync();
             //    userId = newUser.Id.ToString();
             //}
             //else
             //{
             //    userId = getUser.Id.ToString();
             //}
             //var claims = new List<Claim>
             //   {
             //       new Claim(ClaimTypes.NameIdentifier, userId)
             //   };

             //var claimsIdentity = new ClaimsIdentity(
             //    claims, CookieAuthenticationDefaults.AuthenticationScheme);

             //ctx.Principal.AddIdentity(claimsIdentity);
             return;

         };
     }
     ); ;

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
