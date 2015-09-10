using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.InsightProfile.ViewModels.BenchmarkMeasure
{
    public class BenchmarkMeasureViewModel : CriteriaObjViewModel
    {
        public IList<object> ListLeaverDestinationData { get; set; }
        public IList<LeaverDestination> ListLeaverDestinationDataAbdCity { get; set; }
    }
}