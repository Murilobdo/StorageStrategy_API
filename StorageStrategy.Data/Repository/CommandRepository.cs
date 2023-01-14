using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository
{
    public class CommandRepository : RepositoryBase<CommandEntity>, ICommandRepository
    {
        public CommandRepository(StorageDbContext context) : base(context)
        {
        }
    }
}
