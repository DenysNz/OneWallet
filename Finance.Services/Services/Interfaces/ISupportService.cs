using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Services.Services.Interfaces
{
    public interface ISupportService
    {
        Task AddRequestAsync(int? userId, string userName, string userEmail, string question);
    }
}
