using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Common.Helper
{
    public static class ApiHelper
    {
        public static async Task<HttpResponseMessage> GetApiAsync(string url)
        {
            try
            {
                using HttpClient client = new HttpClient();
                return await client.GetAsync(url);
            }
            catch
            {
                throw;
            }
        }
        public static async Task<HttpResponseMessage> GetApiAsync(string uriHost, string pathApi, string token = "")
        {
            try
            {
                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(uriHost);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await client.GetAsync(pathApi);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<HttpResponseMessage> PostApiAsync(string uriHost, string pathApi, string obj, string token = "")
        {
            try
            {
                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(uriHost);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(obj, null, "application/json");
                return await client.PostAsJsonAsync(pathApi, content);
            }
            catch
            {
                throw;
            }
        }

        public static async Task<HttpResponseMessage> PostApiAsync<T>(string uriHost, string pathApi, T obj, string token = "") where T : class
        {
            try
            {
                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(uriHost);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await client.PostAsJsonAsync(pathApi, obj);
            }
            catch
            {
                throw;
            }
        }
    }
}
