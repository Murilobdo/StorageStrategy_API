using AutoMapper;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public abstract class HandlerBase
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
            return new Result(new Models.Error(errorMessage));
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
