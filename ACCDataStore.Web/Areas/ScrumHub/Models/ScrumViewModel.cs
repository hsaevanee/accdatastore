using ACCDataStore.Entity.ScrumHub;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.ScrumHub.Models
{
    public class ScrumViewModel
    {
        public List<ScrumCourseModel> ListCourses { get; set; }
        public List<ScrumTable> scrumlist { get; set; }
        public List<UserCourses> joined { get; set; }
        public ScrumTable Meta1 { get; set; }
        public ScrumCourseModel Meta2 { get; set; }
        public List<string> paramlist { get; set; }
    }
}