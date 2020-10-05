using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class UsersController : Controller
    {
        private string getApi = "http://localhost:58559/TblUsers";
        private string getRoleApi = "http://localhost:58559/TblRolets";

        public async Task<IActionResult> IndexAsync()
        {
            List<Users> MyUsers = new List<Users>();
            List<Rolet> MyRolets = new List<Rolet>();

            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi);
                string apiResponse = await response.Content.ReadAsStringAsync();
                MyUsers = JsonConvert.DeserializeObject<List<Users>>(apiResponse);

                using var responseRolet = await httpClient.GetAsync(getRoleApi);
                string apiResponseRolet = await responseRolet.Content.ReadAsStringAsync();
                MyRolets = JsonConvert.DeserializeObject<List<Rolet>>(apiResponseRolet);
            }

            foreach (var user in MyUsers)
            {
                foreach (var role in MyRolets)
                {
                    if (user.RoleId == role.RoletId)
                    {
                        user.Role = role;
                    }
                }
            }

            return View(MyUsers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting user!");
            }

            Users user = new Users();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<Users>(apiResponse);

                using var responseRolet = await httpClient.GetAsync(getRoleApi + "/" + user.RoleId);
                string apiResponseRolet = await responseRolet.Content.ReadAsStringAsync();
                Rolet rolet = JsonConvert.DeserializeObject<Rolet>(apiResponseRolet);

                user.Role = rolet;
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting user!");
            }

            return View(user);
        }

        public async Task<IActionResult> CreateAsync()
        {
            using var httpClient = new HttpClient();
            using var responseRolet = await httpClient.GetAsync(getRoleApi);
            string apiResponseRolet = await responseRolet.Content.ReadAsStringAsync();
            List<Rolet> MyRolets = JsonConvert.DeserializeObject<List<Rolet>>(apiResponseRolet);

            ViewBag.RoletId = MyRolets;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Users user)
        {
            if (ModelState.IsValid)
            {
                Users receivedUser = new Users();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                    using var response = await httpClient.PostAsync(getApi, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedUser = JsonConvert.DeserializeObject<Users>(apiResponse);

                    string success = response.StatusCode.ToString();
                    if (success == "Created")
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "There was an error while registering role!");
                    }
                }
                return View(receivedUser);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all form!");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Users user = new Users();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<Users>(apiResponse);
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Users user)
        {
            Users receivedUser = new Users();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(user.Username), "Username" }
                };

                using var response = await httpClient.PostAsync(getApi, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Result = "Success";
                receivedUser = JsonConvert.DeserializeObject<Users>(apiResponse);
            }

            return View(receivedUser);
        }

        public async Task<IActionResult> DeleteDetails(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting user!");
            }

            Users user = new Users();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<Users>(apiResponse);

                using var responseRolet = await httpClient.GetAsync(getRoleApi + "/" + user.RoleId);
                string apiResponseRolet = await responseRolet.Content.ReadAsStringAsync();
                Rolet rolet = JsonConvert.DeserializeObject<Rolet>(apiResponseRolet);

                user.Role = rolet;
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting user!");
            }

            return View(user);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using var response = await httpClient.DeleteAsync(getApi + "/" + id.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
