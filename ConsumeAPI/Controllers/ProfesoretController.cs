using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ConsumeAPI.GettingAPI;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConsumeAPI.Controllers
{
    public class ProfesoretController : Controller
    {
        private string getApi = "http://localhost:58559/TblProfesorets";

        public async Task<IActionResult> Index()
        {
            List<Profesoret> MyProfesorets = new List<Profesoret>();
            List<Lendet> MyLendets = new List<Lendet>();

            using (var httpClient = new HttpClient())
            {
                MyProfesorets = await GetAPI.GetProfesoretListAsync(httpClient);
                MyLendets = await GetAPI.GetLendetListAsync(httpClient);
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
                profesoret = await GetAPI.GetProfesoretAsync(httpClient, id);
                Lendet lendet = await GetAPI.GetLendetAsync(httpClient, profesoret.LendaId);

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
            List<Lendet> MyLendets = await GetAPI.GetLendetListAsync(httpClient);

            ViewData["LendaId"] = new SelectList(MyLendets, "LendetId", "EmriLendes");

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

                    List<Lendet> MyLendets = await GetAPI.GetLendetListAsync(httpClient);

                    ViewData["LendaId"] = new SelectList(MyLendets, "LendetId", "EmriLendes", profesoret.LendaId);
                }
                return View(receivedPofesori);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all form!");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Profesoret profesoret = new Profesoret();
            using (var httpClient = new HttpClient())
            {
                profesoret = await GetAPI.GetProfesoretAsync(httpClient, id);
                List<Lendet> MyLendets = await GetAPI.GetLendetListAsync(httpClient);

                ViewData["LendaId"] = new SelectList(MyLendets, "LendetId", "EmriLendes");
            }

            return View(profesoret);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Profesoret profesoret)
        {
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.PutAsJsonAsync<Profesoret>(getApi + "/" + profesoret.ProfesoretId, profesoret);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Result = "Success";

                    return RedirectToAction(nameof(Index));
                }

                List<Lendet> MyLendets = await GetAPI.GetLendetListAsync(httpClient);

                ViewData["LendaId"] = new SelectList(MyLendets, "LendetId", "EmriLendes", profesoret.LendaId);
            }

            return View();
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
                profesoret = await GetAPI.GetProfesoretAsync(httpClient, id);
                Lendet lendet = await GetAPI.GetLendetAsync(httpClient, profesoret.LendaId);

                profesoret.Lenda = lendet;

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
