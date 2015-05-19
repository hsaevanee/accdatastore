using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACCDataStore.Entity;

namespace ACCDataStore.Web.Areas.Nationality.ViewModels.NationalityViewModel
{
    public class NationalityViewModel
    {
        public List<NationalityObj> ListNationalityData { get; set; }
        //public string selectedschoolname { get; set; }
        public List<string> ListNationalCode { get; set; }
        public List<string> ListNational { get; set; }
        public List<string> ListGender { get; set; }
        public List<string> ListYear { get; set; }
        public Dictionary<string, string> DicNational { get; set; }
        public bool IsShowCriteria { get; set; }
    }
}