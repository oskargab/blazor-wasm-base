using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Users
{
    public class DiscordAccount : ExternalAccount, IExternalAccount
    {
        public string? Username { get; set; }
        public int? Discriminator { get; set; }
        public DiscordAccount() { }
        public DiscordAccount(bool allowLogin, IEnumerable<Claim> claims)
        {
            AllowLogin = allowLogin;
            ExternalId = claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            Username = claims.Single(x => x.Type == ClaimTypes.Email).Value;
            Discriminator = int.Parse(claims.SingleOrDefault(x => x.Type == "discriminator").Value);
        }
        public IExternalAccount Init(bool allowLogin, IEnumerable<Claim> claims)
        {
            AllowLogin = allowLogin;
            ExternalId = claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            Username = claims.Single(x => x.Type == ClaimTypes.Email).Value;
            Discriminator = int.Parse(claims.SingleOrDefault(x => x.Type == "discriminator").Value);
            return this;
        }

        public User CreateUser()
        {
            return new User() { DiscordAccount = this };
        }
    }
}
