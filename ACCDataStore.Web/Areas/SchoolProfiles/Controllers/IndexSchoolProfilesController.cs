using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
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
            //get listpupils from database
            List<PupilObj> listPupilData = this.rpGeneric3nd.FindAll<PupilObj>().ToList();
            Session["listPupilData"] = listPupilData;
            //get list of school from database
            List<School> listCostcentre = GetListSchool();
            vmIndexSchoolProfilesModel.listSchoolname = listCostcentre;
            //select primary_pupil data based on shooltype id = 2 
            List<PupilObj> listPrimaryPupilData = listPupilData.Where(b => listCostcentre.Any(a => Convert.ToInt16(a.seedcode) == b.CostCentreKey)).ToList();
            Session["listPrimary_PupilData"] = listPrimaryPupilData;
            Session["lististCostcentre"] = listCostcentre;
            Dictionary<string, string> DicEthnicBG = GetDicEhtnicBG();
            DataTable nationalityTable = new DataTable();
            //nationalityTable.Columns.Add("Nationality", typeof(string));
            //nationalityTable.Columns.Add("Aberdeencity", typeof(double));

            //nationalityTable.Rows.Add(listResult, 100);
            //nationalityTable.Rows.Add("AA", 100);
            //nationalityTable.Rows.Add("AA", 100);

            //vmIndexSchoolProfilesModel.nationalityData = nationalityTable;
            return View(vmIndexSchoolProfilesModel);
        }

        public ActionResult GetData()
        {
            var vmIndexSchoolProfilesModel = new IndexSchoolProfilesViewModel();
            List<PupilObj> listPupilData = Session["listPrimary_PupilData"] as List<PupilObj>;
            List<School> listCostcentre = Session["lististCostcentre"] as List<School>;
            List<string> listSelectedSchoolname = new List<string>();
            List<School> listSelectedSchool = new List<School>();
            List<object> listobject = new List<object>();
            bool schoolIsSelect = false;
            if (Request["listSelectedSchoolname"] != null)
            {
                schoolIsSelect = true;
                listSelectedSchoolname = Request["listSelectedSchoolname"].Split(',').ToList();
                foreach (var item in listSelectedSchoolname) {
                    listSelectedSchool.Add(listCostcentre.First(x =>x.seedcode.Equals(item)));
                }
            }

            if (schoolIsSelect)
            {
                DataTable ethnicBackgroundTable = GetEthnicBackgroundDataTable(listPupilData, listSelectedSchool);

             }
            return View(vmIndexSchoolProfilesModel);
        }

        protected DataTable GetEthnicBackgroundDataTable(List<PupilObj> listPupilData, List<School> listSelectedSchool)
        {
            List<object> listobject = new List<object>();

            var mydicEhtnicBG = GetDicEhtnicBG();     
   
            // get data for all primary schools
            var listResultforAll = listPupilData.GroupBy(x => x.EthnicBackground).Select(y => new { EthnicBackground = y.Key , list = y.ToList(), count = y.ToList().Count() }).ToList();
            var sum = (decimal)listResultforAll.Select(r => r.count).Sum();
            var listResultwithPercentage = listResultforAll.Select(y => new { CostCentreKey = 18775, EthnicBackground = y.EthnicBackground, list = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
            listobject.Add(listResultwithPercentage);
            
            
            var listResult = listPupilData.GroupBy(x => new { x.CostCentreKey, x.EthnicBackground }).Select(y => new { CostCentreKey = y.Key.CostCentreKey, EthnicBackground = y.Key.EthnicBackground, list = y.ToList(), count = y.ToList().Count() }).ToList();
 
            foreach (var item in listSelectedSchool)
            {
                var temp = listResult.FindAll(x => item.seedcode.Contains(x.CostCentreKey.ToString())).ToList();
                sum = (decimal)temp.Select(r => r.count).Sum();
                listResultwithPercentage = temp.Select(y => new { CostCentreKey = y.CostCentreKey, EthnicBackground = y.EthnicBackground, list = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                listobject.Add(listResultwithPercentage);
            }



            DataTable ethnicBackgroundTable = new DataTable();
            //add column to data table

            ethnicBackgroundTable.Columns.Add("Nationality", typeof(string));
 
            foreach (var temp in mydicEhtnicBG)
            {
                var temppp = temp.Key;
            }




           
            

            //nationalityTable.Rows.Add(listResult, 100);
            //nationalityTable.Rows.Add("AA", 100);
            //nationalityTable.Rows.Add("AA", 100);


            return ethnicBackgroundTable;

        }
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