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
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Collections;
using Rotativa;
using Rotativa.Options;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class EthnicBackgroundController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(EthnicBackgroundController));

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

            var sethnicityCriteria =  new List<string>();
            var setGenderCriteria = new List<string>();


            List<EthnicObj> ListEthnicData = new List<EthnicObj>();
            List<EthnicObj> temp = new List<EthnicObj>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmEthnicbackground.ListSchoolNameData = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW EthnicBackground FROM test_3 group by EthnicBackground");

            fooList = listResult.OfType<string>().ToList();

            vmEthnicbackground.ListEthnicCode = fooList;
            vmEthnicbackground.DicEthnicBG = GetDicEhtnicBG();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Gender FROM test_3 group by Gender");

            fooList = listResult.OfType<string>().ToList();

            vmEthnicbackground.ListGenderCode = fooList;
            vmEthnicbackground.DicGender = GetDicGender();

            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmEthnicbackground.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmEthnicbackground.IsShowCriteria = false;
                    // set default criteria
                    vmEthnicbackground.ListSelectedGender = new List<string>(new string[] { "Total" });
                    Session["ListSelectedGender"] = vmEthnicbackground.ListSelectedGender;
                    Session["sSchoolName"] = sSchoolName;
                }

            }
            else // post method
            {
                vmEthnicbackground.IsShowCriteria = true;
                sSchoolName = Request["selectSchoolname"];
                Session["sSchoolName"] = sSchoolName;
                if (Request["ethnicity"] != null){
                    sethnicityCriteria = Request["ethnicity"].Split(',').ToList();
                }
                else
                {
                    sethnicityCriteria = null;
                }                    
                if (Request["gender"] != null){
                    vmEthnicbackground.ListSelectedGender = Request["gender"].Split(',').ToList();
                }                    
                else
                {
                    vmEthnicbackground.ListSelectedGender = new List<string>(new string[] { "Total" });
                }
                
                Session["ListSelectedGender"] = vmEthnicbackground.ListSelectedGender;
                // get parameter from Request object
            }

            vmEthnicbackground.DicGenderWithSelected = GetDicGenderWithSelected(vmEthnicbackground.ListSelectedGender);
           
            // process data
            if (sSchoolName != null)
            {
                vmEthnicbackground.selectedschoolname = sSchoolName;
                ListEthnicData = GetEthnicityDatabySchoolname(rpGeneric, sSchoolName);
                if (sethnicityCriteria == null)
                {
                    vmEthnicbackground.ListEthnicData = null;
                }
                else if (sethnicityCriteria.Count != 0 && sethnicityCriteria != null)
                {
                    vmEthnicbackground.ListEthnicData = ListEthnicData.Where(x => sethnicityCriteria.Contains(x.EthinicCode)).ToList();
                }
                else {
                    vmEthnicbackground.ListEthnicData = ListEthnicData;
                }                
                Session["SessionListEthnicData"] = vmEthnicbackground.ListEthnicData;
            }

            return View("index", vmEthnicbackground);
            // export pdf
            //return new ViewAsPdf("index", vmEthnicbackground)
            //{
            //    FileName = "testexport.pdf",
            //    PageOrientation = Orientation.Landscape,
            //    PageMargins = new Margins(0, 0, 0, 0)
            //};

        }

        //public List<EthnicObj> GetEthnicityDatabySchoolname(string mSchoolname)
        //{
        //    Console.Write("GetEthnicityData ==> ");

        //    var singlelistChartData = new List<ChartData>();
        //    List<EthnicObj> listDataseries = new List<EthnicObj>();
        //    List<EthnicObj> listtemp = new List<EthnicObj>();
        //    EthnicObj tempEthnicObj = new EthnicObj();

        //    //% for All school
        //    var listResult = this.rpGeneric.FindByNativeSQL("Select EthnicBackground, (Count(EthnicBackground)* 100 / (Select Count(*) From test_3))  From test_3  Group By EthnicBackground ");
        //    if (listResult != null)
        //    {
        //        foreach (var itemRow in listResult)
        //        {
        //            tempEthnicObj = new EthnicObj();
        //            tempEthnicObj.EthinicCode = Convert.ToString(itemRow[0]);
        //            tempEthnicObj.EthinicName = GetDicEhtnicBG().ContainsKey(tempEthnicObj.EthinicCode) ? GetDicEhtnicBG()[tempEthnicObj.EthinicCode] : "NO NAME";
        //            tempEthnicObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);
        //            listtemp.Add(tempEthnicObj);  
        //        }
        //    }


        //    //% for specific schoolname
        //    string query = " Select EthnicBackground, (Count(EthnicBackground)* 100 /";
        //    query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
        //    query += " From test_3 where Name in ('" + mSchoolname + " ') Group By EthnicBackground ";

        //    listResult = this.rpGeneric.FindByNativeSQL(query);
        //    if (listResult != null)
        //    {
        //        foreach (var itemRow in listResult)
        //        {
        //            tempEthnicObj = listtemp.Find(x => x.EthinicCode.Equals(Convert.ToString(itemRow[0])));
        //            tempEthnicObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);

        //            listDataseries.Add(tempEthnicObj);
 
        //        }
        //    }
  

        //    return listDataseries;
        //}

        // private Dictionary<string, string> GetDicEhtnicBG()
        //{
        //    var dicNational = new Dictionary<string, string>();
        //    dicNational.Add("01", "White – Scottish");
        //    dicNational.Add("02", "African – African / Scottish / British");
        //    dicNational.Add("03", "Caribbean or Black – Caribbean / British / Scottish");
        //    dicNational.Add("05", "Asian – Indian/British/Scottish");
        //    dicNational.Add("06", "Asian – Pakistani / British / Scottish");
        //    dicNational.Add("07", "Asian –Bangladeshi / British / Scottish");
        //    dicNational.Add("08", "Asian – Chinese / British / Scottish");
        //    dicNational.Add("09", "White – Other");
        //    dicNational.Add("10", "Not Disclosed");
        //    dicNational.Add("12", "Mixed or multiple ethnic groups");
        //    dicNational.Add("17", "Asian – Other");
        //    dicNational.Add("19", "White – Gypsy/Traveller");
        //    dicNational.Add("21", "White – Other British");
        //    dicNational.Add("22", "White – Irish");
        //    dicNational.Add("23", "White – Polish");
        //    dicNational.Add("24", "Caribbean or Black – Other");
        //    dicNational.Add("25", "African – Other");
        //    dicNational.Add("27", "Other – Arab");
        //    dicNational.Add("98", "Not Known");
        //    dicNational.Add("99", "Other – Other");
        //    return dicNational;
        //}

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

            var ListSelectedGender = Session["ListSelectedGender"] as List<string>;
            var schoolname = Session["sSchoolName"];

            foreach (var itemGender in ListSelectedGender) {
                if (itemGender.Equals("F"))
                {
                    listChartData.Add(new { name = "FemaleAllSchool", data = listEthnicFilter.Select(x => x.PercentageFemaleAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname+" Female", data = listEthnicFilter.Select(x => x.PercentageFemaleInSchool).ToArray() });
                }                

                if (itemGender.Equals("M"))
                {
                    listChartData.Add(new { name = "MaleAllSchool", data = listEthnicFilter.Select(x => x.PercentageMaleAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname+" Male", data = listEthnicFilter.Select(x => x.PercentageMaleInSchool).ToArray() });
                }
                if (itemGender.Equals("Total"))
                {
                    listChartData.Add(new { name = "TotalAllSchool", data = listEthnicFilter.Select(x => x.PercentageAllSchool).ToArray() });
                    listChartData.Add(new { name = schoolname+" Total", data = listEthnicFilter.Select(x => x.PercentageInSchool).ToArray() });
                }                

            }


            return listChartData;
        }

        [HttpPost]
        public JsonResult GetChartDataEthnic2(string[] arrParameterFilter)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[arrParameterFilter.Length];

                var listEthnicData = Session["SessionListEthnicData"] as List<EthnicObj>;
                var listEthnicData2 = Session["SessionListEthnicData2"] as List<EthnicObj>;

                if (listEthnicData != null || listEthnicData2 != null)
                {
                    var listEthnicFilter = listEthnicData.Where(x => arrParameterFilter.Contains(x.EthinicCode)).ToList();
                    var listEthnicFilter2 = listEthnicData2.Where(x => arrParameterFilter.Contains(x.EthinicCode)).ToList();

                    if (listEthnicFilter != null)
                    {

                        Categories = listEthnicFilter.Select(x => x.EthinicName).ToArray();
                    }
                    else {
                        Categories = listEthnicFilter2.Select(x => x.EthinicName).ToArray();
                    
                    }



                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "test",
                        ChartCategories = listEthnicFilter.Select(x => x.EthinicName).ToArray(),
                        ChartSeries = ProcessChartDataEthnic(listEthnicFilter, listEthnicFilter2)
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

        private List<object> ProcessChartDataEthnic(List<EthnicObj> listEthnicFilter, List<EthnicObj> listEthnicFilter2)
        {
            var listChartData = new List<object>();

            var ListSelectedGender = Session["ListSelectedGender"] as List<string>;
            string sSchoolName = Session["sSchoolName"] as string;
            string sSchoolName2 = Session["sSchoolName2"] as string;            

            foreach (var itemGender in ListSelectedGender)
            {
                if (itemGender.Equals("F"))
                {
                    if (sSchoolName.Equals(sSchoolName2))
                    {
                        listChartData.Add(new { name = sSchoolName + " Female", data = listEthnicFilter.Select(x => x.PercentageFemaleInSchool).ToArray() });
                    }else{
                    
                        if (sSchoolName != null && !sSchoolName.Equals(""))
                        {
                            listChartData.Add(new { name = sSchoolName + " Female", data = listEthnicFilter.Select(x => x.PercentageFemaleInSchool).ToArray() });
                        }
                        if (sSchoolName2 != null && !sSchoolName2.Equals(""))
                        {
                            listChartData.Add(new { name = sSchoolName2 + " Female", data = listEthnicFilter2.Select(x => x.PercentageFemaleInSchool).ToArray() });
                        }
                    }

                    if (sSchoolName != null && !sSchoolName.Equals(""))
                    {

                        listChartData.Add(new { name = "FemaleAllSchool", data = listEthnicFilter.Select(x => x.PercentageFemaleAllSchool).ToArray() });
                    }
                    else
                    {
                        listChartData.Add(new { name = "FemaleAllSchool", data = listEthnicFilter2.Select(x => x.PercentageFemaleAllSchool).ToArray() });

                    }
                    
                }

                if (itemGender.Equals("M"))
                {
                    if (sSchoolName.Equals(sSchoolName2))
                    {
                        listChartData.Add(new { name = sSchoolName + " Male", data = listEthnicFilter.Select(x => x.PercentageMaleInSchool).ToArray() });
                    }
                    else
                    {

                        if (sSchoolName != null && !sSchoolName.Equals(""))
                        {
                            listChartData.Add(new { name = sSchoolName + " Male", data = listEthnicFilter.Select(x => x.PercentageMaleInSchool).ToArray() });
                        }
                        if (sSchoolName2 != null && !sSchoolName2.Equals(""))
                        {
                            listChartData.Add(new { name = sSchoolName2 + " Male", data = listEthnicFilter2.Select(x => x.PercentageMaleInSchool).ToArray() });
                        }
                    }

                    if (sSchoolName != null && !sSchoolName.Equals(""))
                    {

                        listChartData.Add(new { name = "MaleAllSchool", data = listEthnicFilter.Select(x => x.PercentageMaleAllSchool).ToArray() });
                    }
                    else
                    {
                        listChartData.Add(new { name = "MaleAllSchool", data = listEthnicFilter2.Select(x => x.PercentageMaleAllSchool).ToArray() });

                    }
                    //listChartData.Add(new { name = "MaleAllSchool", data = listEthnicFilter.Select(x => x.PercentageMaleAllSchool).ToArray() });
                    //listChartData.Add(new { name = sSchoolName + " Male", data = listEthnicFilter.Select(x => x.PercentageMaleInSchool).ToArray() });
                }
                if (itemGender.Equals("Total"))
                {
                    if (sSchoolName.Equals(sSchoolName2))
                    {
                        listChartData.Add(new { name = sSchoolName + " Total", data = listEthnicFilter.Select(x => x.PercentageInSchool).ToArray() });
                    }
                    else
                    {

                        if (sSchoolName != null && !sSchoolName.Equals(""))
                        {
                            listChartData.Add(new { name = sSchoolName + " Total", data = listEthnicFilter.Select(x => x.PercentageInSchool).ToArray() });
                        }
                        if (sSchoolName2 != null && !sSchoolName2.Equals(""))
                        {
                            listChartData.Add(new { name = sSchoolName2 + " Total", data = listEthnicFilter2.Select(x => x.PercentageInSchool).ToArray() });
                        }
                    }

                    if (sSchoolName != null && !sSchoolName.Equals(""))
                    {

                        listChartData.Add(new { name = "TotalAllSchool", data = listEthnicFilter.Select(x => x.PercentageAllSchool).ToArray() });
                    }
                    else
                    {
                        listChartData.Add(new { name = "TotalAllSchool", data = listEthnicFilter2.Select(x => x.PercentageAllSchool).ToArray() });

                    }
                    //listChartData.Add(new { name = "TotalAllSchool", data = listEthnicFilter.Select(x => x.PercentageAllSchool).ToArray() });
                    //listChartData.Add(new { name = sSchoolName + " Total", data = listEthnicFilter.Select(x => x.PercentageInSchool).ToArray() });
                }

            }


            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            var listEthnicData = Session["SessionListEthnicData"] as List<EthnicObj>;
            string schoolname = Session["sSchoolName"].ToString();

            var dataStream = GetWorkbookDataStream(GetData(), schoolname);
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "export.xlsx");
        }

        private List<EthnicObj> GetData()
        {
            // simulate datatable
            var listEthnicData = Session["SessionListEthnicData"] as List<EthnicObj>;
            var listEthnicData2 = Session["SessionListEthnicData2"] as List<EthnicObj>;

            var dtResult = new DataTable();
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

            return listEthnicData;
        }

        private MemoryStream GetWorkbookDataStream(List<EthnicObj> dtResult, string schoolname)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = schoolname; // use cell address in range
            worksheet.Cell("A2").Value = "Ethnicbackground"; // use cell address in range
            worksheet.Cell(3, 1).InsertTable(dtResult); // use row & column index
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public ActionResult Index2()
        {
            var vmEthnicBg2 = new EthnicBgViewModel2();
            return View("index2", vmEthnicBg2);
        }


        public ActionResult Compareable()
        {
            var vmEthnicbackground2 = new EthnicBgViewModel2();

            var schoolname = new List<string>();

            var sethnicityCriteria = new List<string>();
            var setGenderCriteria = new List<string>();


            List<EthnicObj> ListEthnicData = new List<EthnicObj>();
            List<EthnicObj> ListEthnicData2 = new List<EthnicObj>();
            List<EthnicObj> temp = new List<EthnicObj>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmEthnicbackground2.ListSchoolNameData = fooList;
            vmEthnicbackground2.ListSchoolNameData2 = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW EthnicBackground FROM test_3 group by EthnicBackground");

            fooList = listResult.OfType<string>().ToList();

            vmEthnicbackground2.ListEthnicCode = fooList;
            vmEthnicbackground2.DicEthnicBG = GetDicEhtnicBG();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Gender FROM test_3 group by Gender");

            fooList = listResult.OfType<string>().ToList();

            vmEthnicbackground2.ListGenderCode = fooList;
            vmEthnicbackground2.DicGender = GetDicGender();


            var sSchoolName1 = Request["selectedschoolname"];

            var sSchoolName2 = Request["selectedschoolname2"];

            if (sSchoolName1 == null && sSchoolName2 == null)
            {
                sSchoolName1 = vmEthnicbackground2.ListSchoolNameData[0]; //set default schoolname
                sSchoolName2 = vmEthnicbackground2.ListSchoolNameData2[1]; //set default schoolname
            }

            vmEthnicbackground2.selectedschoolname = sSchoolName1;
            vmEthnicbackground2.selectedschoolname2 = sSchoolName2;

            Session["sSchoolName"] = vmEthnicbackground2.selectedschoolname;
            Session["sSchoolName2"] = vmEthnicbackground2.selectedschoolname2;


                if (Request["ethnicity"] != null)
                {
                    sethnicityCriteria = Request["ethnicity"].Split(',').ToList();
                }
                else
                {
                    sethnicityCriteria = null;
                }
                if (Request["gender"] != null)
                {
                    vmEthnicbackground2.ListSelectedGender = Request["gender"].Split(',').ToList();
                }
                else
                {
                    vmEthnicbackground2.ListSelectedGender = new List<string>(new string[] { "Total" });
                }

                vmEthnicbackground2.ListSelectedEthnicBg = sethnicityCriteria;
                Session["ListSelectedGender"] = vmEthnicbackground2.ListSelectedGender;
                // get parameter from Request object
        

            vmEthnicbackground2.DicGenderWithSelected = GetDicGenderWithSelected(vmEthnicbackground2.ListSelectedGender);

            //calculate number of selectedschool
            if (sSchoolName1.Equals("") && sSchoolName2.Equals("")) {
                vmEthnicbackground2.NoSelectedSchool = 0;
            }
            else if (sSchoolName1.Equals(sSchoolName2))
            {
                vmEthnicbackground2.NoSelectedSchool = 1;
            }
            else if (sSchoolName1.Equals("") || sSchoolName2.Equals(""))
            {
                vmEthnicbackground2.NoSelectedSchool = 1;
            }
            else {
                vmEthnicbackground2.NoSelectedSchool = 2;
            }

            // process data
            if (sSchoolName1.Equals("") && sSchoolName2.Equals(""))
            {
                vmEthnicbackground2.IsShowData = false;
            }else if (sSchoolName1 != null && sSchoolName2 != null)
            {
                ListEthnicData = GetEthnicityDatabySchoolname(rpGeneric, sSchoolName1);
                ListEthnicData2 = GetEthnicityDatabySchoolname(rpGeneric, sSchoolName2);
                if (sethnicityCriteria == null)
                {
                    vmEthnicbackground2.IsShowData = false;
                }
                else if (sethnicityCriteria.Count != 0 && sethnicityCriteria != null)
                {
                    vmEthnicbackground2.IsShowData = true;
                    vmEthnicbackground2.ListEthnicData = ListEthnicData.Where(x => sethnicityCriteria.Contains(x.EthinicCode)).ToList();
                    vmEthnicbackground2.ListEthnicData2 = ListEthnicData2.Where(x => sethnicityCriteria.Contains(x.EthinicCode)).ToList();
                }
                else
                {
                    vmEthnicbackground2.IsShowData = true;
                    vmEthnicbackground2.ListEthnicData = ListEthnicData;
                    vmEthnicbackground2.ListEthnicData2 = ListEthnicData2;
                }
                Session["SessionListEthnicData"] = vmEthnicbackground2.ListEthnicData;
                Session["SessionListEthnicData2"] = vmEthnicbackground2.ListEthnicData2;
            }             
                

            return View("index2", vmEthnicbackground2);
        }
    }
}