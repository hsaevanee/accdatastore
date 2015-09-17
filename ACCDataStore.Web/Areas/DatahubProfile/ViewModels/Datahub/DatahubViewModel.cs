using ACCDataStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class DatahubViewModel
    {

        public IList<School> ListSchoolNameData { get; set; }
        public School selectedschool { get; set; }
        public string selectedschoolcode { get; set; }
        public DatahubData AberdeencityData { get; set; }
        public DatahubData SchoolData { get; set; }
    }
}