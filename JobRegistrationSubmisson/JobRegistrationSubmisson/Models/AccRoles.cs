using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Models
{
    [Table("Tb_AccRoles")]
    public class AccRoles : IdentityUserRole<string>
    {
        public int Id { get; set; }
        public Accounts Accounts { get; set; }
        public Roles Roles  { get; set; }
    }
}
