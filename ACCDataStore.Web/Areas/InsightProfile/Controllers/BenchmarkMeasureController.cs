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
using ClosedXML.Excel;
using System.Data;
using System.IO;

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
            var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL("SELECT DISTINCTROW t1.centre, t2.SchoolName from la100pupils t1 INNER JOIN la100schools t2 on t1.centre = t2.SeedCode ");
            List<School> temp = new List<School>();

            if (listResultMySQL.Any())
            {
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
        [HttpPost]
        public ActionResult GetIndexLeaverDestination(BenchmarkMeasureViewModel model)
        {
            var vmBenchmarkMeasure = new BenchmarkMeasureViewModel();

            vmBenchmarkMeasure.ListSchoolNameData = GetListSchoolname(rpGeneric2nd);
            vmBenchmarkMeasure.ListYearData = GetListYear();
            vmBenchmarkMeasure.ListGenderData = model.ListGenderData;
            if (vmBenchmarkMeasure.ListGenderData.Where(x => x.isSelected == true).Count() == 0)
            {
                var temp = (from a in vmBenchmarkMeasure.ListGenderData where a.gendercode==0 select a).First();
                temp.isSelected = true;
               
            }

            vmBenchmarkMeasure.ListLeaverDestinationData = GetLeaverDestinationData(model.selectedschoolcode, model.ListGenderData);
            Session["ListLeaverDestinationData"] = vmBenchmarkMeasure.ListLeaverDestinationData;
            Session["ListGenderData"] = vmBenchmarkMeasure.ListGenderData;
            return View("IndexLeaver", vmBenchmarkMeasure);
        }

        protected List<LeaverdestinationData> GetLeaverDestinationData(string schoolcode, IList<Gender> ListGenderData)
        {
            try
            {
                List<LeaverDestination> listdata = new List<LeaverDestination>();

                List<LeaverDestination> listLeaverDestination = null;

                //var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Pupils>();

                string query = "SELECT t1.leaver_centre, t2.SchoolName,t1.year,t1.gender, t1.leaver_destination_group, count(*) FROM la100pupils t1 Inner join la100schools t2 on t1.leaver_centre = t2.SeedCode where t1.leaver_centre = '" + schoolcode + "'  group by leaver_centre, year, gender, leaver_destination_group";

                query += " union ";

                query += "SELECT t1.leaver_centre, t2.SchoolName,t1.year,0, t1.leaver_destination_group, count(*) FROM la100pupils t1 Inner join la100schools t2 on t1.leaver_centre = t2.SeedCode where t1.leaver_centre = '" + schoolcode + "'  group by leaver_centre, year, leaver_destination_group";

                query += " union ";

                query += "SELECT 100, 'Aberdeen City', t1.year,t1.gender, t1.leaver_destination_group, count(*) FROM la100pupils t1  where t1.leaver_centre != 'NULL' group by year, gender, leaver_destination_group";

                query += " union ";

                query += "SELECT 100, 'Aberdeen City', t1.year,0, t1.leaver_destination_group, count(*) FROM la100pupils t1  where t1.leaver_centre != 'NULL' group by year, leaver_destination_group";

                var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL(query);

                if (listResultMySQL != null)
                {
                    var dicLeaver = new Dictionary<string, LeaverDestination>();
                    foreach (var itemRow in listResultMySQL)
                    {
                        LeaverDestination vmLeaverDestination = null;
                        var sKey = itemRow[0].ToString() + itemRow[2].ToString() + itemRow[3].ToString();
                        var sLeaverDestinationGroup = itemRow[4] != null ? itemRow[4].ToString().ToLower().Equals("null") ? "0" : itemRow[4].ToString() : "0";
                        if (!dicLeaver.ContainsKey(sKey))
                        {
                            vmLeaverDestination = new LeaverDestination();
                            vmLeaverDestination.centrecode = Convert.ToInt32(itemRow[0]);
                            vmLeaverDestination.centrename = itemRow[1].ToString();
                            vmLeaverDestination.year = Convert.ToInt32(itemRow[2]);
                            vmLeaverDestination.academicyear = new Year(itemRow[2].ToString());
                            vmLeaverDestination.gender = new Gender(Convert.ToInt32(itemRow[3]));
                            dicLeaver.Add(sKey, vmLeaverDestination);
                        }
                        else
                        {
                            vmLeaverDestination = dicLeaver[sKey];
                        }
                        switch (sLeaverDestinationGroup)
                        {
                            case "0":
                                vmLeaverDestination.sum0 = Convert.ToInt32(itemRow[5]);
                                break;
                            case "1":
                                vmLeaverDestination.sum1 = Convert.ToInt32(itemRow[5]);
                                break;
                            case "2":
                                vmLeaverDestination.sum2 = Convert.ToInt32(itemRow[5]);
                                break;
                        }
                    }

                    listLeaverDestination = dicLeaver.Values.ToList();

                    var n = listLeaverDestination[0].Percentage;
                    listLeaverDestination = listLeaverDestination.OrderBy(x => x.year).ThenBy(x => x.gender.gendercode).ToList();
                }

                foreach (var gender in ListGenderData)
                {
                    if (gender.isSelected)
                    {
                        List<LeaverDestination> temp = (from a in listLeaverDestination where a.gender.gendercode.Equals(gender.gendercode) select a).ToList();
                        listdata.AddRange(temp);
                    }

                }

                listdata = listdata.OrderBy(x => x.year).ThenBy(x => x.gender.gendercode).ToList();

                //check if listgender is selected at least one in the list 


                var listDataTestPivot = listdata.GroupBy(x => new { x.academicyear.academicyear, x.centrecode, x.centrename }).Select(x => new LeaverdestinationData
                {
                    centrecode = x.Key.centrecode.ToString(),
                    centrename = x.Key.centrename,
                    academicyear = x.Key.academicyear,
                    PercentageMale = x.FirstOrDefault(y => y.gender.gendercode == 1) == null ? 0 : x.FirstOrDefault(y => y.gender.gendercode == 1).Percentage,
                    PercentageFeMale = x.FirstOrDefault(y => y.gender.gendercode == 2) == null ? 0 : x.FirstOrDefault(y => y.gender.gendercode == 2).Percentage,
                    PercentageAll = x.FirstOrDefault(y => y.gender.gendercode == 0) == null ? 0 : x.FirstOrDefault(y => y.gender.gendercode == 0).Percentage,
                }).ToList();

                listDataTestPivot = listDataTestPivot.OrderBy(x => x.centrecode).ThenBy(x => x.academicyear).ToList();

                return listDataTestPivot;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetChartLeaverDestination(string schcode, string selectedschname)
        {
            try
            {
                Object oChartData = new Object();

                var ListLeaverDestinationData = Session["ListLeaverDestinationData"] as List<LeaverdestinationData>;

                var ListGenderData = Session["ListGenderData"] as List<Gender>;

                var listChartData = new List<object>();

                foreach (var itemGender in ListGenderData)
                {
                    if (itemGender.gendercode == 1 && itemGender.isSelected)
                    {
                        listChartData.Add(new { name = selectedschname + " Male", data = ListLeaverDestinationData.Where(x => x.centrecode.ToString().Equals(schcode)).Select(x => x.PercentageMale).ToArray() });
                        listChartData.Add(new { name = "Aberdeen City Male", data = ListLeaverDestinationData.Where(x => x.centrecode.ToString().Equals("100")).Select(x => x.PercentageMale).ToArray() });
                    }

                    if (itemGender.gendercode == 2 && itemGender.isSelected)
                    {
                        listChartData.Add(new { name = selectedschname + " Female", data = ListLeaverDestinationData.Where(x => x.centrecode.ToString().Equals(schcode)).Select(x => x.PercentageFeMale).ToArray() });
                        listChartData.Add(new { name = "Aberdeen City Female", data = ListLeaverDestinationData.Where(x => x.centrecode.ToString().Equals("100")).Select(x => x.PercentageFeMale).ToArray() });

                    }
                    if (itemGender.gendercode == 0 && itemGender.isSelected)
                    {
                        listChartData.Add(new { name = selectedschname, data = ListLeaverDestinationData.Where(x => x.centrecode.ToString().Equals(schcode)).Select(x => x.PercentageAll).ToArray() });
                        listChartData.Add(new { name = "Aberdeen City ", data = ListLeaverDestinationData.Where(x => x.centrecode.ToString().Equals("100")).Select(x => x.PercentageAll).ToArray() });
                    }

                }
                oChartData = new
                {
                    ChartTitle = selectedschname,
                    ChartCategories = ListLeaverDestinationData.Where(x => x.centrecode.ToString().Equals("100")).Select(x => x.academicyear).ToArray(),
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


        [HttpPost]
        public JsonResult GetChartLeaverDestinationBreakdown(string schcode, string selectedschname, string year)
        {
            try
            {
                Object oChartData = new Object();

                List<DestinationObj> listdata = new List<DestinationObj>();

                List<LeaverDestinationBreakdown> listLeaverDestinationBreakdown = GetListDestinationBreakdown(this.rpGeneric2nd).ToList();

                //var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Pupils>();

                string query = "SELECT t1.leaver_centre, t2.SchoolName, t1.year, t1.gender, t1.destination, count(*) FROM la100pupils t1 Inner join la100schools t2 on t1.leaver_centre = t2.SeedCode where t1.leaver_centre = '" + schcode + "'and year='" + year + "' group by leaver_centre, year, gender, destination";

                query += " union ";

                query += "SELECT t1.leaver_centre, t2.SchoolName, t1.year, 0, t1.destination, count(*) FROM la100pupils t1 Inner join la100schools t2 on t1.leaver_centre = t2.SeedCode where t1.leaver_centre = '" + schcode + "'and year='" + year + "' group by leaver_centre, year, destination ";

                query += " union ";
                query += "SELECT 100, 'Aberdeen City', year, gender, destination, count(*) FROM la100pupils where year='" + year + "' group by  year, gender, destination";
                query += " union ";
                query += "SELECT 100, 'Aberdeen City', year, 0, destination, count(*) FROM la100pupils where year='" + year + "' group by year, destination ";

                var listResultMySQL = this.rpGeneric2nd.FindByNativeSQL(query);

                if (listResultMySQL != null)
                {
                    foreach (var itemrow in listResultMySQL)
                    {
                        listdata.Add(new DestinationObj(itemrow[0].ToString(), itemrow[2].ToString(), itemrow[3].ToString(), itemrow[4].ToString(), Convert.ToDouble(itemrow[5])));
                    }

                }

                foreach (var item in listLeaverDestinationBreakdown)
                {
                    double nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals(schcode) && x.gender.Equals("2")).Select(x => x.number).Sum();
                    double ny = listdata.Where(x => x.gender.Equals("2") && x.objname.Equals(schcode)).Select(x => x.number).Sum();
                    item.PercentageFemaleinSchool = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals(schcode) && x.gender.Equals("1")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("1") && x.objname.Equals(schcode)).Select(x => x.number).Sum();
                    item.PercentageMaleinSchool = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals(schcode) && x.gender.Equals("0")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("0") && x.objname.Equals(schcode)).Select(x => x.number).Sum();
                    item.PercentageAllinSchool = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("100") && x.gender.Equals("2")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("2") && x.objname.Equals("100")).Select(x => x.number).Sum();
                    item.PercentageFemaleinAbdcity = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("100") && x.gender.Equals("1")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("1") && x.objname.Equals("100")).Select(x => x.number).Sum();
                    item.PercentageMaleinAbdcity = (nx * 100) / ny;
                    nx = listdata.Where(x => x.destiationcode.Equals(item.destinationcode) && x.objname.Equals("100") && x.gender.Equals("0")).Select(x => x.number).Sum();
                    ny = listdata.Where(x => x.gender.Equals("0") && x.objname.Equals("100")).Select(x => x.number).Sum();
                    item.PercentageAllinAbdcity = (nx * 100) / ny;
                }



                var listChartData = new List<object>();
                listChartData.Add(new { name = selectedschname + " Female", data = listLeaverDestinationBreakdown.Select(x => x.PercentageFemaleinSchool).ToArray() });
                listChartData.Add(new { name = selectedschname + " Male", data = listLeaverDestinationBreakdown.Select(x => x.PercentageMaleinSchool).ToArray() });
                listChartData.Add(new { name = selectedschname, data = listLeaverDestinationBreakdown.Select(x => x.PercentageAllinSchool).ToArray() });

                listChartData.Add(new { name = "Aberdeen City Female", data = listLeaverDestinationBreakdown.Select(x => x.PercentageFemaleinAbdcity).ToArray() });
                listChartData.Add(new { name = "Aberdeen City Male", data = listLeaverDestinationBreakdown.Select(x => x.PercentageMaleinAbdcity).ToArray() });
                listChartData.Add(new { name = "Aberdeen City ", data = listLeaverDestinationBreakdown.Select(x => x.PercentageAllinAbdcity).ToArray() });



                oChartData = new
                {
                    ChartTitle = selectedschname,
                    ChartCategories = listLeaverDestinationBreakdown.Select(x => x.destinationname).ToList(),
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
        public ActionResult ExportExcel()
        {
            //var listNationalityData = Session["SessionListLevelENData"] as List<NationalityObj>;
            //string schoolname = Session["sSchoolName"].ToString();
            var dataStream = GetWorkbookDataStream(GetData());
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LIDExport.xlsx");
        }

        private DataTable GetData()
        {
            // simulate datatable
            var ListLeaverDestinationData = Session["ListLeaverDestinationData"] as List<LeaverdestinationData>;

            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("Schools", typeof(string));
            dtResult.Columns.Add("Academic years", typeof(string));
            dtResult.Columns.Add("Female", typeof(double));
            dtResult.Columns.Add("Male ", typeof(double));
            dtResult.Columns.Add("Total ", typeof(double));

            var transformObject = new
            {
                Col1 = ListLeaverDestinationData.Select(x => x.centrename).ToList(),
                Col2 = ListLeaverDestinationData.Select(x => x.academicyear).ToList(),
                Col3 = ListLeaverDestinationData.Select(x => x.PercentageFeMale).ToList(),
                Col4 = ListLeaverDestinationData.Select(x => x.PercentageMale).ToList(),
                Col5 = ListLeaverDestinationData.Select(x => x.PercentageAll).ToList(),
            };

            for (var i = 0; i < ListLeaverDestinationData.Count; i++)
            {
                dtResult.Rows.Add(
                    transformObject.Col1[i],
                    transformObject.Col2[i],
                    transformObject.Col3[i],
                    transformObject.Col4[i],
                    transformObject.Col5[i]
                    );
            }
            return dtResult;
        }

        private MemoryStream GetWorkbookDataStream(DataTable dtResult)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = "Leaver Initial Destination"; // use cell address in range
            //worksheet.Cell("A2").Value = "Nationality"; // use cell address in range
            worksheet.Cell("A2").Value = "% of Schools Leavers in a Positive Destination";
            worksheet.Cell(3, 1).InsertTable(dtResult); // use row & column index
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public ActionResult IndexAttainment()
        {
            return View("IndexAttainment", null);
        }

        public ActionResult LeaverDestinationBreakdown()
        {
            var vmBenchmarkMeasure = new BenchmarkMeasureViewModel();

            vmBenchmarkMeasure.ListSchoolNameData = GetListSchoolname(rpGeneric2nd);
            vmBenchmarkMeasure.ListGenderData = GetListGender();
            vmBenchmarkMeasure.ListYearData = GetListYear();
            
            return View("IndexLeaverBreakdown", vmBenchmarkMeasure);
        }


        public ActionResult MapData()
        {
            //var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;

            var vmBenchmarkMeasure = new BenchmarkMeasureViewModel();
            vmBenchmarkMeasure.ListSchoolNameData = GetListSchoolname(rpGeneric2nd);
            return View("MapLeaverDestinationIndex", vmBenchmarkMeasure);
        }
        protected JsonResult ThrowJSONError(Exception ex)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
            return Json(new { Message = sErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByName(string keyschcode, string keyschname, string keyname)
        {
            try
            {
                object oChartData = new object();

                List<Gender> gender = new List<Gender>();
                gender.Add(new Gender(0)); //ALL


                if (keyname.Equals("SchCode"))
                {
                    List<LeaverdestinationData> listdata = GetLeaverDestinationData(keyschcode, gender);

                    oChartData = new
                    {
                        dataTitle = keyschname,
                        dataCategories = listdata.Where(x => x.centrecode.Equals("100")).Select(x => x.academicyear).ToArray(),
                        Schooldata = listdata.Where(x => x.centrecode.Equals(keyschcode)).Select(x => x.PercentageAll).ToArray(),
                        Abddata = listdata.Where(x => x.centrecode.Equals("100")).Select(x => x.PercentageAll).ToArray()
                    };

                }
                else if (keyname.Equals("ZoneCode"))
                {
                    oChartData = new
                    {
                        dataTitle = keyschcode,
                        dataSeries = new List<Int16>()
                    };
                }

                // use sName (AB24) to query data from database
                return Json(oChartData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return ThrowJSONError(ex);
            }
        }
    }
}