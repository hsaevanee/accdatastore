using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.EthnicBackground;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class EthnicBackgroundController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfileController));

        private readonly IGenericRepository rpGeneric;

        public EthnicBackgroundController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        // GET: SchoolProfile/EthnicBackground
        public ActionResult Index(string sSchoolName)
        {
            //var vmIndex = new IndexViewModel();
            //var result = this.rpGeneric.FindAll<StudentSIMD>();
            var vmEthnicbackground = new EthnicBgViewModel();

            var schoolname = new List<string>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmEthnicbackground.ListSchoolNameData = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW EthnicBackground FROM test_3 group by EthnicBackground");

            fooList = listResult.OfType<string>().ToList();

            vmEthnicbackground.ListEthnicDefinition = fooList;
            vmEthnicbackground.DicEthnicBG = GetDicEhtnicBG();

            vmEthnicbackground.IsShowCriteria = false;
            vmEthnicbackground.IsShowEthnicData = true;
            sSchoolName = Request["selectSchoolname"];
            vmEthnicbackground.selectedschoolname = sSchoolName;
            vmEthnicbackground.ListEthnicData = GetEthnicityDatabySchoolname(sSchoolName);
            Session["SessionListEthnicData"] = vmEthnicbackground.ListEthnicData;


            //if (Request["selectSchoolname"] == null)
            //{
            //    vmEthnicbackground.IsShowCriteria = true;
            //}
            //else
            //{
            //    if (sSchoolName != null)
            //    {
            //        vmEthnicbackground.selectedschoolname = sSchoolName;
            //        vmEthnicbackground.ListEthnicData = GetEthnicityDatabySchoolname(sSchoolName);
            //        Session["SessionListEthnicData"] = vmEthnicbackground.ListEthnicData;
            //    }
            //}

            //if (Request["selectSchoolname"] != null && Request["ethnicity"] == null)
            //{
            //    vmEthnicbackground.IsShowCriteria = false;
            //    vmEthnicbackground.IsShowEthnicData = true;
            //    sSchoolName = Request["selectSchoolname"];
            //    vmEthnicbackground.selectedschoolname = sSchoolName;
            //    vmEthnicbackground.ListEthnicData = GetEthnicityDatabySchoolname(sSchoolName);
            //    Session["SessionListEthnicData"] = vmEthnicbackground.ListEthnicData;

            //}
            //else if (Request["selectSchoolname"] != null && Request["ethnicity"] != null)
            //{
            //    vmEthnicbackground.IsShowCriteria = true;
            //    vmEthnicbackground.IsShowEthnicData = false;
            //    //vmEthnicbackground.selectedschoolname = sSchoolName;

            //}
            //else if (Request["selectSchoolname"] == null && Request["ethnicity"] == null)
            //{
            //    vmEthnicbackground.IsShowCriteria = true;
            //    vmEthnicbackground.IsShowEthnicData = false;
            //    //vmEthnicbackground.selectedschoolname = sSchoolName;

            //}

            //if (sSchoolName != null)
            //{
            //    vmEthnicbackground.IsShowCriteria = false;
            //    vmEthnicbackground.selectedschoolname = sSchoolName;
            //    vmEthnicbackground.ListEthnicData = GetEthnicityDatabySchoolname(sSchoolName);
            //    Session["SessionListEthnicData"] = vmEthnicbackground.ListEthnicData;
            //}
            //else
            //{
            //    vmEthnicbackground.IsShowCriteria = true;
            //}

            return View("index", vmEthnicbackground);


        }

        public List<EthnicObj> GetEthnicityDatabySchoolname(string mSchoolname)
        {
            Console.Write("GetEthnicityData ==> ");

            var singlelistChartData = new List<ChartData>();
            List<EthnicObj> listDataseries = new List<EthnicObj>();
            List<EthnicObj> listtemp = new List<EthnicObj>();
            EthnicObj tempEthnicObj = new EthnicObj();
            

            //% for specific schoolname
            string query = " Select EthnicBackground, (Count(EthnicBackground)* 100 /";
            query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            query += " From test_3 where Name in ('" + mSchoolname + " ') Group By EthnicBackground ";

            var listResult = this.rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempEthnicObj = new EthnicObj();
                    tempEthnicObj.EthinicCode = Convert.ToString(itemRow[0]);
                    tempEthnicObj.EthinicName = GetDicEhtnicBG().ContainsKey(tempEthnicObj.EthinicCode) ? GetDicEhtnicBG()[tempEthnicObj.EthinicCode] : "NO NAME";
                    tempEthnicObj.EthinicName = GetDicEhtnicBG()[tempEthnicObj.EthinicCode];
                    tempEthnicObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);
                    listtemp.Add(tempEthnicObj);
                }
            }
            //% for All school
            listResult = this.rpGeneric.FindByNativeSQL("Select EthnicBackground, (Count(EthnicBackground)* 100 / (Select Count(*) From test_3))  From test_3  Group By EthnicBackground ");
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempEthnicObj = listtemp.Find(x => x.EthinicCode.Equals(Convert.ToString(itemRow[0])));
                    tempEthnicObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);

                    listDataseries.Add(tempEthnicObj);
                }
            }


            return listDataseries;
        }

        public List<EthnicObj> GetEthnicityDatabyCriteria(string mSchoolname, string zz)
        {
            Console.Write("GetEthnicityData ==> ");

            var singlelistChartData = new List<ChartData>();
            List<EthnicObj> listDataseries = new List<EthnicObj>();
            List<EthnicObj> listtemp = new List<EthnicObj>();
            EthnicObj tempEthnicObj = new EthnicObj();


            //% for specific schoolname
            string query = " Select EthnicBackground, (Count(EthnicBackground)* 100 /";
            query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            query += " From test_3 where Name in ('" + mSchoolname + " ') Group By EthnicBackground ";

            var listResult = this.rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempEthnicObj = new EthnicObj();
                    tempEthnicObj.EthinicCode = Convert.ToString(itemRow[0]);
                    tempEthnicObj.EthinicName = GetDicEhtnicBG().ContainsKey(tempEthnicObj.EthinicCode) ? GetDicEhtnicBG()[tempEthnicObj.EthinicCode] : "NO NAME";
                    tempEthnicObj.EthinicName = GetDicEhtnicBG()[tempEthnicObj.EthinicCode];
                    tempEthnicObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);
                    listtemp.Add(tempEthnicObj);
                }
            }
            //% for All school
            listResult = this.rpGeneric.FindByNativeSQL("Select EthnicBackground, (Count(EthnicBackground)* 100 / (Select Count(*) From test_3))  From test_3  Group By EthnicBackground ");
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempEthnicObj = listtemp.Find(x => x.EthinicCode.Equals(Convert.ToString(itemRow[0])));
                    tempEthnicObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);

                    listDataseries.Add(tempEthnicObj);
                }
            }


            return listDataseries;
        }


        [HttpPost]
        public JsonResult GetEthnicityData(SchCriteriaParameter mSchCriteriaParams)
        {
            Console.Write("GetEthnicityData ==> ");
            string query, queryfromDB = "";

            var singlelistChartData = new List<ChartData>();
            //List<double> listDataseries;

            ////List<object[]> listResult ;

            //foreach (var Ethnicbg in mSchCriteriaParams.ListConditionEthnicbg)
            //{
            //    listDataseries = new List<double>();
            //        foreach (var Ethnicbg in mSchCriteriaParams.ListConditionEthnicbg)
            //        {
            //            //System.out.print("nationalitity =>"+ nationality);
            //            if (Ethnicbg.Equals("TOTAL", StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                // PostgreSql query
            //                query = " SELECT national_identity, COUNT(*) ";
            //                query += queryfromDB;
            //                query += " WHERE national_identity IN ('" + nationality + "')";
            //                query += " GROUP BY national_identity";
            //                // MySQL and Access query
            //                //						query = " SELECT National_Identity, COUNT(*) ";
            //                //						query += queryfromDB;
            //                //						query += " WHERE National_Identity IN ('"+nationality+"')";
            //                //						query += " GROUP BY National_Identity";


            //            }
            //            else
            //            {
            //                //PostgreSql query
            //                query = " SELECT national_identity, COUNT(*)";
            //                query += queryfromDB;
            //                query += " WHERE national_identity IN ('" + nationality + "')";
            //                query += " AND gender IN ('" + gender + "')";
            //                query += " GROUP BY national_identity, gender ";
            //                // MySQL and Access Query
            //                //query = " SELECT National_Identity, COUNT(*)";
            //                //						query += queryfromDB;
            //                //						query += " WHERE National_Identity IN ('"+nationality+"')";						
            //                //						query += " AND Gender IN ('"+gender+"')";
            //                //						query += " GROUP BY National_Identity, Gender ";

            //            }

            //            var listResult = this.rpGeneric.FindByNativeSQL(query);
            //            if (listResult != null)
            //            {
            //                listDataseries.Add(Convert.ToInt32(listResult[0][1]));
            //            }

            //            query = "";
            //        }

            //        singlelistChartData.Add(new ChartData(string.Concat(gender, year), listDataseries));
            //        //sumData.add(singlelistChartData);
            //    }
            //}

            //foreach (ChartData a in singlelistChartData)
            //{
            //    //System.out.println(" Cahrt name==>"+a.getName());
            //    foreach (double num in a.data)
            //    {
            //        Console.Write(" Chart Data Series ==> " + num);
            //    }
            //    Console.WriteLine();
            //}
            return Json(singlelistChartData, JsonRequestBehavior.AllowGet);
        }

        private Dictionary<string, string> GetDicEhtnicBG()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "White – Scottish");
            dicNational.Add("02", "African – African / Scottish / British");
            dicNational.Add("03", "Caribbean or Black – Caribbean / British / Scottish");
            dicNational.Add("05", "Asian – Indian/British/Scottish");
            dicNational.Add("06", "Asian – Pakistani / British / Scottish");
            dicNational.Add("07", "Asian –Bangladeshi / British / Scottish");
            dicNational.Add("08", "Asian – Chinese / British / Scottish");
            dicNational.Add("09", "White – Other");
            dicNational.Add("10", "Not Disclosed");
            dicNational.Add("12", "Mixed or multiple ethnic groups");
            dicNational.Add("17", "Asian – Other");
            dicNational.Add("19", "White – Gypsy/Traveller");
            dicNational.Add("21", "White – Other British");
            dicNational.Add("22", "White – Irish");
            dicNational.Add("23", "White – Polish");
            dicNational.Add("24", "Caribbean or Black – Other");
            dicNational.Add("25", "African – Other");
            dicNational.Add("27", "Other – Arab");
            dicNational.Add("98", "Not Known");
            dicNational.Add("99", "Other – Other");
            return dicNational;
        }

        [HttpPost]
        public JsonResult GetChartDataEthnic(string[] arrParameterFilter)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[arrParameterFilter.Length];

                var listEthnicData = Session["SessionListEthnicData"] as List<EthnicObj>;
                if (listEthnicData != null)
                {
                    var listEthnicFilter = listEthnicData.Where(x => arrParameterFilter.Contains(x.EthinicCode)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "test",
                        ChartCategories = listEthnicFilter.Select(x => x.EthinicName).ToArray(),
                        ChartSeries = ProcessChartDataEthnic(listEthnicFilter)
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

        private List<object> ProcessChartDataEthnic(List<EthnicObj> listEthnicFilter)
        {
            var listChartData = new List<object>();

            listChartData.Add(new { name = "Data 1", data = listEthnicFilter.Select(x => x.PercentageAllSchool).ToArray() });
            listChartData.Add(new { name = "Data 2", data = listEthnicFilter.Select(x => x.PercentageInSchool).ToArray() });

            return listChartData;
        }
    }
}