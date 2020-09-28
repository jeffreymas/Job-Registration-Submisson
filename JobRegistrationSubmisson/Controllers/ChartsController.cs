using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobRegistrationSubmisson.Context;
using JobRegistrationSubmisson.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobRegistrationSubmisson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly MyContext _context;
        public ChartsController(MyContext myContext)
        {
            _context = myContext;
        }
        // GET api/values
        [HttpGet]
        [Route("pie")]
        public async Task<List<PieChartVM>> GetPie()
        {

            var data1 = await _context.JobSeekerLists.Include("Joblist").Include("JobSeeker")
                            .GroupBy(q => q.Joblist.Name)
                            .Select(q => new PieChartVM
                            {
                                JoblistName = q.Key,
                                total = q.Count()
                            }).ToListAsync();
            return data1;
        }


        [HttpGet]
        [Route("jobsApprove")]
        public async Task<List<stsChartVM>> GetPieApprove()
        {

            var data = await _context.JobSeekers
                            .Where(a=> a.Approve == true)
                            .GroupBy(q => q.Approve)
                            .Select(q => new stsChartVM
                            {
                                status = "Approve",
                                total = q.Count()
                            }).ToListAsync();

            return data;
        }

        [HttpGet]
        [Route("jobReject")]
        public async Task<List<stsChartVM>> GetPieReject()
        {

            var data = await _context.JobSeekers
                            .Where(a => a.Reject == true)
                            .GroupBy(q => q.Reject)
                            .Select(q => new stsChartVM
                            {
                                status = "Reject",
                                total = q.Count()
                            }).ToListAsync();
            //var data = ["data1", "data2"];


            return data;
        }

    }
}