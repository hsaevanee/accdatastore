using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.InsightProfile.Controller
{
    public class IndexInsightController : BaseController
    {   
        private static ILog log = LogManager.GetLogger(typeof(IndexInsightController));

        private readonly IGenericRepository rpGeneric;

        public IndexInsightController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        //private void SetDefaultCounter()
        //{
        //    var eGeneralCounter = new GeneralCounter();
        //    eGeneralCounter.Module1Counter = 10;
        //    eGeneralCounter.Module2Counter = 20;
        //    eGeneralCounter.SiteCounter = 100;
        //    TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralCounter, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
        //}

        // GET: InsightProfile/IndexInsight
        public ActionResult Index()
        {
            return View();
        }
    }
}