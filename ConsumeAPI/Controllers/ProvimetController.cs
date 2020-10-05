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
    public class ProvimetController : Controller
    {
        private string getApi = "http://localhost:58559/TblProvimets";
        private string getLendaApi = "http://localhost:58559/TblLendets";
        private string getStudentiApi = "http://localhost:58559/TblStudentis";
        private string getProfesoriApi = "http://localhost:58559/TblProfesorets";

        public async Task<IActionResult> Index()
        {
            List<Provimet> MyProvimets = new List<Provimet>();
            List<Lendet> MyLendets = new List<Lendet>();
            List<Studenti> MyStudentis = new List<Studenti>();
            List<Profesoret> MyProfesorets = new List<Profesoret>();

            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi);
                string apiResponse = await response.Content.ReadAsStringAsync();
                MyProvimets = JsonConvert.DeserializeObject<List<Provimet>>(apiResponse);

                using var repsonseLenda = await httpClient.GetAsync(getLendaApi);
                string apiResponseLenda = await repsonseLenda.Content.ReadAsStringAsync();
                MyLendets = JsonConvert.DeserializeObject<List<Lendet>>(apiResponseLenda);

                using var repsonseStudenti = await httpClient.GetAsync(getStudentiApi);
                string apiResponseStudenti = await repsonseStudenti.Content.ReadAsStringAsync();
                MyStudentis = JsonConvert.DeserializeObject<List<Studenti>>(apiResponseStudenti);

                using var repsonseProfesori = await httpClient.GetAsync(getProfesoriApi);
                string apiResponseProfesori = await repsonseProfesori.Content.ReadAsStringAsync();
                MyProfesorets = JsonConvert.DeserializeObject<List<Profesoret>>(apiResponseProfesori);
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
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                provimi = JsonConvert.DeserializeObject<Provimet>(apiResponse);

                using var repsonseLenda = await httpClient.GetAsync(getLendaApi + "/" + provimi.LendaId);
                string apiResponseLenda = await repsonseLenda.Content.ReadAsStringAsync();
                Lendet lendet = JsonConvert.DeserializeObject<Lendet>(apiResponseLenda);

                using var repsonseStudenti = await httpClient.GetAsync(getStudentiApi + "/" + provimi.StudentiId);
                string apiResponseStudenti = await repsonseStudenti.Content.ReadAsStringAsync();
                Studenti studenti = JsonConvert.DeserializeObject<Studenti>(apiResponseStudenti);

                using var repsonseProfesori = await httpClient.GetAsync(getProfesoriApi + "/" + provimi.ProfesoriId);
                string apiResponseProfesori = await repsonseProfesori.Content.ReadAsStringAsync();
                Profesoret profesoret = JsonConvert.DeserializeObject<Profesoret>(apiResponseProfesori);

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
            using var repsonseLenda = await httpClient.GetAsync(getLendaApi);
            string apiResponseLenda = await repsonseLenda.Content.ReadAsStringAsync();
            List<Lendet> MyLendets = JsonConvert.DeserializeObject<List<Lendet>>(apiResponseLenda);

            using var repsonseStudenti = await httpClient.GetAsync(getStudentiApi);
            string apiResponseStudenti = await repsonseStudenti.Content.ReadAsStringAsync();
            List<Studenti> MyStudentis = JsonConvert.DeserializeObject<List<Studenti>>(apiResponseStudenti);

            using var repsonseProfesori = await httpClient.GetAsync(getProfesoriApi);
            string apiResponseProfesori = await repsonseProfesori.Content.ReadAsStringAsync();
            List<Profesoret> MyProfesorets = JsonConvert.DeserializeObject<List<Profesoret>>(apiResponseProfesori);

            ViewBag.LendetId = MyLendets;
            ViewBag.StudentiId = MyStudentis;
            ViewBag.ProfesoriId = MyProfesorets;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Provimet provimet)
        {
            if (ModelState.IsValid)
            {
                Provimet receivedStudenti = new Provimet();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(provimet), Encoding.UTF8, "application/json");

                    using var response = await httpClient.PostAsync(getApi, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedStudenti = JsonConvert.DeserializeObject<Provimet>(apiResponse);

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
                return View(receivedStudenti);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all form!");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Provimet provimi = new Provimet();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                provimi = JsonConvert.DeserializeObject<Provimet>(apiResponse);

                using var repsonseLenda = await httpClient.GetAsync(getLendaApi);
                string apiResponseLenda = await repsonseLenda.Content.ReadAsStringAsync();
                List<Lendet> MyLendets = JsonConvert.DeserializeObject<List<Lendet>>(apiResponseLenda);

                using var repsonseStudenti = await httpClient.GetAsync(getStudentiApi);
                string apiResponseStudenti = await repsonseStudenti.Content.ReadAsStringAsync();
                List<Studenti> MyStudentis = JsonConvert.DeserializeObject<List<Studenti>>(apiResponseStudenti);

                using var repsonseProfesori = await httpClient.GetAsync(getProfesoriApi);
                string apiResponseProfesori = await repsonseProfesori.Content.ReadAsStringAsync();
                List<Profesoret> MyProfesorets = JsonConvert.DeserializeObject<List<Profesoret>>(apiResponseProfesori);

                ViewBag.LendetId = MyLendets;
                ViewBag.StudentiId = MyStudentis;
                ViewBag.ProfesoriId = MyProfesorets;
            }

            return View(provimi);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Provimet provimet)
        {
            Provimet receivedProvimi = new Provimet();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(provimet.StudentiId.ToString()), "StudentiId" },
                    { new StringContent(provimet.LendaId.ToString()), "LendaId" },
                    { new StringContent(provimet.ProfesoriId.ToString()), "ProfesoriId" },
                    { new StringContent(provimet.Piket.ToString()), "Piket" },
                    { new StringContent(provimet.Nota.ToString()), "Nota" }
                };

                using var response = await httpClient.PostAsync(getApi, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Result = "Success";
                receivedProvimi = JsonConvert.DeserializeObject<Provimet>(apiResponse);
            }

            return View(receivedProvimi);
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
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                provimi = JsonConvert.DeserializeObject<Provimet>(apiResponse);

                using var repsonseLenda = await httpClient.GetAsync(getLendaApi + "/" + provimi.LendaId);
                string apiResponseLenda = await repsonseLenda.Content.ReadAsStringAsync();
                Lendet lendet = JsonConvert.DeserializeObject<Lendet>(apiResponseLenda);

                using var repsonseStudenti = await httpClient.GetAsync(getStudentiApi + "/" + provimi.StudentiId);
                string apiResponseStudenti = await repsonseStudenti.Content.ReadAsStringAsync();
                Studenti studenti = JsonConvert.DeserializeObject<Studenti>(apiResponseStudenti);

                using var repsonseProfesori = await httpClient.GetAsync(getProfesoriApi + "/" + provimi.ProfesoriId);
                string apiResponseProfesori = await repsonseProfesori.Content.ReadAsStringAsync();
                Profesoret profesoret = JsonConvert.DeserializeObject<Profesoret>(apiResponseProfesori);

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
