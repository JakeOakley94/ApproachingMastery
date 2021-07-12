using DatabaseInteraction;
using DatabaseInteraction.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ApproachingMastery.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: UserLogin
        public ActionResult UserLogin()
        {
            return View(new DatabaseInteraction.Models.UserLogin());
        }

        [HttpPost]
        public ActionResult UserLogin(FormCollection col)
        {
            if (Session["LOGIN_ID"] != null)
                RedirectToAction("Dashboard", "User");
            if (!VerifyCaptcha(Request["g-recaptcha-response"]))
            {
                RedirectToAction(nameof(UserLogin));
            }


            DatabaseInteraction.Models.UserLogin li = new DatabaseInteraction.Models.UserLogin()
            {
                Password = col["Password"],
                Email = col["Email"]
            };

            LoginResult loginResult = li.Login(Request.UserHostAddress);

            switch (loginResult)
            {
                case LoginResult.Success:
                    Session["LOGIN_ID"] = li.UserID;
                    return RedirectToAction("Dashboard", "Home");
            }
            return RedirectToAction("../Home/Index", new { ShowLogin = 1 });
        }

        internal static bool VerifyCaptcha(string encodedResponse)
        {
            string secretKey = "6LdUAZgUAAAAAAR-xgGk7Sen9pBvjNacc0iu0dW_";
            bool result = false;
            string captchaAPIUri = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={encodedResponse}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(captchaAPIUri);
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = isSuccess;
                }
            }
            return result;
        }

        public ActionResult ForgotPassword()
        {
            return View(new DatabaseInteraction.Models.UserLogin());
        }

        [HttpPost]
        public ActionResult ForgotPassword(FormCollection col)
        {
            DatabaseInteraction.Emailer.SendEmail(DatabaseInteraction.Emailer.Senders.DoNotReply, col["Email"], "Forgot Password", "test", false);
            return RedirectToAction("../Home/Index");
        }

        public ActionResult AddPassword(UserLogin ul)
        {
            return View(ul);
        }

      

        public ActionResult EditUserPassword()
        {
            if (Session["LOGIN_ID"] == null) return new HttpUnauthorizedResult();
            return View(new UserLogin() { IsEditing = true });
        }

        [HttpPost]
        public ActionResult EditUserPassword(UserLogin ul)
        {
            if (Session["LOGIN_ID"] == null) return new HttpUnauthorizedResult();
            ul.UserID = (Guid)Session["LOGIN_ID"];
            DatabaseInteraction.Models.UserLogin temp;
            DatabaseInteraction.Models.UserLogin.GetUserLogin(ul.UserID, out temp);
            temp.Password = ul.ExistingPassword;
            if (temp.Login(Request.UserHostAddress)==LoginResult.Success)
            {
                ul.Email = temp.Email;
                ul.RoleID = temp.RoleID;

                PasswordResetResult result = ul.UpdatePassword(Request.UserHostAddress);

                if (result==PasswordResetResult.Success)
                {
                    return Json(new { PasswordResult = "Success"});
                }
                else
                {
                    string errorMessage = result == PasswordResetResult.PreviousPassword ? "The new password cannot match any of your 7 last password" : "Unkown error updating password";
                    return Json(new { PasswordResult = "Error", ErrorMessage=errorMessage });
                }
            }
            else
            {
                return Json(new { PasswordResult = "Error", ErrorMessage = "Invalid Password" });
            }
        }

    }

}