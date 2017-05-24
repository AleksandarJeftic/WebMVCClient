using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVCClient.Models
{
    public class Image
    {
        public int ImageID { get; set; }
        public string ImagePath { get; set; }

        public virtual Student Student { get; set; }
    }
}