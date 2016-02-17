using Common.Logging;
using System;
using System.Web;
using System.Web.Mvc;
using ACCDataStore.Repository;
using ACCDataStore.Entity;
using ACCDataStore.Web.ViewModels;
using ACCDataStore.Entity.MySQL;

namespace ACCDataStore.Web.Controllers
{
    public class IndexController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexController));

        //private readonly IGenericRepository rpGeneric;
        //public IndexController(IGenericRepository rpGeneric)
        //{
        //    this.rpGeneric = rpGeneric;
        //}

        public ActionResult Index(string id)
        {
            var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;

            // just git test
            if (id == null)
            {
                eGeneralSettings.HomepgCounter++;
                TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

                var vmIndex = new IndexViewModel();
                vmIndex.ApplicationName = HttpContext.Application["APP_NAME"] as string;
                vmIndex.ApplicationVersion = HttpContext.Application["APP_VERSION"] as string;
                return View("index", vmIndex);

            }
            else
            {
                if (id.ToLower().Equals("theteam"))
                {
                    eGeneralSettings.TeampgCounter++;
                    TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("theTeam");
                }
                else if (id.ToLower().Equals("about"))
                {
                    eGeneralSettings.TeampgCounter++;
                    TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("About");
                }
                else if (id.ToLower().Equals("contact"))
                {
                    eGeneralSettings.TeampgCounter++;
                    TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Contact");
                }
                else if (id.ToLower().Equals("datacentre"))
                {
                    eGeneralSettings.TeampgCounter++;
                    TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("DataCentre");
                }

                else {
                    eGeneralSettings.HomepgCounter++;
                    TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

                    var vmIndex = new IndexViewModel();
                    vmIndex.ApplicationName = HttpContext.Application["APP_NAME"] as string;
                    vmIndex.ApplicationVersion = HttpContext.Application["APP_VERSION"] as string;
                    return View("index", vmIndex);
                }
            }

        }
    }
}