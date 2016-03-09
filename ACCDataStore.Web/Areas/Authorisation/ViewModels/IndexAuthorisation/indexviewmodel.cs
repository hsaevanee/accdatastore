using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.Authorisation.ViewModels
{
    public class IndexViewModel
    {
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string firstname { get; set; }
        public string jobtitle { get; set; }
        public string email { get; set; }
        public string confirmpassword { get; set; }
    }
}