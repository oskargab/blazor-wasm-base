using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    public class GoogleAccount :ExternalAccount, IExternalAccount
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public GoogleAccount()
        {

        }
        public GoogleAccount(bool allowLogin, IEnumerable<Claim> claims) 
        {
            AllowLogin = allowLogin;
            ExternalId = claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            Email = claims.Single(x => x.Type == ClaimTypes.Email).Value;
            FirstName = claims.SingleOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            LastName = claims.SingleOrDefault(x=>x.Type == ClaimTypes.Surname).Value;
        }

        public IExternalAccount Init(bool allowLogin, IEnumerable<Claim> claims)
        {
            AllowLogin = allowLogin;
            ExternalId = claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            Email = claims.Single(x => x.Type == ClaimTypes.Email).Value;
            FirstName = claims.SingleOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            LastName = claims.SingleOrDefault(x => x.Type == ClaimTypes.Surname).Value;
            return this;
        }
        public User CreateUser()
        {
            return new User() { GoogleAccount = this };
        }

    }
}
