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
            List<string> listSelectedSchoolname = new List<string>();
            List<object> listobject = new List<object>();
            bool schoolIsSelect = false;
            if (Request["listSelectedSchoolname"] != null)
            {
                schoolIsSelect = true;
                listSelectedSchoolname = Request["listSelectedSchoolname"].Split(',').ToList();
            }

            if (schoolIsSelect)
            {
                //var listResult = listPupilData.GroupBy(p => p.CostCentreKey, p => p.EthnicBackground, (key, g) => new { CostCentreKey = key, list = g.ToList().Count() });
                var listResult = listPupilData.GroupBy(x => new { x.CostCentreKey, x.EthnicBackground }).Select(y => new { CostCentreKey = y.Key.CostCentreKey, EthnicBackground = y.Key.EthnicBackground, list = y.ToList(), count = y.ToList().Count() }).ToList();
                foreach (var name in listSelectedSchoolname)
                {
                    var temp = listResult.FindAll(x => name.Contains(x.CostCentreKey.ToString())).ToList();
                    var sum = (decimal)temp.Select(r => r.count).Sum();
                    var listResultwithPercentage = temp.Select(y => new { CostCentreKey = y.CostCentreKey, EthnicBackground = y.EthnicBackground, list = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    listobject.Add(listResultwithPercentage);               
                }
            }
            return View(vmIndexSchoolProfilesModel);
        }

        protected DataTable GetEthnicBackgroundDataTable(List<PupilObj> listPupilData, List<string> listSelectedSchoolname)
        {
            List<object> listobject = new List<object>();
            var listResult = listPupilData.GroupBy(x => new { x.CostCentreKey, x.EthnicBackground }).Select(y => new { CostCentreKey = y.Key.CostCentreKey, EthnicBackground = y.Key.EthnicBackground, list = y.ToList(), count = y.ToList().Count() }).ToList();
            foreach (var name in listSelectedSchoolname)
            {
                var temp = listResult.FindAll(x => name.Contains(x.CostCentreKey.ToString())).ToList();
                var sum = (decimal)temp.Select(r => r.count).Sum();
                var listResultwithPercentage = temp.Select(y => new { CostCentreKey = y.CostCentreKey, EthnicBackground = y.EthnicBackground, list = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                listobject.Add(listResultwithPercentage);
            }

            DataTable ethnicBackgroundTable = new DataTable();




           
            ethnicBackgroundTable.Columns.Add("Aberdeencity", typeof(double));

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