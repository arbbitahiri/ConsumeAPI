using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class RoletController : Controller
    {
        private string getApi = "http://localhost:58559/TblRolets";

        public async Task<IActionResult> Index()
        {
            List<Rolet> MyRolets = new List<Rolet>();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi);
                string apiResponse = await response.Content.ReadAsStringAsync();
                MyRolets = JsonConvert.DeserializeObject<List<Rolet>>(apiResponse);
            }

            return View(MyRolets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting role!");
            }

            Rolet rolet = new Rolet();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                rolet = JsonConvert.DeserializeObject<Rolet>(apiResponse);
            }

            if (rolet == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting role!");
            }

            return View(rolet);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Rolet rolet)
        {
            if (ModelState.IsValid)
            {
                Rolet receivedRolet = new Rolet();
                using(var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(rolet), Encoding.UTF8, "application/json");

                    using var response = await httpClient.PostAsync(getApi, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedRolet = JsonConvert.DeserializeObject<Rolet>(apiResponse);

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
                return View(receivedRolet);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all form!");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Rolet rolet = new Rolet();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                rolet = JsonConvert.DeserializeObject<Rolet>(apiResponse);
            }

            return View(rolet);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Rolet rolet)
        {
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.PutAsJsonAsync<Rolet>(getApi + "/" + rolet.RoletId, rolet);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Result = "Success";

                    return RedirectToAction(nameof(Index));
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> DeleteDetails(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting role!");
            }

            Rolet rolet = new Rolet();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                rolet = JsonConvert.DeserializeObject<Rolet>(apiResponse);
            }

            if (rolet == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting role!");
            }

            return View(rolet);
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
