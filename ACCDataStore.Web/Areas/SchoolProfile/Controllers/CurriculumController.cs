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

        protected Dictionary<string, string> GetDicSubject()
        {
            var dicSubject = new Dictionary<string, string>();
            dicSubject.Add("Literacy", "Literacy");
            dicSubject.Add("Reading", "Reading");
            dicSubject.Add("Writing", "Writing");
            dicSubject.Add("LandT", "Listening and Talking");
            dicSubject.Add("Numeracy", "Numeracy");
            dicSubject.Add("NMM", "Number, Money & Measure");
            dicSubject.Add("SPM", "Shape, Position & Movement");
            dicSubject.Add("IH", "Information Handling");
            return dicSubject;
        }

        // GET: SchoolProfile/Curriculum
        public ActionResult Index(string sSchoolName)
        {
            //page counter
            var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            eGeneralSettings.CurriculumpgCounter++;
            TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

            var vmCurriculum = new CurriculumViewModel();

            //var schoolname = new List<string>();
            var sSubjectCriteria = new List<string>();
            var setGenderCriteria = new List<string>();

            List<CurriculumObj> ListCurriculumData = new List<CurriculumObj>();
            List<CurriculumObj> temp = new List<CurriculumObj>();


            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t t2 on t1.SeedCode = t2.SeedCode ");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmCurriculum.ListSchoolNameData = fooList;

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM sch_Student_t group by StudentStage");

            fooList = listResult.OfType<string>().ToList();
            vmCurriculum.ListStageCode = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Gender FROM sch_Student_t group by Gender");

            fooList = listResult.OfType<string>().ToList();
            fooList.Add("T");
            vmCurriculum.ListGenderCode = fooList;
            vmCurriculum.DicGender = GetDicGender();

            fooList = new List<string>();

            fooList.Add("Literacy");
            fooList.Add("Reading");
            fooList.Add("Writing");
            fooList.Add("LandT");
            fooList.Add("Numeracy");
            fooList.Add("NMM");
            fooList.Add("SPM");
            fooList.Add("IH");
            vmCurriculum.ListSubjects = fooList;

            vmCurriculum.DicSubject = GetDicSubject();

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
                        if (subject.Equals("Literacy")) {                            
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
                        else if (subject.Equals("LandT"))
                        {
                            vmCurriculum.ListLandTdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "L_and_T");
                            Session["SessionListLandTdata"] = vmCurriculum.ListLandTdata;
                        }
                        else if (subject.Equals("Numeracy"))
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
                var schoolname = Session["sSchoolName"];
                string subjectname = "";

                if (dataname.Equals("ListLiteracydata"))
                {
                    listCurriculumData = Session["SessionListLiteracydata"] as List<CurriculumObj>;
                    subjectname = "Literacy";
                }
                else if (dataname.Equals("ListReadingdata"))
                {
                    listCurriculumData = Session["SessionListReadingdata"] as List<CurriculumObj>;
                    subjectname = "Reading";
                }
                else if (dataname.Equals("ListWritingdata"))
                {
                    listCurriculumData = Session["SessionListWritingdata"] as List<CurriculumObj>;
                    subjectname = "Writing";
                }
                else if (dataname.Equals("ListLandTdata"))
                {
                    listCurriculumData = Session["SessionListLandTdata"] as List<CurriculumObj>;
                    subjectname = "Listening and Talking";
                }
                else if (dataname.Equals("ListNumeracydata"))
                {
                    listCurriculumData = Session["SessionListNumeracydata"] as List<CurriculumObj>;
                    subjectname = "Numeracy";
                }
                else if (dataname.Equals("ListNMMdata"))
                {
                    listCurriculumData = Session["SessionListNMMdata"] as List<CurriculumObj>;
                    subjectname = "Number, Money & Measure";
                }
                else if (dataname.Equals("ListSPMdata"))
                {
                    listCurriculumData = Session["SessionListSPMdata"] as List<CurriculumObj>;
                    subjectname = "Shape, Position & Movement";
                }
                else if (dataname.Equals("ListIHdata"))
                {
                    listCurriculumData = Session["SessionListIHdata"] as List<CurriculumObj>;
                    subjectname = "Information Handling";
                }

                //var listNationalData = Session["SessionListNationalityData"] as List<NationalityObj>;
                if (listCurriculumData != null)
                {
                    var listCurriculumFilter = listCurriculumData.Where(x => indexDataitem.Contains(x.stage)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "Curriculum for Excellence - " + subjectname +" - "+ schoolname + " (% pupils)",
                        ChartCategories = indexDataitem,
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
            var temp = new List<CurriculumObj>();
                    temp = (from a in listCurriculumdata where a.gender.Equals("T") select a).ToList();
                    listChartData.Add(new { name = "Early", data = temp.Select(x => x.early).ToArray() });
                    listChartData.Add(new { name = "Early Developing", data = temp.Select(x => x.earlydeveloping).ToArray() });
                    listChartData.Add(new { name = "Early Consolidating", data = temp.Select(x => x.earlyconsolidating).ToArray() });
                    listChartData.Add(new { name = "Early Secure", data = temp.Select(x => x.earlysecure).ToArray() });
                    listChartData.Add(new { name = "First Developing", data = temp.Select(x => x.firstdeveloping).ToArray() });
                    listChartData.Add(new { name = "First Consolidating", data = temp.Select(x => x.firstconsolidating).ToArray() });
                    listChartData.Add(new { name = "First Secure", data = temp.Select(x => x.firstsecure).ToArray() });

                    listChartData.Add(new { name = "Second Developing", data = temp.Select(x => x.seconddeveloping).ToArray() });
                    listChartData.Add(new { name = "Second Consolidating", data = temp.Select(x => x.secondconsolidating).ToArray() });
                    listChartData.Add(new { name = "Second Secure", data = temp.Select(x => x.secondsecure).ToArray() });

                    listChartData.Add(new { name = "Third Developing", data = temp.Select(x => x.thirddeveloping).ToArray() });
                    listChartData.Add(new { name = "Third Consolidating", data = temp.Select(x => x.thirdconsolidating).ToArray() });
                    listChartData.Add(new { name = "Third Secure", data = temp.Select(x => x.thirdsecure).ToArray() });
                    listChartData.Add(new { name = "Blank", data = temp.Select(x => x.blank).ToArray() });
            return listChartData;
        }

        public ActionResult ExportExcel(string dataname)
        {
            try
            {
                var listCurriculumData = new List<CurriculumObj>();
                string subjectname = "";

                if (dataname.Equals("ListLiteracydata"))
                {
                    listCurriculumData = Session["SessionListLiteracydata"] as List<CurriculumObj>;
                    subjectname = "Literacy";

                }
                else if (dataname.Equals("ListReadingdata"))
                {
                    listCurriculumData = Session["SessionListReadingdata"] as List<CurriculumObj>;
                    subjectname = "Reading";
                }
                else if (dataname.Equals("ListWritingdata"))
                {
                    listCurriculumData = Session["SessionListWritingdata"] as List<CurriculumObj>;
                    subjectname = "Writing";
                }
                else if (dataname.Equals("ListLandTdata"))
                {
                    listCurriculumData = Session["SessionListLandTdata"] as List<CurriculumObj>;
                    subjectname = "Listening and Talking";
                }
                else if (dataname.Equals("ListNumeracydata"))
                {
                    listCurriculumData = Session["SessionListNumeracydata"] as List<CurriculumObj>;
                    subjectname = "Numeracy";
                }
                else if (dataname.Equals("ListNMMdata"))
                {
                    listCurriculumData = Session["SessionListNMMdata"] as List<CurriculumObj>;
                    subjectname = "Number, Money & Measure";
                }
                else if (dataname.Equals("ListSPMdata"))
                {
                    listCurriculumData = Session["SessionListSPMdata"] as List<CurriculumObj>;
                    subjectname = "Shape, Position & Movement";
                }
                else if (dataname.Equals("ListIHdata"))
                {
                    listCurriculumData = Session["SessionListIHdata"] as List<CurriculumObj>;
                    subjectname = "Information Handling";
                }

                var dataStream = GetWorkbookDataStream(GetData(listCurriculumData), subjectname);

                return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CurriculumExport.xlsx");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }


            
        }

        private DataTable GetData(List<CurriculumObj> listCurriculumdata)
        {
            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("Stage", typeof(string));
            dtResult.Columns.Add("Gender", typeof(string));            
            dtResult.Columns.Add("Early", typeof(double));
            dtResult.Columns.Add("Early Developing", typeof(double));
            dtResult.Columns.Add("Early Consolidating", typeof(double));
            dtResult.Columns.Add("Early Secure", typeof(double));
            dtResult.Columns.Add("First Developing", typeof(double));
            dtResult.Columns.Add("First Consolidating", typeof(double));
            dtResult.Columns.Add("First Secure", typeof(double));
            dtResult.Columns.Add("Second Developing", typeof(double));
            dtResult.Columns.Add("Second Consolidating", typeof(double));
            dtResult.Columns.Add("Second Secure", typeof(double));
            dtResult.Columns.Add("Third Developing", typeof(double));
            dtResult.Columns.Add("Third Consolidating", typeof(double));
            dtResult.Columns.Add("Third Secure", typeof(double));
            dtResult.Columns.Add("blank", typeof(double));

            var transformObject = new
            {
                Col1 = listCurriculumdata.Select(x => x.stage).ToList(),
                Col2 = listCurriculumdata.Select(x => x.gender).ToList(),
                Col3 = listCurriculumdata.Select(x => x.early).ToList(),
                Col4 = listCurriculumdata.Select(x => x.earlydeveloping).ToList(),
                Col5 = listCurriculumdata.Select(x => x.earlyconsolidating).ToList(),
                Col6 = listCurriculumdata.Select(x => x.earlysecure).ToList(),
                Col7 = listCurriculumdata.Select(x => x.firstdeveloping).ToList(),
                Col8 = listCurriculumdata.Select(x => x.firstconsolidating).ToList(),
                Col9 = listCurriculumdata.Select(x => x.firstsecure).ToList(),
                Col10 = listCurriculumdata.Select(x => x.seconddeveloping).ToList(),
                Col11 = listCurriculumdata.Select(x => x.secondconsolidating).ToList(),
                Col12 = listCurriculumdata.Select(x => x.secondsecure).ToList(),
                Col13 = listCurriculumdata.Select(x => x.thirddeveloping).ToList(),
                Col14 = listCurriculumdata.Select(x => x.thirdconsolidating).ToList(),
                Col15 = listCurriculumdata.Select(x => x.thirdsecure).ToList(),
                Col16 = listCurriculumdata.Select(x => x.blank).ToList()
            };

            for (var i = 0; i < listCurriculumdata.Count; i++)
            {
                dtResult.Rows.Add(
                    transformObject.Col1[i],
                    transformObject.Col2[i],
                    transformObject.Col3[i],
                    transformObject.Col4[i],
                    transformObject.Col5[i],
                    transformObject.Col6[i],
                    transformObject.Col7[i],
                    transformObject.Col8[i],
                    transformObject.Col9[i],
                    transformObject.Col10[i],
                    transformObject.Col11[i],
                    transformObject.Col12[i],
                    transformObject.Col13[i],
                    transformObject.Col14[i],
                    transformObject.Col15[i],
                    transformObject.Col16[i]
                    );
            }
            return dtResult;
        }

        private MemoryStream GetWorkbookDataStream(DataTable dtResult,string dataname)
        {
            string sSchoolName = Session["sSchoolName"] as string;

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = "Curriculum for Excellence - "+ sSchoolName; // use cell address in range
            //worksheet.Cell("A2").Value = "Nationality"; // use cell address in range
            worksheet.Cell("A2").Value = dataname;
            worksheet.Cell(3, 1).InsertTable(dtResult); // use row & column index
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public ActionResult MapData()
        {
            //var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;
            return View("MapIndex");
        }

        protected JsonResult ThrowJSONError(Exception ex)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
            return Json(new { Message = sErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByName(string keyvalue, string keyname)
        {
            try
            {
                object oChartData = new object();

                if (keyname.Equals("SchCode"))
                {
                    oChartData = new
                    {
                        dataTitle = GetSchNamebySchCode(int.Parse(keyvalue)),
                        dataSeries = GetdatabySchCode(int.Parse(keyvalue))
                    };

                }
                else if (keyname.Equals("ZoneCode"))
                {
                    oChartData = new
                    {
                        dataTitle = keyvalue,
                        dataSeries = GetdatabyZonecode(keyvalue)
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

        private string GetSchNamebySchCode(int pSchcode)
        {
            Console.Write("GetSchNamebySchCode ==> ");

            string SchoolName = "";
            //% for Specific Area like AB21
            //var listResult = rpGeneric.FindByNativeSQL("Select NationalIdentity,Gender, (Count(NationalIdentity)* 100 / (Select Count(*) From sch_Student_t_v2 where PostOut in (\"" + pPostcode + "\") ))  From sch_Student_t_v2 where PostOut in (\"" + pPostcode + "\")  Group By NationalIdentity, Gender ");
            var listResult = rpGeneric.FindSingleColumnByNativeSQL("Select Name From sch_PrimarySchool_t  where Seedcode =" + pSchcode);

            if (listResult.Any())
            {
                SchoolName = Convert.ToString(listResult[0]);

            }
            else
            {

                SchoolName = "No School data";

            }


            return SchoolName;

        }
        private List<CurriculumObj> GetdatabySchCode(int pSchcode)
        {
            Console.Write("GetdatabySchCode ==> ");

            List<CurriculumObj> listtemp = new List<CurriculumObj>();
            CurriculumObj tempNationalObj = new CurriculumObj();


            //% for Specific Area like AB21
            var listResult = rpGeneric.FindByNativeSQL("Select EthnicBackground,Gender, (Count(*)* 100 / (Select Count(*) From sch_Student_t_v2 where Seedcode =" + pSchcode + " )) From sch_Student_t_v2 where Seedcode =" + pSchcode + " Group By EthnicBackground, Gender ");

            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Nationalcode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Nationalcode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempNationalObj = new CurriculumObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.EthinicCode = Convert.ToString(itemRow[0]);
                            tempNationalObj.EthinicName = GetDicEhtnicBG().ContainsKey(tempNationalObj.EthinicCode) ? GetDicEhtnicBG()[tempNationalObj.EthinicCode] : "NO NAME";

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempNationalObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempNationalObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempNationalObj);
                    }
                }
            }

            foreach (var itemRow in listtemp)
            {
                tempNationalObj = itemRow;
                tempNationalObj.PercentageAllSchool = tempNationalObj.PercentageFemaleAllSchool + tempNationalObj.PercentageMaleAllSchool;
            }

            return listtemp;


        }

        private List<CurriculumObj> GetdatabyZonecode(string pZonecode)
        {
            Console.Write("GetdatabyZonecode ==> ");

            List<CurriculumObj> listtemp = new List<CurriculumObj>();
            CurriculumObj tempNationalObj = new CurriculumObj();

            string query = " Select t1.EthnicBackground, t1.Gender, (Count(*)* 100 /";
            //query += " (Select Count(*)  from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\")))";
            //query += " From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\") Group By NationalIdentity, Gender ";
            query += " (Select Count(*)  from sch_Student_t t1 INNER JOIN CityShire  t2 on  t1.PostCode = t2.PostCode where DataZone in (\"" + pZonecode + "\") )) ";
            query += " From sch_Student_t t1 INNER JOIN CityShire  t2 on  t1.PostCode = t2.PostCode where DataZone in (\"" + pZonecode + "\") Group By EthnicBackground, Gender";

            //Select Count(*)  from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in ('Brimmond Primary School')
            var listResult = rpGeneric.FindByNativeSQL(query);

            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Nationalcode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Nationalcode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempNationalObj = new EthnicObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.EthinicCode = Convert.ToString(itemRow[0]);
                            tempNationalObj.EthinicName = GetDicEhtnicBG().ContainsKey(tempNationalObj.EthinicCode) ? GetDicEhtnicBG()[tempNationalObj.EthinicCode] : "NO NAME";

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempNationalObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempNationalObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempNationalObj);
                    }
                }
            }


            foreach (var itemRow in listtemp)
            {
                tempNationalObj = itemRow;
                tempNationalObj.PercentageAllSchool = tempNationalObj.PercentageFemaleAllSchool + tempNationalObj.PercentageMaleAllSchool;
            }

            return listtemp;


        }


        [HttpPost]
        public JsonResult GetChartDataEthnicforMap(List<CurriculumObj> data)
        {
            try
            {
                object oChartData = new object();
                if (data != null)
                {
                    var listChartData = new List<object>();
                    listChartData.Add(new { name = "Early", data = data.Select(x => x.early).ToArray() });
                    listChartData.Add(new { name = "Early Developing", data = data.Select(x => x.earlydeveloping).ToArray() });
                    listChartData.Add(new { name = "Early Consolidating", data = data.Select(x => x.earlyconsolidating).ToArray() });
                    listChartData.Add(new { name = "Early Secure", data = data.Select(x => x.earlysecure).ToArray() });
                    listChartData.Add(new { name = "First Developing", data = data.Select(x => x.firstdeveloping).ToArray() });
                    listChartData.Add(new { name = "First Consolidating", data = data.Select(x => x.firstconsolidating).ToArray() });
                    listChartData.Add(new { name = "First Secure", data = data.Select(x => x.firstsecure).ToArray() });

                    listChartData.Add(new { name = "Second Developing", data = data.Select(x => x.seconddeveloping).ToArray() });
                    listChartData.Add(new { name = "Second Consolidating", data = data.Select(x => x.secondconsolidating).ToArray() });
                    listChartData.Add(new { name = "Second Secure", data = data.Select(x => x.secondsecure).ToArray() });

                    listChartData.Add(new { name = "Third Developing", data = data.Select(x => x.thirddeveloping).ToArray() });
                    listChartData.Add(new { name = "Third Consolidating", data = data.Select(x => x.thirdconsolidating).ToArray() });
                    listChartData.Add(new { name = "Third Secure", data = data.Select(x => x.thirdsecure).ToArray() });
                    listChartData.Add(new { name = "Blank", data = data.Select(x => x.blank).ToArray() });

                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "Curriculum for Excellence - Primary Schools (%pupils)",
                        ChartCategories = data.Select(x => x.stage).ToArray(),
                        ChartSeries = listChartData
                    };
                }
                else
                {

                    oChartData = new
                    {
                        ChartTitle = "No data available",
                        ChartCategories = new List<string>(),
                        ChartSeries = new List<double>()
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
    }
}