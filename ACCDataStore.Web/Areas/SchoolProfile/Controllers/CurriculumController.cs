using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Curriculum;
using Common.Logging;
using System;
using System.Collections.Generic;
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
                        }
                        else if (subject.Equals("Reading"))
                        {
                            vmCurriculum.ListReadingdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "Reading");
                        }
                        else if (subject.Equals("Writing"))
                        {
                            vmCurriculum.ListWritingdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "Writing");
                        }
                        else if (subject.Equals("L and T"))
                        {
                            vmCurriculum.ListLandTdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "L_and_T");
                        }
                        else if (subject.Equals("Numeracy Primary"))
                        {
                            vmCurriculum.ListNumeracydata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "Numeracy_Primary");
                        }
                        else if (subject.Equals("NMM"))
                        {
                            vmCurriculum.ListNMMdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "NMM");
                        }
                        else if (subject.Equals("SPM"))
                        {
                            vmCurriculum.ListSPMdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "SPM");
                        }
                        else if (subject.Equals("IH"))
                        {
                            vmCurriculum.ListIHdata = GetCurriculumDatabySchoolname(rpGeneric, sSchoolName, "IH");
                        }
                    }
                    vmCurriculum.IsShowData = true;
                }
                Session["SessionListLiteracydata"] = vmCurriculum.ListLiteracydata;
            }
            return View("Index", vmCurriculum);
        }
    }
}