using Common.Logging;
using System;
using System.Web;
using System.Web.Mvc;
using ACCDataStore.Repository;
using ACCDataStore.Entity;
using ACCDataStore.Web.ViewModels;

namespace ACCDataStore.Web.Controllers
{
    public class IndexController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexController));

        private readonly IGenericRepository rpGeneric;
        public IndexController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        public ActionResult Index(string id)
        {
            // just git test
            if (id == null)
            {
                var vmIndex = new IndexViewModel();
                vmIndex.ApplicationName = HttpContext.Application["APP_NAME"] as string;
                vmIndex.ApplicationVersion = HttpContext.Application["APP_VERSION"] as string;
                return View("index", vmIndex);
               
            }
            else
            {
                return View("theTeam");
            }
            
        }
    }
}