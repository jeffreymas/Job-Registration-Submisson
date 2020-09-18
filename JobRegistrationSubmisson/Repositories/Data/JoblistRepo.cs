using JobRegistrationSubmisson.Context;
using JobRegistrationSubmisson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobRegistrationSubmisson.Repositories.Data
{
    public class JoblistRepo : GeneralRepo<Joblist, MyContext>
    {
        public JoblistRepo(MyContext context) : base(context)
        {

        }

    }
}
