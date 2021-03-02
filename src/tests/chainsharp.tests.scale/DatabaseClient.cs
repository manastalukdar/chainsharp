using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace chainsharp.scale
{
    public class DatabaseClient
    {
        private readonly string _endpointUrl;
        private readonly HttpClient _httpClient;

        public DatabaseClient(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            _httpClient = new HttpClient(handler, false)
            {
                Timeout = TimeSpan.FromSeconds(30),
            };
        }

        public Task<HttpResponseMessage> SendWriteRequestAsync(string key, string value)
        {
            var stringContent = JsonConvert.SerializeObject(value);
            byte[] byteArray = Encoding.UTF8.GetBytes(value);
            MemoryStream ms = new MemoryStream(byteArray)
            {
                Position = 0
            };
            var streamContent = new StreamContent(ms);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, new Uri($"{_endpointUrl}/add/{key}"))
            {
                Content = new StringContent(stringContent, Encoding.UTF8, "application/json")
            };

            return _httpClient.SendAsync(request);
        }

        public Task<HttpResponseMessage> SendReadRequestAsync(string key)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{_endpointUrl}/get/{key}"));

            return _httpClient.SendAsync(request);
        }
    }
}
