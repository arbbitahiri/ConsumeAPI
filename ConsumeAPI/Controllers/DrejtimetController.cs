using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ConsumeAPI.GettingAPI;
using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsumeAPI.Controllers
{
    public class DrejtimetController : Controller
    {
        private string getApi = "http://localhost:58559/TblDrejtimets";

        public async Task<IActionResult> Index()
        {
            List<Drejtimet> MyDrejtimets = new List<Drejtimet>();
            using (var httpClient = new HttpClient())
            {
                MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);

                ViewBag.Json = await GetJsonXml.GetJsonDrejtimet(httpClient);
                ViewBag.Xml = await GetJsonXml.GetXmlDrejtimet(httpClient);
            }

            return View(MyDrejtimets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Drejtimet drejtimet = new Drejtimet();
            using (var httpClient = new HttpClient())
            {
                drejtimet = await GetAPI.GetDrejtimiAsync(httpClient, id);
            }

            if (drejtimet == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(drejtimet);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Drejtimet drejtimet)
        {
            if (ModelState.IsValid)
            {
                Drejtimet receivedDrejtimi = new Drejtimet();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(drejtimet), Encoding.UTF8, "application/json");

                    using var response = await httpClient.PostAsync(getApi, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedDrejtimi = JsonConvert.DeserializeObject<Drejtimet>(apiResponse);

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
                return View(receivedDrejtimi);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Plotesoni te gjitha fushat!");
            }
            return View(drejtimet);
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Drejtimet drejtimet = new Drejtimet();
            using (var httpClient = new HttpClient())
            {
                drejtimet = await GetAPI.GetDrejtimiAsync(httpClient, id);
            }

            return View(drejtimet);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Drejtimet drejtimet)
        {
            if (ModelState.IsValid)
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.PutAsJsonAsync<Drejtimet>(getApi + "/" + drejtimet.DrejtimetId, drejtimet);
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
            return View(drejtimet);
        }

        public async Task<IActionResult> DeleteDetails(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Drejtimet drejtimet = new Drejtimet();
            using (var httpClient = new HttpClient())
            {
                drejtimet = await GetAPI.GetDrejtimiAsync(httpClient, id);
            }

            if (drejtimet == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(drejtimet);
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
