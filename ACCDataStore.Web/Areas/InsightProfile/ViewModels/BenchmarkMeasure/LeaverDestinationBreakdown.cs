using ACCDataStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.InsightProfile.ViewModels.BenchmarkMeasure
{
    public class LeaverDestinationBreakdown : CriteriaObjViewModel
    {
        //private string _destinationcode;
        //public string destinationcode {
        //    get {
        //        return this._destinationcode;
        //    }
        //    set
        //    {
        //        this._destinationcode = value;
        //        switch (value)
        //        {
        //            case "1":
        //                this.destinationname = "2007/2008";
        //                break;
        //            case "2":
        //                this.destinationname = "2008/2009";
        //                break;
        //            case "2010":
        //                this.destinationname = "2009/2010";
        //                break;
        //            case "2011":
        //                this.destinationname = "2010/2011";
        //                break;
        //            case "2012":
        //                this.destinationname = "2011/2012";
        //                break;
        //            case "2013":
        //                this.destinationname = "2012/2013";
        //                break;
        //            case "2014":
        //                this.destinationname = "2013/2014";
        //                break;
        //        }
        //    }
        //}

        public string destinationcode { get; set; }
        public string destinationname { get; set; }
        public double PercentageFemale { get; set; } // Activity Agreement
        public double PercentageMale { get; set; } // Employed
        public double PercentageAll { get; set; } // Further Education
    }
}