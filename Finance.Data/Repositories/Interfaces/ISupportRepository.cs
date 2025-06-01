using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Data.Models;

namespace Finance.Data.Repositories.Interfaces
{
    public interface ISupportRepository : IGenericRepository<SupportRequest>
    {
        Task AddRequestAsync(int? userId, string userName, string userEmail, string question);
    }
}
