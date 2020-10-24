using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsumeAPI.GettingAPI
{
    public class GetJsonXml
    {
        public static async Task<string> GetJsonDrejtimet(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getDrejtimetApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static async Task<string> GetXmlDrejtimet(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getDrejtimetApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Drejtimet \":" + apiResponse + "}", "Drejtimet");

            string xmlString = xml.OuterXml;
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    xmlDocument.LoadXml(xmlString);

                    writer.Formatting = System.Xml.Formatting.Indented;

                    xmlDocument.WriteContentTo(writer);
                    writer.Flush();
                    memoryStream.Flush();

                    memoryStream.Position = 0;

                    StreamReader streamReader = new StreamReader(memoryStream);

                    string formattedXml = streamReader.ReadToEnd();
                    result = formattedXml;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public static async Task<string> GetJsonLendet(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getLendaApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static async Task<string> GetXmlLendet(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getLendaApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Lendet \":" + apiResponse + "}", "Lendet");

            string xmlString = xml.OuterXml;
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    xmlDocument.LoadXml(xmlString);

                    writer.Formatting = System.Xml.Formatting.Indented;

                    xmlDocument.WriteContentTo(writer);
                    writer.Flush();
                    memoryStream.Flush();

                    memoryStream.Position = 0;

                    StreamReader streamReader = new StreamReader(memoryStream);

                    string formattedXml = streamReader.ReadToEnd();
                    result = formattedXml;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public static async Task<string> GetJsonProfesoret(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getProfesoriApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static async Task<string> GetXmlProfesoret(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getProfesoriApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Profesoret \":" + apiResponse + "}", "Profesoret");

            string xmlString = xml.OuterXml;
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    xmlDocument.LoadXml(xmlString);

                    writer.Formatting = System.Xml.Formatting.Indented;

                    xmlDocument.WriteContentTo(writer);
                    writer.Flush();
                    memoryStream.Flush();

                    memoryStream.Position = 0;

                    StreamReader streamReader = new StreamReader(memoryStream);

                    string formattedXml = streamReader.ReadToEnd();
                    result = formattedXml;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public static async Task<string> GetJsonProvimet(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getProvimetApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static async Task<string> GetXmlProvimet(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getProvimetApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Provimet \":" + apiResponse + "}", "Provimet");

            string xmlString = xml.OuterXml;
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    xmlDocument.LoadXml(xmlString);

                    writer.Formatting = System.Xml.Formatting.Indented;

                    xmlDocument.WriteContentTo(writer);
                    writer.Flush();
                    memoryStream.Flush();

                    memoryStream.Position = 0;

                    StreamReader streamReader = new StreamReader(memoryStream);

                    string formattedXml = streamReader.ReadToEnd();
                    result = formattedXml;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public static async Task<string> GetJsonRolet(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getRoletApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static async Task<string> GetXmlRolet(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getRoletApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Rolet \":" + apiResponse + "}", "Rolet");

            string xmlString = xml.OuterXml;
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    xmlDocument.LoadXml(xmlString);

                    writer.Formatting = System.Xml.Formatting.Indented;

                    xmlDocument.WriteContentTo(writer);
                    writer.Flush();
                    memoryStream.Flush();

                    memoryStream.Position = 0;

                    StreamReader streamReader = new StreamReader(memoryStream);

                    string formattedXml = streamReader.ReadToEnd();
                    result = formattedXml;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public static async Task<string> GetJsonStudenti(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getStudentApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static async Task<string> GetXmlStudenti(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getStudentApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Studenti \":" + apiResponse + "}", "Studenti");

            string xmlString = xml.OuterXml;
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    xmlDocument.LoadXml(xmlString);

                    writer.Formatting = System.Xml.Formatting.Indented;

                    xmlDocument.WriteContentTo(writer);
                    writer.Flush();
                    memoryStream.Flush();

                    memoryStream.Position = 0;

                    StreamReader streamReader = new StreamReader(memoryStream);

                    string formattedXml = streamReader.ReadToEnd();
                    result = formattedXml;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public static async Task<string> GetJsonUser(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getUserApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static async Task<string> GetXmlUser(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getUserApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"User \":" + apiResponse + "}", "User");

            string xmlString = xml.OuterXml;
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    xmlDocument.LoadXml(xmlString);

                    writer.Formatting = System.Xml.Formatting.Indented;

                    xmlDocument.WriteContentTo(writer);
                    writer.Flush();
                    memoryStream.Flush();

                    memoryStream.Position = 0;

                    StreamReader streamReader = new StreamReader(memoryStream);

                    string formattedXml = streamReader.ReadToEnd();
                    result = formattedXml;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public static async Task<string> GetJsonStatusi(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getStatusiApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return JToken.Parse(apiResponse).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static async Task<string> GetXmlStatusi(HttpClient httpClient)
        {
            using var response = await httpClient.GetAsync(LinkAPI.getStatusiApi);
            string apiResponse = await response.Content.ReadAsStringAsync();

            XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"Statusi \":" + apiResponse + "}", "Statusi");

            string xmlString = xml.OuterXml;
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    xmlDocument.LoadXml(xmlString);

                    writer.Formatting = System.Xml.Formatting.Indented;

                    xmlDocument.WriteContentTo(writer);
                    writer.Flush();
                    memoryStream.Flush();

                    memoryStream.Position = 0;

                    StreamReader streamReader = new StreamReader(memoryStream);

                    string formattedXml = streamReader.ReadToEnd();
                    result = formattedXml;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }
    }
}
