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
    public class IndexAberdeenProfileController : BaseSchoolProfileController
    {

        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        private IndexAberdeenProfileViewModel vmIndexAberdeenCityProfilesModel;

        public IndexAberdeenProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd; //connect to accdatastore database in MySQL
            this.vmIndexAberdeenCityProfilesModel = new IndexAberdeenProfileViewModel();

        }

        public ActionResult IndexAberdeenProfiles(string sSchoolType)
        {
            //get data ready for set up profiles
            vmIndexAberdeenCityProfilesModel.listSchoolname = new List<School>() { new School("Aberdeen City", "Aberdeen City") };
            vmIndexAberdeenCityProfilesModel.listSelectedSchoolname = new List<School>() { new School("Aberdeen City", "Aberdeen City") };
            vmIndexAberdeenCityProfilesModel.listYears = GetListYear();
            vmIndexAberdeenCityProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexAberdeenCityProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexAberdeenCityProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            //vmIndexAberdeenCityProfilesModel.DicStage = GetDicStage(rpGeneric2nd, sSchoolType);
            vmIndexAberdeenCityProfilesModel.DicFreeMeal = GetDicFreeSchoolMeal();
            vmIndexAberdeenCityProfilesModel.DicLookedAfter = GetDicLookAfter();

            Session["vmIndexAberdeenCityProfilesModel"] = vmIndexAberdeenCityProfilesModel;
            return View("IndexAberdeenCity", vmIndexAberdeenCityProfilesModel);
        }


        public ActionResult GetAberdeenProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<School> listSchoolType = GetSchoolType();
            bool yesrIsSelected = false;
            Year selectedYear = null;

            vmIndexAberdeenCityProfilesModel = Session["vmIndexAberdeenCityProfilesModel"] as IndexAberdeenProfileViewModel;
             
            List<Year> templistYears = vmIndexAberdeenCityProfilesModel.listYears;

            if (Request["selectedYear"] != null)
            {
                yesrIsSelected = true;
                string year = Request["selectedYear"].ToString();
                selectedYear = templistYears.Where(x => x.year.Contains(year)).FirstOrDefault();
            }

            if (yesrIsSelected)
            {
                listAllPupils = GetListAllPupils(rpGeneric2nd, selectedYear, sSchoolType);
            }

            vmIndexAberdeenCityProfilesModel.profiletitle = "Aberdeen City";

            //store selected school into view model
            //vmIndexAberdeenCityProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexAberdeenCityProfilesModel.selectedYear = selectedYear;
            vmIndexAberdeenCityProfilesModel.listAllPupils = listAllPupils;
            //setting english data and table
            List<DataSeries> temp = GetDataSeriesForAberdeenCity("englishlevel", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesEnglishLevel = temp;
            //vmIndexPrimarySchoolProfilesModel.englishLevelDataTable = GenerateTransposedTable(CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English"));
            vmIndexAberdeenCityProfilesModel.dataTableEnglishLevel = CreateDataTale(temp, vmIndexAberdeenCityProfilesModel.DicEnglishLevel, "Level of English", "percentage");
            //setting ethnic data and table
            temp = GetDataSeriesForAberdeenCity("ethnicity", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexAberdeenCityProfilesModel.dataTableEthnicBackground = CreateDataTale(temp, vmIndexAberdeenCityProfilesModel.DicEthnicBG, "Ethnicity", "percentage");
            //setting Nationality data and table
            temp = GetDataSeriesForAberdeenCity("nationality", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesNationality = temp;
            vmIndexAberdeenCityProfilesModel.dataTableNationality = CreateDataTale(temp, vmIndexAberdeenCityProfilesModel.DicNationalIdentity, "Nationality", "percentage");
            //setting Stage data and table
            temp = GetDataSeriesForAberdeenCity("stage", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesStage = temp;
            vmIndexAberdeenCityProfilesModel.dataTableStage = CreateDataTaleWithCheckSumTotal(temp, "School Roll", "number");

            //vmIndexAberdeenCityProfilesModel.dataTableStagePrimary = CreateDataTaleWithTotal(temp.Where(x =>x.school.schooltype.Equals("2")).ToList(), GetDicStage(rpGeneric2nd,"2"), "Stage", "number");
            //vmIndexAberdeenCityProfilesModel.dataTableStageSecondary = CreateDataTaleWithTotal(temp.Where(x => x.school.schooltype.Equals("3")).ToList(), GetDicStage(rpGeneric2nd, "3"), "Stage", "number");
            //vmIndexAberdeenCityProfilesModel.dataTableStageSpecial = CreateDataTaleWithTotal(temp.Where(x => x.school.schooltype.Equals("4")).ToList(), GetDicStage(rpGeneric2nd, "4"), "Stage", "number");

            //vmIndexAberdeenCityProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexAberdeenCityProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetDataSeriesForAberdeenCity("freemeal", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexAberdeenCityProfilesModel.dataTableFreeSchoolMeal = CreateDataTale(temp, vmIndexAberdeenCityProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage");

            //setting LookAfter data and table
            temp = GetDataSeriesForAberdeenCity("lookafter", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexAberdeenCityProfilesModel.dataTableLookedAfter = CreateDataTaleWithTotal(temp, vmIndexAberdeenCityProfilesModel.DicLookedAfter, "Looked After Children", "percentage");

            Session["vmIndexSecondarySchoolProfilesModel"] = vmIndexAberdeenCityProfilesModel;
            return View("IndexAberdeenCity", vmIndexAberdeenCityProfilesModel);
        }

        protected List<School> GetSchoolType() {
            List<School> schooltype = new List<School>();

            schooltype.Add(new School("2", "Primary School", "2"));
            schooltype.Add(new School("3", "Secondary School", "3"));
            schooltype.Add(new School("4", "Special School", "4"));
            schooltype.Add(new School("5", "Aberdeen City", "5"));
            return schooltype;
        
        }

        protected List<DataSeries> GetDataSeriesForAberdeenCity(string datatitle, List<StudentObj> listPupilData, List<School> schooltype, Year iyear)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            //var listResultwithPercentage = null;
            double sum = 0.0;
            List<ObjectDetail> listResultwithPercentage = new List<ObjectDetail>();
            //calculate individual schooltype
            foreach (School item in schooltype)
            {
                switch (item.schooltype)
                {
                    case "2": //select only primary pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.StartsWith("P")) && (x.StudentStatus.Equals("01"))).ToList();
                        break;
                    case "3": 
                        //select only secondary pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.StartsWith("S")) && (x.StudentStatus.Equals("01"))).ToList();
                        listtempPupilData = listtempPupilData.Where(x => !x.StudentStage.Equals("SP")).ToList();
                        break;
                    case "4": 
                        //select only special pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.Equals("SP")) && (x.StudentStatus.Equals("01"))).ToList();
                        break;
                    case "5":
                        //select only special pupils
                        listtempPupilData = listPupilData.ToList();
                        break;
                }

                switch (datatitle)
                {

                    case "nationality":
                        var listResultforAll = listtempPupilData.GroupBy(x => x.NationalIdentity).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "ethnicity":
                        listResultforAll = listtempPupilData.GroupBy(x => x.EthnicBackground).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "englishlevel":
                        listResultforAll = listtempPupilData.GroupBy(x => x.LevelOfEnglish).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "stage":
                        listResultforAll = listtempPupilData.GroupBy(x => x.StudentStage).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "freemeal":
                        if (schooltype.Equals("2"))
                        {
                            //select only pupils between stage 4 and 7
                            var temp = (from a in listtempPupilData where a.StudentStage.Equals("P4") || a.StudentStage.Equals("P5") || a.StudentStage.Equals("P6") || a.StudentStage.Equals("P7") select a).ToList();
                            var listResultforP4P7 = temp.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                            listResultforAll = listtempPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();

                            //calculate the total number of pupils in Aberdeen
                            sum = (double)listResultforAll.Select(r => r.count).Sum();
                            listResultwithPercentage = listResultforP4P7.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        }
                        else
                        {
                            listResultforAll = listtempPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                            //calculate the total number of pupils in Aberdeen
                            sum = (double)listResultforAll.Select(r => r.count).Sum();
                            listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        }
                        break;
                    case "lookafter":
                        listResultforAll = listtempPupilData.GroupBy(x => x.StudentLookedAfter).Select(y => new { Code = y.Key == null ? "" : y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;

                }

                listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = item, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });
            }
            
            return listobject;
        }

        protected DataTable CreateDataTaleWithCheckSumTotal(List<DataSeries> listobject, string tabletitle, string showtype)
        {
            // create data table with count total data show in each row
            DataTable dataTable = new DataTable();
            List<string> temprowdata;
 
            if (showtype.Equals("number"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                dataTable.Columns.Add("Total", typeof(string));

                int sum = 0;
                //display number
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    temprowdata.Add(temp.checkSumCount.ToString(""));
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            else
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                dataTable.Columns.Add("Total", typeof(string));

                int sum = 0;
                //display number
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    temprowdata.Add(temp.checkSumPercentage.ToString(""));
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            return dataTable;
        }


    }
}