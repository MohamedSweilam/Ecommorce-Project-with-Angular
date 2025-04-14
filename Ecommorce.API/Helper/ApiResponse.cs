namespace Ecommorce.API.Helper
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message??GetMessageFromStatusCode(StatusCode);
        }
        private string GetMessageFromStatusCode(int statuscode)
        {
            return statuscode switch
            {
                200 => "Done",
                400 => "Bad Request",
                401 => "Un Authorized",
                404=>"Not Found Resources",
                500 => "Server Error",
                _ => null
            };
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
