using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace NoteTakingApp.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetAccessToken(this HttpRequest request) {
            request.Headers.TryGetValue("Authorization", out StringValues value);

            if (StringValues.IsNullOrEmpty(value)) value = request.Query["access_token"];

            return value.ToString().Replace("Bearer ", "");
        }
    }
}
