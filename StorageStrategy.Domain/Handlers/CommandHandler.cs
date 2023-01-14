using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class CommandHandler : HandlerBase,
        IRequestHandler<CreateCommandCommand, Result>,
        IRequestHandler<UpdateCommandCommand, Result>,
        IRequestHandler<DeleteCommandCommand, Result>
    {

        private IProductRepository _repoProduct;
        private ICommandRepository _repoCommand;
        private IMapper _mapper;

        public CommandHandler(IProductRepository repoProduct, ICommandRepository repoCommand, IMapper mapper)
        {
            _repoProduct = repoProduct;
            _mapper = mapper;
            _repoCommand = repoCommand;
        }


        public Task<Result> Handle(CreateCommandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Handle(UpdateCommandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Handle(DeleteCommandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
