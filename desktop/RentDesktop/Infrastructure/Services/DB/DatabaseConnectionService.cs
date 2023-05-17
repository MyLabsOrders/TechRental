using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal class DatabaseConnectionService : IDisposable
    {
        #region Constants

        public const string PROTOCOL = "http";
        public const string HOST = "localhost";
        public const string PORT = "8080";
        public const string SERVER_URL = $"{PROTOCOL}://{HOST}:{PORT}";

        #endregion

        private readonly HttpClient _httpClient;

        public DatabaseConnectionService(string? authorizationToken = null)
        {
            _httpClient = new HttpClient();

            if (authorizationToken is not null)
                AddAuthorizationToken(authorizationToken);
        }

        public void AddAuthorizationToken(string authorizationToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", authorizationToken);
        }

        public Task<HttpResponseMessage> GetAsync(string handle)
        {
            string uri = SERVER_URL + CorrectHandle(handle);
            return _httpClient.GetAsync(uri);
        }

        public Task<HttpResponseMessage> DeleteAsync(string handle)
        {
            string uri = SERVER_URL + CorrectHandle(handle);
            return _httpClient.DeleteAsync(uri);
        }

        public Task<HttpResponseMessage> PostAsync<T>(string handle, T contentObject)
        {
            string uri = SERVER_URL + CorrectHandle(handle);
            HttpContent content = JsonContent.Create(contentObject);

            return _httpClient.PostAsync(uri, content);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string handle, T contentObject)
        {
            string uri = SERVER_URL + CorrectHandle(handle);
            HttpContent content = JsonContent.Create(contentObject);

            return _httpClient.PutAsync(uri, content);
        }

        public void CloseConnection()
        {
            _httpClient.Dispose();
        }

        public void Dispose()
        {
            CloseConnection();
        }

        private static string CorrectHandle(string handle)
        {
            return !handle.StartsWith("/")
                ? "/" + handle
                : handle;
        }
    }
}
