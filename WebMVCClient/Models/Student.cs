using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVCClient.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }

        public virtual Image Image { get; set; }
    }
}