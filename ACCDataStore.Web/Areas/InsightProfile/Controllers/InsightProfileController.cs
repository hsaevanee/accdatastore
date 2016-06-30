using ACCDataStore.Entity;
using ACCDataStore.Entity.MySQL;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.InsightProfile.Controllers
{
    public class InsightProfileController : BaseController
    {

        private static ILog log = LogManager.GetLogger(typeof(InsightProfileController));

        //private readonly IGenericRepository rpGeneric;
        private readonly IGenericRepository2nd rpGeneric2nd;

        public InsightProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            //this.rpGeneric = rpGeneric;
            this.rpGeneric2nd = rpGeneric2nd;
        }

        ////private readonly IGenericRepository rpGeneric;
        //private readonly IGenericRepository2nd rpGeneric2nd;

        //public InsightProfileController(IGenericRepository2nd rpGeneric2nd)
        //{
        //    //this.rpGeneric = rpGeneric;
        //    this.rpGeneric2nd = rpGeneric2nd;
        //}

        // GET: InsightProfile/InsightProfile
        public ActionResult Index()
        {
            // just test
            //var listResultMSAccess = this.rpGeneric.FindAll<ACCDataStore.Entity.StudentSIMD>();

            //var listResultByConditionMSAccess = this.rpGeneric.Find<StudentSIMD>(" from StudentSIMD where SchName = :SchName ", new string[] { "SchName" }, new object[] { "Brimmond Primary School" });

            //var listResultMySQL = this.rpGeneric2nd.FindAll<LA100Schools>();

            var listResultMySQL2 = this.rpGeneric2nd.FindAll<Users>();

            var listResultMySQL21 = this.rpGeneric2nd.Find<Rights>(" from Rights where Users.UserID = :UserID ", new string[] { "UserID" }, new object[] { 1 });

            var listResultMySQL13 = this.rpGeneric2nd.FindByNativeSQL("SELECT * from test_student_2013");
            var listResultMySQL14 = this.rpGeneric2nd.FindByNativeSQL("SELECT * from test_student_2014");
            var listResultMySQL15 = this.rpGeneric2nd.FindByNativeSQL("SELECT * from test_student_2015");
             


            return View();
        }
    }
}
