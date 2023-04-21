using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    public class User
    {
        public Guid Id { get; set; }
        public InternalAccount? InternalAccount {get;set;}
        public GoogleAccount? GoogleAccount { get; set; }
    }
}
