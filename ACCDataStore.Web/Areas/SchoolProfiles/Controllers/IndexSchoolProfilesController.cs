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

            List<PupilObj> listPupilData = this.rpGeneric3nd.FindAll<PupilObj>().ToList();

            var listResult = this.rpGeneric3nd.FindByNativeSQL("Select * from Lu_NationalIdentity");

            DataTable nationalityTable = new DataTable();
            nationalityTable.Columns.Add("Nationality", typeof(string));
            nationalityTable.Columns.Add("Aberdeencity", typeof(double));

            nationalityTable.Rows.Add(listResult, 100);
            nationalityTable.Rows.Add("AA", 100);
            nationalityTable.Rows.Add("AA", 100);

            vmIndexSchoolProfilesModel.nationalityData = nationalityTable;
            return View(vmIndexSchoolProfilesModel);
        }

 

    }
}