using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using totalhr.services.Implementation;
using totalhr.services.Infrastructure;
using totalhr.data.Repositories.Implementation;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;
using totalhr.dummydata;
using totalhr.Shared.Models;
using totalhr.Shared.Infrastructure;
using totalhr.Shared;
using totalhr.services.messaging.Infrastructure;
using totalhr.services.messaging.Implementation;
using totalhr.tests.libs;


namespace totalhr.services.Test
{
    [TestClass]
    public class AccountServiceTests : BaseTest
    {
       
        private NewUserInfo _dummyuserinfo;
        private IAccountService _aservice;
       
        public AccountServiceTests()
        {
            _dummyuserinfo = (new NewUserInfoGenerator()).GetDummy();
            _aservice = ninjectKernel.Get<AccountService>();
            _aservice.ClearUserDataByEmail(_dummyuserinfo.Email);
        }

        [TestMethod]
        public void CanCreateCompany()
        {
            Company company = _aservice.CreateCompany(_dummyuserinfo);

            Assert.IsTrue(company.ID > 0);
        }

        [TestMethod]
        public void CanCreateUser()
        {
            User user = _aservice.CreateUser(_dummyuserinfo);

            Assert.IsTrue(user.id > 0);
        }

        [TestMethod]
        public void CanGenerateUserActivationLink()
        {
            string email = "test@yahoo.com";
            string encryptedemail = CM.Security.Encrypt(email);
            string activationlink = _aservice.GenerateUserActivationLink(email);
            Assert.AreEqual(encryptedemail, activationlink);

        }

        [TestMethod]
        public void CanCheckExistingUserByEmail()
        {
            //insert dummy user first
            User user = _aservice.CreateUser(_dummyuserinfo);
            //check if that user exists
            bool exists = _aservice.UserExistByEmail(_dummyuserinfo.Email);

            Assert.IsTrue(exists);
            
        }

        [TestMethod]
        public void CanGetUserByEmail()
        {
            User user;

            if (!_aservice.UserExistByEmail(_dummyuserinfo.Email))
            {
                user = _aservice.CreateUser(_dummyuserinfo);
            }

            user = _aservice.GetUserByEmail(_dummyuserinfo.Email);

            Assert.IsTrue(user.id > 0);
        }

        [TestMethod]        
        public void CanRegisterUserCompany()
        {
            //clear all test email first
            ClearTestData();          
            
            UserRegStruct userstruct = _aservice.RegisterUserCompany(_dummyuserinfo, _dummystruct);

            Assert.IsTrue(userstruct.UserId > 0 && userstruct.CompanyId > 0);
        }

        [TestMethod]
        public void CanNotRegisterUserCompanyIfUserEmailExist()
        {
            //create user with this dummy email
            User user = _aservice.CreateUser(_dummyuserinfo);

            if (user == null)
            {
                throw new Exception("failed to create user in test method 'CanNotRegisterUserCompanyIfUserEmailExist'");
                
            }
            //now try to register another user with that email
            UserRegStruct userstruct = _aservice.RegisterUserCompany(_dummyuserinfo, _dummystruct);

            Assert.IsTrue(userstruct.UserId ==-1 && userstruct.CompanyId ==-1);
        }

        [TestMethod]
        public void CanNotRegisterUserCompanyIfUserUsernameExist()
        {
            User user = _aservice.GetUserByUsername(_dummyuserinfo.UserName) ?? _aservice.CreateUser(_dummyuserinfo);

            //now try to register another user with that email
            _dummyuserinfo.Email = "dum_dum_" + _dummyuserinfo.Email; 
            UserRegStruct userstruct = _aservice.RegisterUserCompany(_dummyuserinfo, _dummystruct);

            Assert.IsTrue(userstruct.UserId == -1 && userstruct.CompanyId == -1);
        }

        [TestMethod]
        public void CanActivateUser()
        {
            throw new Exception("not implemented");
        }


        private void ClearTestData()
        {
            _aservice.ClearUserDataByEmail(_dummyuserinfo.Email);
        }
       
    }
}
