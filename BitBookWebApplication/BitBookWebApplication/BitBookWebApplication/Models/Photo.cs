using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitBookWebApplication.Models
{
    public class Photo
    {
        public int ID { get; set; }
        public int TypeNo { get; set; }
        public byte[] PhotoInByte { get; set; }
        public string PhotoInString { get; set; }
        public int SignupID { get; set; }
        public string Name { get; set; }
        public int NoOfLike { get; set; }
        public DateTime DateTime { get; set; }
    }
}