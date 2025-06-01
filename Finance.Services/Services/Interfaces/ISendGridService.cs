using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Services.Services.Interfaces
{
    public interface ISendGridService
    {
        Task SendSecurityCodeAsync(string emailTo, string code);

        Task SendEmailAsync(string emailTo, string code, string emailSubject);
    }
}
