using Microsoft.AspNetCore.Identity;
using System;

namespace Ejournal.Identity.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public bool AccountConfirmed { get; set; }
    }
}
