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

        protected IList<School> GetListSchoolname(IGenericRepository2nd rpGeneric2nd)
        {
            // test
            var listTest = this.rpGeneric2nd.FindAll<LA100Pupils>();

            var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL("SELECT DISTINCTROW t1.centre, t2.SchoolName from la100pupils t1 INNER JOIN la100schools t2 on t1.centre = t2.SeedCode ");
            List<School> temp = new List<School>();

            if (listResultMySQL.Any()) {
                foreach (var itemRow in listResultMySQL)
                {
                    temp.Add(new School(itemRow[0].ToString(), itemRow[1].ToString()));
                
                }
            
            }

            return temp; 

        }

        protected IList<Gender> GetListGender()
        {
            List<Gender> temp = new List<Gender>();
            temp.Add(new Gender(1)); //Male
            temp.Add(new Gender(2)); //Female
            temp.Add(new Gender(0)); //ALL
 
            return temp;

        }

        protected IList<Year> GetListYear()
        {
            List<Year> temp = new List<Year>();

            temp.Add(new Year("2008"));
            temp.Add(new Year("2009"));
            temp.Add(new Year("2010"));
            temp.Add(new Year("2011"));
            temp.Add(new Year("2012"));
            temp.Add(new Year("2013"));
            temp.Add(new Year("2014"));

            return temp;

        }

        // GET: InsightProfile/BenchmarkMeasure
        public ActionResult IndexLeaverDestination()
        {
            var vmBenchmarkMeasure = new BenchmarkMeasureViewModel();
 
            vmBenchmarkMeasure.ListSchoolNameData = GetListSchoolname(rpGeneric2nd);
            vmBenchmarkMeasure.ListGenderData = GetListGender();
            vmBenchmarkMeasure.ListYearData = GetListYear();
            return View("IndexLeaver", vmBenchmarkMeasure);
        }

        // GET: InsightProfile/BenchmarkMeasure
        public ActionResult GetIndexLeaverDestination(BenchmarkMeasureViewModel model)
        {
            var vmBenchmarkMeasure = new BenchmarkMeasureViewModel();

            vmBenchmarkMeasure.ListSchoolNameData = GetListSchoolname(rpGeneric2nd);
            vmBenchmarkMeasure.ListGenderData = GetListGender();
            vmBenchmarkMeasure.ListYearData = GetListYear();
            vmBenchmarkMeasure.ListLeaverDestinationData = GetLeaverDestinationDatabySchool(model);
            vmBenchmarkMeasure.ListLeaverDestinationDataAbdCity = GetLeaverDestinationDatabyAbdCity(model);

            return View("IndexLeaver", vmBenchmarkMeasure);
        }

        protected List<LeaverDestination> GetLeaverDestinationDatabySchool(BenchmarkMeasureViewModel model)
        {
            try
            {
                List<object> listdata = new List<object>();

                List<LeaverDestination> listLeaverDestination = null;

                //var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Pupils>();

                string query = "SELECT la100pupils.leaver_centre,la100pupils.year,la100pupils.gender, la100pupils.leaver_destination_group, count(*) FROM accdatastore.la100pupils where la100pupils.leaver_centre = '" + model.selectedschoolcode + "' group by la100pupils.leaver_centre, year, gender, leaver_destination_group";

                query += " union ";

                query += "SELECT la100pupils.leaver_centre,la100pupils.year,0 , la100pupils.leaver_destination_group, count(*) FROM accdatastore.la100pupils where la100pupils.leaver_centre = '" + model.selectedschoolcode + "' group by la100pupils.leaver_centre, year, leaver_destination_group ";

                var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL(query);

                if (listResultMySQL != null)
                {
                    var dicLeaver = new Dictionary<string, LeaverDestination>();
                    foreach (var itemRow in listResultMySQL)
                    {
                        LeaverDestination vmLeaverDestination = null;
                        var sKey = itemRow[0].ToString() + itemRow[1].ToString() + itemRow[2].ToString();
                        var sLeaverDestinationGroup = itemRow[3] != null ? itemRow[3].ToString().ToLower().Equals("null") ? "0" : itemRow[3].ToString() : "0";
                        if (!dicLeaver.ContainsKey(sKey))
                        {
                            vmLeaverDestination = new LeaverDestination();
                            vmLeaverDestination.centrecode = Convert.ToInt32(itemRow[0]);
                            vmLeaverDestination.year = Convert.ToInt32(itemRow[1]);
                            vmLeaverDestination.academicyear = new Year(itemRow[1].ToString());
                            vmLeaverDestination.gender = new Gender(Convert.ToInt32(itemRow[2]));
                            dicLeaver.Add(sKey, vmLeaverDestination);
                        }
                        else
                        {
                            vmLeaverDestination = dicLeaver[sKey];
                        }
                        switch (sLeaverDestinationGroup)
                        {
                            case "0":
                                vmLeaverDestination.sum0 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "1":
                                vmLeaverDestination.sum1 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "2":
                                vmLeaverDestination.sum2 = Convert.ToInt32(itemRow[4]);
                                break;
                        }
                    }

                    listLeaverDestination = dicLeaver.Values.ToList();

                    var n = listLeaverDestination[0].Percentage;
                    listLeaverDestination = listLeaverDestination.OrderBy(x => x.year).ThenBy(x => x.gender.gendercode).ToList();
                }

                //var temp1 = model.ListGenderData.Select(x => x.isSelected == true).ToList();

                //var temp1 = (from a in model.ListGenderData where a.isSelected == true select a).ToList();

                //var temp22 = listLeaverDestination.Where(a => temp1.Any(b => b.gendercode == a.gender.gendercode)).ToList(); 
                return listLeaverDestination;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        protected List<LeaverDestination> GetLeaverDestinationDatabyAbdCity(BenchmarkMeasureViewModel model)
        {
            try
            {
                List<object> listdata = new List<object>();

                List<LeaverDestination> listLeaverDestination = null;

                //var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Pupils>();

                string query = "SELECT la100pupils.year,la100pupils.gender, la100pupils.leaver_destination_group, count(*) FROM accdatastore.la100pupils where la100pupils.leaver_centre !='NULL' group by year, gender, leaver_destination_group";

                query += " union ";

                query += "SELECT la100pupils.year,0 , la100pupils.leaver_destination_group, count(*) FROM accdatastore.la100pupils where la100pupils.leaver_centre !='NULL' group by  year, leaver_destination_group ";

                var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL(query);

                if (listResultMySQL != null)
                {
                    var dicLeaver = new Dictionary<string, LeaverDestination>();
                    foreach (var itemRow in listResultMySQL)
                    {
                        LeaverDestination vmLeaverDestination = null;
                        var sKey = itemRow[0].ToString() + itemRow[1].ToString();
                        var sLeaverDestinationGroup = itemRow[2] != null ? itemRow[2].ToString().ToLower().Equals("null") ? "0" : itemRow[2].ToString() : "0";
                        if (!dicLeaver.ContainsKey(sKey))
                        {
                            vmLeaverDestination = new LeaverDestination();
                            //vmLeaverDestination.centrecode = Convert.ToInt32(itemRow[0]);
                            vmLeaverDestination.year = Convert.ToInt32(itemRow[0]);
                            vmLeaverDestination.academicyear = new Year(itemRow[0].ToString());
                            vmLeaverDestination.gender = new Gender(Convert.ToInt32(itemRow[1]));
                            dicLeaver.Add(sKey, vmLeaverDestination);
                        }
                        else
                        {
                            vmLeaverDestination = dicLeaver[sKey];
                        }
                        switch (sLeaverDestinationGroup)
                        {
                            case "0":
                                vmLeaverDestination.sum0 = Convert.ToInt32(itemRow[3]);
                                break;
                            case "1":
                                vmLeaverDestination.sum1 = Convert.ToInt32(itemRow[3]);
                                break;
                            case "2":
                                vmLeaverDestination.sum2 = Convert.ToInt32(itemRow[3]);
                                break;
                        }
                    }

                    listLeaverDestination = dicLeaver.Values.ToList();

                    var n = listLeaverDestination[0].Percentage;
                    listLeaverDestination = listLeaverDestination.OrderBy(x => x.year).ThenBy(x => x.gender.gendercode).ToList();
                }

                //var temp1 = model.ListGenderData.Select(x => x.isSelected == true).ToList();

                //var temp1 = (from a in model.ListGenderData where a.isSelected == true select a).ToList();

                //var temp22 = listLeaverDestination.Where(a => temp1.Any(b => b.gendercode == a.gender.gendercode)).ToList(); 
                return listLeaverDestination;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetChartLeaverDestination(string schcode, string selectedschname, string year)
        {
            try
            {
                object oChartData = new object();

                List<LeaverDestinationBreakdown> listLeaverDestinationBreakdown = null;

                //var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Pupils>();

                string query = "SELECT leaver_centre, year, gender, destination, count(*) FROM la100pupils where leaver_centre = '" + schcode + "' and year='" + year + "' group by leaver_centre, year, gender, destination ";

                query += " union ";

                query += "SELECT leaver_centre, year, 0, destination, count(*) FROM la100pupils where leaver_centre = '" + schcode + "' and year='" + year + "' group by leaver_centre, year, destination ";

                var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL(query);

                if (listResultMySQL != null)
                {
                    var dicLeaver = new Dictionary<string, LeaverDestinationBreakdown>();
                    foreach (var itemRow in listResultMySQL)
                    {
                        LeaverDestinationBreakdown vmLeaverDestination = null;
                        var sKey = itemRow[0].ToString() + itemRow[1].ToString() + itemRow[2].ToString();
                        var sLeaverDestinationGroup = itemRow[3] != null ? itemRow[3].ToString().ToLower().Equals("null") ? "0" : itemRow[3].ToString() : "0";
                        if (!dicLeaver.ContainsKey(sKey))
                        {
                            vmLeaverDestination = new LeaverDestinationBreakdown();
                            vmLeaverDestination.centrecode = Convert.ToInt32(itemRow[0]);
                            vmLeaverDestination.year = Convert.ToInt32(itemRow[1]);
                            //vmLeaverDestination.academicyear = new Year(itemRow[0].ToString());
                            vmLeaverDestination.gender = new Gender(Convert.ToInt32(itemRow[2]));
                            dicLeaver.Add(sKey, vmLeaverDestination);
                        }
                        else
                        {
                            vmLeaverDestination = dicLeaver[sKey];
                        }
                        switch (sLeaverDestinationGroup)
                        {
                            case "1":
                                vmLeaverDestination.sum1 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "2":
                                vmLeaverDestination.sum2 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "3":
                                vmLeaverDestination.sum3 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "4":
                                vmLeaverDestination.sum4 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "5":
                                vmLeaverDestination.sum5 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "6":
                                vmLeaverDestination.sum6 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "7":
                                vmLeaverDestination.sum7 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "8":
                                vmLeaverDestination.sum8 = Convert.ToInt32(itemRow[4]);
                                break;
                            case "9":
                                vmLeaverDestination.sum9 = Convert.ToInt32(itemRow[4]);
                                break;
                        }
                    }

                    listLeaverDestinationBreakdown = dicLeaver.Values.ToList();

                    ////var n = listLeaverDestinationBreakdown[0].Percentage;
                    //listLeaverDestinationBreakdown = listLeaverDestinationBreakdown.OrderBy(x => x.year).ThenBy(x => x.gender.gendercode).ToList();
                    var listChartData = new List<object>();
                    listChartData.Add(new { name = selectedschname, data = data.Select(x => x.PercentageFemaleAllSchool).ToArray() });
                    listChartData.Add(new { name = "Aberdeen City", data = data.Select(x => x.PercentageMaleAllSchool).ToArray() });
                    //listChartData.Add(new { name = "Total", data = data.Select(x => x.PercentageAllSchool).ToArray() });

                    oChartData = new
                    {
                        ChartTitle = selectedschname,
                        ChartCategories = listEthnicData.Select(x => x.EthinicName).ToArray(),
                        //ChartSeries = ProcessChartDataEthnic(listEthnicFilter)
                    };

                }

                return Json(oChartData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }


        protected List<LeaverDestination> GetLeaverDestinationBreakdownbySchoolcode(string gender)
        {
            try
            {
                List<object> listdata = new List<object>();

                List<LeaverDestination> listLeaverDestination = null;

                //var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Pupils>();

                string query = "SELECT la100pupils.year,la100pupils.gender, la100pupils.leaver_destination_group, count(*) FROM accdatastore.la100pupils where la100pupils.leaver_centre !='NULL' group by year, gender, leaver_destination_group";

                query += " union ";

                query += "SELECT la100pupils.year,0 , la100pupils.leaver_destination_group, count(*) FROM accdatastore.la100pupils where la100pupils.leaver_centre !='NULL' group by  year, leaver_destination_group ";

                var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL(query);

                if (listResultMySQL != null)
                {
                    var dicLeaver = new Dictionary<string, LeaverDestination>();
                    foreach (var itemRow in listResultMySQL)
                    {
                        LeaverDestination vmLeaverDestination = null;
                        var sKey = itemRow[0].ToString() + itemRow[1].ToString();
                        var sLeaverDestinationGroup = itemRow[2] != null ? itemRow[2].ToString().ToLower().Equals("null") ? "0" : itemRow[2].ToString() : "0";
                        if (!dicLeaver.ContainsKey(sKey))
                        {
                            vmLeaverDestination = new LeaverDestination();
                            //vmLeaverDestination.centrecode = Convert.ToInt32(itemRow[0]);
                            vmLeaverDestination.year = Convert.ToInt32(itemRow[0]);
                            vmLeaverDestination.academicyear = new Year(itemRow[0].ToString());
                            vmLeaverDestination.gender = new Gender(Convert.ToInt32(itemRow[1]));
                            dicLeaver.Add(sKey, vmLeaverDestination);
                        }
                        else
                        {
                            vmLeaverDestination = dicLeaver[sKey];
                        }
                        switch (sLeaverDestinationGroup)
                        {
                            case "0":
                                vmLeaverDestination.sum0 = Convert.ToInt32(itemRow[3]);
                                break;
                            case "1":
                                vmLeaverDestination.sum1 = Convert.ToInt32(itemRow[3]);
                                break;
                            case "2":
                                vmLeaverDestination.sum2 = Convert.ToInt32(itemRow[3]);
                                break;
                        }
                    }

                    listLeaverDestination = dicLeaver.Values.ToList();

                    var n = listLeaverDestination[0].Percentage;
                    listLeaverDestination = listLeaverDestination.OrderBy(x => x.year).ThenBy(x => x.gender.gendercode).ToList();
                }

                //var temp1 = model.ListGenderData.Select(x => x.isSelected == true).ToList();

                //var temp1 = (from a in model.ListGenderData where a.isSelected == true select a).ToList();

                //var temp22 = listLeaverDestination.Where(a => temp1.Any(b => b.gendercode == a.gender.gendercode)).ToList(); 
                return listLeaverDestination;
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