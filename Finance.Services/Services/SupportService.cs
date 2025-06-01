using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Data.Repositories.Interfaces;
using Finance.Services.Services.Interfaces;

namespace Finance.Services.Services
{
    public class SupportService : ISupportService
    {
        private ISupportRepository _supportRepository;

        public SupportService(ISupportRepository supportRepository)
        {
            _supportRepository = supportRepository;
        }

        public async Task AddRequestAsync(int? userId, string userName, string userEmail, string question)
        {
            await _supportRepository.AddRequestAsync(userId, userName, userEmail, question);
        }
    }
}
