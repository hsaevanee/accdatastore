using ACCDataStore.Entity;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.Nationality.ViewModels.IndexNationality;
using ACCDataStore.Web.Areas.Nationality.ViewModels.Nationality;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ACCDataStore.Web.Areas.Nationality.Controllers
{
    public class NationalityController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexNationalityController));
        //Git test 2

        private readonly IGenericRepository rpGeneric;
        public NationalityController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        //public ActionResult Index()
        //{
        //    var vmIndex = new IndexViewModel();
        //    vmIndex.ListNationality2012 = this.rpGeneric.Find<Nationality2012>(" from Nationality2012 where Gender = :Gender ", new string[] { "Gender" }, new object[] { "M" });

        //    return View(vmIndex);
        //}
        // GET: Nationality/Nationality
        public ActionResult Index()
        {
            try
            {
                //var vmNationality = new NationalityViewModel();

                //string[] arrIdentity = { "01", "02" };
                //var listNationalityData = new List<NationalityData>();
                //foreach (var item in arrIdentity)
                //{
                //    var listResult = this.rpGeneric.FindByNativeSQL("Select national_identity, count(*) from Nationality2012 where national_identity IN ('" + item + "') Group by national_identity");
                //    var eNationalityData = new NationalityData();
                //    eNationalityData.Identity = listResult[0][0].ToString();
                //    eNationalityData.TotalNumber = Convert.ToInt32(listResult[0][1]);
                //    listNationalityData.Add(eNationalityData);
                //}

                //vmNationality.ListNationalityData = listNationalityData;
                //return View("Nationality", vmNationality);

                var vmNationality = new NationalityViewModel();

                var national = new List<string>();
                var gender = new List<string>();
                // list of year
                var Years = new List<string>();
                Years.Add("2012");
                Years.Add("2013");

                var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT  DISTINCT national_identity from Nationality2012");

                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            //Object[] listRow = (Object[]) itemRow;	
                            log.Info(" distinct nationality: " + itemRow.ToString());
                            national.Add(itemRow.ToString());
                        }
                    }
                }

                listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT  DISTINCT gender from Nationality2012");

                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        //Object[] listRow = (Object[]) itemRow;
                        //listResult return into array of string						
                        gender.Add(itemRow.ToString());
                    }
                }


                //System.out.println(national.size() + " distinct nationality: " + national);

                //System.out.println(gender.size() + " distinct gender: " + gender);

                vmNationality.ListNational = national;
                vmNationality.ListGender = gender;
                vmNationality.ListYear = Years;
                vmNationality.DicNational = GetDicNational();

                return View("Nationality", vmNationality);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return View("Nationality");
            }
        }

        private Dictionary<string, string> GetDicNational()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "Scottish");
            dicNational.Add("02", "English");
            dicNational.Add("03", "Northern Irish");
            dicNational.Add("04", "Welsh");
            dicNational.Add("05", "British");
            dicNational.Add("98", "Other");
            dicNational.Add("99", "Not Disclosed");
            return dicNational;
        }

        [HttpPost]
        public JsonResult GetNationalityData(CriteriaParameters mNationalParams)
        {

            string query, queryfromDB = "";

            var singlelistChartData = new List<ChartData>();
            List<double> listDataseries;

            //List<object[]> listResult ;

            foreach (var year in mNationalParams.ListConditionYear)
            {
                if (year.Equals("2012", StringComparison.CurrentCultureIgnoreCase))
                {
                    // MS Access and MYSQL Query
                    //queryfromDB = " FROM Nationality2012";
                    //PostgreSql query
                    queryfromDB = " FROM nationality2012";
                }
                else
                {
                    // MS and MYSQL Query
                    //queryfromDB = " FROM Nationality2013";
                    //PostgreSql query
                    queryfromDB = " FROM nationality2013";
                }
                foreach (var gender in mNationalParams.ListConditionGender)
                {
                    listDataseries = new List<double>();
                    foreach (var nationality in mNationalParams.ListConditionNationality)
                    {
                        //System.out.print("nationalitity =>"+ nationality);
                        if (gender.Equals("TOTAL", StringComparison.CurrentCultureIgnoreCase))
                        {
                            // PostgreSql query
                            query = " SELECT national_identity, COUNT(*) ";
                            query += queryfromDB;
                            query += " WHERE national_identity IN ('" + nationality + "')";
                            query += " GROUP BY national_identity";
                            // MySQL and Access query
                            //						query = " SELECT National_Identity, COUNT(*) ";
                            //						query += queryfromDB;
                            //						query += " WHERE National_Identity IN ('"+nationality+"')";
                            //						query += " GROUP BY National_Identity";


                        }
                        else
                        {
                            //PostgreSql query
                            query = " SELECT national_identity, COUNT(*)";
                            query += queryfromDB;
                            query += " WHERE national_identity IN ('" + nationality + "')";
                            query += " AND gender IN ('" + gender + "')";
                            query += " GROUP BY national_identity, gender ";
                            // MySQL and Access Query
                            //query = " SELECT National_Identity, COUNT(*)";
                            //						query += queryfromDB;
                            //						query += " WHERE National_Identity IN ('"+nationality+"')";						
                            //						query += " AND Gender IN ('"+gender+"')";
                            //						query += " GROUP BY National_Identity, Gender ";

                        }

                        var listResult = this.rpGeneric.FindByNativeSQL(query);
                        if (listResult != null)
                        {
                            listDataseries.Add(Convert.ToInt32(listResult[0][1]));
                        }

                        query = "";
                    }

                    singlelistChartData.Add(new ChartData(string.Concat(gender, year), listDataseries));
                    //sumData.add(singlelistChartData);
                }
            }

            foreach (ChartData a in singlelistChartData)
            {
                //System.out.println(" Cahrt name==>"+a.getName());
                foreach (double num in a.data)
                {
                    Console.Write(" Chart Data Series ==> " + num);
                }
                Console.WriteLine();
            }

            //return singlelistChartData;

            return Json(singlelistChartData, JsonRequestBehavior.AllowGet);
        }
    }
}