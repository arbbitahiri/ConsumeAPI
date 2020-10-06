using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsumeAPI.GettingAPI;
using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class UsersController : Controller
    {
        private string getApi = "http://localhost:58559/TblUsers";

        public async Task<IActionResult> IndexAsync()
        {
            List<Users> MyUsers = new List<Users>();
            List<Rolet> MyRolets = new List<Rolet>();

            using (var httpClient = new HttpClient())
            {
                MyUsers = await GetAPI.GetUserListAsync(httpClient);
                MyRolets = await GetAPI.GetRoletListAsync(httpClient);
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
                user = await GetAPI.GetUserAsync(httpClient, id);
                Rolet rolet = await GetAPI.GetRoletAsync(httpClient, id);

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
            List<Rolet> MyRolets = await GetAPI.GetRoletListAsync(httpClient);

            ViewData["RoleId"] = new SelectList(MyRolets, "RoletId", "RoleName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Users user)
        {
            Users receivedUser = new Users();
            using (var httpClient = new HttpClient())
            {
                if (ModelState.IsValid)
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
                        ModelState.AddModelError(string.Empty, "Ka ndodhur nje gabim!");
                    }

                    List<Rolet> MyRolets = await GetAPI.GetRoletListAsync(httpClient);

                    ViewData["RoleId"] = new SelectList(MyRolets, "RoletId", "RoleName", user.RoleId);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Plotesoni te gjitha fushat!");
                }
            }

            return View(user);
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Users user = new Users();
            using (var httpClient = new HttpClient())
            {
                user = await GetAPI.GetUserAsync(httpClient, id);
                List<Rolet> MyRolets = await GetAPI.GetRoletListAsync(httpClient);

                ViewData["RoleId"] = new SelectList(MyRolets, "RoletId", "RoleName");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Users user)
        {
            using (var httpClient = new HttpClient())
            {
                if (ModelState.IsValid)
                {
                    using var response = await httpClient.PutAsJsonAsync<Users>(getApi + "/" + user.UsersId, user);
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Result = "Success";

                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Plotesoni te gjitha fushat!");
                }

                List<Rolet> MyRolets = await GetAPI.GetRoletListAsync(httpClient);

                ViewData["RoleId"] = new SelectList(MyRolets, "RoletId", "RoleName", user.RoleId);
            }

            return View();
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
                user = await GetAPI.GetUserAsync(httpClient, id);
                Rolet rolet = await GetAPI.GetRoletAsync(httpClient, id);

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
