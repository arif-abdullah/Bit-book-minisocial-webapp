using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BitBookWebApplication.App_Start.DAL;
using BitBookWebApplication.Models;

namespace BitBookWebApplication.DAL
{
    public class HomeGateway:CommonGateway
    {
        public Person GetPersonInformation(int id)
        {
            Person person=new Person();
            GenarateConnection();
            string query = "SELECT * FROM Person WHERE ID =@ID";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            Command.Parameters.Add("@ID", SqlDbType.VarChar);
            Command.Parameters["@ID"].Value = id;
            Connection.Open();
            Reader = Command.ExecuteReader();
            Reader.Read();
            if (Reader.HasRows)
            {
                person.ProfileID = Reader["ID"].ToString();
                person.Name = Reader["Name"].ToString();
                person.Email = Reader["Email"].ToString();
                person.Password = Reader["Password"].ToString();
            }
            Connection.Close();
            return person;
        }
        public int GetPostIdByCommentId(int id)
        {
            int postId = 0;
            GenarateConnection();
            string query = "SELECT * FROM Comment WHERE ID =@ID";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            Command.Parameters.Add("@ID", SqlDbType.VarChar);
            Command.Parameters["@ID"].Value = id;
            Connection.Open();
            Reader = Command.ExecuteReader();
            Reader.Read();
            if (Reader.HasRows)
            {
                postId = Convert.ToInt32(Reader["PostID"].ToString());
            }
            Connection.Close();
            return postId;
        }
        public Photo GetProfilePhotoByID(int signupID)
        {
            Photo photo = new Photo();
            GenarateConnection();
            string query = "SELECT * FROM ProfilePhoto WHERE SignupID=@SignupID ORDER BY ID DESC";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();
            Command.Parameters.Add("@SignupID", SqlDbType.VarChar);
            Command.Parameters["@SignupID"].Value = signupID;
            Connection.Open();
            Reader = Command.ExecuteReader();

            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    
                    photo.ID = Convert.ToInt32(Reader["ID"].ToString());
                    photo.SignupID = Convert.ToInt32(Reader["SignupID"].ToString());
                    photo.PhotoInByte = (Reader["Photo"]) as byte[];
                    photo.PhotoInString = Reader["Photo"] as string;
                    photo.DateTime = Convert.ToDateTime(Reader["DateTime"].ToString());
                }
            }
            Connection.Close();
            return photo;
        }
        
        public bool SetPostInformation(PostInfo postInfo)
        {
            GenarateConnection();
            using (Connection)
            {
                string query = null;
                Connection.Open();
                if (postInfo.PostPhotoInByte != null & postInfo.PostDetail != null)
                {
                    query = "insert into Post(Detail,SignUpID,NoOfLike,DateTime,Photo) values (@Detail,@SignUpID,@NoOfLike,@DateTime,@Photo);";
                }
                else if (postInfo.PostDetail == null)
                {
                    query = "insert into Post(SignUpID,NoOfLike,DateTime,Photo) values (@SignUpID,@NoOfLike,@DateTime,@Photo);";
                }
                else
                {
                    query = "insert into Post(Detail,SignUpID,NoOfLike,DateTime) values (@Detail,@SignUpID,@NoOfLike,@DateTime);";
                }

                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                if (postInfo.PostDetail != null)
                {
                    Command.Parameters.Add("@Detail", SqlDbType.VarChar);
                    Command.Parameters["@Detail"].Value = postInfo.PostDetail;
                }
                
                Command.Parameters.Add("@NoOfLike", SqlDbType.Int);
                Command.Parameters["@NoOfLike"].Value = 0;
                Command.Parameters.Add("@DateTime", SqlDbType.DateTime);
                Command.Parameters["@DateTime"].Value = DateTime.Now;
                Command.Parameters.Add("@SignUpID", SqlDbType.VarChar);
                Command.Parameters["@SignUpID"].Value = postInfo.SignUpID;
                if (postInfo.PostPhotoInByte != null)
                {
                    Command.Parameters.Add("@Photo", SqlDbType.VarBinary);
                    Command.Parameters["@Photo"].Value = postInfo.PostPhotoInByte;
                }
                
                try
                {
                    Command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception("Error While Entering data into database");
                }
            }
        }
        public bool SetPostInOtherWall(ProfilePost postInfo)
        {
            GenarateConnection();
            using (Connection)
            {
                string query = null;
                Connection.Open();
                if (postInfo.PostPhotoInBytes != null)
                {
                    query = "insert into Post(Detail,SignUpID,NoOfLike,DateTime,Photo,OfWall) values (@Detail,@SignUpID,@NoOfLike,@DateTime,@Photo,@OfWall);";
                }
                else
                {
                    query = "insert into Post(Detail,SignUpID,NoOfLike,DateTime,OfWall) values (@Detail,@SignUpID,@NoOfLike,@DateTime,@OfWall);";
                }

                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@Detail", SqlDbType.VarChar);
                Command.Parameters["@Detail"].Value = postInfo.PostDetail;
                Command.Parameters.Add("@NoOfLike", SqlDbType.Int);
                Command.Parameters["@NoOfLike"].Value = 0;
                Command.Parameters.Add("@DateTime", SqlDbType.DateTime);
                Command.Parameters["@DateTime"].Value = DateTime.Now;
                Command.Parameters.Add("@SignUpID", SqlDbType.VarChar);
                Command.Parameters["@SignUpID"].Value = postInfo.SignupId;
                Command.Parameters.Add("@OfWall", SqlDbType.VarChar);
                Command.Parameters["@OfWall"].Value = postInfo.PersonID;
                if (postInfo.PostPhotoInBytes != null)
                {
                    Command.Parameters.Add("@Photo", SqlDbType.VarBinary);
                    Command.Parameters["@Photo"].Value = postInfo.PostPhotoInBytes;
                }

                try
                {
                    Command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception("Error While Entering data into database");
                }
            }
        }
        public bool DeletePost(int postId)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query = "delete from Post where ID=@ID; delete from LikePost where PostID=@ID; delete from Comment where PostID=@ID;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();
                Command.Parameters.Add("@ID", SqlDbType.Int);
                Command.Parameters["@ID"].Value = postId;
                try
                {
                    Command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception("Error While Entering data into database");
                }
            }
        }
        public PostInfo GetPostInformationById(int id)
        {
            PostInfo postInfo = new PostInfo();
            GenarateConnection();
            string query = "SELECT * FROM Post WHERE ID=@ID";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();
            Command.Parameters.Add("@ID", SqlDbType.VarChar);
            Command.Parameters["@ID"].Value = id;
            Connection.Open();
            Reader = Command.ExecuteReader();

            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    postInfo.Id = Convert.ToInt32(Reader["ID"].ToString());
                    postInfo.PostDetail = Reader["Detail"].ToString();
                    postInfo.NoOfLike = Convert.ToInt32(Reader["NoOfLike"].ToString());
                    postInfo.DateTime = Convert.ToDateTime(Reader["DateTime"].ToString());
                    postInfo.SignUpID = Convert.ToInt32(Reader["SignUpID"].ToString());
                    postInfo.PostPhotoInByte = (Reader["Photo"]) as byte[];
                    postInfo.PostPhotoInString = Reader["Photo"] as string;
                }
            }
            Connection.Close();
            return postInfo;
        }
        public List<PostInfo> GetPostInformation(List<int> idList)
        {
            List<PostInfo> postInfos=new List<PostInfo>();
            GenarateConnection();
            string query = "SELECT * FROM Post WHERE ";
            int no = 0;
            foreach (int id in idList)
            {                
                if (no == 0)
                {
                    query += "SignUpID=" + id + " ";
                    no = no + 1;
                }
                else
                {
                    query += "OR SignUpID=" + id + " ";
                }

            }
            query += " ORDER BY ID DESC;";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            //Command.Parameters.Add("@ID", SqlDbType.VarChar);
            //Command.Parameters["@ID"].Value = id;
            Connection.Open();
            Reader = Command.ExecuteReader();
            
            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    PostInfo postInfo = new PostInfo();
                    postInfo.Id = Convert.ToInt32(Reader["ID"].ToString());
                    postInfo.PostDetail = Reader["Detail"].ToString();
                    postInfo.NoOfLike = Convert.ToInt32(Reader["NoOfLike"].ToString());
                    postInfo.DateTime = Convert.ToDateTime(Reader["DateTime"].ToString());
                    postInfo.SignUpID = Convert.ToInt32(Reader["SignUpID"].ToString());
                    postInfo.PostPhotoInByte = Reader["Photo"] as byte[];
                    postInfos.Add(postInfo);
                }               
            }
            Connection.Close();
            return postInfos;
        }

        public List<int> GetAllFriendList(int id)
        {
            List<int> friendList = new List<int>();
            friendList.Add(id);
            GenarateConnection();
            string query = "select * from Friend where (R_PersonID=@ID or A_PersonID=@ID) and Status=@Status;";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            Command.Parameters.Add("@ID", SqlDbType.VarChar);
            Command.Parameters["@ID"].Value = id;
            Command.Parameters.Add("@Status", SqlDbType.VarChar);
            Command.Parameters["@Status"].Value = 2;

            Connection.Open();
            Reader = Command.ExecuteReader();

            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    int aId = Convert.ToInt32(Reader["R_PersonID"].ToString());
                    int bId = Convert.ToInt32(Reader["A_PersonID"].ToString());
                    if (aId==id)
                    {
                        friendList.Add(bId);
                    }
                    else
                    {
                        friendList.Add(aId);
                    }
                    
                }
            }
            Connection.Close();
            return friendList;
        }
        public bool SetPostLike(int postId,int signUpId,int owenerId)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query = "insert into LikePost(PostID,SignUpID,OwenerID,DateTime) values (@PostID,@SignUpID,@OwenerID,@DateTime);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@PostID", SqlDbType.VarChar);
                Command.Parameters["@PostID"].Value = postId;
                Command.Parameters.Add("@SignUpID", SqlDbType.VarChar);
                Command.Parameters["@SignUpID"].Value = signUpId;
                Command.Parameters.Add("@OwenerID", SqlDbType.VarChar);
                Command.Parameters["@OwenerID"].Value = owenerId;
                Command.Parameters.Add("@DateTime", SqlDbType.DateTime);
                Command.Parameters["@DateTime"].Value = DateTime.Now;
                try
                {
                    Command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception("Error While Entering data into database");
                }
            }
        }
        public bool IsPostLike(int postId, int signUpId)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query = "select * from LikePost where PostID=@PostID and SignUpID=@SignUpID;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@PostID", SqlDbType.VarChar);
                Command.Parameters["@PostID"].Value = postId;
                Command.Parameters.Add("@SignUpID", SqlDbType.VarChar);
                Command.Parameters["@SignUpID"].Value = signUpId;
                Reader = Command.ExecuteReader();
                bool result = Reader.HasRows;
                return result;                
            }
        }
        public bool DeletePostLike(int postId, int signUpId)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query = "delete from LikePost where PostID=@PostID and SignUpID=@SignUpID;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();
                Command.Parameters.Add("@PostID", SqlDbType.VarChar);
                Command.Parameters["@PostID"].Value = postId;
                Command.Parameters.Add("@SignUpID", SqlDbType.VarChar);
                Command.Parameters["@SignUpID"].Value = signUpId;
                try
                {
                    Command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception("Error While Entering data into database");
                }
            }
        }
        public List<int> NumberOfPostLike(int postId)
        {
            List<int> likeList=new List<int>();
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query = "select * from LikePost where PostID=@PostID;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@PostID", SqlDbType.VarChar);
                Command.Parameters["@PostID"].Value = postId;
                Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        int id = Convert.ToInt32(Reader["SignUpID"].ToString());
                        likeList.Add(id);
                    }
                }
                return likeList;
            }
        }
        public bool SetPhotoComment(CommentDetail commentDetail, int owenerId)
        {
            //PersonId-- who give the comment
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query = "insert into CommentPhoto(Comment,TypeID,PersonID,OwnerID,DateTime,TypeNo) values (@Comment,@TypeID,@PersonID,@OwnerID,@DateTime,@TypeNo);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@Comment", SqlDbType.VarChar);
                Command.Parameters["@Comment"].Value = commentDetail.Comment;
                Command.Parameters.Add("@TypeID", SqlDbType.VarChar);
                Command.Parameters["@TypeID"].Value = commentDetail.PostID;
                Command.Parameters.Add("@PersonID", SqlDbType.VarChar);
                Command.Parameters["@PersonID"].Value = commentDetail.PersonID;
                Command.Parameters.Add("@OwnerID", SqlDbType.VarChar);
                Command.Parameters["@OwnerID"].Value = owenerId;
                Command.Parameters.Add("@DateTime", SqlDbType.DateTime);
                Command.Parameters["@DateTime"].Value = DateTime.Now;
                Command.Parameters.Add("@TypeNo", SqlDbType.VarChar);
                Command.Parameters["@TypeNo"].Value = commentDetail.TypeNo;

                try
                {
                    Command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception("Error While Entering data into database");
                }
            }
        }
        public bool SetComment(CommentDetail commentDetail, int owenerId)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query = "insert into Comment(Comment,PostID,PersonID,OwenerID,DateTime) values (@Comment,@PostID,@PersonID,@OwenerID,@DateTime);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@Comment", SqlDbType.VarChar);
                Command.Parameters["@Comment"].Value = commentDetail.Comment;
                Command.Parameters.Add("@PostID", SqlDbType.VarChar);
                Command.Parameters["@PostID"].Value = commentDetail.PostID;
                Command.Parameters.Add("@PersonID", SqlDbType.VarChar);
                Command.Parameters["@PersonID"].Value = commentDetail.PersonID;
                Command.Parameters.Add("@OwenerID", SqlDbType.VarChar);
                Command.Parameters["@OwenerID"].Value = owenerId;
                Command.Parameters.Add("@DateTime", SqlDbType.DateTime);
                Command.Parameters["@DateTime"].Value = DateTime.Now;
                try
                {
                    Command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception("Error While Entering data into database");
                }
            }
        }
        public List<CommentDetail> GetAllComment(int id)
        {
            List<CommentDetail> commentDetails = new List<CommentDetail>();
            GenarateConnection();
            string query = "select * from Comment where PostID=@PostID ORDER BY ID DESC;";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            Command.Parameters.Add("@PostID", SqlDbType.VarChar);
            Command.Parameters["@PostID"].Value = id;
            Connection.Open();
            Reader = Command.ExecuteReader();

            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    CommentDetail commentDetail=new CommentDetail();
                    commentDetail.ID = Convert.ToInt32(Reader["ID"].ToString());
                    commentDetail.Comment = Reader["Comment"].ToString();
                    commentDetail.PostID = Convert.ToInt32(Reader["PostID"].ToString());
                    commentDetail.PersonID = Convert.ToInt32(Reader["PersonID"].ToString());
                    commentDetails.Add(commentDetail);
                }
            }
            Connection.Close();
            return commentDetails;
        }
        public List<CommentDetail> GetAllPhotoComment(int typeId,int typeNo)
        {
            List<CommentDetail> commentDetails = new List<CommentDetail>();
            GenarateConnection();
            string query = "select * from CommentPhoto where TypeID=@TypeID and TypeNo=@TypeNo ORDER BY ID DESC;";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            Command.Parameters.Add("@TypeID", SqlDbType.VarChar);
            Command.Parameters["@TypeID"].Value = typeId;
            Command.Parameters.Add("@TypeNo", SqlDbType.VarChar);
            Command.Parameters["@TypeNo"].Value = typeNo;
            Connection.Open();
            Reader = Command.ExecuteReader();

            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    CommentDetail commentDetail = new CommentDetail();
                    commentDetail.ID = Convert.ToInt32(Reader["ID"].ToString());
                    commentDetail.Comment = Reader["Comment"].ToString();
                    commentDetail.PostID = Convert.ToInt32(Reader["TypeID"].ToString());
                    commentDetail.PersonID = Convert.ToInt32(Reader["PersonID"].ToString());
                    commentDetail.TypeNo = Convert.ToInt32(Reader["TypeNo"].ToString());
                    commentDetails.Add(commentDetail);
                }
            }
            Connection.Close();
            return commentDetails;
        }   
    }
}