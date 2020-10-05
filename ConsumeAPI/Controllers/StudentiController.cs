using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsumeAPI.Models;
using ConsumeAPI.GettingAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class StudentiController : Controller
    {
        private string getApi = "http://localhost:58559/TblStudentis";

        public async Task<IActionResult> Index()
        {
            List<Studenti> MyStudents = new List<Studenti>();
            List<Drejtimet> MyDrejtimets = new List<Drejtimet>();
            List<Statuset> MyStatusets = new List<Statuset>();

            using (var httpClient = new HttpClient())
            {
                MyStudents = await GetAPI.GetStudentiListAsync(httpClient);
                MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);
                MyStatusets = await GetAPI.GetStatusiListAsync(httpClient);
            }

            foreach (var student in MyStudents)
            {
                foreach (var drejtim in MyDrejtimets)
                {
                    if (student.DrejtimiId == drejtim.DrejtimetId)
                    {
                        student.Drejtimi = drejtim;
                    }
                }

                foreach (var status in MyStatusets)
                {
                    if (student.StatusiId == status.StatusiId)
                    {
                        student.Statusi = status;
                    }
                }
            }

            return View(MyStudents);
        }

        public IActionResult GetSingleStudent() => View();

        [HttpPost]
        public async Task<IActionResult> GetSingleStudent(string index)
        {
            if (ModelState.IsValid)
            {
                Studenti studenti = new Studenti();
                using (var httpClient = new HttpClient())
                {
                    List<Studenti> MyStudents = await GetAPI.GetStudentiListAsync(httpClient);

                    foreach (var student in MyStudents)
                    {
                        if (student.Indeksi == index)
                        {
                            studenti = student;
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Nuk ekziston student me Indeks te tille!");
                        }
                    }

                    Drejtimet drejtimet = await GetAPI.GetDrejtimiAsync(httpClient, studenti.DrejtimiId);
                    List<Statuset> MyStatusets = await GetAPI.GetStatusiListAsync(httpClient);

                    studenti.Drejtimi = drejtimet;

                    foreach (var status in MyStatusets)
                    {
                        if (studenti.StatusiId == status.StatusiId)
                        {
                            studenti.Statusi = status;
                        }
                    }
                }

                return View(studenti);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Studenti studenti = new Studenti();
            using (var httpClient = new HttpClient())
            {
                studenti = await GetAPI.GetStudentiAsync(httpClient, id);
                Drejtimet drejtimet = await GetAPI.GetDrejtimiAsync(httpClient, studenti.DrejtimiId);
                List<Statuset> MyStatusets = await GetAPI.GetStatusiListAsync(httpClient);

                studenti.Drejtimi = drejtimet;

                foreach (var status in MyStatusets)
                {
                    if (studenti.StatusiId == status.StatusiId)
                    {
                        studenti.Statusi = status;
                    }
                }
            }

            if (studenti == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(studenti);
        }

        public async Task<IActionResult> CreateAsync()
        {
            using var httpClient = new HttpClient();
            List<Drejtimet> MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);
            List<Statuset> MyStatusets = await GetAPI.GetStatusiListAsync(httpClient);

            ViewData["DrejtimiId"] = new SelectList(MyDrejtimets, "DrejtimetId", "EmriDrejtimit");
            ViewData["StatusiId"] = new SelectList(MyStatusets, "StatusiId", "Statusi");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Studenti studenti)
        {
            Studenti receivedStudenti = new Studenti();
            using var httpClient = new HttpClient();
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(studenti), Encoding.UTF8, "application/json");

                using var response = await httpClient.PostAsync(getApi, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                receivedStudenti = JsonConvert.DeserializeObject<Studenti>(apiResponse);

                string success = response.StatusCode.ToString();
                if (success == "Created")
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There was an error while registering role!");
                }
                return View(receivedStudenti);
            }

            List<Drejtimet> MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);
            List<Statuset> MyStatusets = await GetAPI.GetStatusiListAsync(httpClient);

            ViewData["DrejtimiId"] = new SelectList(MyDrejtimets, "DrejtimetId", "EmriDrejtimit", studenti.DrejtimiId);
            ViewData["StatusiId"] = new SelectList(MyStatusets, "StatusiId", "Statusi", studenti.StatusiId);

            return View(studenti);
        }

        public async Task<IActionResult> EditForm(int id)
        {
            Studenti studenti = new Studenti();
            using (var httpClient = new HttpClient())
            {
                studenti = await GetAPI.GetStudentiAsync(httpClient, id);
                List<Drejtimet> MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);
                List<Statuset> MyStatusets = await GetAPI.GetStatusiListAsync(httpClient);

                ViewData["DrejtimiId"] = new SelectList(MyDrejtimets, "DrejtimetId", "EmriDrejtimit");
                ViewData["StatusiId"] = new SelectList(MyStatusets, "StatusiId", "Statusi");
            }

            return View(studenti);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Studenti studenti)
        {
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.PutAsJsonAsync<Studenti>(getApi + "/" + studenti.StudentId, studenti);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Result = "Success";

                    return RedirectToAction(nameof(Index));
                }

                List<Drejtimet> MyDrejtimets = await GetAPI.GetDrejtimiListAsync(httpClient);
                List<Statuset> MyStatusets = await GetAPI.GetStatusiListAsync(httpClient);

                ViewData["DrejtimiId"] = new SelectList(MyDrejtimets, "DrejtimetId", "EmriDrejtimit", studenti.DrejtimiId);
                ViewData["StatusiId"] = new SelectList(MyStatusets, "StatusiId", "Statusi", studenti.StatusiId);
            }

            return View();
        }

        public async Task<IActionResult> DeleteDetails(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            Studenti studenti = new Studenti();
            using (var httpClient = new HttpClient())
            {
                studenti = await GetAPI.GetStudentiAsync(httpClient, id);
                Drejtimet drejtimet = await GetAPI.GetDrejtimiAsync(httpClient, studenti.DrejtimiId);
                List<Statuset> MyStatusets = await GetAPI.GetStatusiListAsync(httpClient);

                studenti.Drejtimi = drejtimet;

                foreach (var status in MyStatusets)
                {
                    if (studenti.StatusiId == status.StatusiId)
                    {
                        studenti.Statusi = status;
                    }
                }
            }

            if (studenti == null)
            {
                ModelState.AddModelError(string.Empty, "There was an error while getting student!");
            }

            return View(studenti);
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