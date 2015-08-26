using ACCDataStore.Entity;
using ACCDataStore.Entity.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.InsightProfile.ViewModels
{
    public class CriteriaObjViewModel
    {
        public IEnumerable<School> ListSchoolNameData { get; set; }
        public string selectedschoolcode { get; set; }
    }
}