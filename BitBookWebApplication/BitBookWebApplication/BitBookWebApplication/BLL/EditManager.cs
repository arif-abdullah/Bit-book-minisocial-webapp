using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitBookWebApplication.DAL;
using BitBookWebApplication.Models;

namespace BitBookWebApplication.BLL
{
    public class EditManager
    {
        EditGateway editGateway=new EditGateway();
        HomeGateway homeGateway=new HomeGateway();
        SignUpGateway signUpGateway=new SignUpGateway();

        public AllPersonalInformation GetAllPersonalInformation(int id)
        {
            AllPersonalInformation allPersonalInformation = new AllPersonalInformation();
            AllPersonalInformation personal = GetPersonInformation(id);
            AllPersonalInformation addition = GetAdditionInformation(id);
            Photo profilePhoto = GetProfilePhotoByID(addition.ProfilePhotoId);
            Photo coverPhoto = GetCoverPhotoByID(addition.CoverPhotoId);

            allPersonalInformation.SignupID = id;
            allPersonalInformation.AdditionalInfoID=addition.AdditionalInfoID;
            allPersonalInformation.Password=personal.Password;
            allPersonalInformation.Email=personal.Email;

            allPersonalInformation.ProfilePhotoId=addition.ProfilePhotoId;
            if (allPersonalInformation.ProfilePhotoId!=0)
            {
                allPersonalInformation.ProfilePhotoInString=profilePhoto.PhotoInString;
                allPersonalInformation.PhotoInByte=profilePhoto.PhotoInByte;
            }
            
            allPersonalInformation.CoverPhotoId=addition.CoverPhotoId;
            if (allPersonalInformation.CoverPhotoId!=0)
            {
                allPersonalInformation.CoverPhotoInString=coverPhoto.PhotoInString;
                allPersonalInformation.CoverPhotoInByte = coverPhoto.PhotoInByte;
            }
            
            allPersonalInformation.Name=personal.Name;
            allPersonalInformation.OldPassword=null;
            allPersonalInformation.NewPassword=null;
            allPersonalInformation.ConfirmPassword=null;
            allPersonalInformation.AboutMe=addition.AboutMe;
            allPersonalInformation.AreaOfInterest=addition.AreaOfInterest;
            allPersonalInformation.MobileNumber=personal.MobileNumber;
            allPersonalInformation.Religion=addition.Religion;
            allPersonalInformation.Address=addition.Address;
            allPersonalInformation.DateOfBirth=personal.DateOfBirth;
            allPersonalInformation.Gender=personal.Gender;


            return allPersonalInformation;
        }

        public bool UpdateAllInformation(AllPersonalInformation allPersonalInformation)
        {
            bool result = false;
            if (allPersonalInformation.PhotoInByte!=null)
            {
                if (!SetProfilePhoto(allPersonalInformation))
                {
                    return result;
                }
                allPersonalInformation.ProfilePhotoId = GetProfilePhotoID(allPersonalInformation.SignupID);
            }
            if (allPersonalInformation.CoverPhotoInByte != null)
            {
                if (!SetCoverPhoto(allPersonalInformation))
                {
                    return result;
                }
                allPersonalInformation.CoverPhotoId = GetCoverPhotoID(allPersonalInformation.SignupID);
            }
            if (!UpdatePersonInformation(allPersonalInformation))
            {
                return result;
            }
            if (!UpdateAdditionalInformation(allPersonalInformation))
            {
                return result;
            }
            result = true;
            return result;
        }
        public AllPersonalInformation GetPersonInformation(int id)
        {
            return editGateway.GetPersonInformation(id);
        }

        public AllPersonalInformation GetAdditionInformation(int id)
        {
            return editGateway.GetAdditionInformation(id);
        }

        public bool UpdatePersonInformation(AllPersonalInformation allPersonalInformation)
        {
            return editGateway.UpdatePersonInformation(allPersonalInformation);
        }

        public bool UpdateAdditionalInformation(AllPersonalInformation allPersonalInformation)
        {
            return editGateway.UpdateAdditionalInformation(allPersonalInformation);
        }

        public Photo GetProfilePhotoByID(int id)
        {
            return editGateway.GetProfilePhotoByID(id);
        }

        public bool SetProfilePhoto(AllPersonalInformation allPersonalInformation)
        {
            Photo photo=new Photo();
            photo.PhotoInByte = allPersonalInformation.PhotoInByte;
            photo.SignupID = allPersonalInformation.SignupID;
            return signUpGateway.SetProfilePhoto(photo);
        }

        public int GetProfilePhotoID(int signupID)
        {
            return signUpGateway.GetProfilePhotoID(signupID);
        }

        public Photo GetCoverPhotoByID(int id)
        {
            return editGateway.GetCoverPhotoByID(id);
        }
        public bool SetCoverPhoto(AllPersonalInformation allPersonalInformation)
        {
            Photo photo = new Photo();
            photo.PhotoInByte = allPersonalInformation.CoverPhotoInByte;
            photo.SignupID = allPersonalInformation.SignupID;
            return signUpGateway.SetCoverPhoto(photo);
        }

        public int GetCoverPhotoID(int signupID)
        {
            return signUpGateway.GetCoverPhotoID(signupID);
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

    }
}