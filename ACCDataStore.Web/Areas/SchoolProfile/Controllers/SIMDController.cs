using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.SIMD;
using ClosedXML.Excel;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class SIMDController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(SIMDController));

        private readonly IGenericRepository rpGeneric;

        public SIMDController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
 
        // GET: SchoolProfile/SIMD
        public ActionResult Index(string sSchoolName)
        {
            var vmSIMD = new SIMDViewModel();

            var schoolname = new List<string>();

            var setSIMDCriteria = new List<string>();
            var setYearCriteria = new List<string>();

            List<SIMDObj> ListSIMDData = new List<SIMDObj>();
            List<SIMDObj> temp = new List<SIMDObj>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();
            fooList.Add("Select School");

            vmSIMD.ListSchoolNameData = fooList;

            fooList = new List<string>();
                
            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW SIMD_2012_decile FROM test_3 group by SIMD_2012_decile");

            if (listResult != null)
            {   
                foreach (var itemRow in listResult)
                {
                    if (itemRow!=null)
                    fooList.Add(Convert.ToString(itemRow));
                }
            }

            vmSIMD.ListSIMDdefinition= fooList;
            vmSIMD.ListYear = new List<string>(new string[] { "2009", "2012"});

            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmSIMD.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmSIMD.IsShowCriteria = false;
                    vmSIMD.ListSelectedYear = new List<string>(new string[] { "2012" });
                    Session["ListSelectedYears"] = vmSIMD.ListSelectedYear;
                    Session["sSchoolName"] = sSchoolName;
                }

            }
            else // post method
            {
                // get parameter from Request object
                vmSIMD.IsShowCriteria = true;
                sSchoolName  = Request["selectSchoolname"];

                Session["sSchoolName"] = sSchoolName;

                if (Request["SIMD"] != null)
                {
                    setSIMDCriteria = Request["SIMD"].Split(',').ToList();
                }
                else
                {
                    setSIMDCriteria = null;
                }

                if (Request["years"] != null)
                {
                    vmSIMD.ListSelectedYear = Request["years"].Split(',').ToList(); 
                }
                else
                {
                    vmSIMD.ListSelectedYear = new List<string>(new string[] { "2012" });
                }

               Session["ListSelectedYears"] = vmSIMD.ListSelectedYear;
                               
            }

            // process data
            if (sSchoolName != null)
            {
                vmSIMD.selectedschoolname = sSchoolName;
                ListSIMDData = GetSIMDDatabySchoolname(rpGeneric, sSchoolName, setYearCriteria);
                if (setSIMDCriteria == null) {
                    vmSIMD.ListSIMDData = null;
                }
                else if (setSIMDCriteria.Count != 0 && setSIMDCriteria != null)
                {
                    vmSIMD.ListSIMDData= ListSIMDData.Where(x => setSIMDCriteria.Contains(x.SIMDCode)).ToList();
                }
                else
                {
                    vmSIMD.ListSIMDData = ListSIMDData;
                }
                Session["SessionListSIMDData"] = vmSIMD.ListSIMDData;
            }

           // vmSIMD.ListSIMDdata = GetSIMDDatabySchoolname(sSchoolName);
            return View("Index", vmSIMD);
        }

        [HttpPost]
        public JsonResult GetChartDataSIMD(string[] arrParameterFilter)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[arrParameterFilter.Length];

                var listSIMDData = Session["SessionListSIMDData"] as List<SIMDObj>;
                if (listSIMDData != null)
                {
                    var listSIMDFilter = listSIMDData.Where(x => arrParameterFilter.Contains(x.SIMDCode)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "Scottish Index of Multiple Deprivation",
                        ChartCategories = listSIMDFilter.Select(x => x.SIMDCode).ToArray(),
                        ChartSeries = ProcessChartDataSIMD(listSIMDFilter)
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

        private List<object> ProcessChartDataSIMD(List<SIMDObj> listSIMDFilter)
        {
            var listChartData = new List<object>();
            var ListSelectedYears =  Session["ListSelectedYears"] as List<string>;
            var schoolname =  Session["sSchoolName"];

            foreach (var itemYear in ListSelectedYears)
            {
                if (itemYear.Equals("2009"))
                {
                    listChartData.Add(new { name = schoolname+"2009", data = listSIMDFilter.Select(x => x.PercentageInSchool2009).ToArray() });
                    listChartData.Add(new { name = "AllSchool2009", data = listSIMDFilter.Select(x => x.PercentageAllSchool2009).ToArray() });
                }

                if (itemYear.Equals("2012"))
                {
                    listChartData.Add(new { name = schoolname+"2012", data = listSIMDFilter.Select(x => x.PercentageInSchool2012).ToArray() });
                    listChartData.Add(new { name = "AllSchool2012", data = listSIMDFilter.Select(x => x.PercentageAllSchool2012).ToArray() });
                }
            }
            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            var listSIMDData = Session["SessionListSIMDData"] as List<SIMDObj>;
            string schoolname = Session["sSchoolName"].ToString();

            var dataStream = GetWorkbookDataStream(listSIMDData, schoolname);
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "export.xlsx");
        }

        private MemoryStream GetWorkbookDataStream(List<SIMDObj> dtResult, string schoolname)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = schoolname; // use cell address in range
            worksheet.Cell("A2").Value = "Scottish Index of Multiple Deprivation "; // use cell address in range
            worksheet.Cell(3, 1).InsertTable(dtResult); // use row & column index
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

    }
}