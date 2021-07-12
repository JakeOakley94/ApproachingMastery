using DatabaseInteraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApproachingMastery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult Dashboard ()
        {
            if (Session["LOGIN_ID"] == null) return RedirectToAction(nameof(Index));
            Guid userID = (Guid)Session["LOGIN_ID"];
            UserLogin u = new UserLogin(userID);
            User u2 = new User() { UserLoginID = u.UserID };


            int GoalsComplete = 0;
            GoalsComplete = u.GetGoalCounts(GoalsComplete, false);
            int GoalsNotCompleted = 0;
            GoalsNotCompleted = u.GetGoalCounts(GoalsNotCompleted, true);
            u.GetStudents();
            u.GetTeachers();
            DatabaseInteraction.Models.User.GetUser((Guid)Session["LOGIN_ID"], out u2);
            ViewBag.Title = "Dashboard";
            ViewBag.UserName = 
            ViewBag.TeacherCount = u.OtherTeachers.Count + 1;
            ViewBag.StudentCount = u.Students.Count;
            ViewBag.UsersName = u2.FirstName;
            ViewBag.GoalCompleteCount = 4;
            ViewBag.PendingGoalCount = 9;
            return View();

        }

        public ActionResult LogOff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }

    }

}