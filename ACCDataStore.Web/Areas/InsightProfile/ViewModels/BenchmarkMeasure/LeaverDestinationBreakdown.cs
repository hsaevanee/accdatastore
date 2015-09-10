using ACCDataStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.InsightProfile.ViewModels.BenchmarkMeasure
{
    public class LeaverDestinationBreakdown : CriteriaObjViewModel
    {
        public string destinationcode { get; set; }
        public string destinationname { get; set; }
        public double PercentageFemaleinSchool { get; set; } // Activity Agreement
        public double PercentageMaleinSchool { get; set; } // Employed
        public double PercentageAllinSchool { get; set; } // Further Education
        public double PercentageFemaleinAbdcity { get; set; } // Activity Agreement
        public double PercentageMaleinAbdcity { get; set; } // Employed
        public double PercentageAllinAbdcity { get; set; } // Further Education

        public LeaverDestinationBreakdown(string lvcode, string lvname) {
            this.destinationcode = lvcode;
            this.destinationname = lvname;
            this.PercentageFemaleinSchool = 0;
            this.PercentageMaleinSchool = 0;
            this.PercentageAllinSchool = 0;
            this.PercentageAllinSchool = 0;
            this.PercentageFemaleinAbdcity = 0;
            this.PercentageMaleinAbdcity = 0;
            this.PercentageAllinAbdcity = 0;   
        }
    }
}