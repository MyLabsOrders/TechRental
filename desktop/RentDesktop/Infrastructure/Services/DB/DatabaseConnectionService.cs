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
        public const string SCHEME = "Bearer";

        #endregion

        private bool _isDisposed;
        private readonly HttpClient _httpClient;

        public static string? AuthorizationToken { get; set; }

        public DatabaseConnectionService(string? authorizationToken = null, bool restoreRegisteredAuthorizationToken = true)
        {
            _httpClient = new HttpClient();

            if (authorizationToken is not null)
                SetAuthorizationToken(authorizationToken);

            else if (restoreRegisteredAuthorizationToken && AuthorizationToken is not null)
                SetAuthorizationToken(AuthorizationToken);
        }

        ~DatabaseConnectionService()
        {
            if (!_isDisposed)
                Dispose();
        }

        public void SetAuthorizationToken(string authorizationToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(SCHEME, authorizationToken);
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

        public Task<HttpResponseMessage> PatchAsync<T>(string handle, T contentObject)
        {
            string uri = SERVER_URL + CorrectHandle(handle);
            HttpContent content = JsonContent.Create(contentObject);

            return _httpClient.PatchAsync(uri, content);
        }

        public void CloseConnection()
        {
            _httpClient.Dispose();
            _isDisposed = true;
        }

        public void Dispose()
        {
            CloseConnection();
            GC.SuppressFinalize(this);
        }

        private static string CorrectHandle(string handle)
        {
            return !handle.StartsWith("/")
                ? "/" + handle
                : handle;
        }
    }
}
