using ACCDataStore.Entity;
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

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class IndexSchoolProfilesController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        private IndexPrimarySchoolProfilesViewModel vmIndexPrimarySchoolProfilesModel;
        private IndexSecondarySchoolProfilesViewModel vmIndexSecondarySchoolProfilesModel;

        public IndexSchoolProfilesController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd; //connect to accdatastore database in MySQL
            this.vmIndexPrimarySchoolProfilesModel = new IndexPrimarySchoolProfilesViewModel();
            this.vmIndexSecondarySchoolProfilesModel = new IndexSecondarySchoolProfilesViewModel();
        }

        // GET: SchoolProfiles/IndexSchoolProfiles
        public ActionResult Index()
        {
           // this.vmIndexPrimarySchoolProfilesModel.listAllPupils = GetListAllPupils(rpGeneric2nd);
            return View("Home");
        }

        public ActionResult IndexPrimaryProfiles(string sSchoolType)
        {
            //get data ready for set up profiles
            vmIndexPrimarySchoolProfilesModel.listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listYears = GetListYear();
            vmIndexPrimarySchoolProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            Session["vmIndexPrimarySchoolProfilesModel"] = vmIndexPrimarySchoolProfilesModel;
            return View("IndexPrimarySchool", vmIndexPrimarySchoolProfilesModel);
        }

        public ActionResult IndexSecondaryProfiles(string sSchoolType)
        {

            vmIndexSecondarySchoolProfilesModel.listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            //vmIndexSecondarySchoolProfilesModel.listAllPupils = GetListAllPupils(rpGeneric2nd, "secondary");
            vmIndexSecondarySchoolProfilesModel.listYears = GetListYear();
            return View("IndexSecondarySchool", vmIndexSecondarySchoolProfilesModel);
        }

        public ActionResult GetPrimaryProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
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
            }

           //store selected school into view model
            vmIndexPrimarySchoolProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexPrimarySchoolProfilesModel.selectedYear = selectedYear;
            vmIndexPrimarySchoolProfilesModel.listAllPupils = listAllPupils;
            //setting english data and table
            List<DataSeries> temp = GetDataSeries("englishlevel", listAllPupils, listSelectedSchoolname, selectedYear);
            vmIndexPrimarySchoolProfilesModel.listenglishDataSeries = temp;
            vmIndexPrimarySchoolProfilesModel.englishLevelDataTable = CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English");
            //setting ethnic data and table
            temp = GetDataSeries("ethnicity", listAllPupils, listSelectedSchoolname, selectedYear);
            vmIndexPrimarySchoolProfilesModel.listethnicityDataSeries = temp;
            vmIndexPrimarySchoolProfilesModel.ethnicityDataTable = CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEthnicBG, "Ethnicity");
            //setting Nationality data and table
            temp = GetDataSeries("nationality", listAllPupils, listSelectedSchoolname, selectedYear);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesNationality = temp;
            vmIndexPrimarySchoolProfilesModel.nationalityDataTable = CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicNationalIdentity, "Nationality");
            //vmIndexPrimarySchoolProfilesModel.nationalityDataTable = GetNationalIdentityDataTable(rpGeneric2nd, listAllPupils, listSelectedSchoolname);
            Session["vmIndexPrimarySchoolProfilesModel"] = vmIndexPrimarySchoolProfilesModel;
            return View("IndexPrimarySchool", vmIndexPrimarySchoolProfilesModel);
        }

        public ActionResult GetListpupils(string datatitle, string Indexrow, string Indexcol)
        {
            PupilsListViewModel vmPupilsListViewModel = new PupilsListViewModel();
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
                case "englishLevel":
                                  listAllPupils = vmIndexPrimarySchoolProfilesModel.listenglishDataSeries;
                                  dataTable = vmIndexPrimarySchoolProfilesModel.englishLevelDataTable;
                        colName = dataTable.Rows[Convert.ToInt16(Indexrow)][0].ToString();
                dictionary = vmIndexPrimarySchoolProfilesModel.DicEnglishLevel;
                code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                //query to get pupils list by code from DataSeries
                listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexcol) - 1).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                title = "Level of English";
                    break;

                case "ethnicityData":
                 listAllPupils = vmIndexPrimarySchoolProfilesModel.listethnicityDataSeries;
                dataTable = vmIndexPrimarySchoolProfilesModel.ethnicityDataTable;
                colName = dataTable.Rows[Convert.ToInt16(Indexrow)][0].ToString();
                dictionary = vmIndexPrimarySchoolProfilesModel.DicEthnicBG;
                code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                //query to get pupils list by code from DataSeries
                listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexcol) - 1).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                title = "Ethnicity";
                    break;

                case "nationalityData":
                                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesNationality;
                dataTable = vmIndexPrimarySchoolProfilesModel.nationalityDataTable;
                colName = dataTable.Rows[Convert.ToInt16(Indexrow)][0].ToString();
                dictionary = vmIndexPrimarySchoolProfilesModel.DicNationalIdentity;
                code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                //query to get pupils list by code from DataSeries
                listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexcol) - 1).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                title = "Nationality";
                    break;


            }
            vmPupilsListViewModel.listPupils = listtempPupilData;
            vmPupilsListViewModel.school = listAllPupils.ElementAt(Convert.ToInt16(Indexcol) - 1).school;
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
            }

            foreach (var item in vmTrendingModel.listYear) {
                listAllPupils = GetListAllPupils(rpGeneric2nd, item, sSchoolType);
                tempDataSeries = GetDataSeries(datatitle, listAllPupils, school, item);
                listobject.Add(tempDataSeries);
            
            }

            vmTrendingModel.school = school;
            vmTrendingModel.listDataSeries = listobject;
            vmTrendingModel.dataSeriesDataTable = CreateDataTale(listobject, dictionary, tabletitle);
             

            return View("Trending", vmTrendingModel);
        }
    }
}