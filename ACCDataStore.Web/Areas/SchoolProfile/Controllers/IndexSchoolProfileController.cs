using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACCDataStore.Entity;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class IndexSchoolProfileController : Controller
    {
        // GET: SchoolProfile/IndexSchoolProfile
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfileController));

        private readonly IGenericRepository rpGeneric;

        public IndexSchoolProfileController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        public ActionResult Index()
        {
            //var vmIndex = new IndexViewModel();
            var result = this.rpGeneric.FindAll<StudentSIMD>();

            return View();
        }
    }
}