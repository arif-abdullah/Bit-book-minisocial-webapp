using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BitBookWebApplication.BLL;
using BitBookWebApplication.Models;

namespace BitBookWebApplication.Controllers
{
    public class ProfileController : Controller
    {
        HomeManager homeManager=new HomeManager();
        FriendManager friendManager=new FriendManager();
        EditManager editManager=new EditManager();
        ProfileManager profileManager=new ProfileManager();
        
        public ActionResult ProfileView(int personID)
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
            ViewBag.PersonID = personID;
            ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);
            ViewBag.PersonInfo = homeManager.GetPersonInformation(personID);

            int coverPhotoId = editManager.GetCoverPhotoID(personID);
            ViewBag.CoverImage = editManager.GetCoverPhotoByID(coverPhotoId).PhotoInString;
            int profilePhotoId = editManager.GetProfilePhotoID(personID);
            ViewBag.ProfileImage = editManager.GetProfilePhotoByID(profilePhotoId).PhotoInString;           
            List<AllAboutPost> postInfos = homeManager.GetAllPostInformationByID(personID);
            ViewBag.PostInfos = postInfos;
            ViewBag.PersonInfo = homeManager.GetPersonInformation(personID);

            Session["PersonID"] = personID;
            ProfilePost profilePost=new ProfilePost(){SignupId = authentication.Id,Password = authentication.Password,Name = authentication.Name,PersonID = personID};
            return View(profilePost);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ProfileView")]
        public ActionResult ProfileView(ProfilePost profilePost)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            HttpPostedFileBase file = profilePost.PostPhoto;
            if (file != null)
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
                    profilePost.PostPhotoInBytes = bytes;
                }
                else
                {
                    int personIDs = profilePost.PersonID;
                    return RedirectToAction("ProfileView", "Profile", new { personID = personIDs });
                }
            }
            else
            {
                profilePost.PostPhotoInBytes = null;
            }

            if (homeManager.SetPostInOtherWall(profilePost))
            {
                ViewBag.Message = "Successfully posted";
            }
            else
            {
                ViewBag.Message = "Post Failed";
            }
            int personID = profilePost.PersonID;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;
            ViewBag.Name = authentication.Name;
            ViewBag.PersonID = personID;
            ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);

            int coverPhotoId = editManager.GetCoverPhotoID(personID);
            ViewBag.CoverImage = editManager.GetCoverPhotoByID(coverPhotoId).PhotoInString;
            int profilePhotoId = editManager.GetProfilePhotoID(personID);
            ViewBag.ProfileImage = editManager.GetProfilePhotoByID(profilePhotoId).PhotoInString;           
            List<AllAboutPost> postInfos = homeManager.GetAllPostInformationByID(personID);
            ViewBag.PostInfos = postInfos;
            ViewBag.PersonInfo = homeManager.GetPersonInformation(personID);

            Session["PersonID"] = personID;
            ProfilePost profilePosts=new ProfilePost(){SignupId = authentication.Id,Password = authentication.Password,Name = authentication.Name,PersonID = personID};
            return View(profilePosts);
        }

        public ActionResult PhotoListView(int userID)
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
            ViewBag.PersonID = userID;
            ViewBag.PersonInfo = homeManager.GetPersonInformation(userID);

            ViewBag.ProfilePhotos = profileManager.GetAllProfilePhotoByID(userID);
            ViewBag.CoverPhotos = profileManager.GetAllCoverPhotoByID(userID);
            ViewBag.PostPhotos = profileManager.GetAllPostPhotoByID(userID);

            return View();
        }
        public ActionResult FriendListView(int personID)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            Person person = homeManager.GetPersonInformation(personID);
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            else if (!person.Password.Equals(authentication.Password))
            {
                return RedirectToAction("SignUpView", "SignUp");
            }
            else
            {
                ViewBag.Name = person.Name;
                ViewBag.SignupID = authentication.Id;
                ViewBag.Password = authentication.Password;
                ViewBag.PersonID = personID;
                ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);
                ViewBag.PersonList = homeManager.AllFriendInformation(personID);
                ProfilePost profilePost = new ProfilePost() { SignupId = authentication.Id, Password = authentication.Password, Name = authentication.Name, PersonID = personID };
                return View(profilePost);
            }
        }
        public ActionResult RemoveFriend(int id,int personID)
        {
            int pID = personID;
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            ViewBag.Name = authentication.Name;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;

            Friend friend = new Friend();
            friend.ID = id;
            friend.RId = pID;
            if (friendManager.DeleteFriendByBothID(friend))
            {
                ViewBag.Message = "Sucessfully Unfriend";
            }
            else
            {
                ViewBag.Message = "Failed to Unfriend";
            }
            ViewBag.PersonID = pID;
            ViewBag.PersonList = homeManager.AllFriendInformation(pID);
            ProfilePost profilePost = new ProfilePost() { SignupId = authentication.Id, Password = authentication.Password, Name = authentication.Name, PersonID = pID };
            return View("FriendListView",profilePost);
        }
        public ActionResult DoLike(int id)
        {
            int owenerId = homeManager.GetPostInformationById(id).SignUpID;
            Authentication authentication = (Authentication)Session["authentication"];
            if (homeManager.IsPostLike(id, authentication.Id))
            {
                homeManager.DeletePostLike(id, authentication.Id);
            }
            else
            {
                homeManager.SetPostLike(id, authentication.Id, owenerId);
            }
            int pid = (int) Session["PersonID"];
            return RedirectToAction("ProfileView", "Profile", new { personID = pid });
        }
        public ActionResult DeletePost(int userPostId)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (homeManager.DeletePost(userPostId))
            {
                ViewBag.Message = "Successfully posted Deleted";
            }
            else
            {
                ViewBag.Message = "Post delete Failed";
            }
            ViewBag.Name = authentication.Name;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;
            ViewBag.PostList = homeManager.GetPostInformation(authentication.Id);
            int pid = (int)Session["PersonID"];
            return RedirectToAction("ProfileView", "Profile", new { personID = pid });
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