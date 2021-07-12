using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseInteraction.Models;
using System.Net;

namespace ApproachingMasteryUnitTests
{
    [TestClass]
    public class UserLoginTests
    {
        [TestMethod]
        public void LoginUserPass()
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");
            
            UserLogin ul = new UserLogin()
            {
                Email = "test_teacher@approaching-mastery.com",
                Password = "TPvP8ELz34"
            };
            LoginResult result = ul.Login(externalip);
            Assert.AreEqual(result, LoginResult.Success);
        }

        [TestMethod]
        public void LoginUserInvalidUserName()
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");

            UserLogin ul = new UserLogin()
            {
                Email = "asdfjkl@approaching-mastery.com",
                Password = "TPvP8ELz34"
            };
            LoginResult result = ul.Login(externalip);
            Assert.AreEqual(result, LoginResult.InvalidUserNameOrPassword);
        }

        [TestMethod]
        public void LoginUserInvalidPassword()
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");

            UserLogin ul = new UserLogin()
            {
                Email = "test_teacher@approaching-mastery.com",
                Password = "asdfj"
            };
            LoginResult result = ul.Login(externalip);
            Assert.AreEqual(result, LoginResult.InvalidUserNameOrPassword);
        }

        [TestMethod]
        public void LoginUserLockedAccount()
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");

            UserLogin ul = new UserLogin()
            {
                Email = "locked_account@approaching-mastery.com",
                Password = "7B86xLAL66"
            };
            LoginResult result = ul.Login(externalip);
            Assert.AreEqual(result, LoginResult.AccountLocked);
        }

        [TestMethod]
        public void GetLoginInfoTest()
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");
            string email = "test_teacher@approaching-mastery.com";
            UserLogin ul = new UserLogin()
            {
                Email = email,
                Password = "TPvP8ELz34"
            };
            ul.Login(externalip);
            ul.GetLoginInfo();
            Assert.AreEqual(ul.Email, email);
            Assert.AreEqual(ul.IsActive, true);
            Assert.AreEqual(ul.IsEmailVerified, true);
            Assert.AreEqual(ul.RoleID, 2);
        }


    }
}
