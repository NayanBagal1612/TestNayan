using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Common
{
    public class Helper
    {
        static HttpClient client;
        public static string BaseURL = "http://localhost/TestApp/";

        static Helper()
        {

        }

        public static async Task<string> Post(string Url, string Json)
        {
            string data = string.Empty;
            var uri = new Uri(string.Format(Url, string.Empty));

            try
            {
                var content = new StringContent(Json, Encoding.UTF8, "application/json");

                client = new HttpClient
                {
                    MaxResponseContentBufferSize = 999999999
                };
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"SUCCESS {0}", $"{Url} {response.StatusCode}");
                }
                else
                    Debug.WriteLine(@"ERROR {0}", $"{Url} {response.StatusCode}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
            return data;
        }

        #region ExecutePostCall

        public static async Task<T> ExecutePostCall<T, R>(R request, string url)
        {

            string ServiceCallURL = BaseURL + url;
            T response = default(T);
            try
            {
                string content = string.Empty;

                if (request != null)
                    content = JsonConvert.SerializeObject(request);

                content = await Post(ServiceCallURL, content);

                if (!string.IsNullOrEmpty(content))
                    response = JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return response;
        }

        #endregion
    }
}
