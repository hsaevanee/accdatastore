using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity.SchoolProfiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles
{
    public class IndexSchoolProfilesViewModel  
    {
        public DataTable simdData { get; set; }
        public DataTable nationalityData { get; set; }
        public DataTable pupilStageData { get; set; }
        public DataTable englishLevelData { get; set; }
        public DataTable ethnicityData { get; set; }
        public DataTable freeSchoolMealData { get; set; }
        public string selectedschoolname2 { get; set; }
        public List<Costcentre> listSchoolname { get; set; }
        public List<Costcentre> listSelectedSchoolname { get; set; }
        public Dictionary<string, string> DicEthnicBG { get; set; }
    }
}