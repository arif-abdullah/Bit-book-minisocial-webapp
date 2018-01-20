using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitBookWebApplication.Models
{
    public class ProfilePost
    {
        public int SignupId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int PersonID { get; set; }
        public string PostDetail { get; set; }
        public HttpPostedFileBase PostPhoto { get; set; }
        public byte[] PostPhotoInBytes { get; set; }

    }
}