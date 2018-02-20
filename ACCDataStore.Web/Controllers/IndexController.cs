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

        //private readonly IGenericRepository rpGeneric;
        //public IndexController(IGenericRepository rpGeneric)
        //{
        //    this.rpGeneric = rpGeneric;
        //}

        public ActionResult Index(string id)
        {
            var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;

            // just git test
            if (id == null)
            {
                eGeneralSettings.HomepgCounter++;
                ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

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
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("theTeam");
                }
                else if (id.ToLower().Equals("about"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("About");
                }
                else if (id.ToLower().Equals("contact"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Contact");
                }
                else if (id.ToLower().Equals("datacentre"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("DataCentre");
                }
                else if (id.ToLower().Equals("finance"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Finance");
                }
                else if (id.ToLower().Equals("management"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Management");
                }
                else if (id.ToLower().Equals("education"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Education");
                }
                else if (id.ToLower().Equals("pandp"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("PandP");
                }
                else {
                    eGeneralSettings.HomepgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

                    var vmIndex = new IndexViewModel();
                    vmIndex.ApplicationName = HttpContext.Application["APP_NAME"] as string;
                    vmIndex.ApplicationVersion = HttpContext.Application["APP_VERSION"] as string;
                    return View("index", vmIndex);
                }
            }

        }
    }
}