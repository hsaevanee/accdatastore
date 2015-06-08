using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.StudentStage;
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
    public class StudentStageController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(NationalityController));

        private readonly IGenericRepository rpGeneric;

        public StudentStageController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
        // GET: SchoolProfile/StudentStage
        public ActionResult Index(string sSchoolName)
        {
            var vmStudentStage = new StudentStageViewModel();

            var schoolname = new List<string>();
            var sStageCriteria = new List<string>();
            var setGenderCriteria = new List<string>();

            List<StdStageObj> ListStdStageData = new List<StdStageObj>();
            List<StdStageObj> temp = new List<StdStageObj>();


            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmStudentStage.ListSchoolNameData = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM test_3 group by StudentStage");

            fooList = listResult.OfType<string>().ToList();
            vmStudentStage.ListStage = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Gender FROM test_3 group by Gender");

            fooList = listResult.OfType<string>().ToList();

            vmStudentStage.ListGenderCode = fooList;
            vmStudentStage.DicGender = GetDicGender();


            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmStudentStage.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmStudentStage.IsShowCriteria = false;
                    vmStudentStage.ListSelectedGender = new List<string>(new string[] { "Total" });
                    Session["ListSelectedGender"] = vmStudentStage.ListSelectedGender;
                    Session["sSchoolName"] = sSchoolName;
                }

            }
            else // post method
            {
                vmStudentStage.IsShowCriteria = true;
                sSchoolName = Request["selectSchoolname"];
                Session["sSchoolName"] = sSchoolName;

                if (Request["stages"] != null)
                {
                    sStageCriteria = Request["stages"].Split(',').ToList();
                }
                else
                {
                    sStageCriteria = null;
                }
                if (Request["gender"] != null)
                {
                    vmStudentStage.ListSelectedGender = Request["gender"].Split(',').ToList();
                }
                else
                {
                    vmStudentStage.ListSelectedGender = new List<string>(new string[] { "Total" });
                }

                Session["ListSelectedGender"] = vmStudentStage.ListSelectedGender;
                // get parameter from Request object
            }

            vmStudentStage.DicGenderWithSelected = GetDicGenderWithSelected(vmStudentStage.ListSelectedGender);

            // process data
            if (sSchoolName != null)
            {
                vmStudentStage.selectedschoolname = sSchoolName;
                ListStdStageData = GetStudentStageDatabySchoolname(rpGeneric, sSchoolName);

                if (sStageCriteria == null)
                {
                    vmStudentStage.ListStdStageData = null;
                }
                else if (sStageCriteria.Count != 0 && sStageCriteria != null)
                {
                    vmStudentStage.ListStdStageData = ListStdStageData.Where(x => sStageCriteria.Contains(x.StageCode)).ToList();
                }
                else
                {
                    vmStudentStage.ListStdStageData = ListStdStageData;
                }
                Session["SessionListStudentStageData"] = vmStudentStage.ListStdStageData;
            }
            return View("Index", vmStudentStage);
        }

        [HttpPost]
        public JsonResult GetChartDataStudentStage(string[] arrParameterFilter)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[arrParameterFilter.Length];

                var listStdStageData = Session["SessionListStudentStageData"] as List<StdStageObj>;
                if (listStdStageData != null)
                {
                    var listStdStageFilter = listStdStageData.Where(x => arrParameterFilter.Contains(x.StageCode)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "Stage",
                        ChartCategories = listStdStageFilter.Select(x => x.StageCode).ToArray(),
                        ChartSeries = ProcessChartDataStdStage(listStdStageFilter)
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

        private List<object> ProcessChartDataStdStage(List<StdStageObj> listStdStageFilter)
        {
            var listChartData = new List<object>();
            var ListSelectedGender = Session["ListSelectedGender"] as List<string>;
            var schoolname = Session["sSchoolName"];

            foreach (var itemGender in ListSelectedGender)
            {
                if (itemGender.Equals("F"))
                {
                    listChartData.Add(new { name = "FemaleAllSchool", data = listStdStageFilter.Select(x => x.PercentageFemaleAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname + " Female", data = listStdStageFilter.Select(x => x.PercentageFemaleInSchool).ToArray() });
                }

                if (itemGender.Equals("M"))
                {
                    listChartData.Add(new { name = "MaleAllSchool", data = listStdStageFilter.Select(x => x.PercentageMaleAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname + " Male", data = listStdStageFilter.Select(x => x.PercentageMaleInSchool).ToArray() });
                }
                if (itemGender.Equals("Total"))
                {
                    listChartData.Add(new { name = "TotalAllSchool", data = listStdStageFilter.Select(x => x.PercentageAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname + " Total", data = listStdStageFilter.Select(x => x.PercentageInSchool).ToArray() });
                }

            }
            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            var listStdStageData = Session["SessionListStudentStageData"] as List<StdStageObj>;
            string schoolname = Session["sSchoolName"].ToString();

            var dataStream = GetWorkbookDataStream(listStdStageData, schoolname);
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "export.xlsx");
        }

        private MemoryStream GetWorkbookDataStream(List<StdStageObj> dtResult, string schoolname)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = schoolname; // use cell address in range
            worksheet.Cell("A2").Value = "Student Stages"; // use cell address in range
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