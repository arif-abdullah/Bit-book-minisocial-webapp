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
    public class FriendGateway:CommonGateway
    {
        public List<Person> GetPersonInformation(string name)
        {
            List<Person> persons=new List<Person>();
            GenarateConnection();
            string query = "SELECT * FROM Person WHERE Name LIKE '%" + name + "%' OR Email LIKE '%" + name + "%' ";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();
            //Command.Parameters.Add("@Name", SqlDbType.VarChar);
            //Command.Parameters["@Name"].Value = name;
            Connection.Open();
            Reader = Command.ExecuteReader();           
            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    Person person = new Person();
                    person.ID = Convert.ToInt32(Reader["ID"].ToString());
                    person.Name = Reader["Name"].ToString();
                    person.Email = Reader["Email"].ToString();
                    persons.Add(person);
                }                
            }
            Connection.Close();
            return persons;
        }
        public Friend IsFriendExist(int rId,int aId)
        {
            Friend friend = null;
            GenarateConnection();
            string query = "SELECT * FROM Friend WHERE R_PersonID =@R_PersonID AND A_PersonID =@A_PersonID";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();
            Command.Parameters.Add("@R_PersonID", SqlDbType.VarChar);
            Command.Parameters["@R_PersonID"].Value = rId;
            Command.Parameters.Add("@A_PersonID", SqlDbType.VarChar);
            Command.Parameters["@A_PersonID"].Value = aId;
            Connection.Open();
            Reader = Command.ExecuteReader();
            if (Reader.HasRows)
            {
                Reader.Read();
                friend=new Friend();
                friend.ID = Convert.ToInt32(Reader["ID"].ToString());
                friend.RId = Convert.ToInt32(Reader["R_PersonID"].ToString());
                friend.AId = Convert.ToInt32(Reader["A_PersonID"].ToString());
                friend.Status = Convert.ToInt32(Reader["Status"].ToString());
            }
            
            Connection.Close();
            return friend;
        }
        public Friend IsFriendExists(int rId,int aId)
        {
            Friend friend = null;
            GenarateConnection();
            string query = "SELECT * FROM Friend WHERE R_PersonID =@A_PersonID AND A_PersonID =@R_PersonID";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();
            Command.Parameters.Add("@R_PersonID", SqlDbType.VarChar);
            Command.Parameters["@R_PersonID"].Value = rId;
            Command.Parameters.Add("@A_PersonID", SqlDbType.VarChar);
            Command.Parameters["@A_PersonID"].Value = aId;
            Connection.Open();
            Reader = Command.ExecuteReader();
            if (Reader.HasRows)
            {
                Reader.Read();
                friend=new Friend();
                friend.ID = Convert.ToInt32(Reader["ID"].ToString());
                friend.RId = Convert.ToInt32(Reader["R_PersonID"].ToString());
                friend.AId = Convert.ToInt32(Reader["A_PersonID"].ToString());
                friend.Status = Convert.ToInt32(Reader["Status"].ToString());
            }
            
            Connection.Close();
            return friend;
        }        
        public bool SetFriend(int rId, int aId)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "insert into Friend(R_PersonID,A_PersonID,Status,DateTime) values (@R_PersonID,@A_PersonID,@Status,@DateTime);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@R_PersonID", SqlDbType.VarChar);
                Command.Parameters["@R_PersonID"].Value = rId;
                Command.Parameters.Add("@A_PersonID", SqlDbType.VarChar);
                Command.Parameters["@A_PersonID"].Value = aId;
                Command.Parameters.Add("@Status", SqlDbType.VarChar);
                Command.Parameters["@Status"].Value = 1;
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
        public bool UpdateFriend(Friend friend)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "update Friend set Status=@Status , DateTime=@DateTime where ID=@ID;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@ID", SqlDbType.VarChar);
                Command.Parameters["@ID"].Value = friend.ID;
                Command.Parameters.Add("@Status", SqlDbType.Int);
                Command.Parameters["@Status"].Value = 2;
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
        public bool DeleteFriend(Friend friend)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "delete from Friend where ID=@ID;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();
                Command.Parameters.Add("@ID", SqlDbType.VarChar);
                Command.Parameters["@ID"].Value = friend.ID;
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
        public bool DeleteFriendByBothID(Friend friend)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "delete from Friend where (R_PersonID=@R_PersonID and A_PersonID=@A_PersonID) or (A_PersonID=@R_PersonID and R_PersonID=@A_PersonID);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();
                Command.Parameters.Add("@A_PersonID", SqlDbType.VarChar);
                Command.Parameters["@A_PersonID"].Value = friend.ID;
                Command.Parameters.Add("@R_PersonID", SqlDbType.VarChar);
                Command.Parameters["@R_PersonID"].Value = friend.RId;
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
    }
}