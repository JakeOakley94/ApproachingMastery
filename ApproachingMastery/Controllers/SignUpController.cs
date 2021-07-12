using DatabaseInteraction;
using DatabaseInteraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ApproachingMastery.Controllers
{
    public class SignUpController : Controller
    {
        public ActionResult Index(UserLogin model = null)
        {
            if (model == null) model = new UserLogin();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailCheck(FormCollection col)
        {

            UserLogin model = new UserLogin() { Email = col["Email"] };

            if (!UserLoginController.VerifyCaptcha(Request["g-recaptcha-response"]))
                return View(nameof(Index), model);

            const string expression = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            Regex re = new Regex(expression);
            if (!re.IsMatch(model.Email))
                return View(nameof(Index), model);

            string domain = model.Email.Substring(model.Email.IndexOf("@") + 1);
            if (SchoolDistrict.CheckDomainExists(domain))
            {
                Models.AddEditAccount account = new Models.AddEditAccount()
                {
                    UserLogin = model
                };
                Session["EmailAddress"] = model.Email;
                return View(nameof(CreateAccount), account);
            }
            return View(nameof(Index), model);
        }

        public ActionResult CreateAccount()
        {
            /*if (Session["EmailAddress"] == null) return RedirectToAction(nameof(Index));*/
            return View(new Models.AddEditAccount());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(FormCollection col)
        {
            UserLogin ul = new UserLogin()
            {
                Email = Session["EmailAddress"].ToString(),
                Password = col["UserLogin.Password"],
            };
            User user = new User()
            {
                FirstName = col["UserInformation.FirstName"],
                MiddleName = col["UserInformation.MiddleName"],
                LastName = col["UserInformation.LastName"],
                PhoneNumber = col["UserInformation.PhoneNumber"]
            };

            

            if (!ul.CreateNewAccount(HelperFunctions.GetClientIPAddress(Request), user))
            {
                ViewBag.ErrorMessage = "Could not create your account, please verify that your information is correct and try again";
                return View(new Models.AddEditAccount()
                {
                    UserLogin = ul,
                    UserInformation = user
                });
            }
            else
            {
                Session["EmailAddress"] = null;
                Session["UserID"] = ul.UserID;
                string messageText = "";
                string path = System.Web.HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/ConfirmAccount.html");
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                {
                    messageText = sr.ReadToEnd();
                    sr.Close();
                }

                messageText = messageText.Replace("<guid_here>", ul.EmailValidationID.ToString());

                Emailer.SendEmail(Emailer.Senders.DoNotReply, ul.Email, "Confirm Email Address", messageText, true);
                return RedirectToAction(nameof(Confirm));
            }
        }
        public ActionResult Confirm()
        {
            UserLogin ul = new UserLogin();
            if (Request["guid"] != null)
            {
                ul = new UserLogin() { EmailValidationID = Guid.Parse(Request["guid"].ToString()) };
                ul.ConfirmEmail();
            }
            return View(ul);
        }

        public ActionResult EditAccount()
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Session id cannot be null");
            DatabaseInteraction.Models.User u;
            DatabaseInteraction.Models.User.GetUser((Guid)Session["LOGIN_ID"], out u);
            return View(u);
        }
        [HttpPost]
        public ActionResult EditAccount(Models.AddEditAccount ca)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Session id cannot be null");
            
            if (ModelState.IsValid)
            {
                ca.UserInformation.UserLoginID = (Guid)Session["LOGIN_ID"];
                ca.UserInformation.UpdateUserInformation();
                User u;
                DatabaseInteraction.Models.User.GetUser(ca.UserInformation.UserLoginID, out u);
                ca.UserInformation = u;
            }
            return Json(ca);
        }
    }
}