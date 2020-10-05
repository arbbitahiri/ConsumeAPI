using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
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
                using var response = await httpClient.GetAsync(getApi);
                string apiResponse = await response.Content.ReadAsStringAsync();
                MyDrejtimets = JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponse);

                string jsonFormatted = JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
                XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Drejtimet \":" + apiResponse + "}", "Drejtimet");

                ViewBag.Json = jsonFormatted;
                ViewBag.XML = xml.OuterXml;
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
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                drejtimet = JsonConvert.DeserializeObject<Drejtimet>(apiResponse);
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
                ModelState.AddModelError(string.Empty, "Please fill all form!");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Drejtimet drejtimet = new Drejtimet();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                drejtimet = JsonConvert.DeserializeObject<Drejtimet>(apiResponse);
            }

            return View(drejtimet);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Drejtimet drejtimet)
        {
            Drejtimet receivedDrejtimi = new Drejtimet();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(drejtimet.EmriDrejtimit), "EmriDrejtimit" },
                    { new StringContent(drejtimet.Koment), "Koment" }
                };

                using var response = await httpClient.PostAsync(getApi, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Result = "Success";
                receivedDrejtimi = JsonConvert.DeserializeObject<Drejtimet>(apiResponse);
            }

            return View(receivedDrejtimi);
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
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                drejtimet = JsonConvert.DeserializeObject<Drejtimet>(apiResponse);
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
