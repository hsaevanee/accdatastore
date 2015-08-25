using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.InsightProfile.Controllers
{
    public class InsightProfileController : BaseController
    {

        private static ILog log = LogManager.GetLogger(typeof(InsightProfileController));

        private readonly IGenericRepository rpGeneric;

        public InsightProfileController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
        // GET: InsightProfile/InsightProfile
        public ActionResult Index()
        {
            return View();
        }
    }
}