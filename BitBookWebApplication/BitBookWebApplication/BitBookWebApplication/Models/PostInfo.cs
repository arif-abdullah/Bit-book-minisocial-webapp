using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitBookWebApplication.Models
{
    public class PostInfo
    {
        public int Id { get; set; }
        public int PersonID { get; set; }
        public string PostDetail { get; set; }
        public int NoOfLike { get; set; }
        public DateTime DateTime { get; set; }
        public int SignUpID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public HttpPostedFileBase PostPhoto { get; set; }
        public byte[] PostPhotoInByte { get; set; }
        public string PostPhotoInString { get; set; }
    }
}