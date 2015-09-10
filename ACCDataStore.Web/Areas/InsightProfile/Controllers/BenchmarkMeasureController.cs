using ACCDataStore.Entity;
using ACCDataStore.Entity.InsightProfile;
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

        protected List<object> GetLeaverDestinationDatabySchool(BenchmarkMeasureViewModel model)
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

                //    oChartData = new
                //    {
                //        ChartTitle = selectedschname,
                //        ChartCategories = listEthnicData.Select(x => x.EthinicName).ToArray(),
                //        //ChartSeries = ProcessChartDataEthnic(listEthnicFilter)
                //    };

                // Select distinct academic year from list
                //var distinctyear = (from m in listLeaverDestination.Select(x => x.academicyear.academicyear).ToList() select m).Distinct().ToList();

                //var lx = listLeaverDestination.Intersect(distinctyear);

                //foreach (var year in distinctyear)
                //{

                //    if (year) { 
                    
                    
                    
                //    }
                
                //}


                return listdata;
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
                Object oChartData = new Object();

                List<DestinationObj> listdata = new List<DestinationObj>();

                List<LeaverDestinationBreakdown> listLeaverDestinationBreakdown = GetListDestinationBreakdown(this.rpGeneric2nd).ToList();

                //var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Pupils>();

                string query = "SELECT 'school', year, gender, destination, count(*) FROM la100pupils where leaver_centre = '" + schcode + "'and year='" + year + "' group by leaver_centre, year, gender, destination";

                query += " union ";

                query += "SELECT 'school', year, 0, destination, count(*) FROM la100pupils where leaver_centre = '" + schcode + "'and year='" + year + "' group by leaver_centre, year, destination ";

                query += " union ";
                query += "SELECT 'Abdcity', year, gender, destination, count(*) FROM la100pupils where year='" + year + "' group by  year, gender, destination";
                query += " union ";
                query += "SELECT 'Abdcity', year, 0, destination, count(*) FROM la100pupils where year='" + year + "' group by year, destination ";

                var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL(query);

                if (listResultMySQL != null)
                {
                    foreach (var itemrow in listResultMySQL)
                    {
                        listdata.Add(new DestinationObj(itemrow[0].ToString(), itemrow[1].ToString(),itemrow[2].ToString(),itemrow[3].ToString(),Convert.ToDouble(itemrow[4])));                   
                    }
                                   
                }

                foreach (var item in listLeaverDestinationBreakdown)
                {
                    double nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("school") && x.gender.Equals("2")).Select(x => x.number).Sum();
                    double ny = listdata.Where(x => x.gender.Equals("2") && x.objname.Equals("school")).Select(x => x.number).Sum();
                    item.PercentageFemaleinSchool = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("school") && x.gender.Equals("1")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("1") && x.objname.Equals("school")).Select(x => x.number).Sum();
                    item.PercentageMaleinSchool = (nx*100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("school") && x.gender.Equals("0")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("0") && x.objname.Equals("school")).Select(x => x.number).Sum();
                    item.PercentageAllinSchool = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("Abdcity") && x.gender.Equals("2")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("2") && x.objname.Equals("Abdcity")).Select(x => x.number).Sum();
                    item.PercentageFemaleinAbdcity = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("Abdcity") && x.gender.Equals("1")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("1") && x.objname.Equals("Abdcity")).Select(x => x.number).Sum();
                    item.PercentageMaleinAbdcity = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("Abdcity") && x.gender.Equals("0")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("0") && x.objname.Equals("Abdcity")).Select(x => x.number).Sum();
                    item.PercentageAllinAbdcity = (nx * 100) / ny;
                }



                var listChartData = new List<object>();
                listChartData.Add(new { name = selectedschname+"_Female", data = listLeaverDestinationBreakdown.Select(x=>x.PercentageFemaleinSchool).ToArray()});
                listChartData.Add(new { name = selectedschname + "Male", data = listLeaverDestinationBreakdown.Select(x => x.PercentageMaleinSchool).ToArray() });
                listChartData.Add(new { name = selectedschname, data = listLeaverDestinationBreakdown.Select(x => x.PercentageAllinSchool).ToArray() });

                listChartData.Add(new { name = "Aberdeen City Female", data = listLeaverDestinationBreakdown.Select(x => x.PercentageFemaleinAbdcity).ToArray() });
                listChartData.Add(new { name = "Aberdeen City Male", data = listLeaverDestinationBreakdown.Select(x => x.PercentageMaleinAbdcity).ToArray() });
                listChartData.Add(new { name = "Aberdeen City ", data = listLeaverDestinationBreakdown.Select(x => x.PercentageAllinAbdcity).ToArray() });
 


                    oChartData = new
                    {
                        ChartTitle = selectedschname,
                        ChartCategories = listLeaverDestinationBreakdown.Select(x=>x.destinationname).ToList(),
                        ChartSeries = listChartData
                    };

                

                return Json(oChartData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        protected IList<LeaverDestinationBreakdown> GetListDestinationBreakdown(IGenericRepository2nd rpGeneric2nd)
        {
            // test
            var listTest = this.rpGeneric2nd.FindAll<LA100Pupils>();

            var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL("select destinationcode,destinationtype from destinationcategory");
            List<LeaverDestinationBreakdown> temp = new List<LeaverDestinationBreakdown>();

            if (listResultMySQL.Any())
            {
                foreach (var itemRow in listResultMySQL)
                {
                    temp.Add(new LeaverDestinationBreakdown(itemRow[0].ToString(), itemRow[1].ToString()));

                }

            }

            return temp;

        }

 
        public ActionResult IndexAttainment()
        {
            return View("IndexAttainment", null);
        }
    }
}