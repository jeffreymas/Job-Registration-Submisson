using JobRegistrationSubmisson.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Models
{
    [Table("TB_Trans_Joblist")]
    public class Joblist : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateData { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset DeleteData { get; set; }
        public bool isDelete { get; set; }

        //public string JobSeekerId { get; set; }
        //public JobSeeker JobSeeker { get; set; }
    }
}
