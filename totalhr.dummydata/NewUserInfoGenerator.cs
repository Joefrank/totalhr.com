using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared.Models;

namespace totalhr.dummydata
{
    public class NewUserInfoGenerator
    {
        public NewUserInfo GetDummy()
        {
            return new NewUserInfo
            {
                GenderId = 1,
                Address1 = "Address 1",
                Address2 = "addres 2",
                AnnualRevenue = 200000,
                City = "London",
                Address3 = "",
                CompanyAddress1 = "Company Add 1",
                CompanyAddress2 = "compan add 2",
                CompanyAddress3 = "co 333",
                CompanyCity = "London",
                CompanyCountryId = 2,
                CompanyEmail = "admin@testcompany.co.uk",
                CompanyName = "Test Company 1",
                CompanyNoEmployees = 4000,
                CompanyNumber = "234nnn",
                CompanyPhone1 = "020790909090",
                CompanyPhone2 = "029890899898",
                CompanyPostCode = "E3 2LB",
                CompanyState = "England",
                CompanyType = "Limited Company",
                CountryId = 5,
                Email = "jbolla@cyberminds.co.uk", //"tester.dummy@totalhr.com",
                FirstName = "Joe",
                MiddleNames = "M",
                MobilePhone = "0798888888",
                OtherTitle = "",
                Password = "Password123",
                PasswordConfirm = "Password123",
                PersonalPhone = "078800000000",
                PostCode = "E3 4LB",
                PreferedLanguageId = 1,
                State = "",
                Surname = "dummy_test_test",
                TaxID = "TXT111",
                TermsAccepted = true,
                Title = 1,
                UseEmailAsUserName = false,
                UserName = "joe_tester",
                VATID = "VAT123"

            };
        }
    }
}
