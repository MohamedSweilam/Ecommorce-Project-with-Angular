namespace Ecommorce.API.Helper
{
    public class ApiExecption : ApiResponse
    {
        public ApiExecption(int statusCode, string? message = null,string details=null) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }

       
    }
}
