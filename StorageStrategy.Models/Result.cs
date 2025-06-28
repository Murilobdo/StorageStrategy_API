
namespace StorageStrategy.Models
{
    public class Result
    {
        public object Response { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public List<Error> Errors { get; set; } = new();
        public bool Success => Errors.Count == 0;

        public Result(object data, string message)
        {
            Response = data;
            Message = message;
            Errors = new List<Error>();
        }

        public Result(string message)
        {
            Message = message;
            Errors = new List<Error>();
        }

        public Result(List<Error> errors, string errorMessage)
        {
            Errors = errors;
            ErrorMessage= errorMessage;
        }

        public Result(Error error)
        {
            Errors.Add(error);
            ErrorMessage = error.ErrorMessage;
        }
    }
}
