using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolRollForecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolRollForecast.ViewModels
{
    public class SchoolRollForecastViewModel
    {
        public SchoolRollForecastObj schObj { get; set; }
        public List<School> listSchoolname { get; set; }
        public string selectedschoolname { get; set; }
    }
}