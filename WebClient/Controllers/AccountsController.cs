﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    public class AccountsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44303/api/")
        };

        [Route("login")]
        public IActionResult Login()
        {
            return View("~/Views/Accounts/Login.cshtml");
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            //var jwtauthmanager = new IJwtAuthManager();
            //var userName = User.Identity.Name;
            //_jwtAuthManager.RemoveRefreshTokenByUserName(userName); // can be more specific to ip, user agent, device name, etc.
            //_logger.LogInformation($"User [{userName}] logged out the system.");

            //HttpContext.Session.Remove("lvl");
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [Route("verify")]
        public IActionResult Verify()
        {
            return View();
        }

        [Route("notfound")]
        public IActionResult Notfound()
        {
            return View();
        }

        [Route("validate")]
        public IActionResult Validate(UserVM userVM)
        {
            var json = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (userVM.Username == null)
            { // Login
                HttpResponseMessage result = null;
                if (userVM.VerifyCode != null)
                {
                    result = client.PostAsync("users/code/", byteContent).Result;
                }
                else if (userVM.VerifyCode == null)
                {
                    result = client.PostAsync("users/login/", byteContent).Result;
                }

                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    if (data != null)
                    {
                        HttpContext.Session.SetString("token", "Bearer " + data);
                        var handler = new JwtSecurityTokenHandler();
                        var tokenS = handler.ReadJwtToken(data);
                        var jwtPayloadSer = JsonConvert.SerializeObject(tokenS.Payload.ToDictionary(x => x.Key, x => x.Value));
                        var jwtPayloadDes = JsonConvert.DeserializeObject(jwtPayloadSer).ToString();
                        var account = JsonConvert.DeserializeObject<UserVM>(jwtPayloadSer);

                        if (!account.VerifyCode.Equals(""))
                        {
                            return Json(new { status = true, msg = "VerifyCode" });
                        }
                        else if (account.RoleName != null)
                        {
                            HttpContext.Session.SetString("id", account.Id);
                            HttpContext.Session.SetString("uname", account.Username);
                            HttpContext.Session.SetString("email", account.Email);
                            HttpContext.Session.SetString("lvl", account.RoleName);
                            //HttpContext.Session.SetInt32("joblist", account.Joblists);
                            if (account.RoleName == "HR")
                            {
                                return Json(new { status = true, msg = "Login Successfully !" });
                                //return View("~/Views/Auth/verify.cshtml");
                            }
                            return Json(new { status = true, msg = "Login Successfully !" });
                        }
                        return Json(new { status = false, msg = "You Don't Have Permissions! Please Contact Administrator" });
                    }
                    return Json(new { status = false, msg = result.Content.ReadAsStringAsync().Result });
                }
                return Json(new { status = false, msg = result.Content.ReadAsStringAsync().Result });
            }
            else if (userVM.Username != null)
            { // Register
                var result = client.PostAsync("users/register/", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(new { status = true, code = result, msg = "Register Success! " });
                }
                return Json(new { status = false, msg = result.Content.ReadAsStringAsync().Result });
            }
            return Redirect("/login" +
                "");
        }
        

        [Route("tes")]
        public IActionResult Tes()
        {
            var stream = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6ImQ3NzcwODVkLTQ0ODctNGM4NS1hMDFkLTgyZGJlNzVhZjY1MyIsIlVzZXJuYW1lIjoiSmVwcmkgSmVwcmkiLCJFbWFpbCI6ImplcHJpLnR1Z2FzQGdtYWlsLmNvbSIsIlJvbGVOYW1lIjoiSFIiLCJWZXJpZnlDb2RlIjoiMDk2NCIsImV4cCI6MTYwMDMzODAwNCwiaXNzIjoiSm9iUmVnaXN0cmF0aW9uU3VibWlzc29uU2VydmVyIiwiYXVkIjoiSm9iUmVnaXN0cmF0aW9uU3VibWlzc29uU2VydmVyIn0.znQKkxq0QJ0NnCHPniJLoUXqx3TCU3iq_vykpZglNAM";
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadJwtToken(stream);

            var jwtPayloadSer = JsonConvert.SerializeObject(tokenS.Payload.ToDictionary(x => x.Key, x => x.Value));
            var jwtPayloadDes = JsonConvert.DeserializeObject(jwtPayloadSer).ToString();
            var account = JsonConvert.DeserializeObject<UserVM>(jwtPayloadSer);

            // Output the whole thing to pretty Json object formatted.
            return Json(new { account.Id, account.Username, account.Email, account.RoleName, account.VerifyCode });
        }

    }
}