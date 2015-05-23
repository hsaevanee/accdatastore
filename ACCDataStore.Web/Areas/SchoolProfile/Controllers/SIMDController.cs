using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.SIMD;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class SIMDController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(SIMDController));

        private readonly IGenericRepository rpGeneric;

        public SIMDController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
 
        // GET: SchoolProfile/SIMD
        public ActionResult Index(string sSchoolName)
        {
            var vmSIMD = new SIMDViewModel();

            var schoolname = new List<string>();

            var setSIMDCriteria = new List<string>();
            var setYearCriteria = new List<string>();

            List<SIMDObj> ListSIMDData = new List<SIMDObj>();
            List<SIMDObj> temp = new List<SIMDObj>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmSIMD.ListSchoolNameData = fooList;

            fooList = new List<string>();
                
            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW SIMD_2012_decile FROM test_3 group by SIMD_2012_decile");

            if (listResult != null)
            {   
                foreach (var itemRow in listResult)
                {
                    if (itemRow!=null)
                    fooList.Add(Convert.ToString(itemRow));
                }
            }

            vmSIMD.ListSIMDdefinition= fooList;
            vmSIMD.ListYear = new List<string>(new string[] { "2009", "2012"});

            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmSIMD.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmSIMD.IsShowCriteria = false;
                }

            }
            else // post method
            {
                // get parameter from Request object
                vmSIMD.IsShowCriteria = true;
                sSchoolName = Request["selectSchoolname"];
                setSIMDCriteria = Request["SIMD"].Split(',').ToList();
                setYearCriteria = Request["Years"].Split(',').ToList();                
            }

            // process data
            if (sSchoolName != null)
            {
                vmSIMD.selectedschoolname = sSchoolName;
                ListSIMDData = GetSIMDDatabySchoolname(rpGeneric, sSchoolName);
                if (setSIMDCriteria.Count != 0)
                {
                    vmSIMD.ListSIMDData= ListSIMDData.Where(x => setSIMDCriteria.Contains(x.SIMDCode)).ToList();
                }
                else
                {
                    vmSIMD.ListSIMDData = ListSIMDData;
                }
                Session["SessionListSIMDData"] = vmSIMD.ListSIMDData;
            }

           // vmSIMD.ListSIMDdata = GetSIMDDatabySchoolname(sSchoolName);
            return View("Index", vmSIMD);
        }


    }
}