using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using totalhr.Resources;
using System.ComponentModel.DataAnnotations;
using totalhr.Shared.Models.Attributes;

namespace totalhr.Shared.Models
{
    public class NewUserInfo : NewEmployeeInfo
    {       

        #region Companydetails

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_CompanyName_Rq")]
        [MaxLength(100, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Company_Name_Too_long")]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Email_Reminder")]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Invalid_Email")]
        [MaxLength(100, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Email_Too_long")]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Addres1_Rq")]
        [MaxLength(255, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Address1_Too_Long")]
        public string CompanyAddress1 { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Address2_Too_Long")]
        public string CompanyAddress2 { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Address3_Too_Long")]
        public string CompanyAddress3 { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_City_Rq")]
        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_City_Too_Long")]
        public string CompanyCity { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_State_Too_Long")]
        public string CompanyState { get; set; }

        [MaxLength(30, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Postcode_Too_Long")]
        public string CompanyPostCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Country_Required")]
        public int CompanyCountryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Phone1_Rq")]
        [MaxLength(30, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Phone_Too_Long")]
        public string CompanyPhone1 { get; set; }

        [MaxLength(30, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Phone_Too_Long")]
        public string CompanyPhone2 { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_TaxtID_Too_Long")]
        public string TaxID { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_VATID_Too_Long")]
        public string VATID { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_CompanyNo_Too_Long")]
        public string CompanyNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_CompanyNoEmployees_Rq")]
        public int CompanyNoEmployees { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_CompanyType_Too_Long")]
        public string CompanyType { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Annual_Revenue_Rq")]
        public decimal? AnnualRevenue { get; set; }

        [MaxLength(500, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Website_Too_Long")]
        public string Website { get; set; }

       
        #endregion


        /* used to validate terms and conditions checkbox */
        private bool MyTrue
        { get { return true; } }      

    }
}
