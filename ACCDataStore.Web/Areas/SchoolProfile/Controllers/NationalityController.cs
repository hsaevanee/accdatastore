using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Nationality;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class NationalityController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfileController));

        private readonly IGenericRepository rpGeneric;

        public NationalityController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
        // GET: SchoolProfile/Nationality
        public ActionResult Index()
        {
            var vmNationality = new NationalityViewModel();

            var schoolname = new List<string>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmNationality.ListSchoolNameData = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW NationalIdentity FROM test_3 group by NationalIdentity");

            fooList = listResult.OfType<string>().ToList();
            vmNationality.ListNationalCode = fooList;
            vmNationality.DicNational = GetDicNational();

            return View();
        }
        private Dictionary<string, string> GetDicNational()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "Scottish");
            dicNational.Add("02", "English");
            dicNational.Add("03", "Northern Irish");
            dicNational.Add("04", "Welsh");
            dicNational.Add("05", "British");
            dicNational.Add("99", "Other");
            dicNational.Add("10", "Not Disclosed");
            dicNational.Add("98", "Not Known");
            return dicNational;
        }
    }
}