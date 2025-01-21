using System;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System.Web.Mvc;
//using Comment_App.Models;
namespace Comment_App.Controllers
{
    
    public class HomeController : Controller
    {
        CommentsappEntities1 db = new CommentsappEntities1();

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(SignUp personDetails)
        {

            var data = db.SignUps.Find(personDetails.Email);
            if (data != null)
            {
                Response.Write("<script language=javascript>alert('Email Id Already Exists!');</script>");
                return View();
            }
            if (ModelState.IsValid)
            {
                db.SignUps.Add(personDetails);
                db.SaveChanges();
                return RedirectToAction("SignIn");
            }
            ViewBag.Message = "NotValid";
            return View();

        }

        [HttpGet]
        public ActionResult SignIn()
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string Email, string Password)
        {
            ObjectResult<Nullable<int>> validation = db.sp_ValidateUserSignIn(Email, Password);
            int value = 0;
            foreach (Nullable<int> result in validation)
            {
                value = result.Value;
            }

            if (value == 1)
            {
                Session["UserName"] = Email;


                return RedirectToAction("FeedbackForm");
            }
            else if (value == 0)
            {
                Response.Write("<script language=javascript>alert('Please Check Your Email Id');</script>");
                
            }
            else if (value == 2)
            {
                Response.Write("<script language=javascript>alert('Wrong Password!');</script>");
              
            }
            return View();
        }

        [HttpGet]
        public ActionResult FeedbackForm()
        {
           
            ViewBag.loggedInEmail = Session["UserName"];
            ViewBag.AllComments = db.Comments.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult FeedbackForm(string UserComment)
        {
            ViewBag.loggedInEmail = Session["UserName"];
            if (UserComment.Trim() == "")
            {
                Response.Write("<script language=javascript>alert('Comments should not be empty');</script>");
            }
            else
            {
                Comment cs = new Comment()
                {

                    Email = Session["UserName"].ToString(),
                    UserComment = UserComment
                };

                db.Comments.Add(cs);
                db.SaveChanges();
            }
            return View();
        }


        [HttpGet]
        public ActionResult ForgotPassword()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(String Email, String Secret)
        {
            

            var data = db.SignUps.Find(Email);

            if (data == null)
            {
                ViewBag.password = "NotUser";
            }
            else
            {
                if (data.Secret == Secret)
                {
                    ViewBag.password = data.Password;
                }
                else
                {
                    ViewBag.password = "WrongSecret";
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult UserComments()
        {
            var data = (from row in db.Comments select row).ToList();
            ViewBag.userComments = data;
            return View();
        }

        [HttpPost]
        public ActionResult UserComments(string LoggedInEmail = null)
        {
            LoggedInEmail = Session["UserName"].ToString();
            var data = from row in db.Comments
                       where LoggedInEmail == row.Email
                       select row;
            ViewBag.userComments = data;
            return View();
        }
    }
}