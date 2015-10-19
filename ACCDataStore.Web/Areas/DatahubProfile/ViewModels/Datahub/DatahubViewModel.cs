using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class DatahubViewModel
    {

        public IList<School> ListSchoolNameData { get; set; }
        public IList<School> ListNeighbourhoodsName { get; set; }
        public School selectedschool { get; set; }
        public string selectedschoolcode { get; set; }
        public string selectedneighbourhoods { get; set; }
        public DatahubData AberdeencityData { get; set; }
        public DatahubData SchoolData { get; set; }
        public IList<DatahubDataObj> Listpupils { get; set; }
        public string levercategory { get; set; }
        // dataview for Heatmap
        public string selecteddataset { get; set; }
        public IList<string> ListDatasets { get; set; }

    }
}