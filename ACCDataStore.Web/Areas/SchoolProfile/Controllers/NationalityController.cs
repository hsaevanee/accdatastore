﻿using ACCDataStore.Entity;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Nationality;
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
    public class NationalityController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfileController));

        private readonly IGenericRepository rpGeneric;

        public NationalityController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
        // GET: SchoolProfile/Nationality
        public ActionResult Index(string sSchoolName)
        {
            var vmNationality = new NationalityViewModel();

            var schoolname = new List<string>();
            var sNationalCriteria = new List<string>();

            List<NationalityObj> ListNationalData = new List<NationalityObj>();
            List<NationalityObj> temp = new List<NationalityObj>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmNationality.ListSchoolNameData = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW NationalIdentity FROM test_3 group by NationalIdentity");

            fooList = listResult.OfType<string>().ToList();
            vmNationality.ListNationalCode = fooList;
            vmNationality.DicNational = GetDicNational();

            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmNationality.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmNationality.IsShowCriteria = false;
                }

            }
            else // post method
            {
                vmNationality.IsShowCriteria = true;
                sSchoolName = Request["selectSchoolname"];
                sNationalCriteria = Request["nationality"].Split(',').ToList();
                // get parameter from Request object
            }

            // process data
            if (sSchoolName != null)
            {
                vmNationality.selectedschoolname = sSchoolName;
                ListNationalData = GetNationalityDatabySchoolname(sSchoolName);
                if (sNationalCriteria.Count != 0)
                {
                    vmNationality.ListNationalityData = ListNationalData.Where(x => sNationalCriteria.Contains(x.IdentityCode)).ToList();
                }
                else
                {
                    vmNationality.ListNationalityData = ListNationalData;
                }
                Session["SessionListNationalityData"] = vmNationality.ListNationalityData;
            }
            return View("Index", vmNationality);
        }

        public List<NationalityObj> GetNationalityDatabySchoolname(string mSchoolname)
        {
            Console.Write("GetNationalityData ==> ");

            var singlelistChartData = new List<ChartData>();
            List<NationalityObj> listDataseries = new List<NationalityObj>();
            List<NationalityObj> listtemp = new List<NationalityObj>();
            NationalityObj tempNationalObj = new NationalityObj();
            

            //% for All school
            var listResult = this.rpGeneric.FindByNativeSQL("Select NationalIdentity, (Count(NationalIdentity)* 100 / (Select Count(*) From test_3))  From test_3  Group By NationalIdentity ");
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempNationalObj = new NationalityObj();
                    tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
                    tempNationalObj.IdentityName = GetDicNational().ContainsKey(tempNationalObj.IdentityCode) ? GetDicNational()[tempNationalObj.IdentityCode] : "NO NAME";
                    tempNationalObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);
                    listtemp.Add(tempNationalObj);

                    //tempNationalObj = listtemp.Find(x => x.IdentityCode.Equals(Convert.ToString(itemRow[0])));
                    //tempNationalObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);

                    //listDataseries.Add(tempNationalObj);
                }
            }


            //% for specific schoolname
            string query = " Select NationalIdentity, (Count(NationalIdentity)* 100 /";
            query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            query += " From test_3 where Name in ('" + mSchoolname + " ') Group By NationalIdentity ";

            listResult = this.rpGeneric.FindByNativeSQL(query);

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    //tempNationalObj = new NationalityObj();
                    //tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
                    //tempNationalObj.IdentityName = GetDicNational().ContainsKey(tempNationalObj.IdentityCode) ? GetDicNational()[tempNationalObj.IdentityCode] : "NO NAME";
                    //tempNationalObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);
                    //listtemp.Add(tempNationalObj);
                    tempNationalObj = listtemp.Find(x => x.IdentityCode.Equals(Convert.ToString(itemRow[0])));
                    tempNationalObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);

                    listDataseries.Add(tempNationalObj);
 
                }
            }


            return listDataseries;
        }

        private Dictionary<string, string> GetDicNational()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "Scottish");
            dicNational.Add("02", "English");
            dicNational.Add("03", "Northern Irish");
            dicNational.Add("04", "Welsh");
            dicNational.Add("05", "British");
            dicNational.Add("99", "Other");
            dicNational.Add("10", "Not Disclosed");
            dicNational.Add("98", "Not Known");
            return dicNational;
        }

        [HttpPost]
        public JsonResult GetChartDataNationality(string[] arrParameterFilter)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[arrParameterFilter.Length];

                var listNationalData = Session["SessionListNationalityData"] as List<NationalityObj>;
                if (listNationalData != null)
                {
                    var listNationalFilter = listNationalData.Where(x => arrParameterFilter.Contains(x.IdentityCode)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "test",
                        ChartCategories = listNationalFilter.Select(x => x.IdentityName).ToArray(),
                        ChartSeries = ProcessChartDataEthnic(listNationalFilter)
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

        private List<object> ProcessChartDataEthnic(List<NationalityObj> listNationalFilter)
        {
            var listChartData = new List<object>();

            listChartData.Add(new { name = "Data 1", data = listNationalFilter.Select(x => x.PercentageAllSchool).ToArray() });
            listChartData.Add(new { name = "Data 2", data = listNationalFilter.Select(x => x.PercentageInSchool).ToArray() });

            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            var dataStream = GetWorkbookDataStream(GetData());
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "export.xlsx");
        }

        private List<NationalityObj> GetData()
        {
            // simulate datatable
            var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;
            //var dtResult = new DataTable();
            //dtResult.Columns.Add("EthnicBackground", typeof(string));
            //dtResult.Columns.Add("Drug", typeof(string));
            //dtResult.Columns.Add("Patient", typeof(string));
            //dtResult.Columns.Add("Date", typeof(DateTime));

            //// add row
            //dtResult.Rows.Add(25, "Indocin", "David", DateTime.Now);
            //dtResult.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            //dtResult.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            //dtResult.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            //dtResult.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);

            return listNationalityData;
        }

        private MemoryStream GetWorkbookDataStream(List<NationalityObj> dtResult)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = "Nationality"; // use cell address in range
            worksheet.Cell(2, 1).InsertTable(dtResult); // use row & column index
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}