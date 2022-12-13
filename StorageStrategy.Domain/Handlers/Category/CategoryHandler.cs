using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Category
{
    public class CategoryHandler : HandlerBase,
        IRequestHandler<CreateCategoryCommand, Result>
    {
        private ICategoryRepository _repo;
        private IMapper _mapper;

        public CategoryHandler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var category = await _repo.FindByName(request.Name);

            if (category is not null)
                return CreateError(null, "Ja existe uma categoria com esse nome.");

            category = _mapper.Map<CategoryEntity>(request);

            _repo.Add(category);
            await _repo.SaveAsync();

            return CreateResponse(category, "Categoria cadastrada com sucesso.");
        }
    }
}
