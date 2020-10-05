using ConsumeAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsumeAPI.GettingAPI
{
    public class GetAPI
    {
        public static async Task<List<Studenti>> GetStudentiListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getStudentApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Studenti>>(apiResponse);
        }

        public static async Task<Studenti> GetStudentiAsync(HttpClient httpClient, int? id)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getStudentApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Studenti>(apiResponse);
        }

        public static async Task<List<Drejtimet>> GetDrejtimiListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getDrejtimetApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Drejtimet>>(apiResponse);
        }

        public static async Task<Drejtimet> GetDrejtimiAsync(HttpClient httpClient, int? id)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getDrejtimetApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Drejtimet>(apiResponse);
        }

        public static async Task<List<Statuset>> GetStatusiListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getStatusiApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Statuset>>(apiResponse);
        }

        public static async Task<List<Rolet>> GetRoletListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getRoletApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Rolet>>(apiResponse);
        }

        public static async Task<Rolet> GetRoletAsync(HttpClient httpClient, int? id)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getRoletApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Rolet>(apiResponse);
        }

        public static async Task<List<Users>> GetUserListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getUserApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Users>>(apiResponse);
        }

        public static async Task<Users> GetUserAsync(HttpClient httpClient, int? id)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getUserApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Users>(apiResponse);
        }

        public static async Task<List<Profesoret>> GetProfesoretListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getProfesoriApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Profesoret>>(apiResponse);
        }

        public static async Task<Profesoret> GetProfesoretAsync(HttpClient httpClient, int? id)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getProfesoriApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Profesoret>(apiResponse);
        }

        public static async Task<List<Provimet>> GetProvimetListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getProvimetApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Provimet>>(apiResponse);
        }

        public static async Task<Provimet> GetProvimetAsync(HttpClient httpClient, int? id)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getProvimetApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Provimet>(apiResponse);
        }

        public static async Task<List<Lendet>> GetLendetListAsync(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getLendaApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Lendet>>(apiResponse);
        }

        public static async Task<Lendet> GetLendetAsync(HttpClient httpClient, int? id)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getLendaApi + "/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Lendet>(apiResponse);
        }
    }
}
