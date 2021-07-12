using ApproachingMastery.Models;
using DatabaseInteraction.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApproachingMastery.Controllers
{
    public class StudentController : Controller
    {

        #region Display/StudentViewer

        //Get: Display Student Name
        public ActionResult displayStudentName()
        {

            Guid studentID = Guid.Parse(Request["StudentID"]);
            Student s = new Student(studentID);
            return View(s.FirstName, s.LastName);
        }


        public ActionResult StudentViewer()
        {
            //User Info
            User u2 = new User();
            DatabaseInteraction.Models.User.GetUser((Guid)Session["LOGIN_ID"], out u2);
            ViewBag.UsersName = u2.FirstName;

            //Student Info
            if (Request["StudentID"] == null) throw new Exception("Student id cannot be null");
            Guid studentID = Guid.Parse(Request["StudentID"]);
            Student s;
            Student.GetStudent(studentID, out s);
            return View(s);
        }

        #endregion

        #region Add/Edit_Student

        // GET: Student
        public ActionResult AddStudent()
        {
            Guid currentUserID = new Guid(Session["LOGIN_ID"].ToString());
            AddEditStudentModal modal = new AddEditStudentModal(currentUserID);
            return View(modal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(FormCollection col)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                Guid currentUserID = new Guid(Session["LOGIN_ID"].ToString());
                UserLogin ul = new UserLogin(currentUserID);

                string school = col["StudentSchool"];

                if (!Convert.ToBoolean(col["AddExistingStudent"]))
                {
                    result = ul.AddStudent(new Student(col["NewStudent.FirstName"], col["NewStudent.MiddleName"], col["NewStudent.LastName"],
                        DateTime.ParseExact(col["NewStudent.Birthday"], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        col["NewStudent.GradeLevel"],
                        DateTime.ParseExact(col["NewStudent.IEPDueDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        DateTime.ParseExact(col["NewStudent.ETRDueDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture)), new Guid(col["StudentSchool"]));
                }
                else
                {
                    result = ul.AddStudent(new Student(new Guid(col["ExistingStudent"].ToString())), Guid.Empty);
                }
            }
            if (result) return Json(new { Success = true, Message = "Student Added Succesfully" }, JsonRequestBehavior.AllowGet);
            else return Json(new { Success = false, Message = "Error Adding Student" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EditStudent(Guid studentID)
        {
            AddEditStudentModal aesm = new AddEditStudentModal((Guid)Session["LOGIN_ID"]);
            Student s;
            Student.GetStudent(studentID, out s);
            aesm.NewStudent = s;
            aesm.IsEditing = true;
            return View(aesm);
        }

        [HttpPost]
        public ActionResult EditStudent(AddEditStudentModal aesm)
        {

            if (aesm.NewStudent.AddUpdateStudent())
                return Json(new { Success = true, Message = "Student updated succesfully" }, JsonRequestBehavior.AllowGet);
            else return Json(new { Success = false, Message = "Error updating student" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateDueDates(FormCollection col)
        {
            NameValueCollection request = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
            if (request["StudentID"] == null) throw new Exception("Student ID cannot be null");

            Student s;
            Student.GetStudent(new Guid(request["StudentID"]), out s);

            s.IEPDueDate = DateTime.ParseExact(col["IEPDueDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            s.ETRDueDate = DateTime.ParseExact(col["ETRDueDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            s.AddUpdateStudent();
            return Redirect(Request.UrlReferrer.ToString());
        }



        #endregion

        #region Assignments

        //Get: Assignment Evaluation Tests
        public ActionResult Assignment()
        {
            return View();
        }

        #endregion

        #region Add/Edit Goals

        public JsonResult AddAcademicGoal()
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            Goal g = new Goal()
            {
                GoalArea = GoalArea.Academic,
                DueDate = DateTime.Now + new TimeSpan(7, 0, 0, 0)
            };

            return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, "AddAcademicGoal", g) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditAcademicGoal(Guid goalID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;

            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.GetAcademicGoals())
            {
                Goal g = s.AcademicGoals.Find(goal => goal.GoalID == goalID);
                return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(EditAcademicGoal), g) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = "Error loading selected goal" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EditAcademicGoal(Goal goal)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;

            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.AddGoal(goal))
                return Json(new { Success = true, Message = "Goal Updated!" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error updating selected goal" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddAcademicGoal(Goal g)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            g.AssignedBy = (Guid)Session["LOGIN_ID"];
            if (s.AddGoal(g))
                return Json(new { Success = true, Message = "New Goal Added" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = true, Message = "Error adding new goal" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAcademicGoal(Guid goalID)
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.RemoveGoal(new Goal() { GoalID = goalID }))
                return Json(new { Success = true, Message = "Goal removed!" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = true, Message = "Error removing goal" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddBehaviorGoal()
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            Goal g = new Goal()
            {
                GoalArea = GoalArea.Behavioral,
                DueDate = DateTime.Now + new TimeSpan(7, 0, 0, 0)
            };

            return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, "AddBehaviorGoal", g) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditBehaviorGoal(Guid goalID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;

            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.GetBehavioralGoals())
            {
                Goal g = s.BehavioralGoals.Find(goal => goal.GoalID == goalID);
                return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(EditBehaviorGoal), g) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = "Error loading selected goal" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EditBehaviorGoal(Goal goal)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;

            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.AddGoal(goal))
                return Json(new { Success = true, Message = "Goal Updated!" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error updating selected goal" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddBehaviorGoal(Goal g)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            g.AssignedBy = (Guid)Session["LOGIN_ID"];
            if (s.AddGoal(g))
                return Json(new { Success = true, Message = "New Goal Added" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = true, Message = "Error adding new goal" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBehaviorGoal(Guid goalID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.RemoveGoal(new Goal() { GoalID = goalID }))
                return Json(new { Success = true, Message = "Goal removed!" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = true, Message = "Error removing goal" }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddGoalAssignment(Guid goalID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            Assignment assign = new Assignment()
            {
                GoalID = goalID
            };

            return Json(new { Success = false, html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(AddGoalAssignment), assign) }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult AddGoalAssignment(Assignment assignment)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            assignment.AddedBy = new User() { UserLoginID = (Guid)Session["LOGIN_ID"] };
            Goal g = new Goal() { GoalID = assignment.GoalID };
            if (g.AddAssignment(assignment))
                return Json(new { Success = true, Message = "Assignment added!", goalID = g.GoalID }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error adding assignment!", goalID = g.GoalID }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteGoalAssignment(Guid goalID, int assignmentID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Goal g = new Goal() { GoalID = goalID };
            if (g.RemoveAssignment(assignmentID))
                return Json(new { Success = true, Message = "Assignment removed!" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error removing assignment!" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditGoalAssignment(Guid goalID, int assignmentID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");

            Goal g = new Goal() { GoalID = goalID };
            if (g.GetAssignment())
            {
                Assignment assignToEdit = g.Assignments.Find(a => a.AssignmentID == assignmentID);
                if (assignToEdit != null)
                    return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(EditGoalAssignment), assignToEdit) },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, Message = "Error getting assignment" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditGoalAssignment(Assignment assignment)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            assignment.AddedBy = new User() { UserLoginID = (Guid)Session["LOGIN_ID"] };
            Goal g = new Goal() { GoalID = assignment.GoalID };
            if (g.AddAssignment(assignment))
                return Json(new { Success = true, Message = "Assignment updated!", goalID = g.GoalID }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error adding updated!", goalID = g.GoalID }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetGoalAssignments(Guid? goalID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");

            Goal g = new Goal() { GoalID = goalID };
            if (g.GetAssignment())
                return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, "GoalAssignments", g) }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error getting assignments!", goalID = g.GoalID }, JsonRequestBehavior.AllowGet);
        }






        #endregion

        #region Accomodations

        //Get: Accomendation Evaluation Tests
        public JsonResult Accommodations()
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            Accomodation a = new Accomodation() { StudentID = s.StudentID };

            return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(Accommodations), a) }, JsonRequestBehavior.AllowGet);

        }

        private bool GetCurrrentUser(out Guid currentUser)
        {
            currentUser = Guid.Empty;
            var sessionVar = Session["LOGIN_ID"];
            if (sessionVar == null) return false;
            currentUser = (Guid)sessionVar;
            return true;
        }

        [HttpPost]
        public JsonResult AddAccommodation(Accomodation accomodation)
        {
            Guid currentUser;
            if (!GetCurrrentUser(out currentUser)) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            accomodation.AddedBy = currentUser;
            if (s.AddAccomodation(accomodation))
            {
                return Json(new { Success = true, Message = "Accommodation Added!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = "Error adding accommodation" }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddAccommodationDetail(short accommodationID)
        {
            try
            {
                Guid currentUser;
                if (!GetCurrrentUser(out currentUser)) throw new Exception("Not Authorized");
                Student s;
                if (!GetStudentFromReferrer(out s))
                {
                    return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
                }

                AccomodationDetail detail = new AccomodationDetail() { StudentID = s.StudentID, AccomodationID = accommodationID };
                return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(AddAccommodationDetail), detail) }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false, Message = "Error creating window" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult EditAccommodationDetail(short accommodationID, DetailType detailType)
        {
            try
            {
                Guid currentUser;
                if (!GetCurrrentUser(out currentUser)) throw new Exception("Not Authorized");
                Student s;
                if (!GetStudentFromReferrer(out s))
                {
                    return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
                }
                if (!s.GetAccomodations())
                {
                    return Json(new { Success = false, Message = "Error getting current student accommodations" }, JsonRequestBehavior.AllowGet);
                }
                Accomodation ac = s.Accomodations.Find(a => a.AccomodationID == accommodationID);

                if (ac == null) return Json(new { Success = false, Message = "Error getting current student accommodations" }, JsonRequestBehavior.AllowGet);

                AccomodationDetail detail = ac.AccomodationDetails != null ? ac.AccomodationDetails.Find(d => d.DetailType == detailType) : null;
                if (detail == null) return Json(new { Success = false, Message = "Error getting current accommodation detail" }, JsonRequestBehavior.AllowGet);
                detail.AccomodationID = ac.AccomodationID;
                return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(EditAccommodationDetail), detail) }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false, Message = "Error creating window" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult EditAccommodationDetail(AccomodationDetail detail)
        {
            try
            {
                Guid currentUser;
                if (!GetCurrrentUser(out currentUser)) throw new Exception("Not Authorized");
                Student s;
                if (!GetStudentFromReferrer(out s))
                {
                    return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
                }
                if (!s.GetAccomodations())
                {
                    return Json(new { Success = false, Message = "Error getting current student accommodations" }, JsonRequestBehavior.AllowGet);
                }
                detail.StudentID = s.StudentID;
                if (!detail.UpdateDetail())
                    return Json(new { Success = false, Message = "Error updating detail" }, JsonRequestBehavior.AllowGet);
                return Json(new { Success = true, Message = "Detail Updated" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false, Message = "Error creating window" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult RemoveAccommodation(short accommodationID)
        {
            Guid currentUser;
            if (!GetCurrrentUser(out currentUser)) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.RemoveAccommodation(accommodationID))
                return Json(new { Success = true, Message = "Accommodation Removed" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error removing accommodation" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveAccomodationTest(Guid accomodationTestID)
        {
            Guid currentUser;
            if (!GetCurrrentUser(out currentUser)) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.RemoveAccommodationTest(accomodationTestID))
                return Json(new { Success = false, Message = "Test deleted" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error deleteing test" }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public JsonResult RemoveAccommodationDetail(short accommodationID, DetailType detailType)
        {
            Guid currentUser;
            if (!GetCurrrentUser(out currentUser)) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            Accomodation ac = new Accomodation() { StudentID = s.StudentID, AccomodationID = accommodationID };
            if (ac.RemoveAccommodationDetail(detailType))
                return Json(new { Success = true, Message = "Accommodation detail removed" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error removing accommodation detail" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddAccommodationDetail(AccomodationDetail detail)
        {
            try
            {
                Guid currentUser;
                if (!GetCurrrentUser(out currentUser)) throw new Exception("Not Authorized");
                Student s;
                if (!GetStudentFromReferrer(out s))
                {
                    return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
                }

                Accomodation ac = new Accomodation() { StudentID = s.StudentID, AccomodationID = detail.AccomodationID };
                if (ac.AddAccommodationDetail(detail))
                    return Json(new { Success = true, Message = "Detail added!" }, JsonRequestBehavior.AllowGet);
                return Json(new { Success = false, Message = "Error adding detial" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false, Message = "Error adding detial" }, JsonRequestBehavior.AllowGet);
            }

        }


        //Get: Accomendation List 
        public JsonResult GetAccomodationsList()
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (s.GetAccomodations())
                return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetAccomodationsList), s) }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = false, Message = "Error getting accomodations" }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult AddAccomodationTest()
        {

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            if (s.GetAccomodations())
            {

                ApproachingMastery.Models.AddEditAccomodationTestModel aeatm = new AddEditAccomodationTestModel()
                {

                    s = s

                };

                return Json(new
                {
                    Success = true,
                    html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(AddAccomodationTest), aeatm)
                },
                JsonRequestBehavior.AllowGet);


            }

            return Json(new { Success = false, Message = "Error getting accomodations" }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccomodationTest(AddEditAccomodationTestModel aeatm)
        {
            bool result = false;

            NameValueCollection request = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);

            if (request["StudentID"] == null) throw new Exception("Student ID cannot be null");
            Guid studentID = new Guid(request["StudentID"]);

            Student s = new Student(studentID);

            result = s.AddAccomodationTest(studentID, aeatm.Test);

            Guid currentUserID = new Guid(Session["LOGIN_ID"].ToString());
            UserLogin ul = new UserLogin(currentUserID);


            if (result) return Json(new { Success = true, Message = "Accomodation test added" }, JsonRequestBehavior.AllowGet);
            else return Json(new { Success = false, Message = "Error adding Accomodation test" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetMathAccomodationTests()
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            if (!s.GetAccomodationTests())
                return Json(new { Success = false, Message = "Error getting accomodation tests for the current student" }, JsonRequestBehavior.AllowGet);

            s.AccomodationTests.Sort((t1, t2) => t2.Date.CompareTo(t1.Date));
            s.AccomodationTests = s.AccomodationTests.FindAll(t => t.Type == AccomodationTest.AccomodationTestType.Math);
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetMathAccomodationTests), s)
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReadingAccomodationTests()
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            if (!s.GetAccomodationTests())
                return Json(new { Success = false, Message = "Error getting accomodation tests for the current student" }, JsonRequestBehavior.AllowGet);

            s.AccomodationTests.Sort((t1, t2) => t2.Date.CompareTo(t1.Date));
            s.AccomodationTests = s.AccomodationTests.FindAll(t => t.Type == AccomodationTest.AccomodationTestType.Reading);
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetReadingAccomodationTests), s)
            },
            JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetWritingAccomodationTests()
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            if (!s.GetAccomodationTests())
                return Json(new { Success = false, Message = "Error getting accomodation tests for the current student" }, JsonRequestBehavior.AllowGet);

            s.AccomodationTests.Sort((t1, t2) => t2.Date.CompareTo(t1.Date));
            s.AccomodationTests = s.AccomodationTests.FindAll(t => t.Type == AccomodationTest.AccomodationTestType.Writing);
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetWritingAccomodationTests), s)
            },
            JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditAccomodationTest(Guid AccomodationTestID)
        {
            AccomodationTest t;
            if (!AccomodationTest.GetAccomodationTest(AccomodationTestID, out t)) return Json(new { Success = false, Message = "Error getting airtest" }, JsonRequestBehavior.AllowGet);
            AddEditAccomodationTestModel aeatm = new AddEditAccomodationTestModel(t) { IsEditing = true };

            return View(aeatm);
        }


        //[HttpPost]
        //public ActionResult EditAccomodationTest(AddEditAirTestModal aeatm)
        //{
        //    if (aeatm.Test.UpdateTest()) return Json(new { Success = true, Message = "Air Test Updated!" }, JsonRequestBehavior.AllowGet);
        //    else return Json(new { Success = false, Message = "Error updating test!" }, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Tests

        #region MainTests

        //Get: MainTests
        public ActionResult MainTests()
        {
            /*if (Request["StudentID"] == null) throw new Exception("Student ID cannot be null");
            Guid studentID = new Guid(Request["StudentID"]);
            Student s = new Student(studentID);
            s.GetTests();*/
            return View();
        }

        #endregion

        #region AirTests

        private bool GetStudentFromReferrer(out Student s)
        {
            try
            {
                NameValueCollection request = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);

                if (request["StudentID"] == null) throw new Exception("Student ID cannot be null");
                Guid studentID = new Guid(request["StudentID"]);
                s = new Student(studentID);
                return true;
            }
            catch (Exception ex)
            {
                s = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        ////
        ///OLD - FOR BACKUPS

        //Get: BenchMarkTests 
        public ActionResult GetAirTest()
        {

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            if (!s.GetAirTests())
                return Json(new { Success = false, Message = "Error getting air tests for the current student" }, JsonRequestBehavior.AllowGet);

            s.AirTests.Sort((t1, t2) => t2.Year.CompareTo(t1.Year));

            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetAirTest), s)
            },
            JsonRequestBehavior.AllowGet);

        }



        ////Get: BenchMarkTests 
        public ActionResult AddAirTest()
        {
            ApproachingMastery.Models.AddEditAirTestModal aeatm = new AddEditAirTestModal();
            return View(aeatm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAirTest(AddEditAirTestModal aeatm)
        {
            bool result = false;

            NameValueCollection request = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);

            if (request["StudentID"] == null) throw new Exception("Student ID cannot be null");
            Guid studentID = new Guid(request["StudentID"]);

            Student s = new Student(studentID);

            result = s.AddAirTest(aeatm.Test);

            Guid currentUserID = new Guid(Session["LOGIN_ID"].ToString());
            UserLogin ul = new UserLogin(currentUserID);


            if (result) return Json(new[] { "Air test added" }, JsonRequestBehavior.AllowGet);
            else return Json(new[] { "Error adding air test" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EditAirTest(Guid airTestID)
        {
            AirTest t;
            if (!AirTest.GetAirTest(airTestID, out t)) return Json(new { Success = false, Message = "Error getting airtest" }, JsonRequestBehavior.AllowGet);
            AddEditAirTestModal aesm = new AddEditAirTestModal(t) { IsEditing = true };

            return View(aesm);
        }

        [HttpPost]
        public ActionResult EditAirTest(AddEditAirTestModal aeatm)
        {
            if (aeatm.Test.UpdateTest()) return Json(new { Success = true, Message = "Air Test Updated!" }, JsonRequestBehavior.AllowGet);
            else return Json(new { Success = false, Message = "Error updating test!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteAirTest(Guid airTestID)
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }


            if (!AirTest.RemoveAirTest(airTestID))
                return Json(new { Success = false, Message = "Error getting air tests for the current student" }, JsonRequestBehavior.AllowGet);

            return Json(new { Success = true, Message = "Air Test Removed!" }, JsonRequestBehavior.AllowGet);
        }





        #endregion

        #region BenchMarkTests


        //Get: BenchMarkTests 
        public ActionResult BenchMarkTests()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BenchMarkTests(FormCollection col)
        {
            NameValueCollection request = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
            if (request["StudentID"] == null) throw new Exception("Student ID cannot be null");

            Student s = new Student(new Guid(request["StudentID"].ToString()));

            Test t = new Test((TestType)Enum.Parse(typeof(TestType), col["TestType"], true),
                (Semester)Enum.Parse(typeof(Semester), col["Semester"], true)
                , Convert.ToInt16(col["Year"]), Convert.ToDouble(col["Score"]));

            s.AddTest(t);
            return Redirect(Request.UrlReferrer.ToString());
        }

        #endregion

        #region ProCore

        //Get: ProCoreTests 
        public ActionResult GetProCore()
        {

            const TestType type = TestType.ProCore;

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" });
            }
            if (!s.GetTests(type))
                return Json(new { Success = false, Message = "Error getting SRI tests for the current student" });

            List<short> testYears = GetTestYears(s.Tests);
            List<TestYearModal> testYearModals = new List<TestYearModal>();
            foreach (short year in testYears)
            {
                testYearModals.Add(new TestYearModal(s.Tests.FindAll(t => t.Year == year).ToList(), type));
            }

            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetProCore), testYearModals)
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditProCoreYear(short year)
        {
            const TestType type = TestType.ProCore;
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            if (!s.GetTests(type))
                return Json(new { Success = false, Message = "Error getting SRI tests for the current student" }, JsonRequestBehavior.AllowGet);

            TestYearModal tests = new TestYearModal(s.Tests.FindAll(t => t.Year == year), type)
            {
                TestType = type
            };
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, "EditTestYear", tests)
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddProCoreYear()
        {
            const TestType type = TestType.ProCore;
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }
            TestYearModal tests = new TestYearModal(new List<Test>(), type)
            {
                TestType = type
            };
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, "EditTestYear", tests)
            },
            JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region SRI

        //Get: SRITests 
        public ActionResult GetSRI()
        {
            const TestType type = TestType.SRI;

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" });
            }
            if (!s.GetTests(type))
                return Json(new { Success = false, Message = "Error getting SRI tests for the current student" });

            List<short> testYears = GetTestYears(s.Tests);
            List<TestYearModal> testYearModals = new List<TestYearModal>();
            foreach (short year in testYears)
            {
                testYearModals.Add(new TestYearModal(s.Tests.FindAll(t => t.Year == year).ToList(), type));
            }

            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetSRI), testYearModals)
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSRIYear(short year)
        {
            const TestType type = TestType.SRI;
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" });
            }
            if (!s.GetTests(type))
                return Json(new { Success = false, Message = "Error getting SRI tests for the current student" });

            TestYearModal tests = new TestYearModal(s.Tests.FindAll(t => t.Year == year), type);
            tests.TestType = type;
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, "EditTestYear", tests)
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSRIYear()
        {
            const TestType type = TestType.SRI;
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" });
            }
            if (!s.GetTests(type))
                return Json(new { Success = false, Message = "Error getting SRI tests for the current student" });

            TestYearModal tests = new TestYearModal(new List<Test>(), type);
            tests.TestType = type;
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, "EditTestYear", tests)
            },
            JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult EditTestYear(TestYearModal tests)
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" });
            }
            bool hasFailure = false;
            List<Test> allTests = new List<Test>
            {
                tests.Fall,
                tests.Spring,
                tests.Summer,
                tests.Winter
            };
            foreach (Test t in allTests)
            {
                t.TestType = tests.TestType;
                t.Year = tests.Year;
                if (t.Score != null)
                    if (!s.AddTest(t)) hasFailure = true;
            }
            if (hasFailure) return Json(new { Success = true, TestType = (int)tests.TestType, Message = "One or more tests failed to update" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = true, TestType = (int)tests.TestType, Message = "All tests updated" }, JsonRequestBehavior.AllowGet);

        }

        private List<short> GetTestYears(List<Test> tests)
        {
            List<short> result = new List<short>();
            foreach (Test t in tests)
                if (!result.Contains(t.Year))
                    result.Add(t.Year);
            result.Sort((y1, y2) => y2.CompareTo(y1));
            return result;
        }

        #endregion

        #region TenMark

        //Get: TenMarkTets 
        public ActionResult GetTenMark()
        {

            const TestType type = TestType.TenMark;

            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" });
            }
            if (!s.GetTests(type))
                return Json(new { Success = false, Message = "Error getting SRI tests for the current student" });

            List<short> testYears = GetTestYears(s.Tests);
            List<TestYearModal> testYearModals = new List<TestYearModal>();
            foreach (short year in testYears)
            {
                testYearModals.Add(new TestYearModal(s.Tests.FindAll(t => t.Year == year).ToList(), type));
            }

            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetTenMark), testYearModals)
            },
            JsonRequestBehavior.AllowGet);

        }

        public ActionResult EditTenMarkYear(short year)
        {
            const TestType type = TestType.TenMark;
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" });
            }
            if (!s.GetTests(type))
                return Json(new { Success = false, Message = "Error getting SRI tests for the current student" });

            TestYearModal tests = new TestYearModal(s.Tests.FindAll(t => t.Year == year), type);
            tests.TestType = type;
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, "EditTestYear", tests)
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddTenMarkYear()
        {
            const TestType type = TestType.TenMark;
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" });
            }
            if (!s.GetTests(type))
                return Json(new { Success = false, Message = "Error getting SRI tests for the current student" });

            TestYearModal tests = new TestYearModal(new List<Test>(), type);
            tests.TestType = type;
            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, "EditTestYear", tests)
            },
            JsonRequestBehavior.AllowGet);

        }


        #endregion

        #endregion

        #region Behavior/ABC

        //Get: ABCEntry Evaluation Tests
        public ActionResult AddABCEntry()
        {
            return View(new ABCEntry() { IncidentDate = DateTime.Now });
        }

        public ActionResult EditABCEntry(Guid entryID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            if (!s.GetABCEntries())
                return Json(new { Success = false, Message = "Error getting existing entries" }, JsonRequestBehavior.AllowGet);

            ABCEntry entry = s.ABCEntries.Find(e => e.ABCID == entryID);
            if (entry == null)
                return Json(new { Success = false, Message = "Error getting specified entry" }, JsonRequestBehavior.AllowGet);

            return Json(new { Success = true, html = RazorHelpers.RenderRazorViewToString(ControllerContext, "EditABCEntry", entry) },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditABCEntry(ABCEntry entry)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            if (s.AddABCEntry(entry))
                return Json(new
                {
                    Success = true,
                    Message = "ABC entry edited"
                },
            JsonRequestBehavior.AllowGet);

            return Json(new
            {
                Success = false,
                Message = "Error editing ABC entry"
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddABCEntry(ABCEntry entry)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student", JsonRequestBehavior.AllowGet });
            }

            entry.AddedBy = (Guid)Session["LOGIN_ID"];
            if (s.AddABCEntry(entry))
                return Json(new
                {
                    Success = true,
                    Message = "ABC Entry Added"
                },
            JsonRequestBehavior.AllowGet);

            return Json(new
            {
                Success = false,
                Message = "Error adding ABC Entry"
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteABCEntry(Guid entryID)
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student", JsonRequestBehavior.AllowGet });
            }
            if (s.DeleteABCEntry(entryID))
                return Json(new
                {
                    Success = true,
                    Message = "ABC entry removed"
                },
          JsonRequestBehavior.AllowGet);
            return Json(new
            {
                Success = false,
                Message = "Error removing abc entry"
            },
        JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetABCChart()
        {
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student" }, JsonRequestBehavior.AllowGet);
            }

            if (!s.GetABCEntries())
                return Json(new { Success = false, Message = "Error getting abc entries for the current student" }, JsonRequestBehavior.AllowGet);

            s.ABCEntries.Sort((e1, e2) => e2.IncidentDate.CompareTo(e1.IncidentDate));

            return Json(new
            {
                Success = true,
                html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetABCChart), s)
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBehaviorGoal()
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student", JsonRequestBehavior.AllowGet });
            }

            if (s.GetBehavioralGoals())
                return Json(new
                {
                    Success = true,
                    html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetBehaviorGoal), s)
                },
               JsonRequestBehavior.AllowGet);

            return Json(new
            {
                Success = false,
                Message = "Error getting student Academic goals"
            },
         JsonRequestBehavior.AllowGet);
        }

        public ActionResult GoalAssignments(Goal g)
        {
            return View(g);
        }


        #endregion

        #region AcademicGoal

        public ActionResult GetAcademicGoal()
        {
            if (Session["LOGIN_ID"] == null) throw new Exception("Not Authorized");
            Student s;
            if (!GetStudentFromReferrer(out s))
            {
                return Json(new { Success = false, Message = "Error getting current student", JsonRequestBehavior.AllowGet });
            }

            if (s.GetAcademicGoals())
                return Json(new
                {
                    Success = true,
                    html = RazorHelpers.RenderRazorViewToString(ControllerContext, nameof(GetAcademicGoal), s)
                },
               JsonRequestBehavior.AllowGet);

            return Json(new
            {
                Success = false,
                Message = "Error getting student Academic goals"
            },
         JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult DisplaySRIChart()
        {
            return View();
        }


    }
}