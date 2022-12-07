namespace StorageStrategy.Models
{
    public class Result
    {
        public object Data { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public List<Error> Errors { get; set; } = new();
        public bool Success { get; set; }

        public Result(object data, string message)
        {
            Data = data;
            Message = message;
        }

        public Result(string message)
        {
            Message = message;
        }

        public Result(List<Error> errors, string errorMessage)
        {
            Errors= errors;
            ErrorMessage= errorMessage;
        }

        public void AddError(Error error)
        {
            Errors.Add(error);
        }
    }
}
