using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class ProfesoretController : Controller
    {
        private string getApi = "http://localhost:58559/TblProfesorets";
        private string getLendaApi = "http://localhost:58559/TblLendets";

        public async Task<IActionResult> Index()
        {
            List<Profesoret> MyProfesorets = new List<Profesoret>();
            List<Lendet> MyLendets = new List<Lendet>();

            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi);
                string apiResponse = await response.Content.ReadAsStringAsync();
                MyProfesorets = JsonConvert.DeserializeObject<List<Profesoret>>(apiResponse);

                using var responseLenda = await httpClient.GetAsync(getLendaApi);
                string apiResponseLenda = await responseLenda.Content.ReadAsStringAsync();
                MyLendets = JsonConvert.DeserializeObject<List<Lendet>>(apiResponseLenda);
            }

            foreach (var profesor in MyProfesorets)
            {
                foreach (var lende in MyLendets)
                {
                    if (profesor.LendaId == profesor.LendaId)
                    {
                        profesor.Lenda = lende;
                    }
                }
            }

            return View(MyProfesorets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Profesoret profesoret = new Profesoret();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                profesoret = JsonConvert.DeserializeObject<Profesoret>(apiResponse);

                using var responseLenda = await httpClient.GetAsync(getLendaApi + "/" + profesoret.LendaId);
                string apiResponseLenda = await responseLenda.Content.ReadAsStringAsync();
                Lendet lendet = JsonConvert.DeserializeObject<Lendet>(apiResponseLenda);

                profesoret.Lenda = lendet;
            }

            if (profesoret == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(profesoret);
        }

        public async Task<IActionResult> CreateAsync()
        {
            using var httpClient = new HttpClient();
            using var responseLenda = await httpClient.GetAsync(getLendaApi);
            string apiResponseLenda = await responseLenda.Content.ReadAsStringAsync();
            List<Lendet> MyLendets = JsonConvert.DeserializeObject<List<Lendet>>(apiResponseLenda);

            ViewBag.LendetId = MyLendets;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Profesoret profesoret)
        {
            if (ModelState.IsValid)
            {
                Profesoret receivedPofesori = new Profesoret();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(profesoret), Encoding.UTF8, "application/json");

                    using var response = await httpClient.PostAsync(getApi, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedPofesori = JsonConvert.DeserializeObject<Profesoret>(apiResponse);

                    string success = response.StatusCode.ToString();
                    if (success == "Created")
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ka ndodhur nje gabim gjate regjistrimit te profesorit!");
                    }
                }
                return View(receivedPofesori);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all form!");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            Profesoret profesoret = new Profesoret();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                profesoret = JsonConvert.DeserializeObject<Profesoret>(apiResponse);

                using var responseLenda = await httpClient.GetAsync(getLendaApi);
                string apiResponseLenda = await responseLenda.Content.ReadAsStringAsync();
                List<Lendet> MyLendets = JsonConvert.DeserializeObject<List<Lendet>>(apiResponseLenda);

                ViewBag.LendetId = MyLendets;
            }

            return View(profesoret);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Profesoret profesoret)
        {
            Profesoret receivedPofesori = new Profesoret();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(profesoret.EmriProfesorit), "EmriProfesorit" },
                    { new StringContent(profesoret.LendaId.ToString()), "LendaId" }
                };

                using var response = await httpClient.PostAsync(getApi, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Result = "Success";
                receivedPofesori = JsonConvert.DeserializeObject<Profesoret>(apiResponse);
            }

            return View(receivedPofesori);
        }

        public async Task<IActionResult> DeleteDetails(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Profesoret profesoret = new Profesoret();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                profesoret = JsonConvert.DeserializeObject<Profesoret>(apiResponse);

                using var responseLenda = await httpClient.GetAsync(getLendaApi + "/" + profesoret.LendaId);
                string apiResponseLenda = await responseLenda.Content.ReadAsStringAsync();
                Lendet lendet = JsonConvert.DeserializeObject<Lendet>(apiResponseLenda);

                profesoret.Lenda = lendet;
            }

            if (profesoret == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(profesoret);
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
