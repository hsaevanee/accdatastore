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
    public class IndexSchoolProfilesController : Controller
    {

        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository3nd rpGeneric3nd;

        public IndexSchoolProfilesController(IGenericRepository3nd rpGeneric3nd)
        {
            this.rpGeneric3nd = rpGeneric3nd; // Connect with ScotXed_15 Database
        }

        // GET: SchoolProfiles/IndexSchoolProfiles
        public ActionResult Index()
        {
            var vmIndexSchoolProfilesModel = new IndexSchoolProfilesViewModel();
            //get list primary school pupils from database 
            List<PupilObj> listPrimaryPupilData = this.rpGeneric3nd.Find<PupilObj>(" from PupilObj where Costcentre.schoolType_id = :schoolType_id ", new string[] { "schoolType_id" }, new object[] { 2 }).ToList(); 
            Session["listPrimary_PupilData"] = listPrimaryPupilData;
            vmIndexSchoolProfilesModel.listSchoolname = listPrimaryPupilData.Select(x => (Costcentre)x.Costcentre).Distinct().OrderBy(x=>x.name).ToList();
            Session["listCostcentre"] = vmIndexSchoolProfilesModel.listSchoolname;

            return View(vmIndexSchoolProfilesModel);
        }

        public ActionResult GetData()
        {
            var vmIndexSchoolProfilesModel = new IndexSchoolProfilesViewModel();
            List<PupilObj> listPupilData = Session["listPrimary_PupilData"] as List<PupilObj>;
            List<Costcentre> listCostcentre = Session["listCostcentre"] as List<Costcentre>;
            List<string> listSelectedSchoolname = new List<string>();
            List<Costcentre> listSelectedCostcentre = new List<Costcentre>();
            var mydicEhtnicBG = GetDicEhtnicBG();

            bool schoolIsSelect = false;
            if (Request["listSelectedSchoolname"] != null)
            {
                schoolIsSelect = true;
                //get CostCentreKey from dropdownlist in UI
                listSelectedSchoolname = Request["listSelectedSchoolname"].Split(',').ToList();
                //select selected CostCentre from dropdownlist in UI
                listSelectedCostcentre = listCostcentre.Where(x => listSelectedSchoolname.Any(y => Int32.Parse(y) == x.CostCentreKey)).ToList();
                
            }

            if (schoolIsSelect)
            {
                DataTable ethnicBackgroundTable = GetEthnicBackgroundDataTable(listPupilData, listSelectedCostcentre);

             }
            return View(vmIndexSchoolProfilesModel);
        }

        protected DataTable GetEthnicBackgroundDataTable(List<PupilObj> listPupilData, List<Costcentre> listSelectedCostcentre)
        {
            List<object> listobject = new List<object>();

            var mydicEhtnicBG = GetDicEhtnicBG();

            // get data for all primary schools
            var listResultforAll = listPupilData.GroupBy(x => x.EthnicBackground.ScotXedcode).Select(y => new { EthnicBackgroundcode = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
            //calculate the total number of pupils in Aberdeen
            var sum = (decimal)listResultforAll.Select(r => r.count).Sum();
            var listResultwithPercentage = listResultforAll.Select(y => new { CostCentreKey = 18775, EthnicBackgroundcode = y.EthnicBackgroundcode, list = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
            listobject.Add(listResultwithPercentage);

            //select primary pupils for selected school
            var listtempPupilData = listPupilData.Where(x => listSelectedCostcentre.Any(y => y.CostCentreKey == x.Costcentre.CostCentreKey)).ToList();

            var listResult = listtempPupilData.GroupBy(x => new { x.Costcentre.CostCentreKey, x.EthnicBackground.ScotXedcode}).Select(y => new { CostCentreKey = y.Key.CostCentreKey, EthnicBackgroundcode = y.Key.ScotXedcode, list = y.ToList(), count = y.ToList().Count() }).ToList();

            foreach (var item in listSelectedCostcentre)
            {
                var temp = listResult.FindAll(x => x.CostCentreKey == item.CostCentreKey).ToList();
                sum = (decimal)temp.Select(r => r.count).Sum();
                listResultwithPercentage = temp.Select(y => new { CostCentreKey = y.CostCentreKey, EthnicBackgroundcode = y.EthnicBackgroundcode, list = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                listobject.Add(listResultwithPercentage);
            }



            DataTable ethnicBackgroundTable = new DataTable();
            //add column to data table

            ethnicBackgroundTable.Columns.Add("Nationality", typeof(string));

            foreach (var item in listSelectedCostcentre)
            {
                ethnicBackgroundTable.Columns.Add(item.name, typeof(string));
                foreach (var temp in mydicEhtnicBG)
                {
                    var temppp = temp.Key;
                }
            }








            //nationalityTable.Rows.Add(listResult, 100);
            //nationalityTable.Rows.Add("AA", 100);
            //nationalityTable.Rows.Add("AA", 100);


            return ethnicBackgroundTable;

        }

        //not using
        protected List<School> GetListSchool()
        {
            var listResult = this.rpGeneric3nd.FindByNativeSQL("Select CostCentreKey, Name from View_Costcentre where  schoolType_id = 2 and costCentreKey in (Select distinct CostCentreKey from sx_pupil)");

            //var listResult = this.rpGeneric3nd.FindAll<Costcentre>().ToList();

            List<School> listdata = new List<School>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        listdata.Add(new School(itemRow[0].ToString(), itemRow[1].ToString()));
                    }
                }
            }
            return listdata;
             
        }

        protected Dictionary<string, string> GetDicEhtnicBG()
        {
            var listResult = this.rpGeneric3nd.FindByNativeSQL("Select distinct ScotXedcode, value from Lu_EthnicBackground");

            var dicNational = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dicNational.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dicNational;

        }

    }
}