using ACCDataStore.Entity.PupilsPopulation;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.PupilsPopulation.ViewModels.Pupils05;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.PupilsPopulation.Controllers
{
    public class IndexPupilsPopulationController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexPupilsPopulationController));

        private readonly IGenericRepository rpGeneric;

        public IndexPupilsPopulationController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
        // GET: PupilsPopulation/IndexPupilsPopulation
        public ActionResult Index()
        {
            return View("index");
        }

        public ActionResult HeatMapData()
        {
            Session["ListPupils05Dataforheatmap"] = GetPupils05dataAllZonecode(this.rpGeneric);

            return View("HeatMapIndex");
        }

        protected List<PupilsInfoObj> GetPupils05databyZonecode(IGenericRepository rpGeneric, string zonecode)
        {
            var listpupilsdata = this.rpGeneric.FindAll<ACCDataStore.Entity.PupilsPopulation.PupilsInfoObj>();
            var listneighbourhooddata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.NeighbourhoodObj>();
            var listdata = new List<PupilsInfoObj>();
            if (zonecode != null)
            {
                listdata = (from a in listpupilsdata join b in listneighbourhooddata on a.postcode equals b.CSS_Postcode where b.DataZone.Contains(zonecode) select a).ToList();
            }
            return listdata;
        }

        protected IList<Pupils05Data> GetPupils05dataAllZonecode(IGenericRepository rpGeneric)
        {
            List<Pupils05Data> listpupils05data = new List<Pupils05Data>();

            var listzonecode = rpGeneric.FindSingleColumnByNativeSQL("Select distinct Datazone from Neighbourhood_Postcodes1 t1 INNER JOIN Age05sinfo t2 on  t1.CSS_Postcode = t2.Postcode ");

            var listpupilsdata = this.rpGeneric.FindAll<PupilsInfoObj>();
            var listneighbourhooddata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.NeighbourhoodObj>();
            var listdata = new List<PupilsInfoObj>();

            if (listzonecode != null)
            {
                foreach (var item in listzonecode)
                {
                    if (item != null)
                    {
                        listdata = (from a in listpupilsdata join b in listneighbourhooddata on a.postcode equals b.CSS_Postcode where b.DataZone.Contains(item.ToString()) select a).ToList();
                        listpupils05data.Add(CreatPupilsdata(listdata, item.ToString()));
                    }

                }
            }
            return listpupils05data;
        }

        protected Pupils05Data CreatPupilsdata(List<PupilsInfoObj> listdata, string datahubcode)
        {
            var pupils05Data = new Pupils05Data();
            pupils05Data.datacode = datahubcode;
            pupils05Data.allpupils05 = listdata.Count(x => x.dbyear == 10);
            pupils05Data.allpupils04 = listdata.Count(x => x.dbyear == 11);
            pupils05Data.allpupils03 = listdata.Count(x => x.dbyear == 12);
            pupils05Data.allpupils02 = listdata.Count(x => x.dbyear == 13);
            pupils05Data.allpupils01 = listdata.Count(x => x.dbyear == 14);
            pupils05Data.allpupils00 = listdata.Count(x => x.dbyear == 15);
            return pupils05Data;
        }


        [HttpPost]
        public JsonResult GetdataforHeatmap(string datasetname)
        {
            try
            {

                object data = new object();

                var Listpupils05data = Session["ListPupils05Dataforheatmap"] as List<Pupils05Data>;

                Listpupils05data = Listpupils05data.OrderBy(x => x.datacode).ToList();

                List<double> tempdata = new List<double>();

                if (datasetname.Equals("Age between 0-5"))
                {

                    tempdata = Listpupils05data.Select(x => x.Allpupilsbetween0to5()).ToList();
                }
                else if (datasetname.Equals("Age between 0-4"))
                {
                    tempdata = Listpupils05data.Select(x => x.Allpupilsbetween0to4()).ToList();
                }
                else if (datasetname.Equals("Age between 0-3"))
                {

                    tempdata = Listpupils05data.Select(x => x.Allpupilsbetween0to3()).ToList();
                }
                else if (datasetname.Equals("Age between 0-2"))
                {

                    tempdata = Listpupils05data.Select(x => x.Allpupilsbetween0to2()).ToList();
                }
                else if (datasetname.Equals("Age between 0-1"))
                {

                    tempdata = Listpupils05data.Select(x => x.Allpupilsbetween0to1()).ToList();
                }
                else if (datasetname.Equals("Age 0"))
                {

                    tempdata = Listpupils05data.Select(x => Convert.ToDouble(x.allpupils00)).ToList();
                }
                else if (datasetname.Equals("Age 1"))
                {

                    tempdata = Listpupils05data.Select(x => Convert.ToDouble(x.allpupils01)).ToList();
                }
                else if (datasetname.Equals("Age 2"))
                {

                    tempdata = Listpupils05data.Select(x => Convert.ToDouble(x.allpupils02)).ToList();
                }
                else if (datasetname.Equals("Age 3"))
                {

                    tempdata = Listpupils05data.Select(x => Convert.ToDouble(x.allpupils03)).ToList();
                }
                else if (datasetname.Equals("Age 4"))
                {

                    tempdata = Listpupils05data.Select(x => Convert.ToDouble(x.allpupils04)).ToList();
                }
                else if (datasetname.Equals("Age 5"))
                {

                    tempdata = Listpupils05data.Select(x => Convert.ToDouble(x.allpupils05)).ToList();
                }

                data = new
                {
                    datacode = Listpupils05data.Select(x => x.datacode).ToList(),
                    data = tempdata,
                    minimum = tempdata.Min(),
                    maximum = tempdata.Max(),
                };

                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return ThrowJSONError(ex);
            }
        }

        protected JsonResult ThrowJSONError(Exception ex)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
            return Json(new { Message = sErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByName(string keyvalue, string keyname)
        {
            try
            {
                string schname = "";

                var Schooldata = new Pupils05Data();
                
                if (keyname.ToLower().Equals("zonecode"))
                {
                    //Schooldata = GetDatahubdatabyZonecode(rpGeneric, keyvalue);
                    Schooldata = CreatPupilsdata(GetPupils05databyZonecode(rpGeneric, keyvalue), keyvalue);
                    schname = keyvalue;

                }

                object data = new object();


                data = new
                {
                    dataTitle = "Pupils 0-5s Population for Zonecode: " + schname,
                    dataname = schname,
                    dataCategories = new string[] { "Age 0", "Age 1", "Age 2", "Age 3","Age 4","Age 5", "Totals" },
                    Schdata = new double[] { Schooldata.allpupils00, Schooldata.allpupils01, Schooldata.allpupils02, Schooldata.allpupils03, Schooldata.allpupils04, Schooldata.allpupils05, Schooldata.Allpupilsbetween0to5() },
                    //Abdcitydata = new double[] { Abddata.Participating(), Abddata.NotParticipating(), Abddata.Percentage(Abddata.pupilsinUnknown) }

                };

                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return ThrowJSONError(ex);
            }
        }
    }
}