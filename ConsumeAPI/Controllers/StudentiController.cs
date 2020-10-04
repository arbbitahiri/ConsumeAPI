using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class StudentiController : Controller
    {
        private string getApi = "http://localhost:58559/TblStudentis";
        private string getDrejtimetApi = "http://localhost:58559/TblDrejtimets";
        private string getStatusiApi = "http://localhost:58559/TblStatusis";

        public async Task<IActionResult> Index()
        {
            List<Studenti> MyStudents = new List<Studenti>();
            List<Drejtimet> MyDrejtimets = new List<Drejtimet>();
            List<Statuset> MyStatusets = new List<Statuset>();

            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi);
                string apiResponse = await response.Content.ReadAsStringAsync();
                MyStudents = JsonConvert.DeserializeObject<List<Studenti>>(apiResponse);

                using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi);
                string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                MyDrejtimets = JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponseDrejtimet);

                using var responseStatusi = await httpClient.GetAsync(getStatusiApi);
                string apiResponseStatusi = await responseStatusi.Content.ReadAsStringAsync();
                MyStatusets = JsonConvert.DeserializeObject<List<Statuset>>(apiResponseStatusi);
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
        public async Task<IActionResult> GetSingleStudent(int id)
        {
            if (ModelState.IsValid)
            {
                Studenti studenti = new Studenti();
                using (var httpClient = new HttpClient())
                {
                    using var response = await httpClient.GetAsync(getApi + "/" + id);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    studenti = JsonConvert.DeserializeObject<Studenti>(apiResponse);

                    using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi + "/" + studenti.DrejtimiId);
                    string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                    Drejtimet drejtimet = JsonConvert.DeserializeObject<Drejtimet>(apiResponseDrejtimet);

                    using var responseStatusi = await httpClient.GetAsync(getStatusiApi);
                    string apiResponseStatusi = await responseStatusi.Content.ReadAsStringAsync();
                    Statuset statuset = JsonConvert.DeserializeObject<Statuset>(apiResponseStatusi);

                    studenti.Drejtimi = drejtimet;
                    studenti.Statusi = statuset;
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
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                studenti = JsonConvert.DeserializeObject<Studenti>(apiResponse);

                using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi + "/" + studenti.DrejtimiId);
                string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                Drejtimet drejtimet = JsonConvert.DeserializeObject<Drejtimet>(apiResponseDrejtimet);

                using var responseStatusi = await httpClient.GetAsync(getStatusiApi);
                string apiResponseStatusi = await responseStatusi.Content.ReadAsStringAsync();
                List<Statuset> MyStatusets = JsonConvert.DeserializeObject<List<Statuset>>(apiResponseStatusi);

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
            using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi);
            string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
            List<Drejtimet> MyDrejtimets = JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponseDrejtimet);

            using var responseStatusi = await httpClient.GetAsync(getStatusiApi);
            string apiResponseStatusi = await responseStatusi.Content.ReadAsStringAsync();
            List<Statuset> MyStatusets = JsonConvert.DeserializeObject<List<Statuset>>(apiResponseStatusi);

            ViewBag.DrejtimiId = MyDrejtimets;
            ViewBag.StatusiId = MyStatusets;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Studenti studenti)
        {
            if (ModelState.IsValid)
            {
                Studenti receivedStudenti = new Studenti();
                using (var httpClient = new HttpClient())
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
                }
                return View(receivedStudenti);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all form!");
            }
            return View(studenti);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Studenti studenti = new Studenti();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                studenti = JsonConvert.DeserializeObject<Studenti>(apiResponse);

                using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi);
                string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                List<Drejtimet> MyDrejtimets = JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponseDrejtimet);

                using var responseStatusi = await httpClient.GetAsync(getStatusiApi);
                string apiResponseStatusi = await responseStatusi.Content.ReadAsStringAsync();
                List<Statuset> MyStatusets = JsonConvert.DeserializeObject<List<Statuset>>(apiResponseStatusi);

                ViewBag.DrejtimiId = MyDrejtimets;
                ViewBag.StatusiId = MyStatusets;
            }

            return View(studenti);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Studenti studenti)
        {
            Studenti receivedStudenti = new Studenti();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(studenti.Emri), "Emri" },
                    { new StringContent(studenti.Mbiemri), "Mbiemri" },
                    { new StringContent(studenti.DataLindjes.ToString()), "DataLindjes" },
                    { new StringContent(studenti.Indeksi), "Indexi" },
                    { new StringContent(studenti.DrejtimiId.ToString()), "DataLindjes" },
                    { new StringContent(studenti.StatusiId.ToString()), "DataLindjes" }
                };

                using var response = await httpClient.PostAsync(getApi, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Result = "Success";
                receivedStudenti = JsonConvert.DeserializeObject<Studenti>(apiResponse);
            }

            return View(receivedStudenti);
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
                using var response = await httpClient.GetAsync(getApi + "/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                studenti = JsonConvert.DeserializeObject<Studenti>(apiResponse);

                using var responseDrejtimet = await httpClient.GetAsync(getDrejtimetApi + "/" + studenti.DrejtimiId);
                string apiResponseDrejtimet = await responseDrejtimet.Content.ReadAsStringAsync();
                Drejtimet drejtimet = JsonConvert.DeserializeObject<Drejtimet>(apiResponseDrejtimet);

                using var responseStatusi = await httpClient.GetAsync(getStatusiApi);
                string apiResponseStatusi = await responseStatusi.Content.ReadAsStringAsync();
                List<Statuset> MyStatusets = JsonConvert.DeserializeObject<List<Statuset>>(apiResponseStatusi);

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
