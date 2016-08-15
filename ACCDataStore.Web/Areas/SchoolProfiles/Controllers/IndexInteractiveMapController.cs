using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class IndexInteractiveMapController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        private IndexInteractiveMapController vmIndexAberdeenCityProfilesModel;


        // GET: SchoolProfiles/IndexInteractiveMap
        public ActionResult Index()
        {
            return View("MapIndex");
        }
    }
}