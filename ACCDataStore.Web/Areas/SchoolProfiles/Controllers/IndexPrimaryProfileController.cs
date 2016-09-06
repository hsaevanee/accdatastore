using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.Census;
using ACCDataStore.Entity.SchoolProfiles.InCAS;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
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
    public class IndexPrimaryProfileController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexPrimaryProfileController));

        private readonly IGenericRepository2nd rpGeneric2nd;
        
        private IndexPrimarySchoolProfilesViewModel vmIndexPrimarySchoolProfilesModel;

        public IndexPrimaryProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd; //connect to accdatastore database in MySQL
            this.vmIndexPrimarySchoolProfilesModel = new IndexPrimarySchoolProfilesViewModel();

        }
        
        [AdminAuthentication]
        [Transactional]
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
            //vmIndexPrimarySchoolProfilesModel.DicAttendance = GetDicAttendance(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.selectedYear = new Year("2015");
            vmIndexPrimarySchoolProfilesModel.showTableInCAS = null;
            Session["vmIndexPrimarySchoolProfilesModel"] = vmIndexPrimarySchoolProfilesModel;
            return View("IndexPrimarySchool", vmIndexPrimarySchoolProfilesModel);
        }
        // GET: SchoolProfiles/IndexPrimaryProfile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPrimaryProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<PIPSObj> listPIPSPupils = new List<PIPSObj>();
            List<InCASObj> listInCASPupils = new List<InCASObj>();
            List<AaeAttendanceObj> listAaeAttendancelists = new List<AaeAttendanceObj>();
            List<ExclusionStudentObj> listExclusionPupils = new List<ExclusionStudentObj>();
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
                listInCASPupils = GetInCASPupils(rpGeneric2nd, selectedYear, listSelectedSchoolname);
                listAaeAttendancelists = GetAaeAttendanceLists(rpGeneric2nd, sSchoolType, selectedYear, listSelectedSchoolname, listAllPupils);
                listExclusionPupils = GetListExclusionPupils(rpGeneric2nd, selectedYear, sSchoolType);
            }

            //create profiletitle
            string tempProfiletitle = "";
            foreach (var item in listSelectedSchoolname)
            {
                if (tempProfiletitle.Equals(""))
                {
                    tempProfiletitle = item.name;
                }
                else
                {
                    tempProfiletitle = tempProfiletitle + " / " + item.name;

                }


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
            vmIndexPrimarySchoolProfilesModel.dataTableEnglishLevel = CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English", "percentage");
            //setting ethnic data and table
            temp = GetDataSeries("ethnicity", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableEthnicBackground = CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicEthnicBG, "Ethnicity", "percentage");
            //setting Nationality data and table
            temp = GetDataSeries("nationality", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesNationality = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableNationality = CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicNationalIdentity, "Nationality", "percentage");
            //setting Stage data and table
            temp = GetDataSeries("stage", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesStage = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexPrimarySchoolProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetDataSeries("freemeal", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableFreeSchoolMeal = CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage");
            //setting LookAfter data and table
            temp = GetDataSeries("lookafter", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableLookedAfter = CreateDataTaleWithTotal(temp, vmIndexPrimarySchoolProfilesModel.DicLookedAfter, "Looked After Children", "no+%");
            //setting PIPS dataseries and datatable         
            temp = GetPIPsDataSeries(listPIPSPupils, listSelectedSchoolname, selectedYear);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesPIPS = temp;
            vmIndexPrimarySchoolProfilesModel.dataTablePIPS = CreatePIPSDataTable(temp, "P1");

            //setting InCAS dataseries and datatable     

            vmIndexPrimarySchoolProfilesModel.showTableInCAS = listInCASPupils.Count == 0 ? false : true;
            temp = listInCASPupils.Count == 0? new List<DataSeries>() : GetInCASDataSeries(listInCASPupils, listSelectedSchoolname, selectedYear, "1");
            vmIndexPrimarySchoolProfilesModel.listDataSeriesInCASP2 = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableInCASP2 = temp.Count == 0? null: CreatePIPSDataTable(temp, "P2");

            temp = listInCASPupils.Count == 0 ? new List<DataSeries>() : GetInCASDataSeries(listInCASPupils, listSelectedSchoolname, selectedYear, "3");
            vmIndexPrimarySchoolProfilesModel.listDataSeriesInCASP4 = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableInCASP4 = temp.Count == 0 ? null : CreatePIPSDataTable(temp, "P4");

            temp = listInCASPupils.Count == 0 ? new List<DataSeries>() : GetInCASDataSeries(listInCASPupils, listSelectedSchoolname, selectedYear, "5");
            vmIndexPrimarySchoolProfilesModel.listDataSeriesInCASP6 = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableInCASP6 = temp.Count == 0 ? null : CreatePIPSDataTable(temp, "P6");

            //Attendance
            vmIndexPrimarySchoolProfilesModel.showTableAttendance = listAaeAttendancelists.Count == 0 ? false : true;
            temp = listAaeAttendancelists.Count == 0 ? new List<DataSeries>() : GetAaeAttendanceDataSeries("attendance", listAaeAttendancelists, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesAttendance = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableAttendance = temp.Count == 0 ? null : CreateDataTable(temp, "School Attendance", "percentage");

            //Exclusion
            vmIndexPrimarySchoolProfilesModel.showTableExclusion = listExclusionPupils.Count == 0 ? false : true;
            temp = listExclusionPupils.Count == 0 ? new List<DataSeries>() : GetExclusionDataSeries("exclusion", listExclusionPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesExclusion = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableExclusion = temp.Count == 0 ? null : CreateDataTable(temp, "Exclusions-Annual", "number");

            //vmIndexPrimarySchoolProfilesModel.jsondata = JsonConvert.SerializeObject(tempdt, Formatting.Indented); ;
            Session["vmIndexPrimarySchoolProfilesModel"] = vmIndexPrimarySchoolProfilesModel;
            return View("IndexPrimarySchool", vmIndexPrimarySchoolProfilesModel);
        }

        protected List<DataSeries> GetPIPsDataSeries(List<PIPSObj> listPupilData, List<School> listSelectedSchool, Year iyear)
        {
            List<PIPSObj> listtempPupilData = new List<PIPSObj>();
            List<PIPSObjDetail> listResult = new List<PIPSObjDetail>();
            List<DataSeries> listobject = new List<DataSeries>();
            //calculate individual school

            foreach (School item in listSelectedSchool)
            {
                //select data for each school
                listtempPupilData = listPupilData.Where(x => x.DfES.ToString().Equals(item.seedcode)).ToList();
                //calculate School Start P1
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Where(x => x.Szr.HasValue==true).Select(r => r.Szr).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempPupilData.Where(x => x.Szm.HasValue == true).Select(r => r.Szm).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempPupilData.Where(x => x.Szp.HasValue == true).Select(r => r.Szp).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempPupilData.Where(x => x.Szt.HasValue == true).Select(r => r.Szt).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Start P1", school = item, year = iyear, listPIPSdataitems = listResult });
                listResult = new List<PIPSObjDetail>();
                //calculate School End P1
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Where(x => x.Ezr.HasValue == true).Select(r => r.Ezr).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempPupilData.Where(x => x.Ezm.HasValue == true).Select(r => r.Ezm).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempPupilData.Where(x => x.Ezp.HasValue == true).Select(r => r.Ezp).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempPupilData.Where(x => x.Ezt.HasValue == true).Select(r => r.Ezt).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "End P1", school = item, year = iyear, listPIPSdataitems = listResult });
            }

            //calculate for aberdeen city
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listPupilData.Where(x => x.Szr.HasValue == true).Select(r => r.Szr).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listPupilData.Where(x => x.Szm.HasValue == true).Select(r => r.Szm).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listPupilData.Where(x => x.Szp.HasValue == true).Select(r => r.Szp).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Total", average = listPupilData.Where(x => x.Szt.HasValue == true).Select(r => r.Szt).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "Start P1", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listPupilData.Where(x => x.Ezr.HasValue == true).Select(r => r.Ezr).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listPupilData.Where(x => x.Ezm.HasValue == true).Select(r => r.Ezm).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listPupilData.Where(x => x.Ezp.HasValue == true).Select(r => r.Ezp).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Total", average = listPupilData.Where(x => x.Ezt.HasValue == true).Select(r => r.Ezt).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "End P1", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });

            return listobject;
        }
        protected List<PIPSObj> GetPIPSPupils(IGenericRepository2nd rpGeneric2nd, Year year, List<School> schools)
        {
            List<PIPSObj> listResult = new List<PIPSObj>();
            switch (year.year)
            {
                case "2008":
                    listResult = rpGeneric2nd.FindAll<PIPS2008>().ToList<PIPSObj>();
                    break;
                case "2009":
                    listResult = rpGeneric2nd.FindAll<PIPS2009>().ToList<PIPSObj>();
                    break;
                case "2010":
                    listResult = rpGeneric2nd.FindAll<PIPS2010>().ToList<PIPSObj>();
                    break;
                case "2011":
                    listResult = rpGeneric2nd.FindAll<PIPS2011>().ToList<PIPSObj>();
                    break;
                case "2012":
                    listResult = rpGeneric2nd.FindAll<PIPS2012>().ToList<PIPSObj>();
                    break;
                case "2013":
                    listResult = rpGeneric2nd.FindAll<PIPS2013>().ToList<PIPSObj>();
                    break;
                case "2014":
                    listResult = rpGeneric2nd.FindAll<PIPS2014>().ToList<PIPSObj>();
                    break;
                case "2015":
                    listResult = rpGeneric2nd.FindAll<PIPS2015>().ToList<PIPSObj>();
                    break;
            }

            return listResult;

        }

        protected DataTable CreatePIPSDataTable(List<DataSeries> listobject, string firstColName)
        {
            DataTable dataTable = new DataTable();
            List<string> temprowdata = new List<string>();

            //create column names
            dataTable.Columns.Add(firstColName, typeof(string));

            if (listobject.Count == 0)
            {
                dataTable.Rows.Add("Data is not available");
            }
            else {

                //if (listobject != null && listobject[0].listPIPSdataitems.Count() > 0)
                //{
                    foreach (var item in listobject[0].listPIPSdataitems)
                    {
                        dataTable.Columns.Add(item.dataName, typeof(string));
                    }

                //}


                //adding row data
                foreach (var item in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(item.school.name + " " + item.dataSeriesNames);
                    foreach (var temp in item.listPIPSdataitems)
                    {
                        //temprowdata.Add("na" );
                        temprowdata.Add((temp.average.HasValue && !Double.IsNaN(temp.average.Value)) == false ? "na" : temp.average.Value.ToString("0.00"));
                    }
                    dataTable.Rows.Add(temprowdata.ToArray());
                }
            
            }

            

            return dataTable;
        }

        protected List<InCASObj> GetInCASPupils(IGenericRepository2nd rpGeneric2nd, Year year, List<School> schools)
        {
            List<InCASObj> listResult = new List<InCASObj>();
            switch (year.year)
            {
                case "2008":
                    listResult = new List<InCASObj>();
                    break;
                case "2009":
                    listResult = new List<InCASObj>();
                    break;
                case "2010":
                    listResult = new List<InCASObj>();
                    break;
                case "2011":
                    listResult = new List<InCASObj>();
                    break;
                case "2012":
                    listResult = rpGeneric2nd.FindAll<InCAS2012>().ToList<InCASObj>();
                    break;
                case "2013":
                    listResult = rpGeneric2nd.FindAll<InCAS2013>().ToList<InCASObj>();
                    break;
                case "2014":
                    listResult = rpGeneric2nd.FindAll<InCAS2014>().ToList<InCASObj>();
                    break;
                case "2015":
                    listResult = rpGeneric2nd.FindAll<InCAS2015>().ToList<InCASObj>();
                    break;
            }

            return listResult;

        }

        protected List<DataSeries> GetInCASDataSeries(List<InCASObj> listPupilData, List<School> listSelectedSchool, Year iyear, string YearGroup)
        {
            List<InCASObj> listtempPupilData = new List<InCASObj>();
            List<PIPSObjDetail> listResult = new List<PIPSObjDetail>();
            List<DataSeries> listobject = new List<DataSeries>();
            
            //select list of pupils by yeargroup
            listPupilData = listPupilData.Where(x => x.YearGroup.Equals(YearGroup)).ToList();

            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select data for each school
                listtempPupilData = listPupilData.Where(x => x.SchoolId.ToString().Equals(item.seedcode)).ToList();
                //calculate Develop Ability
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_DevAbil", average = listtempPupilData.Where(x => x.AgeAtTest_DevAbil.HasValue == true).Select(r => r.AgeAtTest_DevAbil).Average() });
               // listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_DevAbil", average = listtempPupilData.Where(x => x.AgeAtTest_DevAbil != 0).Select(r => r.AgeAtTest_DevAbil).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_DevAbil", average = listtempPupilData.Where(x => x.AgeEquiv_DevAbil.HasValue == true).Select(r => r.AgeEquiv_DevAbil).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_DevAbil", average = listtempPupilData.Where(x => x.AgeDiff_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Standardised_DevAbil", average = listtempPupilData.Where(x => x.Standardised_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Develop Ability", school = item, year = iyear, listPIPSdataitems = listResult });
                //calculate reading
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_Reading", average = listtempPupilData.Where(x => x.AgeAtTest_Reading.HasValue == true).Select(r => r.AgeAtTest_Reading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_Reading", average = listtempPupilData.Where(x => x.AgeEquiv_Reading.HasValue == true).Select(r => r.AgeEquiv_Reading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_Reading", average = listtempPupilData.Where(x => x.AgeDiff_Reading.HasValue == true).Select(r => r.AgeDiff_Reading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Standardised_Reading", average = listtempPupilData.Where(x => x.Standardised_Reading.HasValue == true).Select(r => r.Standardised_Reading).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Reading", school = item, year = iyear, listPIPSdataitems = listResult });
                //calculate Spelling
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_Spelling", average = listtempPupilData.Where(x => x.AgeAtTest_Spelling.HasValue == true).Select(r => r.AgeAtTest_Spelling).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_Spelling", average = listtempPupilData.Where(x => x.AgeEquiv_Spelling.HasValue == true).Select(r => r.AgeEquiv_Spelling).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_Spelling", average = listtempPupilData.Where(x => x.AgeDiff_Spelling.HasValue == true).Select(r => r.AgeDiff_Spelling).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Standardised_Reading", average = double.NaN });
                listobject.Add(new DataSeries { dataSeriesNames = "Spelling", school = item, year = iyear, listPIPSdataitems = listResult });
                //calculate General Math
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_GenMaths", average = listtempPupilData.Where(x => x.AgeAtTest_GenMaths.HasValue == true).Select(r => r.AgeAtTest_GenMaths).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_GenMaths", average = listtempPupilData.Where(x => x.AgeEquiv_GenMaths.HasValue == true).Select(r => r.AgeEquiv_GenMaths).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_GenMaths", average = listtempPupilData.Where(x => x.AgeDiff_GenMaths.HasValue == true).Select(r => r.AgeDiff_GenMaths).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Standardised_GenMaths", average = listtempPupilData.Where(x => x.Standardised_GenMaths.HasValue == true).Select(r => r.Standardised_GenMaths).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "General Maths", school = item, year = iyear, listPIPSdataitems = listResult });
                //calculate Mental Arithmetics
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_MentArith", average = listtempPupilData.Where(x => x.AgeAtTest_MentArith.HasValue == true).Select(r => r.AgeAtTest_MentArith).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_MentArith", average = listtempPupilData.Where(x => x.AgeEquiv_MentArith.HasValue == true).Select(r => r.AgeEquiv_MentArith).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_MentArith", average = listtempPupilData.Where(x => x.AgeDiff_MentArith.HasValue == true).Select(r => r.AgeDiff_MentArith).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Standardised_MentArith", average = listtempPupilData.Where(x => x.Standardised_MentArith.HasValue == true).Select(r => r.Standardised_MentArith).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Mental Arithmetics", school = item, year = iyear, listPIPSdataitems = listResult });

            }

            //calculate for aberdeen city
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_DevAbil", average = listPupilData.Where(x => x.AgeAtTest_DevAbil.HasValue == true).Select(r => r.AgeAtTest_DevAbil).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_DevAbil", average = listPupilData.Where(x => x.AgeEquiv_DevAbil.HasValue == true).Select(r => r.AgeEquiv_DevAbil).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_DevAbil", average = listPupilData.Where(x => x.AgeDiff_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Standardised_DevAbil", average = listPupilData.Where(x => x.Standardised_DevAbil.HasValue == true).Select(r => r.Standardised_DevAbil).Average() });

            //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_DevAbil", average = listPupilData.Where(x => x.AgeAtTest_DevAbil != 0).Select(r => r.AgeAtTest_DevAbil).Average() });
            //listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_DevAbil", average = listPupilData.Where(x => x.AgeEquiv_DevAbil != 0).Select(r => r.AgeEquiv_DevAbil).Average() });
            //listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_DevAbil", average = listPupilData.Where(x => x.AgeDiff_DevAbil != 0).Select(r => r.AgeDiff_DevAbil).Average() });
            //listResult.Add(new PIPSObjDetail { dataName = "Standardised_DevAbil", average = listPupilData.Where(x => x.Standardised_DevAbil != 0).Select(r => r.Standardised_DevAbil).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "Develop Ability", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
            //calculate reading
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_Reading", average = listPupilData.Where(x => x.AgeAtTest_Reading.HasValue == true).Select(r => r.AgeAtTest_Reading).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_Reading", average = listPupilData.Where(x => x.AgeEquiv_Reading.HasValue == true).Select(r => r.AgeEquiv_Reading).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_Reading", average = listPupilData.Where(x => x.AgeDiff_Reading.HasValue == true).Select(r => r.AgeDiff_Reading).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Standardised_Reading", average = listPupilData.Where(x => x.Standardised_Reading.HasValue == true).Select(r => r.Standardised_Reading).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "Reading", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
            //calculate Spelling
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_Spelling", average = listPupilData.Where(x => x.AgeAtTest_Spelling.HasValue == true).Select(r => r.AgeAtTest_Spelling).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_Spelling", average = listPupilData.Where(x => x.AgeEquiv_Spelling.HasValue == true).Select(r => r.AgeEquiv_Spelling).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_Spelling", average = listPupilData.Where(x => x.AgeDiff_Spelling.HasValue == true).Select(r => r.AgeDiff_Spelling).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Standardised_Reading", average = double.NaN });
            listobject.Add(new DataSeries { dataSeriesNames = "Spelling", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
            //calculate General Math
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_GenMaths", average = listPupilData.Where(x => x.AgeAtTest_GenMaths.HasValue == true).Select(r => r.AgeAtTest_GenMaths).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_GenMaths", average = listPupilData.Where(x => x.AgeEquiv_GenMaths.HasValue == true).Select(r => r.AgeEquiv_GenMaths).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_GenMaths", average = listPupilData.Where(x => x.AgeDiff_GenMaths.HasValue == true).Select(r => r.AgeDiff_GenMaths).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Standardised_GenMaths", average = listPupilData.Where(x => x.Standardised_GenMaths.HasValue == true).Select(r => r.Standardised_GenMaths).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "General Maths", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
            //calculate Mental Arithmetics
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_MentArith", average = listPupilData.Where(x => x.AgeAtTest_MentArith.HasValue == true).Select(r => r.AgeAtTest_MentArith).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_MentArith", average = listPupilData.Where(x => x.AgeEquiv_MentArith.HasValue == true).Select(r => r.AgeEquiv_MentArith).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_MentArith", average = listPupilData.Where(x => x.AgeDiff_MentArith.HasValue == true).Select(r => r.AgeDiff_MentArith).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Standardised_MentArith", average = listPupilData.Where(x => x.Standardised_MentArith.HasValue == true).Select(r => r.Standardised_MentArith).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "Mental Arithmetics", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });

            return listobject;
        }


        
    }
}