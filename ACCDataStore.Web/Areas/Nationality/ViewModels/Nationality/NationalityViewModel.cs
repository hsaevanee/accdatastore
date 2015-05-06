using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACCDataStore.Entity;

namespace ACCDataStore.Web.Areas.Nationality.ViewModels.Nationality
{
    public class NationalityViewModel
    {
        public List<NationalityData> ListNationalityData { get; set; }
        public List<string> ListNational { get; set; }
        public List<string> ListGender { get; set; }
        public List<string> ListYear { get; set; }
        public Dictionary<string, string> DicNational { get; set; }
    }
}