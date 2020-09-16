using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Models
{
    [Table("Tb_M_UserRole")]
    public class UserRole : IdentityUserRole<string>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
