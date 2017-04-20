using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class CitySchoolProfileController : BaseSchoolProfilesController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public CitySchoolProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        [SchoolAuthentication]
        [Transactional]
        // GET: SchoolProfiles/CitySchoolProfile
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("SchoolProfiles/CitySchoolProfile/GetCondition")]
        public JsonResult GetCondition()
        {
            try
            {
                object oResult = null;

                var listSchool = new List<School>() { new School("1002", "Aberdeen City") };
                var listYear = GetListYear();
                var eYearSelected = listYear != null ? listYear.Where(x => x.year.Equals("2016")).First() : null;
                List<School> ListSchoolSelected = listSchool != null ? listSchool.Where(x => x.seedcode.Equals("1002")).ToList() : null;
                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()),
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected != null ? eYearSelected.GetJson() : null,
                    ListSchoolSelected = listSchool.Select(x => x.GetJson()),
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        [SchoolAuthentication]
        [Transactional]
        [HttpGet]
        [Route("SchoolProfiles/CitySchoolProfile/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] string sYear) // get selected list of school's id
        {
            try
            {
                object oResult = null;

                var listSchool = new List<School>() { new School("1002", "Aberdeen City", "5") };
                var listYear = GetListYear();
                var eYearSelected = new Year(sYear);
                List<School> ListSchoolSelected = new List<School>() ;

                var listSchoolData = GetSchoolData(ListSchoolSelected, sYear);

                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()), // all school
                    ListSchoolSelected = listSchool.Select(x => x.GetJson()), // set selected list of school
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected.GetJson(),
                    ListingData = listSchoolData, // table data
                    ChartData = GetChartData(listSchoolData, eYearSelected),
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        private List<SPSchool> GetSchoolData(List<School> tListSchoolSelected, string sYear)
        {
            var listYear = GetListYear();
            var listSchoolData = new List<SPSchool>();
            SPSchool tempSchool = new SPSchool();

            //add Aberdeen Primary School data
            tListSchoolSelected.Add(new School("2", "Aberdeen Primary Schools", "2"));
            //add Aberdeen Secondary School data
            tListSchoolSelected.Add(new School("3", "Aberdeen Secondary Schools", "3"));
            //add Aberdeen Special School data
            tListSchoolSelected.Add(new School("4", "Aberdeen Special Schools", "4"));
            //Aberdeen City
            tListSchoolSelected.Add(new School("1002", "Aberdeen City", "5"));

            Year selectedyear = new Year(sYear);

            foreach (School school in tListSchoolSelected)
            {
                tempSchool = new SPSchool();
                tempSchool.SeedCode = school.seedcode;
                tempSchool.SchoolName = school.name;
                tempSchool.SchoolCostperPupil = GetSchoolCostperPupil(school);
                tempSchool.listNationalityIdentity = GetHistoricalNationalityData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.NationalityIdentity = tempSchool.listNationalityIdentity.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listEthnicbackground = GetHistoricalEthnicData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.Ethnicbackground = tempSchool.listEthnicbackground.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listLevelOfEnglish = GetHistoricalEALData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.LevelOfEnglish = tempSchool.listLevelOfEnglish.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listLookedAfter = GetHistoricalLookedAfterData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.LookedAfter = tempSchool.listLookedAfter.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.SchoolRoll = GetSchoolRollData(school, selectedyear);
                tempSchool.SchoolRollForecast = GetSchoolRollForecastData(rpGeneric2nd, school);
                tempSchool.listStudentStage = GetHistoricalStudentStageData(rpGeneric2nd, school.seedcode, listYear);
                tempSchool.StudentStage = tempSchool.listStudentStage.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listSIMD = GetHistoricalSIMDData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.SIMD = tempSchool.listSIMD.Where(x => x.YearInfo.year.Equals("2016")).FirstOrDefault();
                tempSchool.listFSM = GetHistoricalFSMData(rpGeneric2nd, school.schooltype, school.seedcode,listYear);
                tempSchool.FSM = tempSchool.listFSM.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listStudentNeed = GetHistoricalStudentNeed(rpGeneric2nd, school.schooltype, school.seedcode, tempSchool.SchoolRoll, listYear);
                tempSchool.StudentNeed = tempSchool.listStudentNeed.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listAttendance = GetHistoricalAttendanceData(rpGeneric2nd, school.schooltype, school, listYear);
                tempSchool.SPAttendance = tempSchool.listAttendance.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listExclusion = GetHistoricalExclusionData(rpGeneric2nd, school.schooltype, school, listYear);
                tempSchool.SPExclusion = tempSchool.listExclusion.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listSPCfElevel = GetHistoricalCfELevelData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.SPCfElevel = tempSchool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
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
                ChartAttendance = GetChartAttendance(listSchool, "Attendance"),
                ChartAuthorisedAbsence = GetChartAttendance(listSchool, "Authorised Absence"),
                ChartUnauthorisedAbsence = GetChartAttendance(listSchool, "Unauthorised Absence"),
                ChartTotalAbsence = GetChartAttendance(listSchool, "Total Absence"),
                ChartNumberofDaysLostExclusion = GetChartExclusion(listSchool, "Number of Days Lost Per 1000 Pupils Through Exclusions"),
                ChartNumberofExclusionRFR = GetChartExclusion(listSchool, "Number of Removals from the Register"),
                ChartNumberofExclusionTemporary = GetChartExclusion(listSchool, "Number of Temporary Exclusions")
            };
        }

        private string GetSchoolCostperPupil(School school)
        {
            string costperpupil = "";

            if (school.seedcode.Equals("1002"))
            {
                costperpupil = NumberFormatHelper.FormatNumber(4101.2, 1).ToString();
            }
            else if (school.seedcode.Equals("2"))
            {
                //primary schools
                costperpupil = NumberFormatHelper.FormatNumber(6195.6, 1).ToString();

            }
            else if (school.seedcode.Equals("3"))
            {
                //secondary schools
                costperpupil = NumberFormatHelper.FormatNumber(4101.2, 1).ToString();

            }
            else if (school.seedcode.Equals("4"))
            {
                //Special schools
                costperpupil = NumberFormatHelper.FormatNumber(0.0, 1).ToString();

            }

            return costperpupil;
        }

        //Get SchoolRoll data
        private SchoolRoll GetSchoolRollData(School school, Year year)
        {

            SchoolRoll SchoolRoll = new SchoolRoll();

            if (school.seedcode.Equals("1002"))
            {
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where year = " + year.year);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            SchoolRoll = new SchoolRoll();
                            SchoolRoll.year = year;
                            SchoolRoll.capacity = school.schoolCapacity;
                            SchoolRoll.schoolroll = Convert.ToInt16(itemRow[1].ToString());//NumberFormatHelper.FormatNumber(school.costperpupil, 1).ToString();    
                            SchoolRoll.percent = 0.00F;
                            //SchoolRoll.spercent = NumberFormatHelper.FormatNumber(itemRow[1], 1).ToString();
                            SchoolRoll.sschoolroll = NumberFormatHelper.FormatNumber(itemRow[1], 0).ToString();  
                        }
                    }
                }

            }
            else
            {
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where year = " + year.year + " and SchoolType =" + school.schooltype);
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
                            //SchoolRoll.spercent = NumberFormatHelper.FormatNumber(itemRow[1], 1).ToString();
                            SchoolRoll.sschoolroll = NumberFormatHelper.FormatNumber(itemRow[1], 0).ToString();  
                        }
                    }
                }


            }

            return SchoolRoll;
        }

        //Get SchoolRoll data
        private new SPSchoolRollForecast GetSchoolRollForecastData(IGenericRepository2nd rpGeneric2nd, School school)
        {

            SPSchoolRollForecast SchoolRollForecast = new SPSchoolRollForecast();
            List<GenericSchoolData> tempdataActualnumber = new List<GenericSchoolData>();

            if (!school.schooltype.Equals("5"))
            {
                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, 'Seedcode', SchoolType, sum(Count) from summary_schoolroll where SchoolType = " + school.schooltype + " group by schooltype, year");
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tempdataActualnumber.Add(new GenericSchoolData(new Year(itemRow[0].ToString()).academicyear, itemRow[3]==null? 0: Convert.ToInt32(itemRow[3].ToString())));
                        }
                    }
                }

                SchoolRollForecast.ListActualSchoolRoll = tempdataActualnumber;
            }
            else {
                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, 'Seedcode', 'SchoolType', sum(Count) from summary_schoolroll group by year");
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tempdataActualnumber.Add(new GenericSchoolData(new Year(itemRow[0].ToString()).academicyear, itemRow[3] == null ? 0 : Convert.ToInt32(itemRow[3].ToString())));
                        }
                    }
                }

                SchoolRollForecast.ListActualSchoolRoll = tempdataActualnumber;
            }

            return SchoolRollForecast;
        }

        // SchoolRoll Forecast Chart
        private new SplineCharts GetChartSchoolRollForecast(List<SPSchool> listSchool) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = " School Roll ";
            eSplineCharts.yAxis.title.text = "Number of Pupils";
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();
            //finding subject index to query data from list

            if (listSchool != null && listSchool.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchool[0].SchoolRollForecast.ListActualSchoolRoll.Select(x => x.Code).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "Number of Pupils" };

                foreach (var eSchool in listSchool)
                {
                    if (!eSchool.SeedCode.Equals("1002"))
                    {
                        var listSeriesActual = eSchool.SchoolRollForecast.ListActualSchoolRoll.Select(x => float.Parse(x.sCount) == 0 ? null : (float?)float.Parse(x.sCount)).ToList();

                        eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                        {
                            name = eSchool.SchoolName,
                            color = colors[indexColor],
                            lineWidth = 2,
                            data = listSeriesActual,
                            visible = true
                        });

                    }
                    indexColor++;
                }
            }

            eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
            {
                enabled = true
            };

            eSplineCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };
            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        protected override List<ViewObj> GetListViewObj(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string datatitle)
        {
            List<ViewObj> listResult = new List<ViewObj>();
            string query = "";
            switch (datatitle)
            {
                case "eal":
                    query = "Select * from summary_levelofenglish";
                    break;
                case "ethnicbackground":
                    query = "Select * from summary_ethnicbackground";
                    break;
                case "stage":
                    query = "Select * from summary_studentstage";
                    break;
                case "nationality":
                    query = "Select * from summary_nationality";
                    break;
                case "needtype":
                    //to calculate IEP CSP
                    query = "Select * from summary_studentneed";
                    break;
                case "lookedafter":
                    //to calculate IEP CSP
                    query = "Select * from summary_studentlookedafter";
                    break;
                case "simd":
                    //to calculate IEP CSP
                    query = "Select * from summary_simd";
                    break;
                case "attendance":
                    //to calculate IEP CSP
                    query = "Select * from summary_attendance ";
                    break;
                case "schoolroll":
                    //to calculate IEP CSP
                    query = "Select * from summary_schoolroll ";
                    break;
            }

            var listtemp = rpGeneric2nd.FindByNativeSQL(query);
            foreach (var itemrow in listtemp)
            {
                if (itemrow != null)
                {
                    ViewObj temp = new ViewObj();
                    temp.year = new Year(itemrow[0].ToString());
                    temp.seedcode = itemrow[1].ToString();
                    temp.schooltype = itemrow[2].ToString();
                    temp.code = itemrow[3].ToString();
                    temp.count = Convert.ToInt32(itemrow[4].ToString());
                    listResult.Add(temp);
                }
            }


            return listResult;

        }
        //Historical StudentStage data
        private List<StudentStage> GetHistoricalStudentStageData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, List<Year> listyear)
        {
            List<StudentStage> listStudentStage = new List<StudentStage>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            StudentStage StudentStage = new StudentStage();

            Dictionary<string, string> DictStage = GetDicStage(rpGeneric2nd, sSchoolType);

            foreach (var item in DictStage)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "stage");

                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictStage[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sCount = NumberFormatHelper.FormatNumber(y.Select(a => a.count).Sum(), 0).ToString(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    StudentStage = new StudentStage();
                    StudentStage.YearInfo = year;
                    StudentStage.ListGenericSchoolData = groupedList;
                    StudentStage.totalschoolroll = total;
                    StudentStage.stotalschoolroll = NumberFormatHelper.FormatNumber(total, 0).ToString();
                    listStudentStage.Add(StudentStage);
                }


            return listStudentStage.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical NationalityData
        private new List<NationalityIdentity> GetHistoricalNationalityData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<NationalityIdentity> listNationalityIdentity = new List<NationalityIdentity>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            NationalityIdentity NationalityIdentity = new NationalityIdentity();

            Dictionary<string, string> DictNationality = GetDicNationalIdenity(rpGeneric2nd);

            foreach (var item in DictNationality)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "nationality");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && !x.code.Equals("08")).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Sum(x => x.count),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    NationalityIdentity = new NationalityIdentity();
                    NationalityIdentity.YearInfo = year;
                    NationalityIdentity.ListGenericSchoolData = groupedList;
                    listNationalityIdentity.Add(NationalityIdentity);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Sum(x => x.count),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    NationalityIdentity = new NationalityIdentity();
                    NationalityIdentity.YearInfo = year;
                    NationalityIdentity.ListGenericSchoolData = groupedList;
                    listNationalityIdentity.Add(NationalityIdentity);
                }

            }

            return listNationalityIdentity.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical EthnicBackground data
        private new List<Ethnicbackground> GetHistoricalEthnicData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<Ethnicbackground> listEthnicbackground = new List<Ethnicbackground>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            Ethnicbackground Ethnicbackground = new Ethnicbackground();

            Dictionary<string, string> DictNationality = GetDicEhtnicBG(rpGeneric2nd);

            foreach (var item in DictNationality)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "ethnicbackground");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    Ethnicbackground = new Ethnicbackground();
                    Ethnicbackground.YearInfo = year;
                    Ethnicbackground.ListGenericSchoolData = groupedList;
                    listEthnicbackground.Add(Ethnicbackground);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    Ethnicbackground = new Ethnicbackground();
                    Ethnicbackground.YearInfo = year;
                    Ethnicbackground.ListGenericSchoolData = groupedList;
                    listEthnicbackground.Add(Ethnicbackground);
                }

            }

            return listEthnicbackground.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        //Historical Level of English data
        private new List<LevelOfEnglish> GetHistoricalEALData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<LevelOfEnglish> listLevelOfEnglish = new List<LevelOfEnglish>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            LevelOfEnglish LevelOfEnglish = new LevelOfEnglish();

            Dictionary<string, string> DictEnglisheLevel = GetDicEnglisheLevel(rpGeneric2nd);

            foreach (var item in DictEnglisheLevel)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "eal");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictEnglisheLevel[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LevelOfEnglish = new LevelOfEnglish();
                    LevelOfEnglish.YearInfo = year;
                    LevelOfEnglish.ListGenericSchoolData = groupedList.OrderBy(x => x.Name).ToList();
                    listLevelOfEnglish.Add(LevelOfEnglish);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictEnglisheLevel[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LevelOfEnglish = new LevelOfEnglish();
                    LevelOfEnglish.YearInfo = year;
                    LevelOfEnglish.ListGenericSchoolData = groupedList.OrderBy(x => x.Name).ToList();
                    listLevelOfEnglish.Add(LevelOfEnglish);
                }

            }

            return listLevelOfEnglish.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        //Historical Attendance Data
        private new List<SPAttendance> GetHistoricalAttendanceData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, School school, List<Year> listyear)
        {
            List<SPAttendance> listAttendance = new List<SPAttendance>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            SPAttendance SPAttendance = new SPAttendance();

            Dictionary<string, string> DictAttendance = GetDicAttendance(rpGeneric2nd);

            foreach (var item in DictAttendance)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "attendance");

            if (school.seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    if (listresult.Count > 0)
                    {
                        tempdata = new List<GenericSchoolData>();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictAttendance[y.Key.ToString()],
                            count = y.Sum(x => x.count)
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        int possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();
                        int sum = groupedList.Where(x => x.Code.Equals("10") || x.Code.Equals("11") || x.Code.Equals("12")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });
                        int sumUnauthorised = groupedList.Where(x => x.Code.StartsWith("3")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = sumUnauthorised,
                            sum = possibledays,
                            Percent = sumUnauthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumUnauthorised * 100.00F / possibledays), 1).ToString()
                        });

                        int sumAuthorised = groupedList.Where(x => x.Code.StartsWith("2") || x.Code.Equals("13")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = sumAuthorised,
                            sum = possibledays,
                            Percent = sumAuthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumAuthorised * 100.00F / possibledays), 1).ToString()
                        });

                        sum = groupedList.Where(x => x.Code.Equals("40")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = sumAuthorised + sumUnauthorised,
                            sum = possibledays,
                            Percent = (sumAuthorised + sumUnauthorised) * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber(((sumAuthorised + sumUnauthorised) * 100.00F / possibledays), 1).ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);

                    }
                    else
                    {
                        tempdata = new List<GenericSchoolData>();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);
                    }

                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    if (listresult.Count > 0)
                    {
                        tempdata = new List<GenericSchoolData>();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictAttendance[y.Key.ToString()],
                            count = y.Sum(x => x.count)
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        int possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();
                        int sum = groupedList.Where(x => x.Code.Equals("10") || x.Code.Equals("11") || x.Code.Equals("12")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });
                        int sumUnauthorised = groupedList.Where(x => x.Code.StartsWith("3")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = sumUnauthorised,
                            sum = possibledays,
                            Percent = sumUnauthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumUnauthorised * 100.00F / possibledays), 1).ToString()
                        });

                        int sumAuthorised = groupedList.Where(x => x.Code.StartsWith("2") || x.Code.Equals("13")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = sumAuthorised,
                            sum = possibledays,
                            Percent = sumAuthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumAuthorised * 100.00F / possibledays), 1).ToString()
                        });

                        sum = groupedList.Where(x => x.Code.Equals("40")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = sumAuthorised + sumUnauthorised,
                            sum = possibledays,
                            Percent = (sumAuthorised + sumUnauthorised) * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber(((sumAuthorised + sumUnauthorised) * 100.00F / possibledays), 1).ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);

                    }
                    else
                    {
                        tempdata = new List<GenericSchoolData>();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);
                    }
                }

            }

            return listAttendance.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical Looked After data
        private new List<LookedAfter> GetHistoricalLookedAfterData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<LookedAfter> listLookedAfter = new List<LookedAfter>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            LookedAfter LookedAfter = new LookedAfter();

            Dictionary<string, string> DictLookedAfter = GetDicLookAfter();

            foreach (var item in DictLookedAfter)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "lookedafter");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictLookedAfter[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LookedAfter = new LookedAfter();
                    LookedAfter.YearInfo = year;
                    LookedAfter.GenericSchoolData = new GenericSchoolData()
                    {
                        Code = "1&2",
                        Name = "LookedafterPupils",
                        Value = "",
                        count = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.count).Sum(),
                        sum = total,
                        Percent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.Percent).Sum(),
                        sPercent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => Convert.ToDouble(x.sPercent)).Sum().ToString()
                    };
                    LookedAfter.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => x.Code).ToList(); ;
                    listLookedAfter.Add(LookedAfter);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictLookedAfter[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LookedAfter = new LookedAfter();
                    LookedAfter.YearInfo = year;
                    LookedAfter.GenericSchoolData = new GenericSchoolData()
                    {
                        Code = "1&2",
                        Name = "LookedafterPupils",
                        Value = "",
                        count = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.count).Sum(),
                        sum = total,
                        Percent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.Percent).Sum(),
                        sPercent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => Convert.ToDouble(x.sPercent)).Sum().ToString()
                    };
                    LookedAfter.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => x.Code).ToList(); ;
                    listLookedAfter.Add(LookedAfter);
                }

            }

            return listLookedAfter.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        //Historical StudentNeed
        private List<StudentNeed> GetHistoricalStudentNeed(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, SchoolRoll schoolroll, List<Year> listyear)
        {
            StudentNeed StudentNeed = new StudentNeed(); ;
            List<StudentNeed> listStudentNeed = new List<StudentNeed>();

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "needtype");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    StudentNeed = new StudentNeed();
                    StudentNeed.year = year;
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int totalcount = listresult.Where(x => x.code.Equals("02")).Select(x => x.count).Sum();
                    StudentNeed.IEP = new GenericSchoolData()
                    {
                        Code = "02",
                        Name = "IEP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = schoolroll.schoolroll,
                        Percent = schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("01")).Select(x => x.count).Sum();
                    StudentNeed.CSP = new GenericSchoolData()
                    {
                        Code = "01",
                        Name = "CSP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = schoolroll.schoolroll,
                        Percent = schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.00F), 1).ToString()
                    };
                    listStudentNeed.Add(StudentNeed);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    StudentNeed = new StudentNeed();
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    StudentNeed.year = year;
                    int totalcount = listresult.Where(x => x.code.Equals("02")).Select(x => x.count).Sum();
                    StudentNeed.IEP = new GenericSchoolData()
                    {
                        Code = "02",
                        Name = "IEP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = schoolroll.schoolroll,
                        Percent = schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("01")).Select(x => x.count).Sum();
                    StudentNeed.CSP = new GenericSchoolData()
                    {
                        Code = "01",
                        Name = "CSP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = schoolroll.schoolroll,
                        Percent = schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.00F), 1).ToString()
                    };
                    listStudentNeed.Add(StudentNeed);
                }

            }

            return listStudentNeed.OrderBy(x => x.year.year).ToList();
        }

        //Historical Exclusion Data
        private new List<SPExclusion> GetHistoricalExclusionData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, School school, List<Year> listyear)
        {
            List<SPExclusion> listExclusion = new List<SPExclusion>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>() { new GenericSchoolData("0", "Temporary Exclusions"), new GenericSchoolData("1", "Removed From Register") };
            SPExclusion SPExclusion = new SPExclusion();
            GenericSchoolData tempobj = new GenericSchoolData();
            string queryExclusion, querySchoolRoll = "";
            int schoolroll = 0;

            foreach (Year year in listyear)
            {
                if (school.seedcode.Equals("1002"))
                {
                    queryExclusion = "SELECT Year, 1002, Code, sum(Count), sum(Sum)  FROM summary_exclusion where year = " + year.year + " group by Year, Code";
                    querySchoolRoll = "Select Year, sum(Count) from summary_schoolroll where year = " + year.year;
                }
                else
                {
                    queryExclusion = "SELECT Year, Seedcode, Code, sum(Count), sum(Sum)  FROM summary_exclusion where  SchoolType= " + sSchoolType + "  and year = " + year.year + " group by Year, Code;";
                    querySchoolRoll = "Select Year, sum(Count) from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year ;
                }

                var listResult = rpGeneric2nd.FindByNativeSQL(queryExclusion);
                if (listResult != null)
                {
                    tempdata = new List<GenericSchoolData>();
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tempobj = new GenericSchoolData(itemRow[2].ToString(), itemRow[2].ToString().Equals("0") ? "Temporary Exclusions" : "Removed From Register");
                            tempobj.count = Convert.ToInt16(itemRow[3].ToString());
                            tempobj.sum = Convert.ToInt16(itemRow[4].ToString());
                            tempobj.Percent = Convert.ToInt16(itemRow[3].ToString());
                            tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                            tempdata.Add(tempobj);
                        }
                    }
                    var listResultSchoolRoll = rpGeneric2nd.FindByNativeSQL(querySchoolRoll);
                    if (listResultSchoolRoll != null)
                    {
                        foreach (var itemRow in listResultSchoolRoll)
                        {
                            if (itemRow != null)
                            {
                                schoolroll = Convert.ToInt16(itemRow[1].ToString());
                            }
                        }
                    }

                    tempdata.AddRange(foo.Where(x => tempdata.All(p1 => !p1.Code.Equals(x.Code))));
                    SPExclusion = new SPExclusion();
                    SPExclusion.YearInfo = new Year(year.year);
                    tempobj = new GenericSchoolData("2", "Number of days per 1000 pupils lost to exclusions");
                    tempobj.count = tempdata.Sum(x => x.sum);  //Sum length of exclusion
                    tempobj.sum = schoolroll;   //school Roll
                    tempobj.Percent = tempobj.count / 2.0F / schoolroll * 1000.0F;
                    tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                    //tempdata.Add(tempobj);
                    SPExclusion.ListGenericSchoolData = new List<GenericSchoolData>() { tempdata.Where(x => x.Code.Equals("0")).First(), tempdata.Where(x => x.Code.Equals("1")).First(), tempobj };
                    listExclusion.Add(SPExclusion);
                }

            }

            return listExclusion.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical SIMD data
        private new List<SPSIMD> GetHistoricalSIMDData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<SPSIMD> listSIMD = new List<SPSIMD>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            SPSIMD SPSIMD = new SPSIMD();

            Dictionary<string, string> DictSIMD = GetDicSIMDDecile();

            foreach (var item in DictSIMD)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "simd");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    if (listresult != null && listresult.Count > 0)
                    {
                        int total = listresult.Select(s => s.count).Sum();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictSIMD[y.Key.ToString()],
                            count = y.Select(a => a.count).Sum(),
                            sum = total,
                            Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                            sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        SPSIMD = new SPSIMD();
                        SPSIMD.YearInfo = year;
                        SPSIMD.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => Convert.ToInt16(x.Code)).ToList();
                        listSIMD.Add(SPSIMD);
                    }
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    if (listresult != null && listresult.Count > 0)
                    {
                        int total = listresult.Select(s => s.count).Sum();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictSIMD[y.Key.ToString()],
                            count = y.Select(a => a.count).Sum(),
                            sum = total,
                            Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                            sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        SPSIMD = new SPSIMD();
                        SPSIMD.YearInfo = year;
                        SPSIMD.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => Convert.ToInt16(x.Code)).ToList();
                        listSIMD.Add(SPSIMD);
                    }

                }

            }

            return listSIMD.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical Free School Meal Registered data
        private List<FreeSchoolMeal> GetHistoricalFSMData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<FreeSchoolMeal> listFSM = new List<FreeSchoolMeal>();

            listFSM = GetHistoricalFSMData(rpGeneric2nd, "1002", listyear, sSchoolType);

            return listFSM.OrderBy(x => x.year.year).ToList();
        }

        //Historical Free School Meal Registered data
        private List<SPCfElevel> GetHistoricalCfELevelData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<SPCfElevel> listSPCfElevel = new List<SPCfElevel>();

            if (sSchoolType.Equals("2")){

                listSPCfElevel = GetHistoricalPrimaryCfeLevelData(rpGeneric2nd, "1002", sSchoolType);
            }
            else if (sSchoolType.Equals("3"))
            {
                listSPCfElevel = GetHistoricalSecondaryCfeLevelData(rpGeneric2nd, "1002", sSchoolType);
            }

 

            return listSPCfElevel.OrderBy(x => x.year.year).ToList();
        }

    }
}