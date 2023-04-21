using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    public class GoogleAccount :ExternalAccount
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GoogleAccount(IEnumerable<Claim> claims) 
        {
            Id = claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            Email = claims.Single(x => x.Type == ClaimTypes.Email).Value;
            FirstName = claims.SingleOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            LastName = claims.SingleOrDefault(x=>x.Type == ClaimTypes.Surname).Value;
        }
        
    }
}
