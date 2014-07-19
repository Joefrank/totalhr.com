using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using totalhr.Resources;
using System.ComponentModel.DataAnnotations;
using totalhr.Shared.Models.Attributes;

namespace totalhr.Shared.Models
{
    public class NewUserInfo
    {
        #region personaldetails

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Gender_Rq")]
        public int GenderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Title_Rq")]
        public int Title { get; set; }

        [MaxLength(30, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_OtherTitle_Too_long")]
        public string OtherTitle { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Firstname_Rq")]
        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_FirstName_Too_long")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Surname_Rq")]
        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_SurName_Too_long")]
        public string Surname { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_OtherName_Too_long")]
        public string MiddleNames { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Email_Reminder")]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Invalid_Email")]
        [MaxLength(100, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Email_Too_long")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Addres1_Rq")]
        [MaxLength(255, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Address1_Too_Long")]
        public string Address1 { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Address2_Too_Long")]
        public string Address2 { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Address3_Too_Long")]
        public string Address3 { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_City_Rq")]
        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_City_Too_Long")]
        public string City { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_State_Too_Long")]
        public string State { get; set; }

        [MaxLength(30, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Postcode_Too_Long")]
        public string PostCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Country_Required")]
        public int CountryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Phone1_Rq")]
        [MaxLength(30, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_PersonalPhone_Too_Long")]
        public string PersonalPhone { get; set; }

        [MaxLength(30, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_MobilePhone_Too_Long")]
        public string MobilePhone { get; set; }

        [AlwaysTrue(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Terms_Not_Accepted_Rq")]
        public bool TermsAccepted { get; set; }


        #endregion


        #region logondetails

        public bool UseEmailAsUserName { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_UserName_Too_long")]
        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Username_Rq")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Password_Rq")]
        [StringLength(15, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Password_NotInRange", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Password_Conf_Rq")]
        [Compare("Password", ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Password_Not_Matched")]
        // [MaxLength(15, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_PasswordConfirm_Too_long")]
        [StringLength(15, ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_PasswordConfirm_NotInRange", MinimumLength = 6)]
        public string PasswordConfirm { get; set; }

        public int PreferedLanguageId { get; set; }

        #endregion


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

        #region additionalfiels

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        #endregion

    }
}
