using ACCDataStore.Entity;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.Nationality.ViewModels.IndexNationality;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.Nationality.Controllers
{
    public class IndexNationalityController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexNationalityController));

        private readonly IGenericRepository rpGeneric;
        public IndexNationalityController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        public ActionResult Index()
        {
            var vmIndex = new IndexViewModel();
            vmIndex.ListNationality2012 = this.rpGeneric.Find<Nationality2012>(" from Nationality2012 where Gender = :Gender ", new string[] { "Gender" }, new object[] { "M" });

            return View(vmIndex);
        }
    }
}