﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.ViewModel
{
    public class UserVM
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }
        public string VerifyCode { get; set; }
        public int Joblists { get; set; }
        public string JoblistName { get; set; }
    }
}
