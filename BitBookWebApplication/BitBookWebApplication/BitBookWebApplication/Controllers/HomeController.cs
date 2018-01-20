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
    public class HomeController : Controller
    {
        HomeManager homeManager=new HomeManager();
        EditManager editManager=new EditManager();
        public ActionResult HomePage(int authenticationInfo)
        {
            Authentication authentication = (Authentication) Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            Person person = homeManager.GetPersonInformation(authenticationInfo);
            if (person == null)
            {
                return RedirectToAction("SignUpView", "SignUp");
            }
            if (person.Password.Equals(authentication.Password))
            {
                    TempData["msg"] = null;
                    ViewBag.Name = person.Name;
                    ViewBag.SignupID = authentication.Id;
                    ViewBag.Password = authentication.Password;
                    ViewBag.UserPhoto = homeManager.GetProfilePhotoByID(authentication.Id);
                    authentication.Name = person.Name;
                    Session["authentication"] = authentication;
                    ViewBag.PostList = homeManager.GetPostInformation(authentication.Id);
                    ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);
                    ViewBag.PersonInfo = homeManager.GetPersonInformation(authenticationInfo);
                byte[] photoInByte = homeManager.GetProfilePhotoByID(authentication.Id).PhotoInByte;
                if (photoInByte!=null)
                {
                    string base64String = Convert.ToBase64String(photoInByte);
                    ViewBag.Image = base64String;
                }
                
                    
                    
                    return View();
                }
                else
                {
                    TempData["msg"] = "<script>alert('Please Log in First');</script>";
                    return RedirectToAction("SignUpView", "SignUp");
                }                      
            
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "HomePage")]
        public ActionResult HomePage(PostInfo postInfo)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }            

            HttpPostedFileBase file = postInfo.PostPhoto;
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
                    postInfo.PostPhotoInByte = bytes;
                }
                else
                {
                    return RedirectToAction("HomePage", "Home", new { authenticationInfo = authentication.Id });
                }
            }
            else
            {
                postInfo.PostPhotoInByte=null;
            }


            if (homeManager.SetPostInformation(postInfo))
            {
                ViewBag.Message = "Successfully posted";
            }
            else
            {
                ViewBag.Message = "Post Failed";
            }

            ViewBag.Name = authentication.Name;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;
            ViewBag.PostList = homeManager.GetPostInformation(postInfo.SignUpID);
            ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);
            ViewBag.PersonInfo = homeManager.GetPersonInformation(authentication.Id);
            byte[] photoInByte = homeManager.GetProfilePhotoByID(authentication.Id).PhotoInByte;
            if (photoInByte != null)
            {
                string base64String = Convert.ToBase64String(photoInByte);
                ViewBag.Image = base64String;
            }
            return View();
        }

        public ActionResult CommentPhoto(int photoId,int typeNo)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }

            Photo photo=new Photo();
            List<CommentDetail> commentDetails=new List<CommentDetail>();

            if (typeNo==1)
            {
                photo = editManager.GetProfilePhotoByID(photoId);
                photo.TypeNo = 1;               
            }
            else if (typeNo == 2)
            {
                photo = editManager.GetCoverPhotoByID(photoId);
                photo.TypeNo = 2;
            }
            else
            {
                return RedirectToAction("HomePage", "Home", new { authenticationInfo = authentication.Id });
            }
            photo.Name = editManager.GetPersonInformation(photo.SignupID).Name;
            commentDetails = homeManager.GetAllPhotoComment(photoId, typeNo);

            ViewBag.Name = authentication.Name;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;
           
            ViewBag.Photo = photo;
            ViewBag.CommentList = commentDetails;
            ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);
            return View();

        }
        [HttpPost]
        public ActionResult CommentPhoto(CommentDetail commentDetail)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }

            homeManager.SetPhotoComment(commentDetail, authentication.Id);

            return RedirectToAction("CommentPhoto", "Home", new { photoId = commentDetail.PostID, typeNo = commentDetail.TypeNo });
        }
        public ActionResult DecideRedirectPost(int postType, int id)
        {
            int newId = id;
            //profile photo or profile
            if (postType==1||postType == 2)
            {                
                return RedirectToAction("CommentPhoto", "Home", new { photoId = id, typeNo = postType });
            }
            //Post
            else if (postType == 3 || postType == 4 || postType == 5 || postType == 8 || postType == 9)
            {
                //newId = homeManager.GetPostIdByCommentId(id);
                return RedirectToAction("Comment", "Home", new { id = newId });
            }
            //Profile
            else if (postType == 7 || postType == 6)
            {
                
                return RedirectToAction("ProfileView", "Profile", new { personID = newId });
            }
            Authentication authentication = (Authentication)Session["authentication"];
            return RedirectToAction("HomePage", "Home", new { authenticationInfo =authentication.Id});
        }
        public ActionResult FriendListView(int userID)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            Person person = homeManager.GetPersonInformation(userID);
            if (authentication==null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            else if (!person.Password.Equals(authentication.Password))
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            else
            {
                return RedirectToAction("FriendListView", "Profile", new { personID = userID });
            }
        }
        public ActionResult DoLike(int id)
        {
            int owenerId = homeManager.GetPostInformationById(id).SignUpID;
            Authentication authentication = (Authentication)Session["authentication"];
            if (homeManager.IsPostLike(id,authentication.Id))
            {
                homeManager.DeletePostLike(id, authentication.Id);
            }
            else
            {
                homeManager.SetPostLike(id, authentication.Id, owenerId);
            }
            return RedirectToAction("HomePage", "Home", new { authenticationInfo = authentication.Id });
        }
        public ActionResult Comment(int id)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            ViewBag.Name = authentication.Name;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;
            ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);
            PostInfo postInfo = homeManager.GetPostInformationById(id);
            ViewBag.PostInfo = postInfo;
            List<CommentDetail> commentDetails = homeManager.GetAllComment(id);
            ViewBag.CommentList = commentDetails;
            return View(id);
        }
        [HttpPost]
        public ActionResult Comment(CommentDetail commentDetail)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            commentDetail.PersonID = authentication.Id;
            int owenerId = homeManager.GetPostInformationById(commentDetail.PostID).SignUpID;
            if (homeManager.SetComment(commentDetail,owenerId))
            {
                ViewBag.Message = "Successfully comment in the post";
            }                      
            PostInfo postInfo = homeManager.GetPostInformationById(commentDetail.ID);
            List<CommentDetail> commentDetails = homeManager.GetAllComment(commentDetail.PostID);

            ViewBag.Name = authentication.Name;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;
            ViewBag.PostInfo = postInfo;
            ViewBag.CommentList = commentDetails;
            return View(commentDetail.ID);
        }
        public ActionResult ShowWhoLiked(int id)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            ViewBag.Name = authentication.Name;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;

            List<int> noOfPeopleLike = homeManager.NumberOfPostLike(id);
            List<Person> persons=new List<Person>();
            foreach (int pId in noOfPeopleLike)
            {
                persons.Add(homeManager.GetPersonInformation(pId));
            }
            ViewBag.PersonInfo = homeManager.GetPersonInformation(authentication.Id);
            ViewBag.PeopleWhoLike = persons;
            return View(id);
        }       
        public ActionResult DeletePost(int userPostId)
        {
            Authentication authentication = (Authentication)Session["authentication"];
            if (authentication == null)
            {
                TempData["msg"] = "<script>alert('Please Log in First');</script>";
                return RedirectToAction("SignUpView", "SignUp");
            }
            if (homeManager.DeletePost(userPostId))
            {
                ViewBag.Message = "Successfully posted Deleted";
            }
            else
            {
                ViewBag.Message = "Post delete Failed";
            }
            Person person = homeManager.GetPersonInformation(authentication.Id);
            ViewBag.Name = person.Name;
            ViewBag.SignupID = authentication.Id;
            ViewBag.Password = authentication.Password;
            ViewBag.UserPhoto = homeManager.GetProfilePhotoByID(authentication.Id);
            authentication.Name = person.Name;
            Session["authentication"] = authentication;
            ViewBag.PostList = homeManager.GetPostInformation(authentication.Id);
            ViewBag.NotificationList = homeManager.GetAllNotification(authentication.Id);
             ViewBag.PersonInfo = homeManager.GetPersonInformation(authentication.Id);
            byte[] photoInByte = homeManager.GetProfilePhotoByID(authentication.Id).PhotoInByte;
            if (photoInByte != null)
            {
                string base64String = Convert.ToBase64String(photoInByte);
                ViewBag.Image = base64String;
            }
                
                    
            return View("HomePage");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SearchPage")]
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