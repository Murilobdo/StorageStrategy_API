using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Tests.Factory.Entity;
using StorageStrategy.Tests.Faktory.Entity;

namespace StorageStrategy.Tests.FakeRepository
{
    public class FakeCommandRepository : ICommandRepository
    {
        public readonly List<CommandEntity> commands;

        public readonly List<CommandItemEntity> items;

        public FakeCommandRepository()
        {
            commands = new List<CommandEntity>()
            {
                new CreateCommandEntityWithItemsFactory().command,
                new CreateCommandEntityWithoutItemFactory().command
            };
        }

        public Task AddAsync(CommandEntity model)
        {
            return Task.CompletedTask;
        }

        public Task<List<CommandEntity>> GetAllAsync(Func<CommandEntity, bool> filter, string[] includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Task AddItemsAsync(IEnumerable<CommandItemEntity> items)
        {
            if(items.Count() > 0)
            {
                int commandId = items.First().CommandId;

                foreach (var command in commands)
                {
                    if (commandId == command.CommandId)
                        command.Items.AddRange(items);
                }
            }


            commands[1].Items.AddRange(items);
            return Task.CompletedTask;
        }

        public async Task AddItemsAsync(CommandItemEntity item)
        {
            commands[1].Items.Add(item);
        }

        public void Clear()
        {
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public Task CreateTranscationAsync()
        {
            return Task.CompletedTask;
        }

        public void Delete(CommandEntity id)
        {
        }

        public Task<CommandEntity> GetById(int id)
        {
            return Task.FromResult(commands.FirstOrDefault(p => p.CommandId == id));
        }

        public Task<CommandEntity> GetCommandByIdAsync(int commandId, int companyId)
        {
            return Task.FromResult(commands.FirstOrDefault(p => p.CommandId == commandId  && p.CompanyId == companyId));
        }

        public Task<List<CommandItemEntity>> ReadCommandsForDaysAsync(int companyId, int day, int month)
        {
            var result =  commands
                .Where(p => p.CompanyId == companyId)
                .Where(p => p.FinalDate != null && p.FinalDate.Value.Day == day && p.FinalDate.Value.Month == month)
                .SelectMany(p => p.Items)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<List<CommandEntity>> ReadCommandsForPeriodAsync(int companyId, int initialMonth, int finalMounth = 0)
        {
            var query = commands
                .Where(p => p.FinalDate != null)
                .Where(p => p.InitialDate.Month == initialMonth)
                .Where(p => p.CompanyId == companyId)
                .AsQueryable();

            if (finalMounth > 0)
                query = query.Where(p => p.FinalDate.Value.Month == finalMounth);

            return Task.FromResult(query.ToList());
        }

        public Task<List<CommandEntity>> ReadCommandsForPeriodWithItensAsync(int companyId, int month)
        {
            var response = commands
                .Where(p => p.FinalDate != null)
                .Where(p => p.InitialDate.Month == month)
                .Where(p => p.CompanyId == companyId)
                .ToList();

            return Task.FromResult(response);
        }

        public Task<decimal> ReadTotalSalesByCompany(int companyId, int month)
        {
            decimal result = commands
                .Where(p => p.CompanyId == companyId)
                .Where(p => p.FinalDate.Value != null && p.FinalDate.Value.Month == month)
                .Sum(p => p.TotalPrice);

            return Task.FromResult(result);
        }

        public void UpdateCommandItemAsync(CommandItemEntity productItemDb)
        {
        }

        public Task RemoveCommandItemsAsync(List<CommandItemEntity> items)
        {
            if(items.Count > 0)
            {
                int commandId = items[0].CommandId;
                foreach (var command in commands)
                {
                    if (commandId == command.CommandId)
                        command.Items = new List<CommandItemEntity>();
                }
            }

            return Task.CompletedTask;
        }

        public void RemoveRange(CommandEntity model)
        {
        }

        public Task RollbackAsync()
        {
            return Task.CompletedTask;
        }

        public void Save()
        {
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }

        public Task<List<CommandEntity>> ToListAsync(int companyId, bool haveEndDate)
        {
            var response = new List<CommandEntity>();
            
            if (haveEndDate)
                response = commands
                    .Where(p => p.CompanyId == companyId)
                    .Where(p => p.FinalDate != null)
                    .ToList();
            else
                response = commands
                    .Where(p => p.CompanyId == companyId)
                    .Where(p => p.FinalDate == null)
                    .ToList();

            return Task.FromResult(response);
        }

        public void Update(CommandEntity model)
        {
        }
    }
}
