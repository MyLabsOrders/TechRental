using Newtonsoft.Json;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class ResponseAnalyzeService
    {
        public static string GetErrorReason(HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;
            object? jsonObject = JsonConvert.DeserializeObject(content);
            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        }
    }
}
