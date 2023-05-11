using DbRepository;
using DbRepository.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorApp1.Server
{
    public static class tmpClass
    {
        public static async Task AddExternalAccountToDb<T>(MyDbContext db, OAuthCreatingTicketContext ctx) where T : class, IExternalAccount, new()
        {
            var externalId = ctx.Identity.FindFirst(ClaimTypes.NameIdentifier);

            var getUser = db.Find<T>(externalId.Value);
            string userId;
            if (getUser == null)
            {
                var newUser = new T().Init(true, ctx.Identity.Claims).CreateUser();
                db.Users.Add(newUser);
                await db.SaveChangesAsync();
                userId = newUser.Id.ToString();
            }
            else
            {
                db.Entry(getUser).Reference(x => x.User).Load();

                userId = getUser.User.Id.ToString();
            }

            await AddClaim(ctx, userId);
            return;
        }
        private static async Task AddClaim(OAuthCreatingTicketContext ctx, string userId)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ctx.Principal.AddIdentity(claimsIdentity);
            return;
        }
    }
}
