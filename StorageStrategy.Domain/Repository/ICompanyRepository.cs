using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Repository
{
    public interface ICompanyRepository : IRepositoryBase<CompanyEntity>
    {
        Task<List<CompanyEntity>> ToList();
    }
}
