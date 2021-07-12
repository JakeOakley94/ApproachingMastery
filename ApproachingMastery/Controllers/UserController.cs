using DatabaseInteraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApproachingMastery.Controllers
{
    public class UserController : Controller
    {
        private bool GetUserID(out Guid userID)
        {
            userID = Guid.Empty;
            object sessionObject = Session["LOGIN_ID"];
            if (sessionObject == null) return false;
            userID = (Guid)sessionObject;
            return true;
        }


        // GET: User
        public ActionResult Dashboard()
        {
            Guid userID = Guid.Parse(Session["LOGIN_ID"].ToString());
            UserLogin ul = new UserLogin(userID);
            ul.GetLoginInfo();
            return View(ul);
        }

        public ActionResult GetStudents()
        {
            if (Session["Login_ID"] == null)
                return RedirectToAction("Index", "Home");
            Guid userID = Guid.Parse(Session["LOGIN_ID"].ToString());
            UserLogin ul = new UserLogin(userID);
            ul.GetStudents();
            return Json(new
            {
                StudentCount = ul.Students.Count,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetStudents), ul)
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTeachers()
        {
            if (Session["Login_ID"] == null)
                return RedirectToAction("Index", "Home");
            Guid userID = Guid.Parse(Session["Login_ID"].ToString());
            UserLogin ul = new UserLogin(userID);
            ul.GetTeachers();
            return View(ul);
        }

        public ActionResult DeactivateStudent()
        {
            if (Request["StudentID"] == null) throw new Exception("StudentID is null");
            if (Session["LOGIN_ID"] == null) throw new Exception("Login ID cannot be null");
            Guid studentID = new Guid(Request["StudentID"]);
            UserLogin ul = new UserLogin((Guid)Session["LOGIN_ID"]);


            if (ul.RemoveStudent(studentID))
            {
                return Json(new { status = "Removed" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "Error removing student" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult EditAccount()
        {
            Guid userID;
            if (!GetUserID(out userID)) return new HttpUnauthorizedResult();
            return View();
        }

        public ActionResult EditUserProfile()
        {
            Guid userID;
            if (!GetUserID(out userID)) return Json(new HttpUnauthorizedResult());
            DatabaseInteraction.Models.User u;
            if (DatabaseInteraction.Models.User.GetUser(userID, out u))
            {
                return View(u);
            }
            else
            {
                return Json("<div>Error retreiving user information</div>", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult AddUser()
        {
            User u = new User();
            return View(u);
        }


        // Need Help Here
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditUserProfile(User u)
        {
            Guid userID;
            if (!GetUserID(out userID)) return Json(new { UpdateStatus = "Error"});
            u.UserLoginID = userID;
            if(!u.UpdateUserInformation()) return Json(new { UpdateStatus = "Error"});
            return Json(new { UpdateStatus = "Success" });
        }

        public ActionResult GetCalendar()
        {


            return View();
        }
    }
}