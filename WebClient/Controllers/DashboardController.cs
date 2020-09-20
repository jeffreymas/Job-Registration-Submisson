using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClosedXML.Excel;
using JobRegistrationSubmisson.Models;
using JobRegistrationSubmisson.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClient.PDF;

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
                    return View("~/Views/Employees/Index.cshtml");
                }
                return Redirect("/profile");
            }
            return Redirect("/Error");
        }

        
        [Route("profile")]
        public IActionResult Profile()
        {            
            if (HttpContext.Session.IsAvailable)
            {
                if (HttpContext.Session.GetString("lvl") == "HR" || HttpContext.Session.GetString("lvl") == "JobSeeker")
                {
                    return View();
                }
                return Redirect("/");
            }
            return Redirect("/Error");
        }

        [Route("Error")]
        public IActionResult Notfound()
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

        public async Task<IActionResult> Excel()
        {
            List<JobSeekerVM> departments = new List<JobSeekerVM>();
            HttpResponseMessage resView = await client.GetAsync("JobSeekers");
            var resultView = resView.Content.ReadAsStringAsync().Result;
            departments = JsonConvert.DeserializeObject<List<JobSeekerVM>>(resultView);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("JobSeekers");
                var currentRow = 1;
                var number = 0;
                worksheet.Cell(currentRow, 1).Value = "No";
                worksheet.Cell(currentRow, 2).Value = "Department Id";
                worksheet.Cell(currentRow, 3).Value = "Department Name";

                foreach (var dep in departments)
                {
                    currentRow++;
                    number++;
                    worksheet.Cell(currentRow, 1).Value = number;
                    worksheet.Cell(currentRow, 2).Value = dep.Name;
                    worksheet.Cell(currentRow, 3).Value = dep.JoblistName;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var conten = stream.ToArray();
                    return File(conten, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "JobSeeker_List.xlsx");
                }
            }
        }
        public ActionResult ExportPdf()
        {
            cvPDF departmentPdf = new cvPDF();
            List<JobSeekerVM> departments = new List<JobSeekerVM>();

            var resTask = client.GetAsync("jobseekers");
            resTask.Wait();
            var result = resTask.Result;

            var readTask = result.Content.ReadAsAsync<List<JobSeekerVM>>();
            readTask.Wait();
            departments = readTask.Result;

            byte[] abytes = departmentPdf.Prepare(departments);
            return File(abytes, "application/pdf");
        }
    }
}