using ACCDataStore.Entity.PupilsPopulation;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
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

        //[AdminAuthentication]
        //[Transactional]
        public ActionResult IndexHome()
        {
            //var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
           
            return View("Home");
        }
        // GET: PupilsPopulation/IndexPupilsPopulation
        public ActionResult Index()
        {
            return View("index");
        }

        public ActionResult HeatMapData()
        {
            Session["ListPupils05DataforAllzone"] = GetPupils05dataAllZonecode(this.rpGeneric);
            //IList<Pupils05Data> ListPupils05Dataallzonecode = GetPupils05dataAllZonecode(this.rpGeneric);
            Session["Pupils05DataforCitywide"]   = GetPupilsdataCitywide(this.rpGeneric);
            
            // calculate 


            return View("HeatMapIndex");
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

        protected Pupils05Data GetPupilsdataCitywide(IGenericRepository rpGeneric)
        {
            var pupils05Data = new Pupils05Data();
            var listpupilsdata = this.rpGeneric.FindAll<PupilsInfoObj>();
            var listneighbourhooddata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.NeighbourhoodObj>();
            var listdata = new List<PupilsInfoObj>();

            if (listpupilsdata != null && listneighbourhooddata!=null)
            {
                listdata = (from a in listpupilsdata join b in listneighbourhooddata on a.postcode equals b.CSS_Postcode select a).ToList();
            }

            pupils05Data.datacode = "100"; //Aberdeen city
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

                var Listpupils05data = Session["ListPupils05DataforAllzone"] as List<Pupils05Data>;
                var Pupils05DataforCitywide = Session["Pupils05DataforCitywide"] as Pupils05Data;

                Listpupils05data = Listpupils05data.OrderBy(x => x.datacode).ToList();

                List<double> tempdata = new List<double>();

                if (datasetname.Equals("Age between 0-5"))
                {
                    //tempx = Listpupils05data.Select(x => new { datacode = x.datacode, percentage = x.Percentage(x.Allpupilsbetween0to5(), Pupils05DataforCitywide.Allpupilsbetween0to5()) }).ToList();
                    tempdata = Listpupils05data.Select(x => x.Percentage(x.Allpupilsbetween0to5(),Pupils05DataforCitywide.Allpupilsbetween0to5())).ToList();
                }
                else if (datasetname.Equals("Age between 0-4"))
                {
                    tempdata = Listpupils05data.Select(x => x.Percentage(x.Allpupilsbetween0to4(), Pupils05DataforCitywide.Allpupilsbetween0to4())).ToList();
                }
                else if (datasetname.Equals("Age between 0-3"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.Allpupilsbetween0to3(), Pupils05DataforCitywide.Allpupilsbetween0to3())).ToList();
                }
                else if (datasetname.Equals("Age between 0-2"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.Allpupilsbetween0to2(), Pupils05DataforCitywide.Allpupilsbetween0to2())).ToList();
                }
                else if (datasetname.Equals("Age between 0-1"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.Allpupilsbetween0to1(), Pupils05DataforCitywide.Allpupilsbetween0to1())).ToList();
                }
                else if (datasetname.Equals("Age 0"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.allpupils00,Pupils05DataforCitywide.allpupils00)).ToList();
                }
                else if (datasetname.Equals("Age 1"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.allpupils01,Pupils05DataforCitywide.allpupils01)).ToList();
                }
                else if (datasetname.Equals("Age 2"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.allpupils02,Pupils05DataforCitywide.allpupils02)).ToList();
                }
                else if (datasetname.Equals("Age 3"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.allpupils03,Pupils05DataforCitywide.allpupils03)).ToList();
                }
                else if (datasetname.Equals("Age 4"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.allpupils04,Pupils05DataforCitywide.allpupils04)).ToList();
                }
                else if (datasetname.Equals("Age 5"))
                {

                    tempdata = Listpupils05data.Select(x => x.Percentage(x.allpupils05,Pupils05DataforCitywide.allpupils05)).ToList();
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
                var Abddata = Session["Pupils05DataforCitywide"] as Pupils05Data;
                
                if (keyname.ToLower().Equals("zonecode"))
                {
                    //Schooldata = GetDatahubdatabyZonecode(rpGeneric, keyvalue);
                    //var tempSchooldata = CreatPupilsdata(GetPupils05databyZonecode(rpGeneric, keyvalue), keyvalue);
                    var Listpupils05data = Session["ListPupils05DataforAllzone"] as List<Pupils05Data>;                   
                    Schooldata = Listpupils05data.First(x => x.datacode==keyvalue);
                    schname = keyvalue;

                }

                object data = new object();


                data = new
                {
                    dataTitle = "Pupils 0-5s Population for Zonecode: " + schname,
                    dataname = schname,
                    dataCategories = new string[] { "Age 0", "Age 1", "Age 2", "Age 3","Age 4","Age 5", "Totals" },
                    Schdata = new double[] { Schooldata.allpupils00, Schooldata.allpupils01, Schooldata.allpupils02, Schooldata.allpupils03, Schooldata.allpupils04, Schooldata.allpupils05, Schooldata.Allpupilsbetween0to5() },
                    Abdcitydata = new double[] { Abddata.allpupils00, Abddata.allpupils01, Abddata.allpupils02, Abddata.allpupils03, Abddata.allpupils04, Abddata.allpupils05, Abddata.Allpupilsbetween0to5() }

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