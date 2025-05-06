using System.Net;

namespace EventHub.Responses
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }          // Indicates if the request was successful
        public string Message { get; set; }       // Response message
        public T Data { get; set; }               // Data returned from the service
        public List<string> Errors { get; set; }  // Optional: Error details
        public int StatusCode { get; set; }       // HTTP Status Code


        // Helper method for setting an error response
        public static ApiResponse<T> SetError(ApiResponse<T> response, string message = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            response.Status = false;
            response.Message = message ?? "An error occurred.";
            response.StatusCode = (int)httpStatusCode;
            response.Errors = new List<string>(); // Initialize the Errors list
            return response;
        }

        // Helper method for setting a success response
        public static ApiResponse<T> SetSuccess(ApiResponse<T> response, T data, string message = null)
        {
            response.Status = true;
            response.Message = message ?? "Request successful.";
            response.Data = data;
            response.Errors = new List<string>();  // Initialize the Errors list
            response.StatusCode = (int)HttpStatusCode.OK;  // Default to OK status
            return response;
        }
    } 
}
