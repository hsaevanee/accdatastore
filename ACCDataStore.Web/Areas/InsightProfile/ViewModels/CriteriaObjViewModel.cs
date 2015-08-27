using ACCDataStore.Entity;
using ACCDataStore.Entity.MySQL;
using ACCDataStore.Web.Areas.InsightProfile.ViewModels.BenchmarkMeasure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.InsightProfile.ViewModels
{
    public class CriteriaObjViewModel
    {
        public IList<School> ListSchoolNameData { get; set; }
        public string selectedschoolcode { get; set; }
        public IList<Gender> ListGenderData { get; set; }        
        public IList<Year> ListYearData { get; set; }
        public string selectedyear { get; set; }
        public IList<LeaverDestination> ListLeaverDestinationData { get; set; }
    }
}