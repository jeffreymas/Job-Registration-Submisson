using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Models
{
    [Table("TB_M_User")]
    public class User : IdentityUser
    {
        public ICollection<UserRole> userRoles { get; set; }
    }
}
