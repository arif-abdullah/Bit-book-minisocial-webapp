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
    public class SignUpGateway:CommonGateway
    {
        public bool IsEmailExist(string email)
        {
            GenarateConnection();
            string query = "SELECT * FROM Person WHERE Email =@Email";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            Command.Parameters.Add("@Email", SqlDbType.VarChar);
            Command.Parameters["@Email"].Value = email;
            Connection.Open();
            Reader = Command.ExecuteReader();

            bool result = Reader.HasRows;
            Connection.Close();
            return result;
        }
        public string GetPersonID(string email)
        {
            GenarateConnection();
            string personId = "0";
            string query = "SELECT * FROM Person WHERE Email =@Email";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            Command.Parameters.Add("@Email", SqlDbType.VarChar);
            Command.Parameters["@Email"].Value = email;
            Connection.Open();
            Reader = Command.ExecuteReader();

            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    personId = Reader["ID"].ToString();
                }
            }
            Connection.Close();
            return personId;
        }
        public string GetPassword(string email)
        {
            GenarateConnection();
            string password = null;
            string query = "SELECT * FROM Person WHERE Email =@Email";
            Command = new SqlCommand(query, Connection);
            Command.Parameters.Clear();

            Command.Parameters.Add("@Email", SqlDbType.VarChar);
            Command.Parameters["@Email"].Value = email;
            Connection.Open();
            Reader = Command.ExecuteReader();

            if (Reader.HasRows)
            {
                while (Reader.Read())
                {
                    password = Reader["Password"].ToString();
                }
            }
            
            Connection.Close();
            return password;
        }
        
        public bool SetPersonInformation(Person person)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "insert into Person(Name,Email,Password,DateOfBirth,Gender) values (@Name,@Email,@Password,@DateOfBirth,@Gender);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@Name", SqlDbType.VarChar);
                Command.Parameters["@Name"].Value = person.Name;
                Command.Parameters.Add("@Email", SqlDbType.VarChar);
                Command.Parameters["@Email"].Value = person.Email;
                Command.Parameters.Add("@Password", SqlDbType.VarChar);
                Command.Parameters["@Password"].Value = person.Password;
                Command.Parameters.Add("@DateOfBirth", SqlDbType.VarChar);
                Command.Parameters["@DateOfBirth"].Value = person.DateOfBirth;
                Command.Parameters.Add("@Gender", SqlDbType.Int);
                Command.Parameters["@Gender"].Value = Convert.ToInt32(person.Gender);
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
        public int GetEducationInformation(AdditionalInfo additionalInfo)
        {
            GenarateConnection();
            int id = 0;
            try
            {
                using (Connection)
                {
                    Connection.Open();
                    string query =
                        "select * from Education where EducationLevel=@EducationLevel and Name=@Name and Year=@year;";
                    Command = new SqlCommand(query, Connection);
                    Command.Parameters.Clear();

                    Command.Parameters.Add("@EducationLevel", SqlDbType.VarChar);
                    Command.Parameters["@EducationLevel"].Value = additionalInfo.EdLevel;
                    Command.Parameters.Add("@Name", SqlDbType.VarChar);
                    Command.Parameters["@Name"].Value = additionalInfo.EdName;
                    Command.Parameters.Add("@Year", SqlDbType.VarChar);
                    Command.Parameters["@Year"].Value = additionalInfo.Edyear;
                    Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            id = Convert.ToInt32(Reader["ID"].ToString());
                        }
                    }
                }
                return id;
            }
            catch (Exception)
            {

                return 0;
            }
            
        }
        public bool SetEducationInformation(AdditionalInfo additionalInfo)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "insert into Education(EducationLevel,Name,Year) values (@EducationLevel,@Name,@year);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@EducationLevel", SqlDbType.VarChar);
                Command.Parameters["@EducationLevel"].Value = additionalInfo.EdLevel;
                Command.Parameters.Add("@Name", SqlDbType.VarChar);
                Command.Parameters["@Name"].Value = additionalInfo.EdName;
                Command.Parameters.Add("@Year", SqlDbType.VarChar);
                Command.Parameters["@Year"].Value = additionalInfo.Edyear;

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
        public int GetJobInformation(AdditionalInfo additionalInfo)
        {
            try
            {           
            GenarateConnection();
                int id = 0;          
            using (Connection)
            {
                Connection.Open();
                string query =
                    "select * from Job where Name=@Name and Position=@Position and BeginningTime=@BeginningTime  and EndingTime=@EndingTime;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@Name", SqlDbType.VarChar);
                Command.Parameters["@Name"].Value = additionalInfo.JobName;
                Command.Parameters.Add("@Position", SqlDbType.VarChar);
                Command.Parameters["@Position"].Value = additionalInfo.JobPosition;
                Command.Parameters.Add("@BeginningTime", SqlDbType.VarChar);
                Command.Parameters["@BeginningTime"].Value = additionalInfo.JobBegin;
                Command.Parameters.Add("@EndingTime", SqlDbType.VarChar);
                Command.Parameters["@EndingTime"].Value = additionalInfo.JobEnd;
                Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        id = Convert.ToInt32(Reader["ID"].ToString());
                    }
                }
            }
            return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public bool SetJobInformation(AdditionalInfo additionalInfo)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "insert into Job(Name,Position,BeginningTime,EndingTime) values (@Name,@Position,@BeginningTime,@EndingTime);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@Name", SqlDbType.VarChar);
                Command.Parameters["@Name"].Value = additionalInfo.JobName;
                Command.Parameters.Add("@Position", SqlDbType.VarChar);
                Command.Parameters["@Position"].Value = additionalInfo.JobPosition;
                Command.Parameters.Add("@BeginningTime", SqlDbType.VarChar);
                Command.Parameters["@BeginningTime"].Value = additionalInfo.JobBegin;
                Command.Parameters.Add("@EndingTime", SqlDbType.VarChar);
                Command.Parameters["@EndingTime"].Value = additionalInfo.JobEnd;

                try
                {
                    Command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public bool SetAdditionInformation(AdditionalInfo additionalInfo)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "insert into PersonalInformation(SignupID,ProfilePhoto,CoverPhoto,AboutMe,Religion,EducationID,JobID) values (@SignupID,@ProfilePhoto,@ProfilePhoto,@AboutMe,@Religion,@EducationID,@JobID);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@SignupID", SqlDbType.VarChar);
                Command.Parameters["@SignupID"].Value = additionalInfo.SignupID;
                Command.Parameters.Add("@ProfilePhoto", SqlDbType.Int);
                Command.Parameters["@ProfilePhoto"].Value = additionalInfo.ProfilePhotoId;
                Command.Parameters.Add("@CoverPhoto", SqlDbType.Int);
                Command.Parameters["@CoverPhoto"].Value = additionalInfo.CoverPhotoId;
                Command.Parameters.Add("@AboutMe", SqlDbType.VarChar);
                Command.Parameters["@AboutMe"].Value = additionalInfo.AboutMe;
                Command.Parameters.Add("@Religion", SqlDbType.VarChar);
                Command.Parameters["@Religion"].Value = additionalInfo.Religion;
                Command.Parameters.Add("@EducationID", SqlDbType.Int);
                Command.Parameters["@EducationID"].Value = additionalInfo.EducationID;
                Command.Parameters.Add("@JobID", SqlDbType.Int);
                Command.Parameters["@JobID"].Value = additionalInfo.JobID;

                

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
        public int GetAdditionalInformationID(AdditionalInfo additionalInfo)
        {
            GenarateConnection();
            int id = 0;
            using (Connection)
            {
                Connection.Open();
                string query = "select * from PersonalInformation where SignupID=@SignupID;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@SignupID", SqlDbType.VarChar);
                Command.Parameters["@SignupID"].Value = additionalInfo.SignupID;
                Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        id = Convert.ToInt32(Reader["ID"].ToString());
                    }
                }
            }
            return id;
        }
        public bool SetAdditionPersonInformation(AdditionalInfo additionalInfo)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "UPDATE Person SET MobileNo=@MobileNo,ProfileID=@ProfileID WHERE ID=@ID;";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@MobileNo", SqlDbType.VarChar);
                Command.Parameters["@MobileNo"].Value = additionalInfo.MobileNumber;
                Command.Parameters.Add("@ProfileID", SqlDbType.Int);
                Command.Parameters["@ProfileID"].Value = additionalInfo.ID;
                Command.Parameters.Add("@ID", SqlDbType.Int);
                Command.Parameters["@ID"].Value = additionalInfo.SignupID;

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
        public bool SetProfilePhoto(Photo photo)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "insert into ProfilePhoto(SignupID,Photo,DateTime) values (@SignupID,@Photo,@DateTime);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@Photo", SqlDbType.VarBinary);
                Command.Parameters["@Photo"].Value = photo.PhotoInByte;
                Command.Parameters.Add("@SignupID", SqlDbType.VarChar);
                Command.Parameters["@SignupID"].Value = photo.SignupID;
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
        public int GetProfilePhotoID(int signupID)
        {
            int id = 0;
            GenarateConnection();
            string query = "SELECT * FROM ProfilePhoto WHERE SignupID=@SignupID";
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
                    id = Convert.ToInt32(Reader["ID"].ToString());                  
                }
            }
            Connection.Close();
            return id;
        }
        public bool SetCoverPhoto(Photo photo)
        {
            GenarateConnection();
            using (Connection)
            {
                Connection.Open();
                string query =
                    "insert into CoverPhoto(SignupID,Photo,DateTime) values (@SignupID,@Photo,@DateTime);";
                Command = new SqlCommand(query, Connection);
                Command.Parameters.Clear();

                Command.Parameters.Add("@Photo", SqlDbType.VarBinary);
                Command.Parameters["@Photo"].Value = photo.PhotoInByte;
                Command.Parameters.Add("@SignupID", SqlDbType.VarChar);
                Command.Parameters["@SignupID"].Value = photo.SignupID;
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
        public int GetCoverPhotoID(int signupID)
        {
            int id = 0;
            GenarateConnection();
            string query = "SELECT * FROM CoverPhoto WHERE SignupID=@SignupID";
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
                    id = Convert.ToInt32(Reader["ID"].ToString());
                }
            }
            Connection.Close();
            return id;
        }
    }
}