using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ConsumeAPI.GettingAPI;
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

        public async Task<IActionResult> Index()
        {
            List<Lendet> MyLendets = new List<Lendet>();
            List<Drejtimet> MyDrejtimets = new List<Drejtimet>();

            using (var httpClient = new HttpClient())
            {
                MyLendets = await GetAPI.GetLendetListAsync(httpClient);
                MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);

                //string jsonFormatted = JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
                //XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Lendet \":" + apiResponse + "}", "Lendet");

                //ViewBag.Json = jsonFormatted;
                //ViewBag.XML = xml.OuterXml;
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
                lendet = await GetAPI.GetLendetAsync(httpClient, id);
                Drejtimet drejtimet = await GetAPI.GetDrejtimiAsync(httpClient, lendet.DrejtimiId);

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
            List<Drejtimet> MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);

            ViewData["DrejtimiId"] = new SelectList(MyDrejtimets, "DrejtimetId", "EmriDrejtimit");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Lendet lendet)
        {
            Lendet receivedLenda = new Lendet();
            using var httpClient = new HttpClient();
            if (ModelState.IsValid)
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
            else
            {
                ModelState.AddModelError(string.Empty, "Plotesoni te gjitha format!");
            }

            List<Drejtimet> MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);

            ViewData["DrejtimiId"] = new SelectList(MyDrejtimets, "DrejtimetId", "EmriDrejtimit", lendet.DrejtimiId);

            return View(lendet);
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Lendet lendet = new Lendet();
            using (var httpClient = new HttpClient())
            {
                lendet = await GetAPI.GetLendetAsync(httpClient, id);
                List<Drejtimet> MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);

                ViewData["DrejtimiId"] = new SelectList(MyDrejtimets, "DrejtimetId", "EmriDrejtimit");
            }

            return View(lendet);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Lendet lendet)
        {
            using (var httpClient = new HttpClient())
            {
                if (ModelState.IsValid)
                {
                    using var response = await httpClient.PutAsJsonAsync<Lendet>(getApi + "/" + lendet.LendetId, lendet);
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Result = "Success";

                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Plotesoni te gjitha format!");
                }

                List<Drejtimet> MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);

                ViewData["DrejtimiId"] = new SelectList(MyDrejtimets, "DrejtimetId", "EmriDrejtimit", lendet.DrejtimiId);
            }

            return View();
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
                lendet = await GetAPI.GetLendetAsync(httpClient, id);
                Drejtimet drejtimet = await GetAPI.GetDrejtimiAsync(httpClient, lendet.DrejtimiId);

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
