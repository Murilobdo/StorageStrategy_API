using AutoMapper;
using StorageStrategy.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StorageStrategy.Domain.Handlers
{
    public class HandlerBase
    {

        public Result CreateResponse<T>(T data, string message)
        {
            return new Result(data, message);
        }

        public Result CreateResponse<T>(string message)
        {
            return new Result(message);
        }

        public Result CreateError(string errorMessage)
        {
            var errors = new List<Models.Error>();
            errors.Add(new Models.Error("", errorMessage));
            return new Result(errors, errorMessage);
        }

        public Result CreateError(List<Models.Error> errors, string errorMessage)
        {
            return new Result(errors, errorMessage);
        }

        public Result CreateError(Result result)
        {
            return result;
        }
    }
}
