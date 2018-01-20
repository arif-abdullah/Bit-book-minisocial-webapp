using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitBookWebApplication.DAL;
using BitBookWebApplication.Models;

namespace BitBookWebApplication.BLL
{
    public class HomeManager
    {
        HomeGateway homeGateway=new HomeGateway();
        SignUpGateway signUpGateway=new SignUpGateway();
        NotificationGateway notificationGateway = new NotificationGateway();

        public Person GetPersonInformation(int id)
        {
            return homeGateway.GetPersonInformation(id);
        }

        public Photo GetProfilePhotoByID(int signupID)
        {
            return homeGateway.GetProfilePhotoByID(signupID);
        }

        public int GetPostIdByCommentId(int id)
        {
            return homeGateway.GetPostIdByCommentId(id);
        }
        public bool SetPhotoComment(CommentDetail commentDetail, int owenerId)
        {
            return homeGateway.SetPhotoComment(commentDetail, owenerId);
        }
        public List<Person> AllFriendInformation(int userid)
        {
            List<int> allFriendId = notificationGateway.GetAllFriendList(userid);
            List<Person> persons=new List<Person>();
            foreach (int id in allFriendId)
            {
                Person person = homeGateway.GetPersonInformation(id);
                persons.Add(person);
            }
            return persons;
        }
        public List<AllAboutPost> GetPostInformation(int id)
        {
            List<int> idList = homeGateway.GetAllFriendList(id);
            List<PostInfo> postInfos = homeGateway.GetPostInformation(idList);
            List<AllAboutPost> allAboutPosts = new List<AllAboutPost>();
            foreach (PostInfo info in postInfos)
            {
                AllAboutPost allAboutPost = new AllAboutPost();
                allAboutPost.Id = info.Id;
                allAboutPost.SignUpID = info.SignUpID;
                allAboutPost.Name = GetPersonInformation(info.SignUpID).Name;
                allAboutPost.NoOfLike = NumberOfPostLike(info.Id).Count;
                allAboutPost.PostDetail = info.PostDetail;
                allAboutPost.DateTime = info.DateTime;
                allAboutPost.PostPhotoInByte = info.PostPhotoInByte;
                if (info.PostPhotoInByte != null)
                {
                    allAboutPost.PostPhotoInString = Convert.ToBase64String(info.PostPhotoInByte);
                }
                allAboutPost.ProfilePhotoInByte = GetProfilePhotoByID(info.SignUpID).PhotoInByte;
                if (GetProfilePhotoByID(info.SignUpID).PhotoInByte != null)
                {
                    allAboutPost.ProfilePhotoInString = Convert.ToBase64String(GetProfilePhotoByID(info.SignUpID).PhotoInByte);
                }

                allAboutPosts.Add(allAboutPost);

            }
            allAboutPosts = allAboutPosts.OrderByDescending(e => e.DateTime).OrderByDescending(e => e.DateTime).ToList();
            //postInfos = (from e in postInfos
            //        orderby e.DateTime, e.DateTime
            //        select e).ToList();
            return allAboutPosts;
        }
        public List<AllAboutPost> GetAllPostInformationByID(int signupId)
        {
            List<AllAboutPost> allAboutPosts = new List<AllAboutPost>();
            List<PostInfo> postInfos = notificationGateway.GetAllPostInformationByID(signupId);
            foreach (PostInfo info in postInfos)
            {
                AllAboutPost allAboutPost = new AllAboutPost();
                allAboutPost.Id = info.Id;
                allAboutPost.SignUpID = info.SignUpID;
                allAboutPost.Name = GetPersonInformation(signupId).Name;
                allAboutPost.NoOfLike = NumberOfPostLike(info.Id).Count;
                allAboutPost.PostDetail = info.PostDetail;
                allAboutPost.DateTime = info.DateTime;
                allAboutPost.PostPhotoInByte = info.PostPhotoInByte;
                if (info.PostPhotoInByte != null)
                {
                    allAboutPost.PostPhotoInString = Convert.ToBase64String(info.PostPhotoInByte);
                }
                allAboutPost.ProfilePhotoInByte = GetProfilePhotoByID(signupId).PhotoInByte;
                if (GetProfilePhotoByID(signupId).PhotoInByte != null)
                {
                    allAboutPost.ProfilePhotoInString = Convert.ToBase64String(GetProfilePhotoByID(signupId).PhotoInByte);
                }

                allAboutPosts.Add(allAboutPost);

            }

            allAboutPosts = allAboutPosts.OrderByDescending(e => e.DateTime).OrderByDescending(e => e.DateTime).ToList();
            return allAboutPosts;
        }
        public bool DeletePost(int postId)
        {
            return homeGateway.DeletePost(postId);
        }
        public bool SetComment(CommentDetail commentDetail, int owenerId)
        {
            return homeGateway.SetComment(commentDetail, owenerId);
        }
        public List<CommentDetail> GetAllComment(int id)
        {
            List<CommentDetail> commentDetails=homeGateway.GetAllComment(id);
            foreach (CommentDetail commentDetail in commentDetails)
            {
                commentDetail.PersonName = GetPersonInformation(commentDetail.PersonID).Name;
            }
            return commentDetails;
        }

        public List<CommentDetail> GetAllPhotoComment(int typeId, int typeNo)
        {
            List<CommentDetail> commentDetails = homeGateway.GetAllPhotoComment(typeId,typeNo);
            foreach (CommentDetail commentDetail in commentDetails)
            {
                commentDetail.PersonName = GetPersonInformation(commentDetail.PersonID).Name;
            }
            return commentDetails;
        }
        public PostInfo GetPostInformationById(int id)
        {
            PostInfo postInfo = homeGateway.GetPostInformationById(id);
            postInfo.Name = GetPersonInformation(postInfo.SignUpID).Name;
            postInfo.NoOfLike = NumberOfPostLike(postInfo.Id).Count;
            if (postInfo.PostPhotoInByte != null)
            {
                postInfo.PostPhotoInString = Convert.ToBase64String(postInfo.PostPhotoInByte);
            }
            return postInfo;
        }
        public bool SetPostInformation(PostInfo postInfo)
        {
            return homeGateway.SetPostInformation(postInfo);
        }

        public bool SetPostInOtherWall(ProfilePost postInfo)
        {
            return homeGateway.SetPostInOtherWall(postInfo);
        }

        

        public List<int> NumberOfPostLike(int postId)
        {
            return homeGateway.NumberOfPostLike(postId);
        }

        public bool SetPostLike(int postId, int signUpId, int owenerId)
        {
            return homeGateway.SetPostLike(postId, signUpId,owenerId);
        }
        public bool IsPostLike(int postId, int signUpId)
        {
            return homeGateway.IsPostLike(postId, signUpId);
        }
        public bool DeletePostLike(int postId, int signUpId)
        {
            return homeGateway.DeletePostLike(postId, signUpId);
        }

        public List<Notification> GetAllNotification(int signupId)
        {
            List<Notification> notifications=new List<Notification>();
            List<int> idList = notificationGateway.GetAllFriendList(signupId);
            foreach (int id in idList)
            {
                
                Person person = homeGateway.GetPersonInformation(id);
                List<Photo> profilePhotos = notificationGateway.GetAllProfilePhotoByID(id);
                foreach (Photo profilePhoto in profilePhotos)
                {
                    Notification n1 = new Notification();
                    n1.Type = 1;
                    n1.TypeId = profilePhoto.ID;
                    n1.OwnerId = profilePhoto.SignupID;
                    n1.OwnerName = person.Name;
                    n1.DateTime = profilePhoto.DateTime;
                    n1.NotificationString = person.Name + " has uploaded a new profile Photo";
                    notifications.Add(n1);
                }
                List<Photo> coverPhotos = notificationGateway.GetAllCoverPhotoByID(id);
                foreach (Photo profilePhoto in coverPhotos)
                {
                    Notification n2 = new Notification();
                    n2.Type = 2;
                    n2.TypeId = profilePhoto.ID;
                    n2.OwnerId = profilePhoto.SignupID;
                    n2.OwnerName = person.Name;
                    n2.DateTime = profilePhoto.DateTime;
                    n2.NotificationString = person.Name + " has uploaded a new cover Photo";
                    notifications.Add(n2);
                }
                List<PostInfo> postInfos = notificationGateway.GetAllPostInformationByID(id);
                foreach (PostInfo postInfo in postInfos)
                {
                    Notification n3 = new Notification();
                    n3.Type = 3;
                    n3.TypeId = postInfo.Id;
                    n3.OwnerId = postInfo.SignUpID;
                    n3.OwnerName = person.Name;
                    n3.DateTime = postInfo.DateTime;
                    n3.NotificationString = person.Name + " has given a new post";
                    notifications.Add(n3);
                }                
            }

            List<PostLike> postLikes = notificationGateway.GetPostLikeList(signupId);
            foreach (PostLike postLike in postLikes)
            {
                Notification n4 = new Notification();
                n4.Type = 4;
                n4.TypeId = postLike.PostID;
                n4.OwnerId = postLike.SignupID;
                n4.OwnerName = homeGateway.GetPersonInformation(postLike.SignupID).Name;
                n4.DateTime = postLike.DateTime;
                n4.NotificationString = n4.OwnerName + " has given like on your post";
                notifications.Add(n4);
            }
            List<PostLike> postComment = notificationGateway.GetPostCommentList(signupId);
            foreach (PostLike postLike in postComment)
            {
                Notification n5 = new Notification();
                n5.Type = 5;
                n5.TypeId = postLike.PostID;
                n5.OwnerId = postLike.SignupID;
                n5.OwnerName = homeGateway.GetPersonInformation(postLike.SignupID).Name;
                n5.DateTime = postLike.DateTime;
                n5.NotificationString = n5.OwnerName + " has given comment on your post";
                notifications.Add(n5);
            }
            List<PostLike> friendNotification = notificationGateway.GetAllFriendNotificationList(signupId);
            foreach (PostLike postLike in friendNotification)
            {
                Notification n6 = new Notification();
                n6.Type = 6;
                n6.TypeId = postLike.SignupID;
                n6.OwnerId = postLike.SignupID;
                n6.OwnerName = homeGateway.GetPersonInformation(postLike.SignupID).Name;
                n6.DateTime = postLike.DateTime;
                n6.NotificationString = n6.OwnerName + " has become your friend";
                notifications.Add(n6);
            }
            List<PostLike> friendRequestNotification = notificationGateway.GetAllFriendRequestList(signupId);
            foreach (PostLike postLike in friendRequestNotification)
            {
                Notification n7 = new Notification();
                n7.Type = 7;
                n7.TypeId = postLike.SignupID;
                n7.OwnerId = postLike.SignupID;
                n7.OwnerName = homeGateway.GetPersonInformation(postLike.SignupID).Name;
                n7.DateTime = postLike.DateTime;
                n7.NotificationString = n7.OwnerName + " has given you a Friend Request";
                notifications.Add(n7);
            }
            List<PostLike> mutualFriendPostLikeNotification = notificationGateway.GetMutualPostLikeList(idList);
            foreach (PostLike postLike in mutualFriendPostLikeNotification)
            {
                Notification n8 = new Notification();
                n8.Type = 8;
                n8.TypeId = postLike.PostID;
                n8.OwnerId = postLike.SignupID;
                n8.OwnerName = homeGateway.GetPersonInformation(postLike.SignupID).Name;
                n8.DateTime = postLike.DateTime;
                n8.NotificationString = homeGateway.GetPersonInformation(postLike.SignupID).Name +" has Liked " + homeGateway.GetPersonInformation(postLike.OwenerID).Name +"'s Post";
                notifications.Add(n8);
            }
            List<PostLike> mutualFriendPostCommentNotification = notificationGateway.GetMutualPostCommentList(idList);
            foreach (PostLike postLike in mutualFriendPostCommentNotification)
            {
                Notification n9 = new Notification();
                n9.Type = 9;
                n9.TypeId = postLike.PostID;
                n9.OwnerId = postLike.SignupID;
                n9.OwnerName = homeGateway.GetPersonInformation(postLike.SignupID).Name;
                n9.DateTime = postLike.DateTime;
                n9.NotificationString = homeGateway.GetPersonInformation(postLike.SignupID).Name + " has Comment on " + homeGateway.GetPersonInformation(postLike.OwenerID).Name + "'s Post";
                notifications.Add(n9);
            }
            notifications = notifications.OrderByDescending(e => e.DateTime).ThenByDescending(e => e.DateTime).ToList();
            //notifications = (from e in notifications
            //             orderby e.DateTime, e.DateTime
            //             select e).ToList();


            return notifications;
        }
       
    }
}