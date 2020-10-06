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
    public class ProvimetController : Controller
    {
        private string getApi = "http://localhost:58559/TblProvimets";

        public async Task<IActionResult> Index()
        {
            List<Provimet> MyProvimets = new List<Provimet>();
            List<Lendet> MyLendets = new List<Lendet>();
            List<Studenti> MyStudentis = new List<Studenti>();
            List<Profesoret> MyProfesorets = new List<Profesoret>();

            using (var httpClient = new HttpClient())
            {
                MyProvimets = await GetAPI.GetProvimetListAsync(httpClient);
                MyLendets = await GetAPI.GetLendetListAsync(httpClient);
                MyStudentis = await GetAPI.GetStudentiListAsync(httpClient);
                MyProfesorets = await GetAPI.GetProfesoretListAsync(httpClient);
            }

            foreach (var provim in MyProvimets)
            {
                foreach (var lenda in MyLendets)
                {
                    if (provim.LendaId == lenda.LendetId)
                    {
                        provim.Lenda = lenda;
                    }
                }

                foreach (var student in MyStudentis)
                {
                    if (provim.StudentiId == student.StudentId)
                    {
                        provim.Studenti = student;
                    }
                }

                foreach (var prof in MyProfesorets)
                {
                    if (provim.ProfesoriId == prof.ProfesoretId)
                    {
                        provim.Profesori = prof;
                    }
                }
            }

            return View(MyProvimets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Provimet provimi = new Provimet();
            using (var httpClient = new HttpClient())
            {
                provimi = await GetAPI.GetProvimetAsync(httpClient, id);
                Lendet lendet = await GetAPI.GetLendetAsync(httpClient, provimi.LendaId);
                Studenti studenti = await GetAPI.GetStudentiAsync(httpClient, provimi.StudentiId);
                Profesoret profesoret = await GetAPI.GetProfesoretAsync(httpClient, provimi.ProfesoriId);

                provimi.Lenda = lendet;
                provimi.Studenti = studenti;
                provimi.Profesori = profesoret;
            }

            if (provimi == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(provimi);
        }

        public async Task<IActionResult> CreateAsync()
        {
            using var httpClient = new HttpClient();
            List<Lendet> MyLendets = await GetAPI.GetLendetListAsync(httpClient);
            List<Studenti> MyStudentis = await GetAPI.GetStudentiListAsync(httpClient);
            List<Profesoret> MyProfesorets = await GetAPI.GetProfesoretListAsync(httpClient);

            ViewData["LendaId"] = new SelectList(MyLendets, "LendetId", "EmriLendes");
            ViewData["StudentiId"] = new SelectList(MyStudentis, "StudentId", "FullName");
            ViewData["ProfesoriId"] = new SelectList(MyProfesorets, "ProfesoretId", "EmriProfesorit");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Provimet provimet)
        {
            Provimet receivedProvimi = new Provimet();
            using (var httpClient = new HttpClient())
            {
                if (ModelState.IsValid)
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(provimet), Encoding.UTF8, "application/json");

                    using var response = await httpClient.PostAsync(getApi, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedProvimi = JsonConvert.DeserializeObject<Provimet>(apiResponse);

                    string success = response.StatusCode.ToString();
                    if (success == "Created")
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ka ndodhur nje gabim gjate regjistrimit te provimit!");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Plotesoni te gjitha fushat!");
                }

                List<Lendet> MyLendets = await GetAPI.GetLendetListAsync(httpClient);
                List<Studenti> MyStudentis = await GetAPI.GetStudentiListAsync(httpClient);
                List<Profesoret> MyProfesorets = await GetAPI.GetProfesoretListAsync(httpClient);

                ViewData["LendaId"] = new SelectList(MyLendets, "LendetId", "EmriLendes", provimet.LendaId);
                ViewData["StudentiId"] = new SelectList(MyStudentis, "StudentId", "FullName", provimet.StudentiId);
                ViewData["ProfesoriId"] = new SelectList(MyProfesorets, "ProfesoretId", "EmriProfesorit", provimet.ProfesoriId);
            }

            return View(provimet);
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Provimet provimi = new Provimet();
            using (var httpClient = new HttpClient())
            {
                provimi = await GetAPI.GetProvimetAsync(httpClient, id);
                List<Lendet> MyLendets = await GetAPI.GetLendetListAsync(httpClient);
                List<Studenti> MyStudentis = await GetAPI.GetStudentiListAsync(httpClient);
                List<Profesoret> MyProfesorets = await GetAPI.GetProfesoretListAsync(httpClient);

                ViewData["LendaId"] = new SelectList(MyLendets, "LendetId", "EmriLendes");
                ViewData["StudentiId"] = new SelectList(MyStudentis, "StudentId", "FullName");
                ViewData["ProfesoriId"] = new SelectList(MyProfesorets, "ProfesoretId", "EmriProfesorit");
            }

            return View(provimi);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Provimet provimet)
        {
            using (var httpClient = new HttpClient())
            {
                if (ModelState.IsValid)
                {
                    using var response = await httpClient.PutAsJsonAsync<Provimet>(getApi + "/" + provimet.ProvimetId, provimet);
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

                List<Lendet> MyLendets = await GetAPI.GetLendetListAsync(httpClient);
                List<Studenti> MyStudentis = await GetAPI.GetStudentiListAsync(httpClient);
                List<Profesoret> MyProfesorets = await GetAPI.GetProfesoretListAsync(httpClient);

                ViewData["LendaId"] = new SelectList(MyLendets, "LendetId", "EmriLendes", provimet.LendaId);
                ViewData["StudentiId"] = new SelectList(MyStudentis, "StudentId", "FullName", provimet.StudentiId);
                ViewData["ProfesoriId"] = new SelectList(MyProfesorets, "ProfesoretId", "EmriProfesorit", provimet.ProfesoriId);
            }

            return View();
        }

        public async Task<IActionResult> DeleteDetails(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Provimet provimi = new Provimet();
            using (var httpClient = new HttpClient())
            {
                provimi = await GetAPI.GetProvimetAsync(httpClient, id);
                Lendet lendet = await GetAPI.GetLendetAsync(httpClient, provimi.LendaId);
                Studenti studenti = await GetAPI.GetStudentiAsync(httpClient, provimi.StudentiId);
                Profesoret profesoret = await GetAPI.GetProfesoretAsync(httpClient, provimi.ProfesoriId);

                provimi.Lenda = lendet;
                provimi.Studenti = studenti;
                provimi.Profesori = profesoret;
            }

            if (provimi == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(provimi);
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
