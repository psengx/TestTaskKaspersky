using System.Net;

namespace AwesomeFilesCore.Models
{
    public class ResponseContract<T>
    {
        public T? Data { get; set; } = default;
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; } = null;
        public object? Details { get; set; } = null;

        public ResponseContract(T data)
        {
            Data = data;
        }
        public ResponseContract(HttpStatusCode code)
        {
            StatusCode = code;
        }

        public ResponseContract(HttpStatusCode code, string message)
        {
            StatusCode = code;
            Message = message;
        }

        public ResponseContract(HttpStatusCode code, string message, object details)
        {
            StatusCode = code;
            Message = message;
            Details = details;
        }


        public ResponseContract(T data, HttpStatusCode code)
        {
            Data = data;
            StatusCode = code;
        }

        public ResponseContract(T data, HttpStatusCode code, string message)
        {
            Data = data;
            StatusCode = code;
            Message = message;
        }
        public ResponseContract(T data, HttpStatusCode code, string message, object details)
        {
            Data = data;
            StatusCode = code;
            Message = message;
            Details = details;
        }

        public ResponseContract()
        {
        }
    }
}
