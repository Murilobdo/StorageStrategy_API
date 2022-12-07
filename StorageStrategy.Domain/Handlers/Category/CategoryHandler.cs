using MediatR;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Category
{
    public class CategoryHandler : HandlerBase,
        IRequestHandler<CreateCategoryCommand, Result>
    {
        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            throw new NotImplementedException();
        }
    }
}
