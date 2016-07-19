using ACCDataStore.Entity;
using ACCDataStore.Web.Helpers;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class IndexSchoolProfilesController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        private IndexPrimarySchoolProfilesViewModel vmIndexPrimarySchoolProfilesModel;
        private IndexSecondarySchoolProfilesViewModel vmIndexSecondarySchoolProfilesModel;
        private IndexSecondarySchoolProfilesViewModel vmIndexSpecialSchoolProfilesModel;

        public IndexSchoolProfilesController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd; //connect to accdatastore database in MySQL
            this.vmIndexPrimarySchoolProfilesModel = new IndexPrimarySchoolProfilesViewModel();
            this.vmIndexSecondarySchoolProfilesModel = new IndexSecondarySchoolProfilesViewModel();
            this.vmIndexSpecialSchoolProfilesModel = new IndexSecondarySchoolProfilesViewModel();

        }

        // GET: SchoolProfiles/IndexSchoolProfiles
        public ActionResult Index()
        {
           // this.vmIndexPrimarySchoolProfilesModel.listAllPupils = GetListAllPupils(rpGeneric2nd);
            return View("Home");
        }

        public ActionResult SchoolWebsites()
        {
            // this.vmIndexPrimarySchoolProfilesModel.listAllPupils = GetListAllPupils(rpGeneric2nd);
            return View("SchoolWebsites");
        }

        public ActionResult IndexPrimaryProfiles(string sSchoolType)
        {
            //get data ready for set up profiles
            vmIndexPrimarySchoolProfilesModel.listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listSelectedSchoolname = vmIndexPrimarySchoolProfilesModel.listSchoolname.Where(x => x.seedcode.Equals("5237521")).ToList();
            vmIndexPrimarySchoolProfilesModel.listYears = GetListYear();
            vmIndexPrimarySchoolProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.DicStage = GetDicStage(rpGeneric2nd, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.DicFreeMeal = GetDicFreeSchoolMeal();
            vmIndexPrimarySchoolProfilesModel.DicLookedAfter = GetDicLookAfter();
            Session["vmIndexPrimarySchoolProfilesModel"] = vmIndexPrimarySchoolProfilesModel;
            return View("IndexPrimarySchool", vmIndexPrimarySchoolProfilesModel);
        }

        public ActionResult IndexSecondaryProfiles(string sSchoolType)
        {

            vmIndexSecondarySchoolProfilesModel.listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listSelectedSchoolname = vmIndexSecondarySchoolProfilesModel.listSchoolname.Where(x => x.seedcode.Equals("5244439")).ToList();
            vmIndexSecondarySchoolProfilesModel.listYears = GetListYear();
            vmIndexSecondarySchoolProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexSecondarySchoolProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexSecondarySchoolProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            vmIndexSecondarySchoolProfilesModel.DicStage = GetDicStage(rpGeneric2nd, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.DicFreeMeal = GetDicFreeSchoolMeal();
            vmIndexSecondarySchoolProfilesModel.DicLookedAfter = GetDicLookAfter();
            Session["vmIndexSecondarySchoolProfilesModel"] = vmIndexSecondarySchoolProfilesModel;
            return View("IndexSecondarySchool", vmIndexSecondarySchoolProfilesModel);
        }

        public ActionResult IndexSpecialProfiles(string sSchoolType)
        {

            vmIndexSpecialSchoolProfilesModel.listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listSelectedSchoolname = vmIndexSpecialSchoolProfilesModel.listSchoolname.Where(x => x.seedcode.Equals("5245044")).ToList();
            vmIndexSpecialSchoolProfilesModel.listYears = GetListYear();
            vmIndexSpecialSchoolProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexSpecialSchoolProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexSpecialSchoolProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            vmIndexSpecialSchoolProfilesModel.DicStage = GetDicStage(rpGeneric2nd, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.DicFreeMeal = GetDicFreeSchoolMeal();
            vmIndexSpecialSchoolProfilesModel.DicLookedAfter = GetDicLookAfter();
            Session["vmIndexSpecialSchoolProfilesModel"] = vmIndexSpecialSchoolProfilesModel;
            return View("IndexSpecialSchool", vmIndexSpecialSchoolProfilesModel);
        }


        public ActionResult GetPrimaryProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<PIPSObj> listPIPSPupils = new List<PIPSObj>();
            List<School> listSelectedSchoolname = new List<School>();
            bool schoolIsSelected = false;
            bool yesrIsSelected = false;         
            Year selectedYear = null;

            vmIndexPrimarySchoolProfilesModel = Session["vmIndexPrimarySchoolProfilesModel"] as IndexPrimarySchoolProfilesViewModel;
            List<School> templistSchoolname = vmIndexPrimarySchoolProfilesModel.listSchoolname;
            List<Year> templistYears = vmIndexPrimarySchoolProfilesModel.listYears;



            if (Request["listSelectedSchoolname"] != null)
            {
                schoolIsSelected = true;
                //get CostCentreKey from dropdownlist in UI
                templistSelectedSchoolname = Request["listSelectedSchoolname"].Split(',').ToList();
                //select selected CostCentre from dropdownlist in UI
                listSelectedSchoolname = templistSchoolname.Where(x => templistSelectedSchoolname.Any(y => y.Contains(x.seedcode))).ToList();
            }

            if (Request["selectedYear"] != null)
            {
                yesrIsSelected = true;
                string year = Request["selectedYear"].ToString();
                selectedYear = templistYears.Where(x => x.year.Contains(year)).FirstOrDefault();
            }

            if (schoolIsSelected && yesrIsSelected)
            {
                listAllPupils = GetListAllPupils(rpGeneric2nd, selectedYear, sSchoolType);
                listPIPSPupils = GetPIPSPupils(rpGeneric2nd, selectedYear, listSelectedSchoolname);          
            }

            //create profiletitle
            string tempProfiletitle = "";
            foreach (var item in listSelectedSchoolname)
            {
                tempProfiletitle = tempProfiletitle + " /"+ item.name;
            
            }
            vmIndexPrimarySchoolProfilesModel.profiletitle = tempProfiletitle;

           //store selected school into view model
            vmIndexPrimarySchoolProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexPrimarySchoolProfilesModel.selectedYear = selectedYear;
            vmIndexPrimarySchoolProfilesModel.listAllPupils = listAllPupils;
            vmIndexPrimarySchoolProfilesModel.listPIPSPupils = listPIPSPupils;
            //setting english data and table
            List<DataSeries> temp = GetDataSeries("englishlevel", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesEnglishLevel = temp;
            //vmIndexPrimarySchoolProfilesModel.englishLevelDataTable = GenerateTransposedTable(CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English"));
            vmIndexPrimarySchoolProfilesModel.dataTableEnglishLevel = CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English", "percentage");
            //setting ethnic data and table
            temp = GetDataSeries("ethnicity", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableEthnicBackground = CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEthnicBG, "Ethnicity", "percentage");
            //setting Nationality data and table
            temp = GetDataSeries("nationality", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesNationality = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableNationality = CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicNationalIdentity, "Nationality", "percentage");
            //setting Stage data and table
            temp = GetDataSeries("stage", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesStage = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexPrimarySchoolProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetDataSeries("freemeal", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableFreeSchoolMeal = CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage");
            //setting LookAfter data and table
            temp = GetDataSeries("lookafter", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableLookedAfter = CreateDataTaleWithTotal(temp, vmIndexPrimarySchoolProfilesModel.DicLookedAfter, "Looked After Children", "percentage");
            //setting PIPS dataseries and datatable         
            temp = GetPIPsDataSeries(listPIPSPupils,listSelectedSchoolname, selectedYear);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesPIPS = temp;
            vmIndexPrimarySchoolProfilesModel.dataTablePIPS = CreatePIPSDataTale(temp);

            //vmIndexPrimarySchoolProfilesModel.jsondata = JsonConvert.SerializeObject(tempdt, Formatting.Indented); ;
            Session["vmIndexPrimarySchoolProfilesModel"] = vmIndexPrimarySchoolProfilesModel;
            return View("IndexPrimarySchool", vmIndexPrimarySchoolProfilesModel);
        }

        public ActionResult GetSecondaryProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<School> listSelectedSchoolname = new List<School>();
            bool schoolIsSelected = false;
            bool yesrIsSelected = false;
            Year selectedYear = null;

            vmIndexSecondarySchoolProfilesModel = Session["vmIndexSecondarySchoolProfilesModel"] as IndexSecondarySchoolProfilesViewModel;
            List<School> templistSchoolname = vmIndexSecondarySchoolProfilesModel.listSchoolname;
            List<Year> templistYears = vmIndexSecondarySchoolProfilesModel.listYears;

            if (Request["listSelectedSchoolname"] != null)
            {
                schoolIsSelected = true;
                //get CostCentreKey from dropdownlist in UI
                templistSelectedSchoolname = Request["listSelectedSchoolname"].Split(',').ToList();
                //select selected CostCentre from dropdownlist in UI
                listSelectedSchoolname = templistSchoolname.Where(x => templistSelectedSchoolname.Any(y => y.Contains(x.seedcode))).ToList();
            }

            if (Request["selectedYear"] != null)
            {
                yesrIsSelected = true;
                string year = Request["selectedYear"].ToString();
                selectedYear = templistYears.Where(x => x.year.Contains(year)).FirstOrDefault();
            }

            if (schoolIsSelected && yesrIsSelected)
            {
                listAllPupils = GetListAllPupils(rpGeneric2nd, selectedYear, sSchoolType);
            }

            string tempProfiletitle = "";
            foreach (var item in listSelectedSchoolname)
            {
                tempProfiletitle = tempProfiletitle + " /" + item.name;

            }



            vmIndexSecondarySchoolProfilesModel.profiletitle = tempProfiletitle;

            //store selected school into view model
            vmIndexSecondarySchoolProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexSecondarySchoolProfilesModel.selectedYear = selectedYear;
            vmIndexSecondarySchoolProfilesModel.listAllPupils = listAllPupils;
            //setting english data and table
            List<DataSeries> temp = GetDataSeries("englishlevel", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesEnglishLevel = temp;
            //vmIndexPrimarySchoolProfilesModel.englishLevelDataTable = GenerateTransposedTable(CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English"));
            vmIndexSecondarySchoolProfilesModel.dataTableEnglishLevel = CreateDataTale(temp, vmIndexSecondarySchoolProfilesModel.DicEnglishLevel, "Level of English", "percentage");
            //setting ethnic data and table
            temp = GetDataSeries("ethnicity", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableEthnicBackground = CreateDataTale(temp, vmIndexSecondarySchoolProfilesModel.DicEthnicBG, "Ethnicity", "percentage");
            //setting Nationality data and table
            temp = GetDataSeries("nationality", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesNationality = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableNationality = CreateDataTale(temp, vmIndexSecondarySchoolProfilesModel.DicNationalIdentity, "Nationality", "percentage");
            //setting Stage data and table
            temp = GetDataSeries("stage", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesStage = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexSecondarySchoolProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetDataSeries("freemeal", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableFreeSchoolMeal = CreateDataTale(temp, vmIndexSecondarySchoolProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage");

            //setting LookAfter data and table
            temp = GetDataSeries("lookafter", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableLookedAfter = CreateDataTaleWithTotal(temp, vmIndexSecondarySchoolProfilesModel.DicLookedAfter, "Looked After Children", "percentage");

            Session["vmIndexSecondarySchoolProfilesModel"] = vmIndexSecondarySchoolProfilesModel;
            return View("IndexSecondarySchool", vmIndexSecondarySchoolProfilesModel);
        }

        public ActionResult GetSpecialProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<School> listSelectedSchoolname = new List<School>();
            bool schoolIsSelected = false;
            bool yesrIsSelected = false;
            Year selectedYear = null;

            vmIndexSpecialSchoolProfilesModel = Session["vmIndexSpecialSchoolProfilesModel"] as IndexSecondarySchoolProfilesViewModel;
            List<School> templistSchoolname = vmIndexSpecialSchoolProfilesModel.listSchoolname;
            List<Year> templistYears = vmIndexSpecialSchoolProfilesModel.listYears;

            if (Request["listSelectedSchoolname"] != null)
            {
                schoolIsSelected = true;
                //get CostCentreKey from dropdownlist in UI
                templistSelectedSchoolname = Request["listSelectedSchoolname"].Split(',').ToList();
                //select selected CostCentre from dropdownlist in UI
                listSelectedSchoolname = templistSchoolname.Where(x => templistSelectedSchoolname.Any(y => y.Contains(x.seedcode))).ToList();
            }

            if (Request["selectedYear"] != null)
            {
                yesrIsSelected = true;
                string year = Request["selectedYear"].ToString();
                selectedYear = templistYears.Where(x => x.year.Contains(year)).FirstOrDefault();
            }

            if (schoolIsSelected && yesrIsSelected)
            {
                listAllPupils = GetListAllPupils(rpGeneric2nd, selectedYear, sSchoolType);
            }

            string tempProfiletitle = "";
            foreach (var item in listSelectedSchoolname)
            {
                tempProfiletitle = tempProfiletitle + " /" + item.name;

            }



            vmIndexSpecialSchoolProfilesModel.profiletitle = tempProfiletitle;

            //store selected school into view model
            vmIndexSpecialSchoolProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexSpecialSchoolProfilesModel.selectedYear = selectedYear;
            vmIndexSpecialSchoolProfilesModel.listAllPupils = listAllPupils;
            //setting english data and table
            List<DataSeries> temp = GetDataSeries("englishlevel", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesEnglishLevel = temp;
            //vmIndexPrimarySchoolProfilesModel.englishLevelDataTable = GenerateTransposedTable(CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English"));
            vmIndexSpecialSchoolProfilesModel.dataTableEnglishLevel = CreateDataTale(temp, vmIndexSpecialSchoolProfilesModel.DicEnglishLevel, "Level of English", "percentage");
            //setting ethnic data and table
            temp = GetDataSeries("ethnicity", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableEthnicBackground = CreateDataTale(temp, vmIndexSpecialSchoolProfilesModel.DicEthnicBG, "Ethnicity", "percentage");
            //setting Nationality data and table
            temp = GetDataSeries("nationality", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesNationality = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableNationality = CreateDataTale(temp, vmIndexSpecialSchoolProfilesModel.DicNationalIdentity, "Nationality", "percentage");
            //setting Stage data and table
            temp = GetDataSeries("stage", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesStage = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexSpecialSchoolProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetDataSeries("freemeal", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableFreeSchoolMeal = CreateDataTale(temp, vmIndexSpecialSchoolProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage");

            //setting LookAfter data and table
            temp = GetDataSeries("lookafter", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableLookedAfter = CreateDataTaleWithTotal(temp, vmIndexSpecialSchoolProfilesModel.DicLookedAfter, "Looked After Children", "percentage");

            Session["vmIndexSpecialSchoolProfilesModel"] = vmIndexSpecialSchoolProfilesModel;
            return View("IndexSecondarySchool", vmIndexSpecialSchoolProfilesModel);
        }


        public ActionResult GetPrimaryListpupils(string datatitle, string Indexrow, string Indexcol)
        {
            PupilsListViewModel vmPupilsListViewModel = new PupilsListViewModel();
            //pull data from session that has been stored in GetPrimaryProfileData function
            vmIndexPrimarySchoolProfilesModel = Session["vmIndexPrimarySchoolProfilesModel"] as IndexPrimarySchoolProfilesViewModel;
            List<DataSeries> listAllPupils = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            DataTable dataTable = new DataTable();
            string colName = "";
            string catagory = "";
            string code = "";
            string title = "";

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            switch (datatitle)
            {
                case "englishlevel":
                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesEnglishLevel;
                    dataTable = vmIndexPrimarySchoolProfilesModel.dataTableEnglishLevel;
                    //colName = dataTable.Rows[Convert.ToInt16(Indexrow)][0].ToString();
                    colName = dataTable.Columns[Convert.ToInt16(Indexcol)].ToString();
                    dictionary = vmIndexPrimarySchoolProfilesModel.DicEnglishLevel;
                    code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                    catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                    //query to get pupils list by code from DataSeries
                    listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                    title = "Level of English";
                    break;

                case "ethnicity":
                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesEthnicBackground;
                    dataTable = vmIndexPrimarySchoolProfilesModel.dataTableEthnicBackground;
                    colName = dataTable.Columns[Convert.ToInt16(Indexcol)].ToString();
                    dictionary = vmIndexPrimarySchoolProfilesModel.DicEthnicBG;
                    code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                    catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                    //query to get pupils list by code from DataSeries
                    listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                    title = "Ethnicity";
                    break;

                case "nationality":
                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesNationality;
                    dataTable = vmIndexPrimarySchoolProfilesModel.dataTableNationality;
                    colName = dataTable.Columns[Convert.ToInt16(Indexcol)].ToString();
                    dictionary = vmIndexPrimarySchoolProfilesModel.DicNationalIdentity;
                    code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                    catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                    //query to get pupils list by code from DataSeries
                    listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                    title = "Nationality";
                    break;

                case "stage":
                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesStage;
                    dataTable = vmIndexPrimarySchoolProfilesModel.dataTableStage;
                    colName = dataTable.Columns[Convert.ToInt16(Indexcol)].ToString();
                    dictionary = vmIndexPrimarySchoolProfilesModel.DicStage;
                    code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                    catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                    //query to get pupils list by code from DataSeries
                    listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                    title = "Satge";
                    break;


            }
            vmPupilsListViewModel.listPupils = listtempPupilData;
            vmPupilsListViewModel.school = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).school;
            vmPupilsListViewModel.catagory = catagory;
            vmPupilsListViewModel.datatile = title;
            return View("Pupilslist", vmPupilsListViewModel);
        }

        public ActionResult GetTrendData(string sSchoolType, string datatitle, string sSchoolName)
        {
            TredningViewModel vmTrendingModel = new TredningViewModel();
            vmTrendingModel.listYear = GetListYear();

            //declare variable
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<DataSeries> tempDataSeries = new List<DataSeries>();
            List<List<DataSeries>> listobject = new List<List<DataSeries>>();
            //get school from list of primaryschool based on schoolname
            List<School> listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            List<School> school = listSchoolname.Where(x => x.name.Equals(sSchoolName)).ToList();

            
            string tabletitle = "";
            string datashowtype = "percentage";
            
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            //get dictionary based on dataset
            switch (datatitle)
            {
                case "englishlevel":
                    dictionary = GetDicEnglisheLevel(rpGeneric2nd);
                    tabletitle = "Level of English";
                    break;
                case "ethnicity":
                    dictionary = GetDicEhtnicBG(rpGeneric2nd);
                    tabletitle = "Ethnicity";
                    break;
                case "nationality":
                    dictionary = GetDicNationalIdenity(rpGeneric2nd);
                    tabletitle = "Nationality";
                    break;
                case "stage":
                    dictionary = GetDicStage(rpGeneric2nd, sSchoolType);
                    tabletitle = "Stage";
                    datashowtype = "number";
                    break;   
            }

            foreach (var item in vmTrendingModel.listYear) {
                listAllPupils = GetListAllPupils(rpGeneric2nd, item, sSchoolType);
                tempDataSeries = GetDataSeries(datatitle, listAllPupils, school, item, sSchoolType);
                listobject.Add(tempDataSeries);
            
            }

            //create dataTable for school
            foreach (var item in listobject)
            {
                List<double> tdata = new List<double>();
                foreach (var obj in item)
                {
                    tdata.Add(obj.percentage);
                }
                //tempChartdata.Add(new ChartData(item.sc)


            }





            if (sSchoolName.Equals("Aberdeen City") && school.Count() == 0)
            {
                school = new List<School>() { new School("Aberdeen City", "Aberdeen City") };
            }
            vmTrendingModel.school = school;
            //vmTrendingModel.listDataSeries = listobject;
            vmTrendingModel.dataTableSchool = CreateDataTale(listobject, dictionary, tabletitle, datashowtype);
            vmTrendingModel.datatitle = tabletitle;
            
            


            return View("Trending", vmTrendingModel);
        }

        private List<DataSeries> CreateDataSeries(string datatitle, List<StudentObj> listPupilData, List<School> listSelectedSchool, Year iyear)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            //get dataSeries for each School
            List<DataSeries> temp = GetDataSeries(datatitle, listPupilData, listSelectedSchool, iyear, "2");
            //temp.Add(GetDataSeries(datatitle, listPupilData, listSelectedSchool, iyear);)

            return listobject;
        
        }

        
    }
}