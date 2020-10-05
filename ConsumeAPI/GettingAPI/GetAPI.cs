using ConsumeAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsumeAPI.GettingAPI
{
    public class GetAPI
    {
        private static string getStudentApi = "http://localhost:58559/TblStudentis";
        private static string getDrejtimetApi = "http://localhost:58559/TblDrejtimets";
        private static string getStatusiApi = "http://localhost:58559/TblStatusis";

        public static async Task<List<Studenti>> GetStudentiListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(getStudentApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Studenti>>(apiResponse);
        }

        public static async Task<Studenti> GetStudentiAsync(HttpClient httpClient, int? id)
        {
            using var response = await httpClient.GetAsync(getStudentApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Studenti>(apiResponse);
        }

        public static async Task<List<Drejtimet>> GetDrejtimiListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(getDrejtimetApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponse);
        }

        public static async Task<Drejtimet> GetDrejtimiAsync(HttpClient httpClient, int id)
        {
            using var response = await httpClient.GetAsync(getDrejtimetApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Drejtimet>(apiResponse);
        }

        public static async Task<List<Statuset>> GetStatusiListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(getStatusiApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Statuset>>(apiResponse);
        }
    }
}
