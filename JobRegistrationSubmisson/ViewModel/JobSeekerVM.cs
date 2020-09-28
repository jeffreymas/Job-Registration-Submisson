using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.ViewModel
{
    public class JobSeekerVM
    {
        public string JobSId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public DateTime Birth_Date { get; set; }
        public string Nationality { get; set; }
        public string Marital_Status { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string Last_Education { get; set; }
        public string GPA { get; set; }
        public string Technical_Skill { get; set; }
        public string Experience { get; set; }
        public string Achievement { get; set; }
        public DateTimeOffset RegistDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset RejectDate { get; set; }
        public bool Approve { get; set; }
        public bool Reject { get; set; }
        public int JoblistId { get; set; }
        public string JoblistName { get; set; }

    }
}
