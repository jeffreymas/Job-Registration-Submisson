using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JobRegistrationSubmisson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebClient.Controllers
{
    public class ManageJoblistController : Controller
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
                    return View("~/Views/Joblists/ManageJoblist.cshtml");
                }
                return Redirect("/Error");
            }
            return Redirect("/Error");
            
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
            HttpContext.Session.SetInt32("joblists", Id);
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

        public IActionResult InsertOrUpdate(Joblist joblist, int id)
        {
            try
            {
                var json = JsonConvert.SerializeObject(joblist);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //var token = HttpContext.Session.GetString("token");
                //client.DefaultRequestHeaders.Add("Authorization", token);
                if (joblist.Id == 0)
                {
                    var result = client.PostAsync("joblists", byteContent).Result;
                    return Json(result);
                }
                else if (joblist.Id == id)
                {
                    var result = client.PutAsync("joblists/" + id, byteContent).Result;
                    return Json(result);
                }

                return Json(404);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Delete(int id)
        {
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var result = client.DeleteAsync("joblists/" + id).Result;
            return Json(result);
        }
    }
}