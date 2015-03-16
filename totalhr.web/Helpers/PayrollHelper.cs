using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.Shared;
using totalhr.Shared.Models;

namespace totalhr.web.Helpers
{
    public class PayrollHelper
    {
        public static decimal CalculateBonus(PayrollStruct payroll)
        {
            if (payroll.BonusType == Variables.BonusType.Percentage)
            {
                return (payroll.BasicSalary * payroll.Bonus) / 100;
            }
            else 
            {
                return payroll.Bonus;
            }
        }

        public static string BonusDisplay(PayrollStruct payroll)
        {
            if (payroll.BonusType == Variables.BonusType.Percentage)
            {
                return  payroll.Bonus + " %";
            }
            else 
            {
                return payroll.Currency.ToString() + payroll.Bonus;
            }
        }
    }
}