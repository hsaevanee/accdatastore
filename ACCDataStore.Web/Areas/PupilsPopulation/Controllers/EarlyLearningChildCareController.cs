using ACCDataStore.Entity.DatahubProfile;
using ACCDataStore.Entity.PupilsPopulation;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.PupilsPopulation.Controllers
{
    public class EarlyLearningChildCareController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(EarlyLearningChildCareController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public EarlyLearningChildCareController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        // GET: PupilsPopulation/EarlyLearningChildCare
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGeoJSON(string id)
        {
            // Id shouldn't be null
            if (id != null)
            {
                // A string to hold our GeoJSON content as recieved by the database
                string result = null;

                // Check the id type and query the corresponding table
                try
                {
                    if (id.StartsWith("S01")) { result = this.rpGeneric2nd.GetById<DataZoneObj>(id).GeoJSON; }
                    else if (id.StartsWith("S02")) { result = this.rpGeneric2nd.GetById<IntermediateZoneObj>(id).GeoJSON; }
                    else if (id.StartsWith("S12")) { result = this.rpGeneric2nd.GetById<CouncilObj>(id).GeoJSON; }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return new HttpStatusCodeResult(404, "Item Not Found");
                }

                return new ContentResult { Content = result, ContentType = "application/json" };

            }

            return new HttpStatusCodeResult(404, "Missing parameter!");
        }


        [HttpGet]
        [Route("PupilsPopulation/EarlylearningChildCare/GetLocationData")]
        public JsonResult GetLocationData(List<string> listSeedCode, string sYear, string tablename)
        {
            try
            {
                List<ERCentre> listResult = new List<ERCentre>();

                var listtemp = rpGeneric2nd.FindByNativeSQL("SELECT Provider_Type,Registration_Name,Registration_Number,Postcode FROM accdatastore.elcc_provision");
                foreach (var itemrow in listtemp)
                {
                    if (itemrow != null)
                    {
                        ERCentre temp = new ERCentre();
                        temp.providerType = itemrow[0] == null ? "" : itemrow[0].ToString();
                        temp.registrationName = itemrow[1] == null ? "" : itemrow[1].ToString();
                        temp.registrationNo = itemrow[2] == null ? "" : itemrow[2].ToString();
                        temp.postcode = itemrow[3] == null ? "" : itemrow[3].ToString();
                        listResult.Add(temp);
                    }
                }

                return Json(listResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

    }
}