using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    public interface IExternalAccount
    {
        public bool AllowLogin { get; set; }
        public string ExternalId { get; set; }
        public IExternalAccount Init(bool allowLogin, IEnumerable<Claim>claims);
        public User CreateUser();
    }
    public abstract class ExternalAccount
    {
        [Key]
        public string ExternalId { get; set; }
        public bool AllowLogin { get; set; }
    }
}
