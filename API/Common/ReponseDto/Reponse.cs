namespace API.Common.ReponseDto
{
    public class Reponse
    {
        public object Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public Reponse(object data = null, string message = "", int statusCode = 0)
        {
            Succeeded = true;
            Message = message ?? GetDefaultMessageStatusCode(statusCode);
            Data = data;
        }

        public Reponse(string message, int statusCode)
        {
            Succeeded = false;
            Message = message ?? GetDefaultMessageStatusCode(statusCode); ;
            StatusCode = statusCode;
        }

        private string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                400 => "A bad request",
                401 => "Authorized",
                403 => "Forbidden",
                404 => "Resource found, it was not",
                500 => "Errors",
                _ => null
            };
        }
    }
}
