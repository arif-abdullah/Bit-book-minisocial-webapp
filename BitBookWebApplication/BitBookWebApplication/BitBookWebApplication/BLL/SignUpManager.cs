using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitBookWebApplication.DAL;
using BitBookWebApplication.Models;

namespace BitBookWebApplication.BLL
{
    public class SignUpManager
    {
        SignUpGateway signUpGateway=new SignUpGateway();

        public bool IsEmailExist(string email)
        {
            return signUpGateway.IsEmailExist(email);
        }

        public string GetPersonID(string email)
        {
            return signUpGateway.GetPersonID(email);
        }

        public string GetPassword(string email)
        {
            return signUpGateway.GetPassword(email);
        }

        public bool SetPersonInformation(Person person)
        {
            return signUpGateway.SetPersonInformation(person);
        }

        public int GetEducationInformation(AdditionalInfo additionalInfo)
        {
            return signUpGateway.GetEducationInformation(additionalInfo);
        }

        public bool SetEducationInformation(AdditionalInfo additionalInfo)
        {
            return signUpGateway.SetEducationInformation(additionalInfo);
        }

        public int GetJobInformation(AdditionalInfo additionalInfo)
        {
            return signUpGateway.GetJobInformation(additionalInfo);
        }

        public bool SetJobInformation(AdditionalInfo additionalInfo)
        {
            return signUpGateway.SetJobInformation(additionalInfo);
        }

        public bool SetAdditionInformation(AdditionalInfo additionalInfo)
        {
            if (additionalInfo.PhotoInByte != null)
            {
                Photo image = new Photo() { PhotoInByte = additionalInfo.PhotoInByte, SignupID = additionalInfo.SignupID };
                if (!signUpGateway.SetProfilePhoto(image))
                {
                    return false;
                }
                additionalInfo.ProfilePhotoId = signUpGateway.GetProfilePhotoID(additionalInfo.SignupID);
            }
            else
            {
                additionalInfo.ProfilePhotoId = 0;
            }

            if (additionalInfo.CoverPhotoInByte != null)
            {
                Photo image = new Photo() { PhotoInByte = additionalInfo.CoverPhotoInByte, SignupID = additionalInfo.SignupID };
                if (!signUpGateway.SetCoverPhoto(image))
                {
                    return false;
                }
                additionalInfo.CoverPhotoId = signUpGateway.GetCoverPhotoID(additionalInfo.SignupID);
            }
            else
            {
                additionalInfo.CoverPhotoId = 0;
            } 
            return signUpGateway.SetAdditionInformation(additionalInfo);
        }

        public int GetAdditionalInformationID(AdditionalInfo additionalInfo)
        {
            return signUpGateway.GetAdditionalInformationID(additionalInfo);
        }

        public bool SetAdditionPersonInformation(AdditionalInfo additionalInfo)
        {                 
            return signUpGateway.SetAdditionPersonInformation(additionalInfo);
        }
    }
}