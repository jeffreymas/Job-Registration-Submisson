using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.ViewModel
{
    public class AccountVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string RoleID { get; internal set; }
        public string RoleName { get; set; }
        public string VerifyCode { get; set; }

    }
}
