﻿using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity.SchoolProfiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles
{
    public class IndexPrimarySchoolProfilesViewModel : BaseSchoolProfilesViewModel
    {
        public DataTable dataTablePIPS { get; set; }
        public List<DataSeries> listDataSeriesPIPS { get; set; }
        public List<PIPSObj> listPIPSPupils { get; set; }
    }
}