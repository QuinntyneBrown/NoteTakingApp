using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;
using static System.Text.Encoding;

namespace NoteTakingApp.Core.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TOut> PostAsAsync<TIn, TOut>(this HttpClient client, string url, TIn content)
        {
            var stringContent = new StringContent(SerializeObject(content), UTF8, "application/json");
            var responseMessage = await client.PostAsync(url, stringContent);
            var responseText = await responseMessage.Content.ReadAsStringAsync();
            return DeserializeObject<TOut>(responseText);
        }

        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, dynamic content)
        {
            var stringContent = new StringContent(SerializeObject(content), UTF8, "application/json");
            return await client.PostAsync(url, stringContent);            
        }

        public static async Task<TOut> PutAsAsync<TIn, TOut>(this HttpClient client, string url, TIn content)
        {
            var stringContent = new StringContent(SerializeObject(content), UTF8, "application/json");
            var responseMessage = await client.PutAsync(url, stringContent);
            return DeserializeObject<TOut>(await responseMessage.Content.ReadAsStringAsync());
        }

        public static async Task<T> GetAsync<T>(this HttpClient client, string url)
        {
            var httpResponseMessage = await client.GetAsync(url);
            return DeserializeObject<T>((await httpResponseMessage.Content.ReadAsStringAsync()));
        }
    }
}