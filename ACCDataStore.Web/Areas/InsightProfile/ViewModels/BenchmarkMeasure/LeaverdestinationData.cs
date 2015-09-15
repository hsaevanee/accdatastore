using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.InsightProfile.ViewModels.BenchmarkMeasure
{
    public class LeaverdestinationData
    {
        public string centrecode { get; set; }
        public string centrename { get; set; }
        public string academicyear { get; set; }
        public double PercentageMale{ get; set; }
        public double PercentageFeMale{ get; set; }
        public double PercentageAll { get; set; }
    }
}