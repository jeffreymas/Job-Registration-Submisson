using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Models
{
    [Table("Tb_M_JobSeekerList")]
    public class JobSeekerList
    {
        public string JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }

        public int JoblistId { get; set; }
        public Joblist Joblist { get; set; }
    }
}
