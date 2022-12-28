using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class CategoryHandler : HandlerBase,
        IRequestHandler<CreateCategoryCommand, Result>,
        IRequestHandler<UpdateCategoryCommand, Result>,
        IRequestHandler<DeleteCategoryCommand, Result>
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

            var category = await _repo.FindByName(request.Name, request.CompanyId);

            if (category is not null)
                return CreateError("Ja existe uma categoria com esse nome.");

            category = _mapper.Map<CategoryEntity>(request);

            await _repo.AddAsync(category);
            await _repo.SaveAsync();

            return CreateResponse(category, "Categoria cadastrada com sucesso.");
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var category = await _repo.GetByIdAsync(request.CategoryId, request.CompanyId);

            if (category is null)
                return CreateError(null, "Categoria não encontrada para edição.");

            category = _mapper.Map<CategoryEntity>(request);

            _repo.Update(category);
            await _repo.SaveAsync();

            return CreateResponse(category, "Categoria atualizada com sucesso.");
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var category = await _repo.GetByIdAsync(request.CategoryId, request.CompanyId);

            if (category is null)
                return CreateError("Categoria não encontrada para exclusão.");

            _repo.Delete(category);
            await _repo.SaveAsync();

            return CreateResponse(category, "Categoria excluida com sucesso.");
        }
    }
}
