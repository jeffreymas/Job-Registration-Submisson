using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Models
{
    [Table ("Tb_Employees")]
    public class Employees
    {
        [Key]
        public string EmpId { get; set; }
        public string Address { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset UpdateTime { get; set; }
        public DateTimeOffset DeleteTime { get; set; }
        public bool IsDelete { get; set; }

        public virtual User User { get; set; }

    }
}
