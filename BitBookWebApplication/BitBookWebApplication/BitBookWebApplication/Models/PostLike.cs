using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitBookWebApplication.Models
{
    public class PostLike
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int OwenerID { get; set; }
        public int SignupID { get; set; }
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
    }
}