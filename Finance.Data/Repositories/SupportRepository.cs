using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;

namespace Finance.Data.Repositories
{
    public class SupportRepository : GenericRepository<SupportRequest>, ISupportRepository
    {
        protected readonly new FinanceDbContext _dbContext;
        public SupportRepository(FinanceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRequestAsync(int? userId, string userName, string userEmail, string question)
        {
            var request = new SupportRequest();
            request.UserDetailId = userId;
            request.RequestName = userName;
            request.RequestEmail = userEmail;
            request.RequestQuestion = question;
            request.CreatedDate = DateTime.UtcNow;

            await _dbContext.SupportRequests.AddAsync(request);
            await _dbContext.SaveChangesAsync();
        }
    }
}
