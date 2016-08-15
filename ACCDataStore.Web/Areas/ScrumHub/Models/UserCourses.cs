using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.ScrumHub.Models
{
    public class UserCourses
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int age { get; set;  }
        public string course_code { get; set;  }
        public string course_title { get; set;  }
        public string coordinator { get; set; }
    }
}