using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JobRegistrationSubmisson.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    public class DashboardController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44303/api/")
        };

        public IActionResult Index()
        {
            if (HttpContext.Session.IsAvailable)
            {
                if (HttpContext.Session.GetString("lvl") == "HR")
                {
                    return View();
                }
                return Redirect("/profile");
            }
            return RedirectToAction("Login", "Auth");
        }

        
        [Route("profile")]
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult InsertOrUpdate(JobSeekerVM jobSeekerVM)
        {
            try
            {
                var json = JsonConvert.SerializeObject(jobSeekerVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //var token = HttpContext.Session.GetString("token");
                //client.DefaultRequestHeaders.Add("Authorization", token);
                if (jobSeekerVM.JobSId.Equals(null))
                {
                    var result = client.PostAsync("JobSeekers", byteContent).Result;
                    return Json(result);
                }
                else if (!jobSeekerVM.JobSId.Equals(null))
                {
                    var result = client.PutAsync("jobSeekers/" + jobSeekerVM.JobSId, byteContent).Result;
                    return Json(result);
                }

                return Json(404);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}