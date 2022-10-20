using Newtonsoft.Json;
using System.Net;

namespace Moodswing.Domain.Dtos
{
    public class BaseResultDto
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        private HttpStatusCode _statusCode;

        public HttpStatusCode StatusCode 
        { 
            get => _statusCode == default ? HttpStatusCode.OK : _statusCode; 
            set => _statusCode = value; 
        }
    }
}
