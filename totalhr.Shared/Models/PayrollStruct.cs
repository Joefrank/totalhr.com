using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared;

namespace totalhr.Shared.Models
{
    public class PayrollStruct
    {
        public char Currency { get; set; }

        public string PayrollNumber { get; set; }

        public string SSNumber { get; set; }

        public string TaxReference { get; set; }

        public decimal Rate { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal CarAllowance { get; set; }

        public decimal Bonus { get; set; }
       
        public Variables.BonusType BonusType { get; set; }

        public Variables.PayType PayType { get; set; }

        public BankDetails BankAccountDetails { get; set; }

       
        public class BankDetails
        {
            public string BankName { get; set; }

            public string BankAddress { get; set; }

            public string AccountName { get; set; }

            public string AccountNumber { get; set; }

            public string SortCode { get; set; }

            public string BankRef { get; set; }
        }
    }
}
