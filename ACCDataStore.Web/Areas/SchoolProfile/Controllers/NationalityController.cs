using ACCDataStore.Entity;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Nationality;
using ClosedXML.Excel;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class NationalityController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(NationalityController));

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
            var setGenderCriteria = new List<string>();

            List<NationalityObj> ListNationalData = new List<NationalityObj>();
            List<NationalityObj> temp = new List<NationalityObj>();


            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmNationality.ListSchoolNameData = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW NationalIdentity FROM test_3 group by NationalIdentity");

            fooList = listResult.OfType<string>().ToList();
            vmNationality.ListNationalityCode = fooList;
            vmNationality.DicNational = GetDicNational();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Gender FROM test_3 group by Gender");

            fooList = listResult.OfType<string>().ToList();
            fooList.Add("T");
            vmNationality.ListGenderCode = fooList;
            vmNationality.DicGender = GetDicGender();


            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmNationality.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmNationality.IsShowCriteria = false;
                    vmNationality.ListSelectedGender = vmNationality.ListGenderCode;
                    vmNationality.ListSelectedNationality = vmNationality.ListNationalityCode;
                    Session["ListSelectedGender"] = vmNationality.ListSelectedGender;
                    //Session["ListSelectedNationality"] = vmNationality.ListSelectedNationality;
                    Session["sSchoolName"] = sSchoolName;
                }

            }
            else // post method
            {
                vmNationality.IsShowCriteria = true;
                sSchoolName = Request["selectedschoolname"];
                vmNationality.selectedschoolname = sSchoolName;
                Session["sSchoolName"] = sSchoolName;

                if (Request["nationality"] != null)
                {
                   sNationalCriteria = Request["nationality"].Split(',').ToList();
                   vmNationality.ListSelectedNationality = sNationalCriteria;
                }
                else
                {
                    sNationalCriteria = null;
                }
                if (Request["gender"] != null)
                {
                    vmNationality.ListSelectedGender = Request["gender"].Split(',').ToList();
                }
                else
                {
                    vmNationality.ListSelectedGender = vmNationality.ListGenderCode;
                }               
                
                Session["ListSelectedGender"] = vmNationality.ListSelectedGender;
                //Session["ListSelectedNationality"] = vmNationality.ListSelectedNationality;
                // get parameter from Request object
            }

            vmNationality.DicGenderWithSelected = GetDicGenderWithSelected(vmNationality.ListSelectedGender);

            // process data
            if (sSchoolName == null || sSchoolName.Equals(""))
            {
                vmNationality.IsShowData = false;
            }
            else if (sSchoolName != null)
            {
                vmNationality.selectedschoolname = sSchoolName;
                ListNationalData = GetNationalityDatabySchoolname(rpGeneric,sSchoolName);

                if (sNationalCriteria == null)
                {
                    vmNationality.IsShowData = false;
                    vmNationality.ListNationalityData = null;
                }
                else if (sNationalCriteria.Count != 0 && sNationalCriteria != null)
                {
                    vmNationality.IsShowData = true;
                    vmNationality.ListNationalityData = ListNationalData.Where(x => sNationalCriteria.Contains(x.IdentityCode)).ToList();
                }
                else
                {
                    vmNationality.IsShowData = true;
                    vmNationality.ListNationalityData = ListNationalData;
                }                
                Session["SessionListNationalityData"] = vmNationality.ListNationalityData;
            }
            return View("Index", vmNationality);
        }

        //public List<NationalityObj> GetNationalityDatabySchoolname(string mSchoolname)
        //{
        //    Console.Write("GetNationalityData ==> ");

        //    var singlelistChartData = new List<ChartData>();
        //    List<NationalityObj> listDataseries = new List<NationalityObj>();
        //    List<NationalityObj> listtemp = new List<NationalityObj>();
        //    NationalityObj tempNationalObj = new NationalityObj();
            

        //    //% for All school
        //    var listResult = this.rpGeneric.FindByNativeSQL("Select NationalIdentity, (Count(NationalIdentity)* 100 / (Select Count(*) From test_3))  From test_3  Group By NationalIdentity ");
        //    if (listResult != null)
        //    {
        //        foreach (var itemRow in listResult)
        //        {
        //            tempNationalObj = new NationalityObj();
        //            tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
        //            tempNationalObj.IdentityName = GetDicNational().ContainsKey(tempNationalObj.IdentityCode) ? GetDicNational()[tempNationalObj.IdentityCode] : "NO NAME";
        //            tempNationalObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);
        //            listtemp.Add(tempNationalObj);

        //            //tempNationalObj = listtemp.Find(x => x.IdentityCode.Equals(Convert.ToString(itemRow[0])));
        //            //tempNationalObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);

        //            //listDataseries.Add(tempNationalObj);
        //        }
        //    }


        //    //% for specific schoolname
        //    string query = " Select NationalIdentity, (Count(NationalIdentity)* 100 /";
        //    query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
        //    query += " From test_3 where Name in ('" + mSchoolname + " ') Group By NationalIdentity ";

        //    listResult = this.rpGeneric.FindByNativeSQL(query);

        //    if (listResult != null)
        //    {
        //        foreach (var itemRow in listResult)
        //        {
        //            //tempNationalObj = new NationalityObj();
        //            //tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
        //            //tempNationalObj.IdentityName = GetDicNational().ContainsKey(tempNationalObj.IdentityCode) ? GetDicNational()[tempNationalObj.IdentityCode] : "NO NAME";
        //            //tempNationalObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);
        //            //listtemp.Add(tempNationalObj);
        //            tempNationalObj = listtemp.Find(x => x.IdentityCode.Equals(Convert.ToString(itemRow[0])));
        //            tempNationalObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);

        //            listDataseries.Add(tempNationalObj);
 
        //        }
        //    }


        //    return listDataseries;
        //}

        //private Dictionary<string, string> GetDicNational()
        //{
        //    var dicNational = new Dictionary<string, string>();
        //    dicNational.Add("01", "Scottish");
        //    dicNational.Add("02", "English");
        //    dicNational.Add("03", "Northern Irish");
        //    dicNational.Add("04", "Welsh");
        //    dicNational.Add("05", "British");
        //    dicNational.Add("99", "Other");
        //    dicNational.Add("10", "Not Disclosed");
        //    dicNational.Add("98", "Not Known");
        //    return dicNational;
        //}

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
            var ListSelectedGender = Session["ListSelectedGender"] as List<string>;
            var schoolname = Session["sSchoolName"];

            foreach (var itemGender in ListSelectedGender)
            {
                if (itemGender.Equals("F"))
                {
                    listChartData.Add(new { name = "FemaleAllSchool", data = listNationalFilter.Select(x => x.PercentageFemaleAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname+" Female", data = listNationalFilter.Select(x => x.PercentageFemaleInSchool).ToArray() });
                }

                if (itemGender.Equals("M"))
                {
                    listChartData.Add(new { name = "MaleAllSchool", data = listNationalFilter.Select(x => x.PercentageMaleAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname+" Male", data = listNationalFilter.Select(x => x.PercentageMaleInSchool).ToArray() });
                }
                if (itemGender.Equals("T"))
                {
                    listChartData.Add(new { name = "TotalAllSchool", data = listNationalFilter.Select(x => x.PercentageAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname+" Total", data = listNationalFilter.Select(x => x.PercentageInSchool).ToArray() });
                }

            }
            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;
            string schoolname = Session["sSchoolName"].ToString();
            var dataStream = GetWorkbookDataStream(GetData());
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "export.xlsx");
        }

        private DataTable GetData()
        {
            // simulate datatable
            var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;
           // var listEthnicData2 = Session["SessionListEthnicData2"] as List<EthnicObj>;
            string sSchoolName = Session["sSchoolName"] as string;
            //string sSchoolName2 = Session["sSchoolName2"] as string;

            //var transformObject = new Object();

            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("IdentityCode", typeof(string));
            dtResult.Columns.Add("Nationality", typeof(string));
            dtResult.Columns.Add("Female in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Female in All Primary school", typeof(double));
            dtResult.Columns.Add("Male in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Male in All  Primary school ", typeof(double));
            dtResult.Columns.Add("Total in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Total in All Primary school", typeof(double));

            var transformObject = new
                {
                    Col1 = listNationalityData.Select(x => x.IdentityCode),
                    Col2 = listNationalityData.Select(x => x.IdentityName),
                    Col3 = listNationalityData.Select(x => x.PercentageFemaleInSchool),
                    Col4 = listNationalityData.Select(x => x.PercentageFemaleAllSchool),
                    Col5 = listNationalityData.Select(x => x.PercentageMaleInSchool),
                    Col6 = listNationalityData.Select(x => x.PercentageMaleAllSchool),
                    Col7 = listNationalityData.Select(x => x.PercentageInSchool),
                    Col8 = listNationalityData.Select(x => x.PercentageAllSchool),
                };

            for (var i = 0; i < listNationalityData.Count; i++)
                {
                    dtResult.Rows.Add(
                        transformObject.Col1.ToList()[i],
                        transformObject.Col2.ToList()[i],
                        transformObject.Col3.ToList()[i],
                        transformObject.Col4.ToList()[i],
                        transformObject.Col5.ToList()[i],
                        transformObject.Col6.ToList()[i],
                        transformObject.Col7.ToList()[i],
                        transformObject.Col8.ToList()[i]
                        );
                }
            return dtResult;
        }

        private MemoryStream GetWorkbookDataStream(DataTable dtResult)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = "Nationality"; // use cell address in range
            //worksheet.Cell("A2").Value = "Nationality"; // use cell address in range
            worksheet.Cell("A2").Value = "% of pupils in each ethnic group"; 
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