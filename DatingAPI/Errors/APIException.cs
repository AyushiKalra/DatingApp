
namespace DatingAPI.Errors
{
    public class APIException
    {
        public APIException(string message, int statusCode, string details)
        {
            Message = message;
            StatusCode = statusCode;
            Details = details;
        }

        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string Details { get; set; }
        
    }
}