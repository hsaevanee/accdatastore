﻿using ACCDataStore.Web.Helpers;
using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub;
using ClosedXML.Excel;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Web.Areas.DatahubProfile.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;
using NHibernate;
using ACCDataStore.Web.Areas.DatahubProfile.Helpers;
using System.Globalization;

namespace ACCDataStore.Web.Areas.DatahubProfile.Controllers
{
    public class IndexDatahubController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexDatahubController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        private School schoolSelection;
        private SummaryDataHelper Helper;
        private CouncilHelper Helper2;
        public IndexDatahubController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
            this.schoolSelection = new School("", "");
            this.Helper = new SummaryDataHelper(rpGeneric2nd);
            this.Helper2 = new CouncilHelper(rpGeneric2nd);
        }

        

        public ActionResult IndexCoucil(string sCouncilname, string sCouncilcode)
        {
            //var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmDatahubViewModel = new DatahubViewModel();
            IList<School> allCouncils = GetListCouncilname();
            //MonthOnMonthOverview(rpGeneric2nd);            
            //vmDatahubViewModel.selectedcouncil = allCouncils.Where(x => x.seedcode.Equals(sCouncilcode)).FirstOrDefault();
            Session["Council"] = vmDatahubViewModel.selectedcouncil;
            vmDatahubViewModel.ListSchoolNameData = GetListSchoolname();
            vmDatahubViewModel.ListNeighbourhoodsName = GetListNeighbourhoodsname(rpGeneric2nd);


            return View("index2", vmDatahubViewModel);
        }
     

        public ActionResult IndexHome()
        {
            //var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmDatahubViewModel = new DatahubViewModel();
            //MonthOnMonthOverview(rpGeneric2nd);

            return View("Home", vmDatahubViewModel);
        }
        // GET: DatahubProfile/IndexDatahub


        //Begin redesign using SummaryData

        public ActionResult ScotlandIndex()
        {
            // We should have a different viewmodel
            var vmDatahubViewModel = new DatahubViewModel();

            // Clearing session (maybe we should do this if a different council is selected
            // The reason i clear the session cookie is that when you select a neighbourhood from a council and then you change the council it will remain there and cause problems
            Session.Clear();
            DateTime currentTime = DateTime.Now; // This wont work next month!
            vmDatahubViewModel.allCouncilTable = new List<SummaryDataViewModel>();

            // Begin section "CHEAT" :D
            // We should have a method in the city helper that calculates this for all available councils
            string[] councils = new string[2] { "S12000033", "S12000033" };
            foreach (string council in councils)
            {
                vmDatahubViewModel.allCouncilTable.Add(Helper.GetSummaryDataForCouncil<AberdeenSummary>(council, currentTime.Month, currentTime.Year));
            }

            // Should implement a helper to get available councils
            vmDatahubViewModel.AvailableCouncis = new[]
            {
                new SelectListItem { Value = "Aberdeen City", Text = "Aberdeen City" },
                new SelectListItem { Value = "Glasgow City", Text = "Glasgow City" },
            };
            return View("ScotlandIndex", vmDatahubViewModel);
        }

        public ActionResult Data(string schoolsubmitButton, string neighbourhoodssubmitButton, int? month, int? year)
        {
            var sup = Request;
            // Should be able to specify which city, month and year are we interested in
            // Timer to benchmark loading
            Stopwatch timer = new Stopwatch();
            timer.Start();
            string name = Request["selectedcouncil"];
            //We dont use these for now

            if (name == null)
            {
                return RedirectToAction("ScotlandIndex");
            }
            IList<School> allCouncils = GetListCouncilname();
            Session["Council"] = name;

            //Declare variables we are going to use
            SummaryDataViewModel councilData = null;
            IList<SummaryDataViewModel> councilSchoolsData = null;
            List<PosNegSchoolList> tableSummaryData = new List<PosNegSchoolList>();

            // To hold our current selection [city,month,year]
            ViewModelParams pageViewModelParams = new ViewModelParams();

            //This is the container for all of the data we are going to send to the page (our view model)
            DatahubViewModel viewModel = new DatahubViewModel();

            // Check if city name is valid [should also do other types of validation such as period validation]
            if(!Helper2.ValidateCouncilName(name)) {
                var currentSession = Session["ViewModelParams"] as ViewModelParams;
                name = currentSession.councilName;
            }
            
            councilData = Helper2.ByName(name).GetSummaryDataForCouncil(08, 2016);

            // This bit should live in a seperate controller IMO
            councilSchoolsData = Helper2.ByName(name).GetSummaryDataForAllSchools(08, 2016);

            //Prepare school participation data
            foreach (var schoolSummary in councilSchoolsData)
            {
                PosNegSchoolList entry = new PosNegSchoolList();
                entry.name = schoolSummary.name;
                entry.participating = (double)schoolSummary.Participating(); // decimal or double for percentages? we should decide
                entry.notParticipating = (double)schoolSummary.NotParticipating();
                entry.unknown = (double)schoolSummary.Percentage(schoolSummary.allPupilsInUnknown);
                tableSummaryData.Add(entry);
            }


            Session["AllSchoolComparisonData"] = tableSummaryData; // I dont know why we do this

            viewModel.CityData = councilData;
            viewModel.summaryTableData = tableSummaryData;
                
            // ???, NVM we need this
            pageViewModelParams.school = schoolsubmitButton;
            pageViewModelParams.neighbourhood = neighbourhoodssubmitButton;
            pageViewModelParams.councilName = name;


            Session["ViewModelParams"] = pageViewModelParams;

            //Now if the schoolsubmitbutton or neighbourhood submitbutton was clicked we populate our viewmodel
            IList<School> allSchoolsList = Helper2.ByName(name).GetSchoolsList();
            viewModel.ListSchoolNameData = allSchoolsList;

            
            if (schoolsubmitButton != null)
            {
                var sSchoolcode = Request["selectedschool"];
                School currentSchool = allSchoolsList.Where(x => x.seedcode == sSchoolcode).FirstOrDefault();
                if (sSchoolcode != null)
                {
                    SummaryDataViewModel currentSchoolData = Helper2.ByName(name).GetSummaryDataForSingleSchool(sSchoolcode, 8, 2016);
                    Session["chartSelectedSchool"] = currentSchoolData;
                    viewModel.selectedschool = currentSchool;
                    viewModel.SelectedData = currentSchoolData;
                }

            }

            IList<School> allIntermediateZonesList = Helper2.ByName(name).GetIntermediateZonesList();
            viewModel.ListNeighbourhoodsName = allIntermediateZonesList;

            if (neighbourhoodssubmitButton != null)
            {
                var datacode = Request["selectedneighbourhoods"];
                School currentIntermediateZone = allIntermediateZonesList.Where(x => x.seedcode == datacode).FirstOrDefault();

                SummaryDataViewModel currentIntermediateZoneData = Helper2.ByName(name).GetSummaryDataForSingleIntermediateZone(currentIntermediateZone.seedcode, 8, 2016);
                Session["chartSelectedNeighbour"] = currentIntermediateZoneData;
                viewModel.selectedschool = currentIntermediateZone;
                viewModel.SelectedData = currentIntermediateZoneData;

            }

            // Stop timer and add benchmark results to viewmodel
            timer.Stop();
            viewModel.benchmarkResults = timer.ElapsedMilliseconds;

            return View("Data", viewModel);


            
            //else { return new HttpStatusCodeResult(404, "Invalid city parameter!"); } // Idealy we want to return an error page
        }

        public JsonResult getBarChartDataNew()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            ViewModelParams selectionParams = Session["ViewModelParams"] as ViewModelParams;
            //List<DatahubDataObj> allStudentData = Getlistpupil(this.rpGeneric2nd);
            //DatahubData CityData = CreatDatahubdata(allStudentData, "100");
            SummaryDataViewModel CityData = Helper.GetSummaryDataForCouncil<AberdeenSummary>("S12000033", 08, 2016);

            MainChartData combinedData = new MainChartData();
            combinedData.totals = new
            {
                name = "Aberdeen city",
                participating = CityData.Participating(),
                notParticipating = CityData.NotParticipating(),
                unknown = CityData.Percentage(CityData.allPupilsInUnknown)
            };
            if (selectionParams.school != null)
            {
                SummaryDataViewModel schoolData = Session["chartSelectedSchool"] as SummaryDataViewModel;
                


                combinedData.selected = new
                {
                    name = schoolData.name,
                    participating = schoolData.Participating(),
                    notParticipating = schoolData.NotParticipating(),
                    unknown = schoolData.Percentage(schoolData.allPupilsInUnknown)
                };
            }
            if (selectionParams.neighbourhood != null)
            {
                SummaryDataViewModel intZoneData = Session["chartSelectedNeighbour"] as SummaryDataViewModel;
                
                combinedData.selected = new
                {
                    name = intZoneData.name,
                    participating = intZoneData.Participating(),
                    notParticipating = intZoneData.NotParticipating(),
                    unknown = intZoneData.Percentage(intZoneData.allPupilsInUnknown)
                };
            }
            timer.Stop();
            combinedData.benchmarkResults = timer.ElapsedMilliseconds;
            return Json(combinedData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult monthlyHistogramNew()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            BenchmarkAjax wrapper = new BenchmarkAjax();
            ViewModelParams selectionParams = Session["ViewModelParams"] as ViewModelParams;
            List<HistogramSeriesData> allseriesoutput = new List<HistogramSeriesData>();
            SummaryDataViewModel currentSchool = Session["chartSelectedSchool"] as SummaryDataViewModel;
            SummaryDataViewModel currentIntZone = Session["chartSelectedNeighbour"] as SummaryDataViewModel;
            int numberofseries = 1;
            if (selectionParams == null)
            {
                selectionParams = new ViewModelParams();
            }
            if (selectionParams.school != null || selectionParams.neighbourhood != null)
            {
                numberofseries = 2;
            }
            for (int j = 0; j < numberofseries; j++)
            {
                List<SummaryDataViewModel> allSeries = new List<SummaryDataViewModel>();
                SummaryDataViewModel comparison;
                string seriesName = "Aberdeen city";
                int[] months = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                //int indexofmonths = Array.IndexOf(months, 10);
                int indexofmonths = 7;
                int year = int.Parse(DateTime.Now.ToString("yyyy")) - 1;
                for (int i = 0; i < 12; i++)
                {
                    indexofmonths++;
                    if (indexofmonths > 11)
                    {
                        indexofmonths = 0;
                        year++;
                    }
                    
                    if (selectionParams.school != null && j > 0)
                    {
                        seriesName = currentSchool.name;
                        comparison = Helper2.ByName(selectionParams.councilName).GetSummaryDataForSingleSchool(currentSchool.dataCode, months[indexofmonths], year);
                        //comparison = comparison.Where(x => !String.IsNullOrWhiteSpace(x.SEED_Code)).Where(x => x.SEED_Code.Equals(schoolSelection.seedcode)).ToList<DatahubDataObj>();
                    }
                    if (selectionParams.neighbourhood != null && j > 0)
                    {
                        seriesName = currentIntZone.name;
                        comparison = Helper2.ByName(selectionParams.councilName).GetSummaryDataForSingleIntermediateZone(currentIntZone.dataCode, months[indexofmonths], year);
                    }else
                    {
                        comparison = Helper2.ByName(selectionParams.councilName).GetSummaryDataForCouncil(months[indexofmonths], year);
                    }


                    allSeries.Add(comparison);
                }
                HistogramSeriesData jsonOut = new HistogramSeriesData();
                jsonOut.months = new List<string>();
                jsonOut.participating = new List<double>();
                jsonOut.notParticipating = new List<double>();
                jsonOut.unknown = new List<double>();
                jsonOut.name = seriesName;
                foreach (SummaryDataViewModel month in allSeries)
                {
                    //Get date
                    string currYear = month.dataYear.ToString();
                    string[] monthname = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    int m = month.dataMonth;
                    string currMonthName = monthname[m - 1];
                    string period = (currMonthName + "-" + currYear);

                    jsonOut.months.Add(month == null ? "" : period);
                    jsonOut.participating.Add(month == null ? -1.00 : Math.Round((double)month.Participating(), 2));
                    jsonOut.notParticipating.Add(month == null ? -1.00 : Math.Round((double)month.NotParticipating(), 2));
                    jsonOut.unknown.Add(month == null ? -1.00 : Math.Round((double)month.Percentage(month.allPupilsInUnknown), 2));
                }
                allseriesoutput.Add(jsonOut);
            }
            wrapper.chart = allseriesoutput;
            timer.Stop();
            wrapper.benchmarkResults = timer.ElapsedMilliseconds;
            return Json(wrapper, JsonRequestBehavior.AllowGet);
        }


        public JsonResult MainPieChartDataNew()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            ViewModelParams selectionParams = Session["ViewModelParams"] as ViewModelParams;
            this.schoolSelection = Session["chartSelectedSchool"] as School;
            //List<DatahubDataObj> allStudentData = this.rpGeneric2nd.FindAll<DatahubDataObj>().ToList();
            //List<DatahubDataObj> allStudentData = Getlistpupil(this.rpGeneric2nd);

            SummaryDataViewModel allStudentData = Helper.GetSummaryDataForCouncil<AberdeenSummary>("S12000033",8,2016); // Should get from session
            MainChartData combinedData = new MainChartData();
            object pieChartTotals = new
            {
                title = "Overall",
                female15 = allStudentData.all15Female,
                male15 = allStudentData.all15Male,
                female16 = allStudentData.all16Female,
                male16 = allStudentData.all16Male,
                female17 = allStudentData.all17Female,
                male17 = allStudentData.all17Male,
                female18 = allStudentData.all18Female,
                male18 = allStudentData.all18Male,
                female19 = allStudentData.all19Female,
                male19 = allStudentData.all19Male,
            };
            combinedData.totals = pieChartTotals;
            if (selectionParams.school != null)
            {
                SummaryDataViewModel schoolData = Session["chartSelectedSchool"] as SummaryDataViewModel;
                object selectedChart = new
                {
                    title = schoolData.name,
                    female15 = schoolData.all15Female,
                    male15 = schoolData.all15Male,
                    female16 = schoolData.all16Female,
                    male16 = schoolData.all16Male,
                    female17 = schoolData.all17Female,
                    male17 = schoolData.all17Male,
                    female18 = schoolData.all18Female,
                    male18 = schoolData.all18Male,
                    female19 = schoolData.all19Female,
                    male19 = schoolData.all19Male,
                };
                combinedData.selected = selectedChart;
            }
            if (selectionParams.neighbourhood != null)
            {
                SummaryDataViewModel intermediateZoneData = Session["chartSelectedNeighbour"] as SummaryDataViewModel;
                object selectedChart = new
                {
                    title = intermediateZoneData.name,
                    female15 = intermediateZoneData.all15Female,
                    male15 = intermediateZoneData.all15Male,
                    female16 = intermediateZoneData.all16Female,
                    male16 = intermediateZoneData.all16Male,
                    female17 = intermediateZoneData.all17Female,
                    male17 = intermediateZoneData.all17Male,
                    female18 = intermediateZoneData.all18Female,
                    male18 = intermediateZoneData.all18Male,
                    female19 = intermediateZoneData.all19Female,
                    male19 = intermediateZoneData.all19Male,
                };
                combinedData.selected = selectedChart;
            }
            timer.Stop();
            combinedData.benchmarkResults = timer.ElapsedMilliseconds;
            return Json(combinedData, JsonRequestBehavior.AllowGet);
        }
        //End redesign


        public ActionResult Index(string schoolsubmitButton, string neighbourhoodssubmitButton)
        {
            //var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            Stopwatch timer = new Stopwatch();
            timer.Start();
            IList<School> allSchools = GetListSchoolname();
            IList<School> allCouncils = GetListCouncilname();

            List<DatahubDataObj> allStudentData = Getlistpupil(this.rpGeneric2nd).Where(z => !String.IsNullOrWhiteSpace(z.SEED_Code)).ToList<DatahubDataObj>();
            List<PosNegSchoolList> tableSummaryData = new List<PosNegSchoolList>();

            foreach (School school in allSchools)
            {
                DatahubData temp = CreatDatahubdata(GetDatahubdatabySchoolcode(allStudentData, school.seedcode),school.seedcode); 
                    PosNegSchoolList entry = new PosNegSchoolList();
                    entry.name = school.name;
                    entry.participating = temp.Participating();
                    entry.notParticipating = temp.NotParticipating();
                    entry.unknown = temp.Percentage(temp.pupilsinUnknown);
                    tableSummaryData.Add(entry);
            }
            Session["AllSchoolComparisonData"] = tableSummaryData;
            DatahubViewModel viewModel = getPageViewModel(schoolsubmitButton, neighbourhoodssubmitButton);
            viewModel.summaryTableData = tableSummaryData;
            ViewModelParams pageViewModelParams = new ViewModelParams();
            pageViewModelParams.school = schoolsubmitButton;
            pageViewModelParams.neighbourhood = neighbourhoodssubmitButton;
            Session["ViewModelParams"] = pageViewModelParams;
            timer.Stop();
            viewModel.benchmarkResults = timer.ElapsedMilliseconds;
            Session["Council"] = new School("S12000033", "Aberdeen City");
            return View("index2", viewModel);
        }

        protected DatahubViewModel getPageViewModel(string schoolsubmitButton, string neighbourhoodssubmitButton)
        {
            var vmDatahubViewModel = new DatahubViewModel();
            var datahubAbdcitydata = new DatahubData();


            vmDatahubViewModel.AberdeencityData = CreatDatahubdata(GetDatahubdatabySchoolcode(rpGeneric2nd, "100"), "100");
            vmDatahubViewModel.ListSchoolNameData = GetListSchoolname();
            vmDatahubViewModel.ListNeighbourhoodsName = GetListNeighbourhoodsname(rpGeneric2nd);

            if (schoolsubmitButton != null)
            {
                var sSchoolcode = Request["selectedschoolcode"];
                if (sSchoolcode != null)
                {
                    vmDatahubViewModel.SchoolData = CreatDatahubdata(GetDatahubdatabySchoolcode(rpGeneric2nd, sSchoolcode), sSchoolcode);
                    vmDatahubViewModel.selectedschoolcode = sSchoolcode;
                    Session["chartSelectedSchool"] = GetListSchoolname().Where(x => x.seedcode.Equals(sSchoolcode)).FirstOrDefault();
                    vmDatahubViewModel.selectedschool = vmDatahubViewModel.ListSchoolNameData.Where(x => x.seedcode.Equals(sSchoolcode)).FirstOrDefault();
                    vmDatahubViewModel.seachby = "School";
                    vmDatahubViewModel.searchcode = sSchoolcode;
                }
            }
            if (neighbourhoodssubmitButton != null)
            {
                var sNeighbourhoods = Request["selectedneighbourhoods"];
                if (sNeighbourhoods != null)
                {
                    vmDatahubViewModel.SchoolData = CreatDatahubdata(GetDatahubdatabyNeighbourhoods(rpGeneric2nd, sNeighbourhoods), sNeighbourhoods);
                    vmDatahubViewModel.selectedneighbourhoods = sNeighbourhoods;
                    School temp = vmDatahubViewModel.ListNeighbourhoodsName.Where(x => x.seedcode.Equals(sNeighbourhoods)).FirstOrDefault();
                    Session["chartSelectedNeighbour"] = vmDatahubViewModel.ListNeighbourhoodsName.Where(x => x.seedcode.Equals(sNeighbourhoods)).FirstOrDefault();
                    vmDatahubViewModel.selectedschool = vmDatahubViewModel.ListNeighbourhoodsName.Where(x => x.seedcode.Equals(sNeighbourhoods)).FirstOrDefault();
                    vmDatahubViewModel.seachby = "Neighbourhood";
                    vmDatahubViewModel.searchcode = sNeighbourhoods;
                }
            }
            return vmDatahubViewModel;
        }

        protected List<DatahubDataObj> Getlistpupil(IGenericRepository2nd rpGeneric2nd)
        {
    
                //List<DatahubDataObj> listdata = this.rpGeneric2nd.FindAll<DatahubDataObj>().ToList() ;
            IList<DatahubDataObj> listdata = this.rpGeneric2nd.QueryOver<DatahubDataAberdeen>()
                                            .Where(r => r.Data_Year == 2016 && r.Data_Month == 08)
                                            .List<DatahubDataObj>();


                List<DatahubDataObj> pupilsmoveoutScotland = listdata.Where(x => x.Current_Status.ToLower().Equals("moved outwith scotland")).ToList();

                List<DatahubDataObj> listResult = listdata.Except(pupilsmoveoutScotland).ToList();


                return listResult;
        }

        protected IList<School> GetListSchoolname()
        {
           List<School> temp = new List<School>();
           temp.Add(new School("100", "Aberdeen City"));
           temp.Add(new School("5244439", "Aberdeen Grammar School"));
           temp.Add(new School("5235634", "Bridge Of Don Academy"));
           temp.Add(new School("5234034", "Bucksburn Academy"));
           temp.Add(new School("5248744", "Cordyce School"));
           temp.Add(new School("5235839", "Cults Academy"));
           temp.Add(new School("5243335", "Dyce Academy"));
           temp.Add(new School("5243238", "Harlaw Academy"));
           temp.Add(new School("5243432", "Hazlehead Academy"));
           temp.Add(new School("5244943", "Hazlewood School"));
           temp.Add(new School("5243831", "Kincorth Academy"));
           temp.Add(new School("5244234", "Northfield Academy"));
           temp.Add(new School("5246237", "Oldmachar Academy"));
           temp.Add(new School("5246431", "St Machar Academy"));
           temp.Add(new School("5244838", "Torry Academy"));
            return temp;

        }

        protected IList<School> GetListCouncilname()
        {
            List<School> temp = new List<School>();
            temp.Add(new School("S12000033", "Aberdeen City"));
            temp.Add(new School("S12000046", "Glasgow City"));
            return temp;

        }

        protected IList<School> GetListNeighbourhoodsname(IGenericRepository2nd rpGeneric2nd)
        {
            List<School> temp = new List<School>();
            var listneighbourhoods = this.rpGeneric2nd.FindSingleColumnByNativeSQL("Select distinct Neighbourhood from Neighbourhood_Postcodes1");
            if (listneighbourhoods != null)
            {
                foreach (var item in listneighbourhoods)
                {
                    if (item != null)
                    {
                        temp.Add(new School(item.ToString(), item.ToString()));
                    }

                }
            }
            return temp;

        }
        protected List<DatahubDataObj> GetDatahubdatabySchoolcode(IGenericRepository2nd rpGeneric2nd, string seedcode)
        {
            //List<DatahubDataObj> listdata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>().ToList() ;
            List<DatahubDataObj> listdata = Getlistpupil(rpGeneric2nd);

            if (seedcode!= null && !seedcode.Equals("100"))
            {
                listdata = (from a in listdata where a.SEED_Code != null && a.SEED_Code.Equals(seedcode) select a).ToList();
            }

            return listdata;
        }

        protected List<DatahubDataObj> GetDatahubdatabySchoolcode(List<DatahubDataObj> listpupils, string seedcode)
        {
            List<DatahubDataObj> listdata = listpupils;

            if (seedcode != null && !seedcode.Equals("100"))
            {
                listdata = (from a in listdata where a.SEED_Code != null && a.SEED_Code.Equals(seedcode) select a).ToList();
            }

            return listdata;
        }

        protected List<DatahubDataObj> GetDatahubdatabyZonecode(IGenericRepository2nd rpGeneric2nd, string zonecode)
        {
            //var listpupilsdata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();
            List<DatahubDataObj> listpupilsdata = Getlistpupil(rpGeneric2nd);
            var listneighbourhooddata = this.rpGeneric2nd.FindAll<ACCDataStore.Entity.DatahubProfile.NeighbourhoodObj>();
            var listdata = new List<DatahubDataObj>();
            if (zonecode != null)
            {
                listdata = (from a in listpupilsdata join b in listneighbourhooddata on a.CSS_Postcode equals b.CSS_Postcode where b.DataZone.Contains(zonecode) select a).ToList();
            }
            return listdata;
        }

        protected List<DatahubDataObj> GetDatahubdatabyNeighbourhoods(IGenericRepository2nd rpGeneric2nd, string neighbourhood)
        {
            //var listpupilsdata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();
            List<DatahubDataObj> listpupilsdata = Getlistpupil(rpGeneric2nd);

            var listneighbourhooddata = this.rpGeneric2nd.FindAll<ACCDataStore.Entity.DatahubProfile.NeighbourhoodObj>();
            var listdata = new List<DatahubDataObj>();
            if (neighbourhood != null)
            {
                listdata = (from a in listpupilsdata join b in listneighbourhooddata on a.CSS_Postcode equals b.CSS_Postcode where b.Neighbourhood.Contains(neighbourhood) select a).ToList();
            }
            return listdata;
        }

        protected DatahubData CreatDatahubdata(List<DatahubDataObj> listdata,string datahubcode)
        {
            var datahubdata = new DatahubData();

            if (listdata.Count() == 0)
            {
                datahubdata = null;
            }
            else
            {
                datahubdata.datacode = datahubcode;
                datahubdata.allpupils = listdata.Count(x => !x.SDS_Client_Ref.Equals(""));
                datahubdata.allFemalepupils = listdata.Count(x => x.Gender.ToLower().Equals("female"));
                datahubdata.allMalepupils = listdata.Count(x => x.Gender.ToLower().Equals("male"));
                datahubdata.all15pupils = listdata.Count(x => x.Age == 15);
                datahubdata.all16pupils = listdata.Count(x => x.Age == 16);
                datahubdata.all17pupils = listdata.Count(x => x.Age == 17);
                datahubdata.all18pupils = listdata.Count(x => x.Age == 18);
                datahubdata.all19pupils = listdata.Count(x => x.Age == 19);

                // Current positive
                datahubdata.schoolpupils = listdata.Count(x => x.Current_Status.ToLower().Equals("school pupil"));
                datahubdata.schoolpupilsintransition = listdata.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition"));
                //datahubdata.schoolpupilsmovedoutinscotland = listdata.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland"));
                datahubdata.pupilsinAtivityAgreement = listdata.Count(x => x.Current_Status.ToLower().Equals("activity agreement"));
                datahubdata.pupilsinEmployFundSt2 = listdata.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2"));
                datahubdata.pupilsinEmployFundSt3 = listdata.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3"));
                datahubdata.pupilsinEmployFundSt4 = listdata.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4"));
                datahubdata.pupilsinFullTimeEmployed = listdata.Count(x => x.Current_Status.ToLower().Equals("full-time employment"));
                datahubdata.pupilsinFurtherEdu = listdata.Count(x => x.Current_Status.ToLower().Equals("further education"));
                datahubdata.pupilsinHigherEdu = listdata.Count(x => x.Current_Status.ToLower().Equals("higher education"));
                datahubdata.pupilsinModernApprenship = listdata.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship"));
                datahubdata.pupilsinPartTimeEmployed = listdata.Count(x => x.Current_Status.ToLower().Equals("part-time employment"));
                datahubdata.pupilsinPersonalSkillDevelopment = listdata.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development"));
                datahubdata.pupilsinSelfEmployed = listdata.Count(x => x.Current_Status.ToLower().Equals("self-employed"));
                datahubdata.pupilsinTraining = listdata.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)"));
                datahubdata.pupilsinVolunteerWork = listdata.Count(x => x.Current_Status.ToLower().Equals("voluntary work"));

                // Non positive 
                //datahubdata.AvgWeekssinceLastPositiveDestination = listdata.Where(x => x.Weeks_since_last_Pos_Status != null).DefaultIfEmpty().Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status??"0"));
                var temp = listdata.Where(x => x.Weeks_since_last_Pos_Status != null).DefaultIfEmpty().ToList();

                datahubdata.AvgWeekssinceLastPositiveDestination = temp.First() == null ? 0.0 : temp.Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status ?? "0"));

                datahubdata.pupilsinCustody = listdata.Count(x => x.Current_Status.ToLower().Equals("custody"));
                datahubdata.pupilsinEconomically = listdata.Count(x => x.Current_Status.ToLower().Equals("economically inactive"));
                datahubdata.pupilsinUnavailableillHealth = listdata.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health"));
                datahubdata.pupilsinUnemployed = listdata.Count(x => x.Current_Status.ToLower().Equals("unemployed"));
                datahubdata.pupilsinUnknown = listdata.Count(x => x.Current_Status.ToLower().Equals("unknown"));
            }

            return datahubdata;
            //return new DatahubData();
        }

        protected DatahubData FormatDatahubdata(DatahubData listdata)
        {
            var datahubdata = new DatahubData();
            PropertyInfo[] properties = typeof(DatahubData).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!property.Name.Equals("datacode"))
                property.SetValue(datahubdata, 15);
            }

            return datahubdata;
        }

        [HttpPost]
        public JsonResult GetdataforHeatmap(string datasetname)
        {
            try
            {
  
                object data = new object();

                var Listdatahubdata = Session["Listdatahubdataforheatmap"] as List<DatahubData>;

                Listdatahubdata = Listdatahubdata.OrderBy(x => x.datacode).ToList();

                List<double> tempdata = new List<double>();

                if(datasetname.Equals("Participating")){

                    tempdata = Listdatahubdata.Select(x=>x.Participating()).ToList();
                }
                else if (datasetname.Equals("Not-Participating"))
                {
                    tempdata = Listdatahubdata.Select(x=>x.NotParticipating()).ToList();
                }
                else { 
                
                    tempdata = Listdatahubdata.Select(x=>x.Percentage(x.pupilsinUnknown)).ToList();
                }

                data = new
                {
                    datacode = Listdatahubdata.Select(x=>x.datacode).ToList(),
                    data = tempdata,
                    minimum = tempdata.Min(),
                    maximum = tempdata.Max(),
                };

                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return ThrowJSONError(ex);
            }
        }

        protected IList<DatahubData> GetDatahubdataAllZonecode(IGenericRepository2nd rpGeneric2nd)
        {
            List<DatahubData> listdatahubdata = new List<DatahubData>();
            
            var datahubdata = new DatahubData();

            var listzonecode = this.rpGeneric2nd.FindSingleColumnByNativeSQL("Select distinct Datazone from Neighbourhood_Postcodes1 t1 INNER JOIN Datahubdata t2 on  t1.CSS_Postcode = t2.CSS_Postcode ");

            IList<DatahubDataObj> listpupilsdata = this.rpGeneric2nd.QueryOver<DatahubDataAberdeen>().List<DatahubDataObj>();
            var listneighbourhooddata = this.rpGeneric2nd.FindAll<ACCDataStore.Entity.DatahubProfile.NeighbourhoodObj>();
            var listdata = new List<DatahubDataObj>();

            if (listzonecode != null)
            {
                foreach (var item in listzonecode)
                {
                    if (item != null)
                    {
                        listdata = (from a in listpupilsdata join b in listneighbourhooddata on a.CSS_Postcode equals b.CSS_Postcode where b.DataZone.Contains(item.ToString()) select a).ToList();
                        listdatahubdata.Add(CreatDatahubdata(listdata, item.ToString()));
                    }

                }
            }
            return listdatahubdata;
        }

        public ActionResult MapData()
        {
            //var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;
            var vmDatahubViewModel = new DatahubViewModel();
            vmDatahubViewModel.ListSchoolNameData = GetListSchoolname();
            return View("MapIndex", vmDatahubViewModel);
        }

        public ActionResult HeatMapData()
        {
            //var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;
            var vmDatahubViewModel = new DatahubViewModel();
            vmDatahubViewModel.ListDatasets = new List<string>() { "Participating","Not-Participating", "Unconfirmed" };

            Session["Listdatahubdataforheatmap"] = GetDatahubdataAllZonecode(this.rpGeneric2nd);

            return View("HeatMapIndex", vmDatahubViewModel);
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
                IList<School> temp = GetListSchoolname();
                School selectedschool = temp.Where(x => x.seedcode.Equals(keyvalue)).FirstOrDefault();
                string schname = "";

                var Schooldata = new DatahubData();

                if (keyname.ToLower().Equals("school"))
                {
                     //Schooldata = GetDatahubdatabySchoolcode(rpGeneric, keyvalue);
                     Schooldata = CreatDatahubdata(GetDatahubdatabySchoolcode(rpGeneric2nd, keyvalue), keyvalue);
                     schname = selectedschool == null ? "" : selectedschool.name;
                }
                else if (keyname.ToLower().Equals("zonecode"))
                {
                     //Schooldata = GetDatahubdatabyZonecode(rpGeneric, keyvalue);
                     Schooldata = CreatDatahubdata(GetDatahubdatabyZonecode(rpGeneric2nd, keyvalue), keyvalue);
                     schname = keyvalue;
                
                }
                else if (keyname.ToLower().Equals("neighbourhood"))
                {
                    //Schooldata = GetDatahubdatabyNeighbourhoods(rpGeneric, keyvalue);
                    Schooldata = CreatDatahubdata(GetDatahubdatabyNeighbourhoods(rpGeneric2nd, keyvalue), keyvalue);
                    schname = keyvalue;

                }
                var Abddata = CreatDatahubdata(GetDatahubdatabySchoolcode(rpGeneric2nd, "100"), "100");
                
                object data = new object();


                data = new
                {
                    dataTitle = "Destination -" + schname,
                    schoolname = schname,
                    searchcode = keyvalue,
                    searchby = keyname.ToLower(),
                    dataCategories = new string[] {"Participating","Not-Participating", "Unconfirmed"},
                    Schdata = new double[] { Schooldata.Participating(), Schooldata.NotParticipating(), Schooldata.Percentage(Schooldata.pupilsinUnknown) },
                    Abdcitydata = new double[] { Abddata.Participating(), Abddata.NotParticipating(), Abddata.Percentage(Abddata.pupilsinUnknown) }

                };

                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return ThrowJSONError(ex);
            }
        }

        protected List<DatahubDataObj> GetListpupilsbydataname(List<DatahubDataObj> listdata, string dataname)
        {
            var tempPupilslist= new List<DatahubDataObj>();
            // 
            if (dataname.ToLower().Equals("not-participating")) {
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("custody") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("economically inactive") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("unavailable - ill health") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("unemployed") select a).ToList());               
            }else if (dataname.ToLower().Equals("participating")) {
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("school pupil") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("school pupil - in transition") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("activity agreement") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("employability fund stage 2") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("employability fund stage 3") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("employability fund stage 4") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("full-time employment") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("further education") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("higher education") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("modern apprenticeship") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("part-time employment") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("personal/ skills development") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("self-employed") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("training (non ntp)") select a).ToList());
                tempPupilslist.AddRange((from a in listdata where a.Current_Status.ToLower().Equals("voluntary work") select a).ToList());
            }
            return tempPupilslist;
        }
        [AdminAuthentication]
        [Transactional]
        public ActionResult GetListpupils(string searchby, string code, string dataname)
        {
            var vmListpupilsViewModel = new DatahubViewModel();

            //var listdata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();
            List<DatahubDataObj> listdata = new List<DatahubDataObj>();

            if (searchby.ToLower().Equals("school")) {

                listdata = GetDatahubdatabySchoolcode(rpGeneric2nd, code);

                IList<School> temp = GetListSchoolname();
                School selectedschool = temp.Where(x => x.seedcode.Equals(code)).FirstOrDefault();

                vmListpupilsViewModel.selectedschool = selectedschool;

            }
            else if (searchby.ToLower().Equals("neighbourhood"))
            {

                listdata = GetDatahubdatabyNeighbourhoods(rpGeneric2nd, code);
                IList<School> temp = GetListNeighbourhoodsname(rpGeneric2nd);
                School selectedschool = temp.Where(x => x.seedcode.Equals(code)).FirstOrDefault();

                vmListpupilsViewModel.selectedschool = selectedschool;
            }
            else if (searchby.ToLower().Equals("zonecode"))
            {

                listdata = GetDatahubdatabyZonecode(rpGeneric2nd, code);
                School selectedschool = new School(code,code);

                vmListpupilsViewModel.selectedschool = selectedschool;
            }
            switch (dataname.ToLower()) {
                case "allclients":
                    listdata = (from a in listdata where a.SDS_Client_Ref!=null select a).ToList();
                    vmListpupilsViewModel.levercategory = "All Clients ";
                    break;
                case "males":
                    listdata = (from a in listdata where a.Gender.ToLower().Equals("male") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Males ";
                    break;
                case "females":
                    listdata = (from a in listdata where a.Gender.ToLower().Equals("female") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Females ";
                    break;
                case "pupils15":
                    listdata = (from a in listdata where a.Age == 15 select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils 15 ";
                    break;
                case "pupils16":
                    listdata = (from a in listdata where a.Age==16 select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils 16 ";
                    break;
                case "pupils17":
                    listdata = (from a in listdata where a.Age == 17 select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils 17 ";
                    break;
                case "pupils18":
                    listdata = (from a in listdata where a.Age == 18 select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils 18 ";
                    break;
                case "pupils19":
                    listdata = (from a in listdata where a.Age == 19 select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils 19 ";
                    break;
                case "schoolpupils":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("school pupil") select a).ToList();
                    vmListpupilsViewModel.levercategory = "School pupils ";
                    break;
                case "schoolpupilsintransition":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("school pupil - in transition") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in transition ";
                    break;
                case "schoolpupilsmovedoutinscotland":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("moved outwith scotland") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupil smoved out in scotland ";
                    break;
                case "pupilsinativityagreement":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("activity agreement") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in Ativity Agreement";
                    break;
                case "pupilsinemployfundst2":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("employability fund stage 2") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in Employability Fund Stage 2";
                    break;
                case "pupilsinemployfundst3":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("employability fund stage 3") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in Employability Fund Stage 3";
                    break;
                case "pupilsinemployfundst4":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("employability fund stage 4") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in Employability Fund Stage 4";
                    break;
                case "pupilsinfulltimeemployed":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("full-time employment") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in full-time employment ";
                    break;
                case "pupilsinfurtheredu":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("further education") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in further education ";
                    break;
                case "pupilsinhigheredu":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("higher education") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in higher education ";
                    break;
                case "pupilsinmodernapprenship":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("modern apprenticeship") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in modern apprenticeship ";
                    break;
                case "pupilsinparttimeemployed":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("part-time employment") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in part-time employment ";
                    break;
                case "pupilsinpersonalskilldevelopment":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("personal/ skills development") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in personal/ skills development ";
                    break;
                case "pupilsinselfemployed":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("self-employed") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in self-employed ";
                    break;
                case "pupilsintraining":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("training (non ntp)") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in training (non ntp)";
                    break;
                case "pupilsinvolunteerwork":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("voluntary work") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in voluntary work ";
                    break;
                case "pupilsincustody":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("custody") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in custody ";
                    break;
                case "pupilsineconomically":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("economically inactive") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in economically inactive";
                    break;
                case "pupilsinunavailableillhealth":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("unavailable - ill health") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in unavailable - ill health";
                    break;
                case "pupilsinunemployed":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("unemployed") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in Unemployed ";
                    break;
                case "pupilsinunknown":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("unknown") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Pupils in Unknown ";
                    break;
                case "not-participating":
                    listdata = GetListpupilsbydataname(listdata, dataname.ToLower());
                    vmListpupilsViewModel.levercategory = "Not-Participating ";
                    break;
                case "participating":
                    listdata = GetListpupilsbydataname(listdata, dataname.ToLower());
                    vmListpupilsViewModel.levercategory = "Participating ";
                    break;
                case "unconfirmed":
                    listdata = (from a in listdata where a.Current_Status.ToLower().Equals("unknown") select a).ToList();
                    vmListpupilsViewModel.levercategory = "Unconfirmed ";
                    break;
                case "allpupilsexcludemovedoutscotland":
                    listdata = (from a in listdata where a.SDS_Client_Ref != null select a).ToList().Except(from b in listdata where b.Current_Status.ToLower().Equals("moved outwith scotland") select b).ToList();
                    vmListpupilsViewModel.levercategory = "Allpupils Exclude Movedout Scotland ";
                    break;
            }

            vmListpupilsViewModel.Listpupils = listdata.OrderBy(x => x.Forename).ToList();

            Session["ListPupilsData"] = vmListpupilsViewModel.Listpupils;

            return View("Pupilslist", vmListpupilsViewModel);
        }


        public ActionResult Getpupilsdetails(string pupil)
        {

            var listdata = Session["ListPupilsData"] as List<DatahubDataObj>;

            DatahubDataObj tempobj = listdata.SingleOrDefault(x => x.SDS_Client_Ref.Equals(pupil));

            return View("PersonalDetails", tempobj);
        }

        [AdminAuthentication]
        [Transactional]
        public ActionResult SearchpupilbyName(string searchsubmitButton)
        {
            var vmListpupilsViewModel = new DatahubViewModel();

            var sForname = Request["forename"];
            var sSurename = Request["surname"];

            if (searchsubmitButton.ToLower().Equals("search") && (!sForname.Equals("") || !sSurename.Equals("")))
            {

                var listdata = this.rpGeneric2nd.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();
                List<DatahubDataObj> tempobj = listdata.Where(x => x.Forename.ToLower().Contains(sForname) && x.Surname.ToLower().Contains(sSurename)).ToList();
                vmListpupilsViewModel.Listpupils = tempobj;
            }
            else {
                vmListpupilsViewModel.Listpupils = null;
            }
            Session["ListPupilsData"] = vmListpupilsViewModel.Listpupils;

            School selectedschool = new School("Search Results", "Search Results");
            vmListpupilsViewModel.levercategory = "Search Results for " + sForname + " " + sSurename;
            vmListpupilsViewModel.selectedschool = selectedschool;

            return View("Pupilslist", vmListpupilsViewModel);


        }


        public ActionResult ExportExcel(string dataname, string schoolname)
        {
            var dataStream = GetWorkbookDataStream(GetData(), dataname,schoolname);
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LeaverExport.xlsx");
        }

        private DataTable GetData()
        {
            // simulate datatable
           var ListLeaverDestinationData = Session["ListPupilsData"] as List<DatahubDataObj>;

           DataTable dtResult = ListLeaverDestinationData.AsDataTable();             

           dtResult = ListLeaverDestinationData.AsDataTable();   

           return dtResult;
        }

        private MemoryStream GetWorkbookDataStream(DataTable dtResult, string dataname, string schoolname)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = schoolname; // use cell address in range
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

        [HttpPost]
        public JsonResult getJsonPupilList(string schoolname, string levercategory)
        {
            try
            {
                object oData = new object();

                var ListLeaverDestinationData = Session["ListPupilsData"] as List<DatahubDataObj>;



                oData = new
                    {
                        schoolname = schoolname,
                        levercategory = levercategory,
                        listpupils = ListLeaverDestinationData
                    };


                // use sName (AB24) to query data from database
                return Json(oData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return ThrowJSONError(ex);
            }
        }

        public JsonResult MainPieChartData()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            ViewModelParams selectionParams = Session["ViewModelParams"] as ViewModelParams;
            this.schoolSelection = Session["chartSelectedSchool"] as School;
            //List<DatahubDataObj> allStudentData = this.rpGeneric2nd.FindAll<DatahubDataObj>().ToList();
            List<DatahubDataObj> allStudentData = Getlistpupil(this.rpGeneric2nd);
            MainChartData combinedData = new MainChartData();
            object pieChartTotals = new
            {
                title = "Overall",
                female15 = (allStudentData.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 15)).ToList<DatahubDataObj>().Count(),
                male15 = (allStudentData.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 15)).ToList<DatahubDataObj>().Count(),
                female16 = (allStudentData.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 16)).ToList<DatahubDataObj>().Count(),
                male16 = (allStudentData.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 16)).ToList<DatahubDataObj>().Count(),
                female17 = (allStudentData.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 17)).ToList<DatahubDataObj>().Count(),
                male17 = (allStudentData.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 17)).ToList<DatahubDataObj>().Count(),
                female18 = (allStudentData.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 18)).ToList<DatahubDataObj>().Count(),
                male18 = (allStudentData.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 18)).ToList<DatahubDataObj>().Count(),
                female19 = (allStudentData.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 19)).ToList<DatahubDataObj>().Count(),
                male19 = (allStudentData.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 19)).ToList<DatahubDataObj>().Count(),
            };
            combinedData.totals = pieChartTotals;
            if (selectionParams.school != null)
            {
                List<DatahubDataObj> refined = allStudentData.Except(allStudentData.Where(z => z.School_Name == null)).ToList().Where(z => z.School_Name.Equals(this.schoolSelection.name)).ToList();
                object selectedChart = new
                {
                    title = this.schoolSelection.name,
                    female15 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 15)).ToList<DatahubDataObj>().Count(),
                    male15 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 15)).ToList<DatahubDataObj>().Count(),
                    female16 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 16)).ToList<DatahubDataObj>().Count(),
                    male16 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 16)).ToList<DatahubDataObj>().Count(),
                    female17 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 17)).ToList<DatahubDataObj>().Count(),
                    male17 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 17)).ToList<DatahubDataObj>().Count(),
                    female18 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 18)).ToList<DatahubDataObj>().Count(),
                    male18 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 18)).ToList<DatahubDataObj>().Count(),
                    female19 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 19)).ToList<DatahubDataObj>().Count(),
                    male19 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 19)).ToList<DatahubDataObj>().Count(),
                };
                combinedData.selected = selectedChart;
            }
            if (selectionParams.neighbourhood != null)
            {
                schoolSelection = Session["chartSelectedNeighbour"] as School;
                List<DatahubDataObj> refined = GetDatahubdatabyNeighbourhoods(this.rpGeneric2nd, schoolSelection.seedcode);
                object selectedChart = new
                {
                    title = schoolSelection.name,
                    female15 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 15)).ToList<DatahubDataObj>().Count(),
                    male15 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 15)).ToList<DatahubDataObj>().Count(),
                    female16 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 16)).ToList<DatahubDataObj>().Count(),
                    male16 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 16)).ToList<DatahubDataObj>().Count(),
                    female17 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 17)).ToList<DatahubDataObj>().Count(),
                    male17 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 17)).ToList<DatahubDataObj>().Count(),
                    female18 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 18)).ToList<DatahubDataObj>().Count(),
                    male18 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 18)).ToList<DatahubDataObj>().Count(),
                    female19 = (refined.Where(z => z.Gender.Equals("Female")).Where(z => z.Age == 19)).ToList<DatahubDataObj>().Count(),
                    male19 = (refined.Where(z => z.Gender.Equals("Male")).Where(z => z.Age == 19)).ToList<DatahubDataObj>().Count(),
                };
                combinedData.selected = selectedChart;
            }
            timer.Stop();
            combinedData.benchmarkResults = timer.ElapsedMilliseconds;
            return Json(combinedData, JsonRequestBehavior.AllowGet);
        }

        protected IList<DatahubDataObj> MonthOnMonthOverview(IGenericRepository2nd rpGeneric2nd, string type, int year)
        {
            string[] monthname = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            int month = Array.IndexOf(monthname, type) + 1;
            IList<DatahubDataObj> selectedMonth = rpGeneric2nd.QueryOver<DatahubDataAberdeen>()
                                    .Where(r => r.Data_Month == month && r.Data_Year == year)
                                    .List<DatahubDataObj>();
            return selectedMonth;
        }

        public JsonResult getBarChartData()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            ViewModelParams selectionParams = Session["ViewModelParams"] as ViewModelParams;
            List<DatahubDataObj> allStudentData = Getlistpupil(this.rpGeneric2nd);
            DatahubData CityData = CreatDatahubdata(allStudentData, "100");
            MainChartData combinedData = new MainChartData();
            combinedData.totals = new
            {
                name = "Aberdeen city",
                participating = CityData.Participating(), 
                notParticipating = CityData.NotParticipating(),
                unknown = CityData.Percentage(CityData.pupilsinUnknown)
            };
            if (selectionParams.school != null)
            {
                schoolSelection = Session["chartSelectedSchool"] as School;
                List<DatahubDataObj> refined = GetDatahubdatabySchoolcode(this.rpGeneric2nd, schoolSelection.seedcode);
                DatahubData SchoolData = CreatDatahubdata(refined, schoolSelection.seedcode);


                combinedData.selected = new
                {
                    name = schoolSelection.name,
                    participating = SchoolData.Participating(),
                    notParticipating = SchoolData.NotParticipating(),
                    unknown = SchoolData.Percentage(SchoolData.pupilsinUnknown)
                };
            }
            if (selectionParams.neighbourhood != null)
            {
                schoolSelection = Session["chartSelectedNeighbour"] as School;
                List<DatahubDataObj> refined = GetDatahubdatabyNeighbourhoods(this.rpGeneric2nd, schoolSelection.seedcode);
                DatahubData SchoolData = CreatDatahubdata(refined, schoolSelection.seedcode);


                combinedData.selected = new
                {
                    name = schoolSelection.name,
                    participating = SchoolData.Participating(),
                    notParticipating = SchoolData.NotParticipating(),
                    unknown = SchoolData.Percentage(SchoolData.pupilsinUnknown)
                };
            }
            timer.Stop();
            combinedData.benchmarkResults = timer.ElapsedMilliseconds;
            return Json(combinedData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult monthlyHistogram()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            BenchmarkAjax wrapper = new BenchmarkAjax();
            ViewModelParams selectionParams = Session["ViewModelParams"] as ViewModelParams;
            List<HistogramSeriesData> allseriesoutput = new List<HistogramSeriesData>();
            this.schoolSelection = Session["chartSelectedSchool"] as School;
            int numberofseries = 1;
            if (selectionParams == null)
            {
                selectionParams = new ViewModelParams();
            }
            if (selectionParams.school != null || selectionParams.neighbourhood != null)
            {
                numberofseries = 2;
            }
            for (int j = 0; j < numberofseries; j++)
            {
                List<DatahubData> allSeries = new List<DatahubData>();
                string seriesName = "Aberdeen city";
                string[] monthname = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                int indexofmonths = Array.IndexOf(monthname, DateTime.Now.ToString("MMM"));
                int year = int.Parse(DateTime.Now.ToString("yyyy")) - 1;
                for (int i = 0; i < 12; i++)
                {
                    indexofmonths++;
                    if (indexofmonths > 11)
                    {
                        indexofmonths = 0;
                        year++;
                    }
                    IList<DatahubDataObj> comparison = MonthOnMonthOverview(rpGeneric2nd, monthname[indexofmonths], year);
                    if (selectionParams.school != null && j > 0)
                    {
                        seriesName = schoolSelection.name;
                        comparison = comparison.Where(x => !String.IsNullOrWhiteSpace(x.SEED_Code)).Where(x => x.SEED_Code.Equals(schoolSelection.seedcode)).ToList<DatahubDataObj>();
                    }
                    if (selectionParams.neighbourhood != null && j > 0)
                    {
                        schoolSelection = Session["chartSelectedNeighbour"] as School;
                        seriesName = schoolSelection.name;
                        comparison = GetDatahubdatabyNeighbourhoods(this.rpGeneric2nd, schoolSelection.seedcode);
                    }

                    
                    allSeries.Add(CreatDatahubdata(comparison.CastTo<List<DatahubDataObj>>(), monthname[indexofmonths]));
                }
                HistogramSeriesData jsonOut = new HistogramSeriesData();
                jsonOut.months = new List<string>();
                jsonOut.participating = new List<double>();
                jsonOut.notParticipating = new List<double>();
                jsonOut.unknown = new List<double>();
                jsonOut.name = seriesName;
                foreach (DatahubData month in allSeries)
                {
                    jsonOut.months.Add(month ==null? "":month.datacode);
                    jsonOut.participating.Add(month == null ? -1.00 : Math.Round(month.Participating(), 2));
                    jsonOut.notParticipating.Add(month == null ? -1.00 : Math.Round(month.NotParticipating(), 2));
                    jsonOut.unknown.Add(month == null ? -1.00 : Math.Round(month.Percentage(month.pupilsinUnknown), 2));
                }
                allseriesoutput.Add(jsonOut);
            }
            wrapper.chart = allseriesoutput;
            timer.Stop();
            wrapper.benchmarkResults = timer.ElapsedMilliseconds;
            return Json(wrapper, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllSchoolComparison()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            BenchmarkAjax wrapper = new BenchmarkAjax();
            List<PosNegSchoolList> tableSummaryData = Session["AllSchoolComparisonData"] as List<PosNegSchoolList>;
            Session["AllSchoolComparisonData"] = null;
            wrapper.data = tableSummaryData;
            timer.Stop();
            wrapper.benchmarkResults = timer.ElapsedMilliseconds;
            return Json(wrapper, JsonRequestBehavior.AllowGet);
        }

        // BEGIN MESS
        

        public ActionResult ApiTestQQQ()
        {
            //var currentSummary = this.rpGeneric2nd.QueryOver<AberdeenSummary>().Where(x => x.type == "Council" && x.dataMonth == 08 && x.dataYear == 2016).SingleOrDefault();
            //SummaryDataViewModel vm = new SummaryDataViewModel(currentSummary);
            //var participating = vm.Participating();
            //var not_participating = vm.NotParticipating();
            //var unknown = vm.Percentage(vm.allPupilsInUnknown);
            //var lazy = participating + not_participating + unknown;

            //var data = CreatDatahubdata(GetDatahubdatabySchoolcode(rpGeneric2nd, "100"), "100");
            //var p = data.Participating();
            //var n = data.NotParticipating();
            //var u = data.Percentage(data.pupilsinUnknown);

            //var lazy2 = p + n + u;

            //var aa = Helper.GetSummaryDataForAllSchools("S12000033", 08, 2016);
            //var pre = Helper._Tester<AberdeenSummary>("Aberdeen City");
            //Type t = Type.GetType(GlasgowSummary);
            //var result = typeof(SummaryDataHelper).GetMethod("GetSummaryDataForAllDataZones").MakeGenericMethod(new GlasgowSummary()).Invoke(null, null);

            var Helpme = new CouncilHelper(rpGeneric2nd);

            IList<School> sc = Helpme.ByName("Aberdeen City").GetSchoolsList();

            var a = Helpme.ByName("Aberdeen City").GetSummaryDataForAllSchools(08, 2016);
            var b = Helpme.ByName("glasgow City").GetSummaryDataForAllSchools(08, 2016);
            var c = Helpme.ByName("Aberdeen City").GetSummaryDataForCouncil(08, 2016);
            var d = Helpme.ByName("Glasgow City").GetSummaryDataForCouncil(08, 2016);
            var e = Helpme.ByName("Aberdeen City").GetSummaryDataForAllDataZones(08, 2016);
            var f = Helpme.ByName("Glasgow City").GetSummaryDataForAllDataZones(08, 2016);
            var g = Helpme.ByName("Aberdeen City").GetSummaryDataForAllIntermediateZones(08, 2016);
            var h = Helpme.ByName("Glasgow City").GetSummaryDataForAllIntermediateZones(08, 2016);
            var i = Helpme.ByName("Aberdeen City").GetSummaryDataForSingleDataZone("S01000001",08, 2016);


            //var r = new SummaryDataHelperNew<GlasgowSummary>(rpGeneric2nd).GetSummaryDataForCouncil(08, 2016);
            //var z = new SummaryDataHelperNew<AberdeenSummary>(rpGeneric2nd).GetSummaryDataForCouncil(08, 2016);

            //var qq = Helpme.ByName(rpGeneric2nd, "glasgow City").GetSummaryDataForCouncil(08, 2016);


            //var a = Helper.GetSummaryDataForAllDataZones<AberdeenSummary>(08, 2016);
            //var b = Helper.GetSummaryDataForAllIntermediateZones<AberdeenSummary>(08, 2016);
            //var c = Helper.GetSummaryDataForAllSchools<AberdeenSummary>("S12000033", 08, 2016);
            //var d = Helper.GetSummaryDataForCouncil<AberdeenSummary>("S12000033", 08, 2016);
            //var e = Helper.GetSummaryDataForSingleDataZone<AberdeenSummary>("S01000011", 08, 2016);
            //var f = Helper.GetSummaryDataForSingleIntermediateZone<AberdeenSummary>("S02000033", 08, 2016);
            //var g = Helper.GetSummaryDataForSingleSchool<AberdeenSummary>("5244439", 08, 2016);
            return Json("" , JsonRequestBehavior.AllowGet);

            
        }

        // END MESS
        // BEGIN MERGE
        public ActionResult GetGeoJSON(string id)
        {
            // Id shouldn't be null
            if (id != null)
            {
                // A string to hold our GeoJSON content as recieved by the database
                string result = null;

                // Check the id type and query the corresponding table
                try
                {
                    if (id.StartsWith("S01")) { result = this.rpGeneric2nd.GetById<DataZoneObj>(id).GeoJSON; }
                    else if (id.StartsWith("S02")) { result = this.rpGeneric2nd.GetById<IntermediateZoneObj>(id).GeoJSON; }
                    else if (id.StartsWith("S12")) { result = this.rpGeneric2nd.GetById<CouncilObj>(id).GeoJSON; }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return new HttpStatusCodeResult(404, "Item Not Found");
                }

                return new ContentResult { Content = result, ContentType = "application/json" };

            }

            return new HttpStatusCodeResult(404, "Missing parameter!");
        }

        public ActionResult GetDatazonesByCouncilName(string councilName)
        {
            string councilId = null;

            //Validate and get corresponding council id
            try
            {
                councilId = this.rpGeneric2nd.QueryOver<CouncilObj>()
                               .Where(r => r.Name == councilName)
                               .SingleOrDefault()
                               .Reference;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.ToString());
                return Content(null);
            }

            IList<string> result = getDatazoneListByCouncilId(councilId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetDatazonesByCouncilId(string councilId)
        {
            // TODO: Should validate councilId
            IList<string> result = getDatazoneListByCouncilId(councilId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IList<string> getDatazoneListByCouncilId(string councilId)
        {
            var datazoneList = this.rpGeneric2nd.QueryOver<DataZoneObj>()
                .Where(r => r.Reference_Council == councilId)
                .List<DataZoneObj>();

            IList<string> result = new Collection<string>();
            foreach (var item in datazoneList)
            {
                result.Add(item.Reference);
            }

            return result;
        }

        public ActionResult GetNeighbourhoodIdsByCouncilId(string councilId)
        {
            // TODO: Should validate councilId
            IList<string> result = getNeighbourhoodIdListByCouncilId(councilId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IList<string> getNeighbourhoodIdListByCouncilId(string councilId)
        {
            var neighbourhoodList = this.rpGeneric2nd.QueryOver<IntermediateZoneObj>()
                .Where(r => r.Reference_Council == councilId)
                .List<IntermediateZoneObj>();

            IList<string> result = new Collection<string>();
            foreach (var item in neighbourhoodList)
            {
                result.Add(item.Reference);
            }

            return result;
        }
        // Comparisons
        //public ActionResult Comparisons()
        //{
        //    var result = new ComparisonsViewModel();

        //    IList<DatahubData> list = new Collection<DatahubData>();
        //    result.ListSchoolNameData = GetListSchoolnameGlasgow();
        //    result.ListNeighbourhoodsName = GetListNeighbourhoodsname(rpGeneric);
        //    var city_data = CreatDatahubdata(GetDatahubdatabySchoolcode(rpGeneric, "100"), "100");
        //    city_data.name = "Glasgow";
        //    list.Add(city_data);
        //    result.ListSelectionData = list;
        //    return View(result);
        //}

        //public ActionResult Test1(ICollection<String> list, string type)
        //{
        //    IList<School> selection = new Collection<School>();
        //    if (type != null)
        //    {
        //        if (type.ToLower().Equals("neighbourhoods"))
        //        {
        //            selection = GetListNeighbourhoodsnameGlasgow(rpGeneric2nd);
        //        }
        //        if (type.ToLower().Equals("schools"))
        //        {
        //            selection = GetListSchoolnameGlasgow();
        //        }
        //    }

        //    IList<DatahubData> result = new Collection<DatahubData>();
        //    var city_data = CreatDatahubdata(GetDatahubdatabySchoolcode(rpGeneric2nd, "100"), "100");
        //    city_data.name = "Glasgow"; // To do
        //    result.Add(city_data);


        //    //foreach (var item in list)
        //    //{ 
        //    //    foreach (School school in schools)
        //    //    {
        //    //        if (!list.Contains(school.seedcode)) return Content("Error");
        //    //    }
        //    //}

        //    foreach (var item in list)
        //    {
        //        var match = selection.FirstOrDefault(p => p.seedcode == item);
        //        if (match == null) return Content("Error");
        //    }




        //    foreach (var item in list)
        //    {
        //        if (type.ToLower().Equals("neighbourhoods"))
        //        {
        //            var data = CreatDatahubdata(GetDatahubdatabyNeighbourhoods(rpGeneric2nd, item), item);
        //            var match = selection.FirstOrDefault(p => p.seedcode == item);

        //            data.name = match.name;

        //            result.Add(data);
        //        };
        //        if (type.ToLower().Equals("schools"))
        //        {
        //            var data = CreatDatahubdata(GetDatahubdatabySchoolcode(rpGeneric2nd, item), item);
        //            var match = selection.FirstOrDefault(p => p.seedcode == item);

        //            data.name = match.name;

        //            result.Add(data);
        //        };

        //    }
        //    return PartialView("_DataTables", result);
        //}

        protected IList<School> GetListSchoolnameGlasgow()
        {
            List<School> temp = new List<School>();
            temp.Add(new School("8448647", "Abercorn School"));
            temp.Add(new School("8431930", "All Saints Secondary School"));
            temp.Add(new School("1003135", "Ashton Secondary School"));
            temp.Add(new School("8458138", "Bannerman High School"));
            temp.Add(new School("8432031", "Bellahouston Academy"));
            temp.Add(new School("8442347", "Cardinal Winning Secondary"));
            temp.Add(new School("8440042", "Cartvale School"));
            temp.Add(new School("8434034", "Castlemilk High School"));
            temp.Add(new School("8432236", "Cleveden Secondary School"));
            temp.Add(new School("8459932", "Drumchapel High School"));
            temp.Add(new School("8433232", "Eastbank Academy"));
            temp.Add(new School("8414041", "Enhanced Vocational Inclusion Programme (EVIP)"));
            temp.Add(new School("8432627", "Glasgow Gaelic Secondary School"));
            temp.Add(new School("8433836", "Govan High School"));
            temp.Add(new School("8441243", "Hazelwood School"));
            temp.Add(new School("8434239", "Hillhead High School"));
            temp.Add(new School("8434336", "Hillpark Secondary School"));
            temp.Add(new School("8440948", "Hollybrook School"));
            temp.Add(new School("8434530", "Holyrood Secondary School"));
            temp.Add(new School("8434638", "Hyndland Secondary School"));
            temp.Add(new School("8458731", "John Paul Academy"));
            temp.Add(new School("8470332", "Jordanhill School"));
            temp.Add(new School("8435138", "Kings Park Secondary School"));
            temp.Add(new School("8435634", "Knightswood Secondary School"));
            temp.Add(new School("8443645", "Linburn Academy"));
            temp.Add(new School("8435731", "Lochend Community High School"));
            temp.Add(new School("8435839", "Lourdes Secondary School"));
            temp.Add(new School("8452741", "Middlefield Residential School"));
            temp.Add(new School("8443742", "Newhills School"));
            temp.Add(new School("8436134", "Notre Dame High School (Glasgow)"));
            temp.Add(new School("8441340", "Parkhill School"));
            temp.Add(new School("8436339", "Rosshall Academy"));
            temp.Add(new School("8436932", "Shawlands Academy"));
            temp.Add(new School("8437130", "Smithycroft Secondary School"));
            temp.Add(new School("8431639", "Springburn Academy"));
            temp.Add(new School("8437238", "St Andrews RC Secondary"));
            temp.Add(new School("8438331", "St Margaret Mary's Secondary School"));
            temp.Add(new School("8438439", "St Mungo's Academy"));
            temp.Add(new School("8442843", "St Oswald's School"));
            temp.Add(new School("8432139", "St Paul's High School"));
            temp.Add(new School("8438730", "St Roch's Secondary School"));
            temp.Add(new School("8438838", "St Thomas Aquinas Secondary School"));
            temp.Add(new School("1000241", "Westmuir High School"));
            temp.Add(new School("8439532", "Whitehill Secondary School"));
            return temp;
        }

        public JsonResult GetdataforHeatmapDatazone(string datasetname)
        {
            IList<SummaryDataViewModel> allSummaryDataZoneVM = Helper.GetSummaryDataForAllDataZones<AberdeenSummary>(08, 2016);
            IList<decimal> percentageData = new Collection<decimal>();

            if (datasetname.Equals("Participating"))
            {

                percentageData = allSummaryDataZoneVM.Select(x => x.Participating()).ToList();
            }
            else if (datasetname.Equals("Not-Participating"))
            {
                percentageData = allSummaryDataZoneVM.Select(x => x.NotParticipating()).ToList();
            }
            else
            {

                percentageData = allSummaryDataZoneVM.Select(x => x.Percentage(x.allPupilsInUnknown)).ToList();
            }

            var result = new
            {
                datacode = allSummaryDataZoneVM.Select(x => x.dataCode).ToList(),
                data = percentageData,
                minimum = percentageData.Min(),
                maximum = percentageData.Max(),
            };


            return Json(result, JsonRequestBehavior.AllowGet);
            //try
            //{

            //    if (datasetname.Equals("Participating"))
            //    {

            //        tempdata = Listdatahubdata.Select(x => x.Participating()).ToList();
            //    }
            //    else if (datasetname.Equals("Not-Participating"))
            //    {
            //        tempdata = Listdatahubdata.Select(x => x.NotParticipating()).ToList();
            //    }
            //    else
            //    {

            //        tempdata = Listdatahubdata.Select(x => x.Percentage(x.pupilsinUnknown)).ToList();
            //    }

            //    data = new
            //    {
            //        datacode = Listdatahubdata.Select(x => x.datacode).ToList(),
            //        data = tempdata,
            //        minimum = tempdata.Min(),
            //        maximum = tempdata.Max(),
            //    };

            //    return Json(data, JsonRequestBehavior.AllowGet);

            //}
            //catch (Exception ex)
            //{
            //    return ThrowJSONError(ex);
            //}
        }

    }
}