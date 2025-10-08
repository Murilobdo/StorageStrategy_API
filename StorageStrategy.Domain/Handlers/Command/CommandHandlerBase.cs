using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Command;

public class CommandHandlerBase<T> : HandlerBase, IRequestHandler<T, Result> where T : IRequest<Result>
{
    protected IProductRepository _repoProduct;
    protected ICommandRepository _repoCommand;
    protected IEmployeeRepository _repoEmployee;
    protected IMapper _mapper;
    protected IClientRepository _clientRepo;

    public CommandHandlerBase(IProductRepository repoProduct, ICommandRepository repoCommand, IEmployeeRepository repoEmployee, IMapper mapper, IClientRepository clientRepo)
    {
        _repoProduct = repoProduct;
        _repoCommand = repoCommand;
        _repoEmployee = repoEmployee;
        _mapper = mapper;
        _clientRepo = clientRepo;
    }

    public virtual Task<Result> Handle(T request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}