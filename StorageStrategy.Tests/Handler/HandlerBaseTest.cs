using StorageStrategy.Models;

namespace StorageStrategy.Tests.Handler
{
    public abstract class HandlerBaseTest
    {
        
        public bool IsValid(Result result, string errorMessage)
        {
            if (result.Errors.Count == 0)
                return false;

            if (result.Errors.Any(p => p.ErrorMessage.Equals(errorMessage)))
                return false;

            return true;
        }

    }
}
