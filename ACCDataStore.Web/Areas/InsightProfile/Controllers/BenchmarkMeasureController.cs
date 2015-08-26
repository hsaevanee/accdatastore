using ACCDataStore.Entity;
using ACCDataStore.Entity.MySQL;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.InsightProfile.ViewModels;
using ACCDataStore.Web.Areas.InsightProfile.ViewModels.BenchmarkMeasure;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.InsightProfile.Controllers
{
    public class BenchmarkMeasureController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(BenchmarkMeasureController));
        
        private readonly IGenericRepository2nd rpGeneric2nd;

        public BenchmarkMeasureController(IGenericRepository2nd rpGeneric2nd)
        {
            //this.rpGeneric = rpGeneric;
            this.rpGeneric2nd = rpGeneric2nd;
        }

        protected IEnumerable<School> GetListSchoolname(IGenericRepository2nd rpGeneric2nd)
        {
            var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL("SELECT DISTINCTROW t1.leaver_centre, t2.SchoolName from la100pupils t1 INNER JOIN la100schools t2 on t1.leaver_centre = t2.SeedCode ");
            List<School> temp = new List<School>();

            if (listResultMySQL.Any()) {
                foreach (var itemRow in listResultMySQL)
                {
                    temp.Add(new School(itemRow[0].ToString(), itemRow[1].ToString()));
                
                }
            
            }

            return temp; 

        }

        // GET: InsightProfile/BenchmarkMeasure
        public ActionResult IndexLeaverDestination()
        {
            var vmBenchmarkMeasure= new BenchmarkMeasureViewModel();
     
            vmBenchmarkMeasure.ListSchoolNameData = GetListSchoolname(rpGeneric2nd);

            return View("IndexLeaver", vmBenchmarkMeasure);
        }

        [HttpPost]
        public JsonResult GetLeaverDestinationData(string schcode, string schname)
        {
            try
            {
                var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Pupils>();

                //select all pupils based on particular selected school (centre = schcode)
                var temp = (from a in listResultMySQL where a.leaver_centre.Equals(Convert.ToString(schcode)) select a).ToList();

                object objData = new object();
 
                    // process chart data
                objData = new
                    {
                        SchTitle = "schname",
                        ChartCategories = "",
                        ChartSeries = ""
                    };



                return Json(objData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public ActionResult IndexAttainment()
        {
            return View("IndexAttainment", null);
        }
    }
}