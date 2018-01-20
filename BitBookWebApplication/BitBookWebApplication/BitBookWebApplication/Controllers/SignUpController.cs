using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BitBookWebApplication.BLL;
using BitBookWebApplication.DAL;
using BitBookWebApplication.Models;

namespace BitBookWebApplication.Controllers
{
    public class SignUpController : Controller
    {
        SignUpManager signUpManager=new SignUpManager();
        public ActionResult SignUpView()
        {
            Session["authentication"] = null;
            Session["PersonID"] = null;
            Session["Password"] = null;
            return View();
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "LoginPage")]
        public ActionResult LoginPage(LoginModels loginAccount)
        {
            if (loginAccount.LoginEmail==null)
            {
                ViewBag.Message = "Please enter a valid email address for login";
                return View("SignUpView");
            }
            string password = signUpManager.GetPassword(loginAccount.LoginEmail);
            if (password==null)
            {
                ViewBag.Message = "No Account of that email Account exist";
                return View("SignUpView");
            }
            if (!password.Equals(loginAccount.LoginPassword))
            {
                ViewBag.Message = "Password Did not Match";
                return View("SignUpView");
            }
            Session["Id"] = signUpManager.GetPersonID(loginAccount.LoginEmail);
            Authentication authentication=new Authentication();
            authentication.Id = Convert.ToInt32(Session["Id"].ToString());
            authentication.Password = loginAccount.LoginPassword;
            Session["authentication"] = authentication;
            Session["authenticationId"] = authentication.Id;
            var authenticationId = (int)Session["authenticationId"];
            return RedirectToAction("HomePage", "Home", new { authenticationInfo=authenticationId });
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SignUpView")]
        public ActionResult SignUpView(Person person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }
            bool isEmailExist = signUpManager.IsEmailExist(person.Email);
            if (isEmailExist)
            {
                ViewBag.Message = "BitBook Account Associated with this account already Exist";
                return View(person);
            }
            bool isNewAccountCreated = signUpManager.SetPersonInformation(person);
            if (!isNewAccountCreated)
            {
                ViewBag.Message = "There is some problem while Storing data";
                return View(person);
            }
            else
            {
                Session["username"] = signUpManager.GetPersonID(person.Email);
                var username = (string) Session["username"];
                Session["Password"] = person.Password;
                return RedirectToAction("Index", "AdditionalInformation", new {id= username});
            }
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public class MultipleButtonAttribute : ActionNameSelectorAttribute
        {
            public string Name { get; set; }
            public string Argument { get; set; }

            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                var isValidName = false;
                var keyValue = string.Format("{0}:{1}", Name, Argument);
                var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

                if (value != null)
                {
                    controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                    isValidName = true;
                }

                return isValidName;
            }
        }
	}
}