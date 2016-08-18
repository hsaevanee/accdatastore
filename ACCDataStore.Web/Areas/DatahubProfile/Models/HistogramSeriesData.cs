using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class HistogramSeriesData
    {
        public List<string> months { get; set; }
        public List<double> participating { get; set; }
        public List<double> notParticipating { get; set; }
        public List<double> unknown { get; set; }
    }
}