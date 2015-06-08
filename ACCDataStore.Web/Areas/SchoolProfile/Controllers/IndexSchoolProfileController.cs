using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACCDataStore.Entity;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.EthnicBackground;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.IndexSchoolProfile;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class IndexSchoolProfileController : BaseSchoolProfileController
    {
        // GET: SchoolProfile/IndexSchoolProfile
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfileController));

        private readonly IGenericRepository rpGeneric;

        public IndexSchoolProfileController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        public ActionResult Index()
        {
            //var vmIndex = new IndexViewModel();
            //var result = this.rpGeneric.FindAll<StudentSIMD>();
            var vmIndexSchoolProfile = new IndexSchoolProfileViewModel();

            var sSchoolName = Request["selectedschoolname"];

            var schoolname = new List<string>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmIndexSchoolProfile.ListSchoolNameData = fooList;

            if (sSchoolName == null)
            {
                vmIndexSchoolProfile.selectedschoolname = fooList[0];
                vmIndexSchoolProfile.ListEthnicData = GetEthnicityDatabySchoolname(this.rpGeneric, fooList[0]);
                vmIndexSchoolProfile.ListNationalityData = GetNationalityDatabySchoolname(this.rpGeneric, fooList[0]);
                vmIndexSchoolProfile.ListSIMDData = GetSIMDDatabySchoolname(this.rpGeneric, fooList[0], new List<string>(new string[] { "2012" }));
                vmIndexSchoolProfile.ListStdStageData = GetStudentStageDatabySchoolname(this.rpGeneric, fooList[0]);
            }
            else {
                vmIndexSchoolProfile.selectedschoolname = sSchoolName;
                vmIndexSchoolProfile.ListEthnicData = GetEthnicityDatabySchoolname(this.rpGeneric, sSchoolName);
                vmIndexSchoolProfile.ListNationalityData = GetNationalityDatabySchoolname(this.rpGeneric, sSchoolName);
                vmIndexSchoolProfile.ListSIMDData = GetSIMDDatabySchoolname(this.rpGeneric, sSchoolName, new List<string>(new string[] { "2012" }));
                vmIndexSchoolProfile.ListStdStageData = GetStudentStageDatabySchoolname(this.rpGeneric, sSchoolName);
            
            }



            return View("index", vmIndexSchoolProfile);



        }
        //private Dictionary<string, string> GetDicEhtnicBG()
        //{
        //    var dicNational = new Dictionary<string, string>();
        //    dicNational.Add("01", "White – Scottish");
        //    dicNational.Add("02", "African – African / Scottish / British");
        //    dicNational.Add("03", "Caribbean or Black – Caribbean / British / Scottish");
        //    dicNational.Add("05", "Asian – Indian/British/Scottish");
        //    dicNational.Add("06", "Asian – Pakistani / British / Scottish");
        //    dicNational.Add("07", "Asian –Bangladeshi / British / Scottish");
        //    dicNational.Add("08", "Asian – Chinese / British / Scottish");
        //    dicNational.Add("09", "White – Other");
        //    dicNational.Add("10", "Not Disclosed");
        //    dicNational.Add("12", "Mixed or multiple ethnic groups");
        //    dicNational.Add("17", "Asian – Other");
        //    dicNational.Add("19", "White – Gypsy/Traveller");
        //    dicNational.Add("21", "White – Other British");
        //    dicNational.Add("22", "White – Irish");
        //    dicNational.Add("23", "White – Polish");
        //    dicNational.Add("24", "Caribbean or Black – Other");
        //    dicNational.Add("25", "African – Other");
        //    dicNational.Add("27", "Other – Arab");
        //    dicNational.Add("98", "Not Known");
        //    dicNational.Add("99", "Other – Other");
        //    return dicNational;
        //}

            

    }
}