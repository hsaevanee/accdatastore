using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class SpecialSchoolProfileController : BaseSchoolProfilesController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public SpecialSchoolProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        // GET: SchoolProfiles/SpecialSchool
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("SchoolProfiles/SpecialSchoolProfile/GetCondition")]
        public JsonResult GetCondition()
        {
            try
            {
                object oResult = null;

                var listSchool = GetListSchool(rpGeneric2nd, "4");
                var listYear = GetListYear();
                var eYearSelected = listYear != null ? listYear.Where(x => x.year.Equals("2016")).First() : null;
                List<School> ListSchoolSelected = listSchool != null ? listSchool.Where(x => x.seedcode.Equals("5245044")).ToList() : null;
                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()),
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected != null ? eYearSelected.GetJson() : null,
                    ListSchoolSelected = ListSchoolSelected.Select(x => x.GetSchoolDetailJson()),
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        [HttpGet]
        [Route("SchoolProfiles/SpecialSchoolProfile/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] string sYear) // get selected list of school's id
        {
            try
            {
                object oResult = null;
                string sSchoolType = "4";
                var listSchool = GetListSchool(rpGeneric2nd, sSchoolType);
                var listYear = GetListYear();
                var eYearSelected = new Year(sYear);
                List<School> ListSchoolSelected = listSeedCode != null && listSeedCode.Count > 0 ? listSchool.Where(x => listSeedCode.Contains(x.seedcode)).ToList() : null;

                var listSchoolData = GetSchoolData(ListSchoolSelected, sYear, sSchoolType);
                //SchoolPIPSTransform TempPIPSTransform = GetSchoolPIPSTransform(listSchoolData);

                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()), // all school
                    ListSchoolSelected = ListSchoolSelected.Where(x => !x.seedcode.Equals("1002")).Select(x => x.GetSchoolDetailJson()), // set selected list of school
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected.GetJson(),
                    ListingData = listSchoolData, // table data
                    ChartData = GetChartData(listSchoolData, eYearSelected),
                    // ListPIPSTransform = TempPIPSTransform
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        private List<SPSchool> GetSchoolData(List<School> tListSchoolSelected, string sYear, string sSchoolType)
        {
            var listYear = GetListYear();
            var listSchoolData = new List<SPSchool>();
            SPSchool tempSchool = new SPSchool();

            //add Aberdeen Primary School data
            tListSchoolSelected.Add(new School("1002", "Aberdeen Special Schools"));

            Year selectedyear = new Year(sYear);

            foreach (School school in tListSchoolSelected)
            {
                tempSchool = new SPSchool();
                tempSchool.SeedCode = school.seedcode;
                tempSchool.SchoolName = school.name;
                tempSchool.listNationalityIdentity = GetHistoricalNationalityData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                tempSchool.NationalityIdentity = tempSchool.listNationalityIdentity.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listEthnicbackground = GetHistoricalEthnicData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                tempSchool.Ethnicbackground = tempSchool.listEthnicbackground.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listLevelOfEnglish = GetHistoricalEALData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                tempSchool.LevelOfEnglish = tempSchool.listLevelOfEnglish.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listLookedAfter = GetHistoricalLookedAfterData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                tempSchool.LookedAfter = tempSchool.listLookedAfter.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.SchoolRoll = GetSchoolRollData(school, selectedyear);
                tempSchool.SchoolRollForecast = GetSchoolRollForecastData(rpGeneric2nd, school);
                tempSchool.listSIMD = GetHistoricalSIMDData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                tempSchool.SIMD = tempSchool.listSIMD.Where(x => x.YearInfo.year.Equals("2016")).FirstOrDefault();
                //tempSchool.listFSM = GetHistoricalFSMData(school.seedcode);
                //tempSchool.FSM = tempSchool.listFSM.Where(x => x.year.year.Equals("2016")).FirstOrDefault();
                tempSchool.listStudentNeed = GetHistoricalStudentNeed(rpGeneric2nd, sSchoolType, school.seedcode, tempSchool.SchoolRoll, listYear);
                tempSchool.StudentNeed = tempSchool.listStudentNeed.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listAttendance = GetHistoricalAttendanceData(rpGeneric2nd, sSchoolType, school, listYear);
                tempSchool.SPAttendance = tempSchool.listAttendance.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listExclusion = GetHistoricalExclusionData(rpGeneric2nd, sSchoolType, school, listYear);
                tempSchool.SPExclusion = tempSchool.listExclusion.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();

                listSchoolData.Add(tempSchool);
            }
            return listSchoolData;
        }

        private ACCDataStore.Entity.SchoolProfiles.Census.Entity.ChartData GetChartData(List<SPSchool> listSchool, Year eYearSelected)
        {
            return new Entity.SchoolProfiles.Census.Entity.ChartData()
            {
                ChartNationalityIdentity = GetChartNationalityIdentity(listSchool, eYearSelected),
                ChartLevelOfEnglish = GetChartLevelofEnglish(listSchool, eYearSelected),
                ChartLevelOfEnglishByCatagories = GetChartLevelofEnglishbyCatagories(listSchool, eYearSelected),
                ChartSIMD = GetChartSIMDDecile(listSchool, eYearSelected),
                CartSchoolRollForecast = GetChartSchoolRollForecast(listSchool),
                ChartIEP = GetChartStudentNeedIEP(listSchool),
                ChartCSP = GetChartStudentNeedCSP(listSchool),
                ChartLookedAfter = GetChartLookedAfter(listSchool),
                ChartAuthorisedAbsence = GetChartAttendance(listSchool, "Authorised Absence"),
                ChartUnauthorisedAbsence = GetChartAttendance(listSchool, "Unauthorised Absence"),
                ChartTotalAbsence = GetChartAttendance(listSchool, "Total Absence"),
                ChartNumberofDaysLostExclusion = GetChartExclusion(listSchool, "Number of Days Lost Per 1000 Pupils Through Exclusions"),
                ChartNumberofExclusionRFR = GetChartExclusion(listSchool, "Number of Removals from the Register"),
            };
        }

        //Get SchoolRoll data
        private SchoolRoll GetSchoolRollData(School school, Year year)
        {

            SchoolRoll SchoolRoll = new SchoolRoll();

            if (school.seedcode.Equals("1002"))
            {

                string tablename = "sch_student_t_" + year.year;
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total,  count(*) from " + tablename + " where Studentstatus = 01 and schooltype=4");
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            SchoolRoll = new SchoolRoll();
                            SchoolRoll.year = year;
                            SchoolRoll.capacity = school.schoolCapacity;
                            SchoolRoll.schoolroll = Convert.ToInt16(itemRow[1].ToString());
                            SchoolRoll.percent = 0.00F;
                        }
                    }
                }

            }
            else
            {

                string tablename = "sch_student_t_" + year.year;
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, count(*) from " + tablename + " where Studentstatus = 01 and schooltype=4 and seedcode =" + school.seedcode);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            SchoolRoll = new SchoolRoll();
                            SchoolRoll.year = year;
                            SchoolRoll.capacity = 0;
                            SchoolRoll.schoolroll = Convert.ToInt16(itemRow[1].ToString());
                            SchoolRoll.percent = 0.00F;
                        }
                    }
                }


            }

            return SchoolRoll;
        }
    }
}