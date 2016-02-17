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
    }
}