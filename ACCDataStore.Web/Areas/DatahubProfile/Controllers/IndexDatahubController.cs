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

namespace ACCDataStore.Web.Areas.DatahubProfile.Controllers
{
    public class IndexDatahubController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexDatahubController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        private School schoolSelection;

        public IndexDatahubController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
            this.schoolSelection = new School("", "");
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
        public ActionResult Index(string schoolsubmitButton, string neighbourhoodssubmitButton)
        {
            //var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            
            IList<School> allSchools = GetListSchoolname();

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
            DatahubViewModel viewModel = getPageViewModel(schoolsubmitButton, neighbourhoodssubmitButton);
            viewModel.summaryTableData = tableSummaryData;
            ViewModelParams pageViewModelParams = new ViewModelParams();
            pageViewModelParams.school = schoolsubmitButton;
            pageViewModelParams.neighbourhood = neighbourhoodssubmitButton;
            Session["ViewModelParams"] = pageViewModelParams;
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
    
                List<DatahubDataObj> listdata = this.rpGeneric2nd.FindAll<DatahubDataObj>().ToList() ;
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


            return datahubdata;
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

            var listpupilsdata = this.rpGeneric2nd.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();
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
            if (this.schoolSelection != null)
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
            return Json(combinedData, JsonRequestBehavior.AllowGet);
        }

        public List<DatahubDataObj> MonthOnMonthOverview(IGenericRepository2nd rpGeneric2nd, string type)
        {
            List<DatahubDataObj> selectedMonth = new List<DatahubDataObj>();
            switch (type)
            {
                case "Jan":
                    selectedMonth = rpGeneric2nd.FindAll<Month1>().ToList<DatahubDataObj>();
                    break;
                case "Feb":
                    selectedMonth = rpGeneric2nd.FindAll<Month2>().ToList<DatahubDataObj>();
                    break;
                case "Mar":
                    selectedMonth = rpGeneric2nd.FindAll<Month3>().ToList<DatahubDataObj>();
                    break;
                case "Apr":
                    selectedMonth = rpGeneric2nd.FindAll<Month4>().ToList<DatahubDataObj>();
                    break;
                case "May":
                    selectedMonth = rpGeneric2nd.FindAll<Month5>().ToList<DatahubDataObj>();
                    break;
                case "Jun":
                    selectedMonth = rpGeneric2nd.FindAll<Month6>().ToList<DatahubDataObj>();
                    break;
                case "Jul":
                    selectedMonth = rpGeneric2nd.FindAll<Month7>().ToList<DatahubDataObj>();
                    break;
                case "Aug":
                    selectedMonth = rpGeneric2nd.FindAll<Month8>().ToList<DatahubDataObj>();
                    break;
                case "Sep":
                    selectedMonth = rpGeneric2nd.FindAll<Month9>().ToList<DatahubDataObj>();
                    break;
                case "Oct":
                    selectedMonth = rpGeneric2nd.FindAll<Month10>().ToList<DatahubDataObj>();
                    break;
                case "Nov":
                    selectedMonth = rpGeneric2nd.FindAll<Month11>().ToList<DatahubDataObj>();
                    break;
                case "Dec":
                    selectedMonth = rpGeneric2nd.FindAll<Month12>().ToList<DatahubDataObj>();
                    break;
            }
            return selectedMonth;
        }

        public JsonResult getBarChartData()
        {
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
            return Json(/*getPageViewModel(selectionParams.school, selectionParams.neighbourhood)*/ combinedData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult monthlyHistogram()
        {
            List<HistogramSeriesData> allseriesoutput = new List<HistogramSeriesData>();
            this.schoolSelection = Session["chartSelectedSchool"] as School;
            int numberofseries = 1;
            if (schoolSelection != null)
            {
                numberofseries = 2;
            }
            for (int j = 0; j < numberofseries; j++)
            {
                List<DatahubData> allSeries = new List<DatahubData>();
                string seriesName = "Aberdeen city";
                string[] monthname = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                int indexofmonths = Array.IndexOf(monthname, DateTime.Now.ToString("MMM"));
                for (int i = 0; i < 12; i++)
                {
                    indexofmonths++;
                    if (indexofmonths > 11)
                    {
                        indexofmonths = 0;
                    }
                    List<DatahubDataObj> comparison = MonthOnMonthOverview(rpGeneric2nd, monthname[indexofmonths]);
                    if (schoolSelection != null && j > 0)
                    {
                        seriesName = schoolSelection.name;
                        comparison = comparison.Where(x => !String.IsNullOrWhiteSpace(x.SEED_Code)).Where(x => x.SEED_Code.Equals(schoolSelection.seedcode)).ToList<DatahubDataObj>();
                    }
                    allSeries.Add(CreatDatahubdata(comparison, monthname[indexofmonths]));
                }
                HistogramSeriesData jsonOut = new HistogramSeriesData();
                jsonOut.months = new List<string>();
                jsonOut.participating = new List<double>();
                jsonOut.notParticipating = new List<double>();
                jsonOut.unknown = new List<double>();
                jsonOut.name = seriesName;
                foreach (DatahubData month in allSeries)
                {
                    jsonOut.months.Add(month.datacode);
                    jsonOut.participating.Add(Math.Round(month.Participating(), 2));
                    jsonOut.notParticipating.Add(Math.Round(month.NotParticipating(), 2));
                    jsonOut.unknown.Add(Math.Round(month.Percentage(month.pupilsinUnknown), 2));
                }
                allseriesoutput.Add(jsonOut);
            }
            return Json(allseriesoutput, JsonRequestBehavior.AllowGet);
        }

    }
}