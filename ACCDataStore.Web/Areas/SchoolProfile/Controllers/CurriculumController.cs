using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Curriculum;
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
    public class CurriculumController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(CurriculumController));

        private readonly IGenericRepository rpGeneric;

        public CurriculumController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        // GET: SchoolProfile/Curriculum
        public ActionResult Index(string sSchoolName)
        {
            //page counter
            var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            eGeneralSettings.NationalitypgCounter++;
            TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

            var vmCurriculum = new CurriculumViewModel();

            //var schoolname = new List<string>();
            var sSubjectCriteria = new List<string>();
            var setGenderCriteria = new List<string>();

            List<CurriculumObj> ListCurriculumData = new List<CurriculumObj>();
            List<CurriculumObj> temp = new List<CurriculumObj>();


            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmCurriculum.ListSchoolNameData = fooList;

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM test_3 group by StudentStage");

            fooList = listResult.OfType<string>().ToList();
            vmCurriculum.ListStageCode = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Gender FROM test_3 group by Gender");

            fooList = listResult.OfType<string>().ToList();
            fooList.Add("T");
            vmCurriculum.ListGenderCode = fooList;
            vmCurriculum.DicGender = GetDicGender();

            fooList = new List<string>();

            fooList.Add("Literacy Primary");
            fooList.Add("Reading");
            fooList.Add("Writing");
            fooList.Add("L and T");
            fooList.Add("Numeracy Primary");
            fooList.Add("NMM");
            fooList.Add("SPM");
            fooList.Add("IH");
            vmCurriculum.ListSubjects = fooList;

            fooList = new List<string>();
            fooList.Add("Early");
            fooList.Add("Early Developing");
            fooList.Add("Early Consolidating");
            fooList.Add("Early Secure");
            fooList.Add("First Developing");
            fooList.Add("First Consolidating");
            fooList.Add("First Secure");
            fooList.Add("Second Developing");
            fooList.Add("Second Consolidating");
            fooList.Add("Second Secure");
            fooList.Add("Third Developing");
            fooList.Add("Third Consolidating");
            fooList.Add("Third Secure");
            fooList.Add("blank");
            fooList.Add("Grand Total");
            vmCurriculum.ListSkills = fooList;

            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmCurriculum.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmCurriculum.IsShowCriteria = false;
                    vmCurriculum.ListSelectedGender = vmCurriculum.ListGenderCode;
                    vmCurriculum.ListSelectedSubject = vmCurriculum.ListSubjects;
                    Session["ListSelectedGender"] = vmCurriculum.ListSelectedGender;
                    //Session["ListSelectedNationality"] = vmNationality.ListSelectedNationality;
                    Session["sSchoolName"] = sSchoolName;
                }

            }
            else // post method
            {
                vmCurriculum.IsShowCriteria = true;
                sSchoolName = Request["selectedschoolname"];
                vmCurriculum.selectedschoolname = sSchoolName;
                Session["sSchoolName"] = sSchoolName;

                if (Request["subject"] != null)
                {
                    sSubjectCriteria = Request["subject"].Split(',').ToList();
                    vmCurriculum.ListSelectedSubject = sSubjectCriteria;
                }
                else
                {
                    sSubjectCriteria = null;
                }

                if (Request["gender"] != null)
                {
                    vmCurriculum.ListSelectedGender = Request["gender"].Split(',').ToList();
                }
                else
                {
                    vmCurriculum.ListSelectedGender = vmCurriculum.ListGenderCode;
                }

                Session["ListSelectedGender"] = vmCurriculum.ListSelectedGender;
                //Session["ListSelectedNationality"] = vmNationality.ListSelectedNationality;
                // get parameter from Request object

            }

            vmCurriculum.DicGenderWithSelected = GetDicGenderWithSelected(vmCurriculum.ListSelectedGender);

            // process data
            if (sSchoolName == null || sSchoolName.Equals(""))
            {
                vmCurriculum.IsShowData = false;
            }
            else if (sSchoolName != null)
            {
                vmCurriculum.selectedschoolname = sSchoolName;
                if (sSubjectCriteria == null)
                {
                    vmCurriculum.IsShowData = false;
                    vmCurriculum.ListLiteracydata = null;
                    vmCurriculum.ListNMMdata = null;
                    vmCurriculum.ListSPMdata = null;
                    vmCurriculum.ListIHdata = null;
                    vmCurriculum.ListLiteracydata = null;
                    vmCurriculum.ListReadingdata = null;
                    vmCurriculum.ListWritingdata = null;
                    vmCurriculum.ListLandTdata = null;
                    vmCurriculum.ListNumeracydata = null;
                }
                //else if (sSubjectCriteria.Count != 0 && sSubjectCriteria != null)
                //{
                //    vmCurriculum.IsShowData = true;
                //    //vmCurriculum.ListNationalityData = ListCurriculumData.Where(x => sNationalCriteria.Contains(x.IdentityCode)).ToList();
                //    vmCurriculum.ListLiteracydata = ListCurriculumData;
                //}
                else
                {
                    foreach (var subject in sSubjectCriteria) {
                        if (subject.Equals("Literacy Primary")) {                            
                            vmCurriculum.ListLiteracydata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "Literacy_Primary");
                            Session["SessionListLiteracydata"] = vmCurriculum.ListLiteracydata;
                        }
                        else if (subject.Equals("Reading"))
                        {
                            vmCurriculum.ListReadingdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "Reading");
                            Session["SessionListReadingdata"] = vmCurriculum.ListReadingdata;
                        }
                        else if (subject.Equals("Writing"))
                        {
                            vmCurriculum.ListWritingdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "Writing");
                            Session["SessionListWritingdata"] = vmCurriculum.ListWritingdata;
                        }
                        else if (subject.Equals("L and T"))
                        {
                            vmCurriculum.ListLandTdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "L_and_T");
                            Session["SessionListLandTdata"] = vmCurriculum.ListLandTdata;
                        }
                        else if (subject.Equals("Numeracy Primary"))
                        {
                            vmCurriculum.ListNumeracydata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "Numeracy_Primary");
                            Session["SessionListNumeracydata"] = vmCurriculum.ListNumeracydata;
                        }
                        else if (subject.Equals("NMM"))
                        {
                            vmCurriculum.ListNMMdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "NMM");
                            Session["SessionListNMMdata"] = vmCurriculum.ListNMMdata;
                        }
                        else if (subject.Equals("SPM"))
                        {
                            vmCurriculum.ListSPMdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "SPM");
                            Session["SessionListSPMdata"] = vmCurriculum.ListSPMdata;
                        }
                        else if (subject.Equals("IH"))
                        {
                            vmCurriculum.ListIHdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "IH");
                            Session["SessionListIHdata"] = vmCurriculum.ListIHdata;
                        }
                    }
                    vmCurriculum.IsShowData = true;
                }               
            }
            return View("Index", vmCurriculum);
        }

        [HttpPost]
        public JsonResult GetChartDataCurriculum(string dataname, string[] indexDataitem)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[indexDataitem.Length];
                var listCurriculumData = new List<CurriculumObj>();

                if (dataname.Equals("ListLiteracydata"))
                {
                    listCurriculumData = Session["SessionListLiteracydata"] as List<CurriculumObj>;
                }
                else if (dataname.Equals("ListReadingdata"))
                {
                    listCurriculumData = Session["SessionListReadingdata"] as List<CurriculumObj>;                   
                }
                else if (dataname.Equals("ListWritingdata"))
                {
                    listCurriculumData = Session["SessionListWritingdata"] as List<CurriculumObj>;   
                }
                else if (dataname.Equals("ListLandTdata"))
                {
                    listCurriculumData = Session["SessionListLandTdata"] as List<CurriculumObj>;   
                }
                else if (dataname.Equals("ListNumeracydata"))
                {
                    listCurriculumData = Session["SessionListNumeracydata"] as List<CurriculumObj>;   
                }
                else if (dataname.Equals("ListNMMdata"))
                {
                    listCurriculumData = Session["SessionListNMMdata"] as List<CurriculumObj>;   
                }
                else if (dataname.Equals("ListSPMdata"))
                {
                    listCurriculumData = Session["SessionListSPMdata"] as List<CurriculumObj>;   
                }
                else if (dataname.Equals("ListIHdata"))
                {
                    listCurriculumData = Session["SessionListIHdata"] as List<CurriculumObj>;   
                }

                //var listNationalData = Session["SessionListNationalityData"] as List<NationalityObj>;
                if (listCurriculumData != null)
                {
                    var listCurriculumFilter = listCurriculumData.Where(x => indexDataitem.Contains(x.stage)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "test",
                        ChartCategories = listCurriculumFilter.Select(x => x.stage).ToArray(),
                        ChartSeries = ProcessChartDataCurriculum(listCurriculumFilter)
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

        private List<object> ProcessChartDataCurriculum(List<CurriculumObj> listCurriculumdata)
        {
            var listChartData = new List<object>();
            var ListSelectedGender = Session["ListSelectedGender"] as List<string>;
            var schoolname = Session["sSchoolName"];

            foreach (var itemGender in ListSelectedGender)
            {
                //if (itemGender.Equals("F"))
                //{
                //    listChartData.Add(new { name = schoolname + " Female", data = listCurriculumdata.Select(x => x.e).ToArray() });
                //    listChartData.Add(new { name = "Female All School", data = listCurriculumdata.Select(x => x.PercentageFemaleAllSchool).ToArray() });
                //}

                //if (itemGender.Equals("M"))
                //{
                //    listChartData.Add(new { name = schoolname + " Male", data = listCurriculumdata.Select(x => x.PercentageMaleInSchool).ToArray() });
                //    listChartData.Add(new { name = "Male All School", data = listCurriculumdata.Select(x => x.PercentageMaleAllSchool).ToArray() });
                //}
                //if (itemGender.Equals("T"))
                //{
                //    listChartData.Add(new { name = schoolname + " Total", data = listCurriculumdata.Select(x => x.PercentageInSchool).ToArray() });
                //    listChartData.Add(new { name = "Total All School", data = listCurriculumdata.Select(x => x.PercentageAllSchool).ToArray() });
                //}

            }
            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;
            //string schoolname = Session["sSchoolName"].ToString();
            var dataStream = GetWorkbookDataStream(GetData());
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NationalityExport.xlsx");
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
                Col1 = listNationalityData.Select(x => x.IdentityCode).ToList(),
                Col2 = listNationalityData.Select(x => x.IdentityName).ToList(),
                Col3 = listNationalityData.Select(x => x.PercentageFemaleInSchool).ToList(),
                Col4 = listNationalityData.Select(x => x.PercentageFemaleAllSchool).ToList(),
                Col5 = listNationalityData.Select(x => x.PercentageMaleInSchool).ToList(),
                Col6 = listNationalityData.Select(x => x.PercentageMaleAllSchool).ToList(),
                Col7 = listNationalityData.Select(x => x.PercentageInSchool).ToList(),
                Col8 = listNationalityData.Select(x => x.PercentageAllSchool).ToList(),
            };

            for (var i = 0; i < listNationalityData.Count; i++)
            {
                dtResult.Rows.Add(
                    transformObject.Col1[i],
                    transformObject.Col2[i],
                    transformObject.Col3[i],
                    transformObject.Col4[i],
                    transformObject.Col5[i],
                    transformObject.Col6[i],
                    transformObject.Col7[i],
                    transformObject.Col8[i]
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