using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BitBookWebApplication.Models;
using BitBookWebApplication.BLL;

namespace BitBookWebApplication.Controllers
{
    public class EditController : Controller
    {
        HomeManager homeManager = new HomeManager();
        EditManager editManager=new EditManager();
        public ActionResult EditView(int signupIds)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            Person person = homeManager.GetPersonInformation(signupIds);
            if (person == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            if (person.Password.Equals(authentication.Password))
            {
                ViewBag.SignupID = authentication.Id;
                ViewBag.Password = authentication.Password;
                ViewBag.Name = authentication.Name;
                AllPersonalInformation allPersonalInformation = editManager.GetAdditionInformation(signupIds);
                ViewBag.AdditionalInfoID = allPersonalInformation.AdditionalInfoID;
                ViewBag.ProfilePhotoId = allPersonalInformation.ProfilePhotoId;
                ViewBag.CoverPhotoId = allPersonalInformation.CoverPhotoId;
                allPersonalInformation = editManager.GetAllPersonalInformation(authentication.Id);
                ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);
                return View(allPersonalInformation);
            }
            else
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
        }

        [HttpPost]
        [HomeController.MultipleButtonAttribute(Name = "action", Argument = "EditView")]
        public ActionResult EditView(AllPersonalInformation allPersonalInformation)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;
            ViewBag.Name = authentication.Name;
            ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);

            if (!ModelState.IsValid)
            {
                return View(allPersonalInformation);
            }

            HttpPostedFileBase file = allPersonalInformation.ProfilePhoto;
            if (file!=null)
            {
            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName);
            int fileSize = file.ContentLength;

            if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".bmp" ||
                fileExtension.ToLower() == ".gif" || fileExtension.ToLower() == ".png")
            {
                Stream stream = file.InputStream;
                BinaryReader binaryReader = new BinaryReader(stream);
                byte[] bytes = binaryReader.ReadBytes((int)stream.Length);
                allPersonalInformation.PhotoInByte = bytes;
            }
            else
            {
                ViewBag.ErrorMessage = "Only images (.jpg, .png, .gif and .bmp) can be uploaded";
                return View(allPersonalInformation);
            }
            }
            else
            {
                allPersonalInformation.ProfilePhotoId = 3;
            }
            HttpPostedFileBase files = allPersonalInformation.CoverPhoto;
            if (files != null)
            {
                string fileNames = Path.GetFileName(files.FileName);
                string fileExtensions = Path.GetExtension(fileNames);
                int fileSizes = files.ContentLength;

                if (fileExtensions.ToLower() == ".jpg" || fileExtensions.ToLower() == ".bmp" ||
                    fileExtensions.ToLower() == ".gif" || fileExtensions.ToLower() == ".png")
                {
                    Stream streams = files.InputStream;
                    BinaryReader binaryReaders = new BinaryReader(streams);
                    byte[] bytess = binaryReaders.ReadBytes((int)streams.Length);
                    allPersonalInformation.CoverPhotoInByte = bytess;
                }
                else
                {
                    ViewBag.ErrorMessage = "Only images (.jpg, .png, .gif and .bmp) can be uploaded";
                    return View(allPersonalInformation);
                }
            }
            else
            {
                allPersonalInformation.CoverPhotoId = 3;
            }
            if (allPersonalInformation.AboutMe == null)
            {
                allPersonalInformation.AboutMe = ".";
            }
            if (allPersonalInformation.Religion == null)
            {
                allPersonalInformation.Religion = ".";
            }
            if (allPersonalInformation.MobileNumber == null)
            {
                allPersonalInformation.MobileNumber = ".";
            }
            if (allPersonalInformation.AreaOfInterest == null)
            {
                allPersonalInformation.AreaOfInterest = ".";
            }
            if (allPersonalInformation.Address == null)
            {
                allPersonalInformation.Address = ".";
            }           
            if (allPersonalInformation.OldPassword != authentication.Password)
            {
                ViewBag.ErrorMessage = "Given Password do not match with old password";
                return View(allPersonalInformation);
            }
            if (allPersonalInformation.NewPassword == null)
            {
                allPersonalInformation.NewPassword = authentication.Password;
            }
            if (editManager.UpdateAllInformation(allPersonalInformation))
            {
                ViewBag.SuccesMessage = "All new information updated successfully";

                return View(allPersonalInformation);
            }
            ViewBag.ErrorMessage = "All new information updated failed";
            return View(allPersonalInformation);
        }


        [HttpPost]
        [HomeController.MultipleButtonAttribute(Name = "action", Argument = "SearchPage")]
        public ActionResult SearchPage(Search search)
        {
            var searchName = search.SearchDetail;
            return RedirectToAction("Index", "Search", new { name = searchName });
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