using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Models
{
    [Table("Tb_M_Role")]
    public class Role : IdentityRole
    {

    }
}
