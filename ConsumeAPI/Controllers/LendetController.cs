using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsumeAPI.Controllers
{
    public class LendetController : Controller
    {
        private string getApi = "http://localhost:58559/TblLendets";
        private string getDrejtimetApi = "http://localhost:58559/TblDrejtimets";

        public async Task<IActionResult> Index()
        {
            List<Lendet> MyLendets = new List<Lendet>();
            List<Drejtimet> MyDrejtimets = new List<Drejtimet>();

            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi);
                string apiResponse = await response.Content.ReadAsStringAsync();
                MyLendets = JsonConvert.DeserializeObject<List<Lendet>>(apiResponse);

                using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi);
                string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                MyDrejtimets = JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponseDrejtimet);

                string jsonFormatted = JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
                XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Lendet \":" + apiResponse + "}", "Lendet");

                ViewBag.Json = jsonFormatted;
                ViewBag.XML = xml.OuterXml;
            }

            foreach (var lende in MyLendets)
            {
                foreach (var drejtim in MyDrejtimets)
                {
                    if (lende.DrejtimiId == drejtim.DrejtimetId)
                    {
                        lende.Drejtimi = drejtim;
                    }
                }
            }

            return View(MyLendets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Lendet lendet = new Lendet();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                lendet = JsonConvert.DeserializeObject<Lendet>(apiResponse);

                using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi + "/" + lendet.DrejtimiId);
                string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                Drejtimet drejtimet = JsonConvert.DeserializeObject<Drejtimet>(apiResponseDrejtimet);

                lendet.Drejtimi = drejtimet;
            }

            if (lendet == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(lendet);
        }

        public async Task<IActionResult> CreateAsync()
        {
            using var httpClient = new HttpClient();
            using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi);
            string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
            List<Drejtimet> MyDrejtimets = JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponseDrejtimet);

            ViewBag.DrejtimiId = MyDrejtimets;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Lendet lendet)
        {
            if (ModelState.IsValid)
            {
                Lendet receivedLenda = new Lendet();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(lendet), Encoding.UTF8, "application/json");

                    using var response = await httpClient.PostAsync(getApi, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedLenda = JsonConvert.DeserializeObject<Lendet>(apiResponse);

                    string success = response.StatusCode.ToString();
                    if (success == "Created")
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ka ndodhur nje gabim gjate regjistrimit te lendes!");
                    }
                }
                return View(receivedLenda);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all form!");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            Lendet lendet = new Lendet();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                lendet = JsonConvert.DeserializeObject<Lendet>(apiResponse);

                using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi);
                string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                List<Drejtimet> MyDrejtimets = JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponseDrejtimet);

                ViewBag.DrejtimiId = MyDrejtimets;
            }

            return View(lendet);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Lendet lendet)
        {
            Lendet receivedLenda = new Lendet();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(lendet.EmriLendes), "EmriLendes" },
                    { new StringContent(lendet.Semestri.ToString()), "Semestri" },
                    { new StringContent(lendet.DrejtimiId.ToString()), "DrejtimiId" }
                };

                using var response = await httpClient.PostAsync(getApi, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Result = "Success";
                receivedLenda = JsonConvert.DeserializeObject<Lendet>(apiResponse);
            }

            return View(receivedLenda);
        }

        public async Task<IActionResult> DeleteDetails(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Lendet lendet = new Lendet();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                lendet = JsonConvert.DeserializeObject<Lendet>(apiResponse);

                using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi + "/" + lendet.DrejtimiId);
                string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                Drejtimet drejtimet = JsonConvert.DeserializeObject<Drejtimet>(apiResponseDrejtimet);

                lendet.Drejtimi = drejtimet;
            }

            if (lendet == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(lendet);
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
