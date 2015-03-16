using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared.Models;

namespace totalhr.services.Infrastructure
{
    public interface IPayrollService
    {
        PayrollStruct GetUserPayrollDetails(int userId);
    }
}
