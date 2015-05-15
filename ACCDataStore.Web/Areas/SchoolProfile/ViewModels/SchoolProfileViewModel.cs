using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels
{
    public class SchoolProfileViewModel
    {
        public List<string> ListSchoolNameData { get; set; }
        public string selectedschoolname { get; set; }
    }
}