using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.services.Infrastructure;
using totalhr.Shared.Models;
using totalhr.Shared;
namespace totalhr.services.Implementation
{
    public class PayrollService : IPayrollService
    {
        public PayrollStruct GetUserPayrollDetails(int userId)
        {
            return new PayrollStruct
            {
                Currency = '£',
                PayrollNumber = "8004321",
                SSNumber = "PX123245",
                BasicSalary = 50000.00M,
                Bonus = 5,
                BonusType = Variables.BonusType.Percentage,
                TaxReference = "Ref1234",
                CarAllowance = 2000.00M,
                PayType = Variables.PayType.Monthly,
                BankAccountDetails = new PayrollStruct.BankDetails
                {
                    BankName = "BNP Paribas",
                    BankAddress = "1200 Les Moulains Sacres, Paris Cedex, France",
                    AccountName = "Blake le Rock",
                    AccountNumber = "4681234983",
                    SortCode = "30-21-45"
                }
            };
        }
    }
}
