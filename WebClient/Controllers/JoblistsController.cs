using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JobRegistrationSubmisson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    public class JoblistsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44303/api/")
        };


        public IActionResult Index()
        {
            return View("~/Views/Joblists/ListJobs.cshtml");
        }

        public IActionResult LoadJoblist()
        {
            IEnumerable<Joblist> joblists = null;
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("joblists");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Joblist>>();
                readTask.Wait();
                joblists = readTask.Result;
            }
            else
            {
                joblists = Enumerable.Empty<Joblist>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(joblists);

        }

        public IActionResult GetById(int Id)
        {
            Joblist joblist = null;
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("joblists/" + Id);
            resTask.Wait();
            HttpContext.Session.SetInt32("joblist", Id);
            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                joblist = JsonConvert.DeserializeObject<Joblist>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(joblist);
        }
    }
}