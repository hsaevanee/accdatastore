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

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmIndexSchoolProfile.ListSchoolNameData = fooList;

            if (sSchoolName == null)
            {
                sSchoolName = fooList[0];
            }

            vmIndexSchoolProfile.selectedschoolname = sSchoolName;
            vmIndexSchoolProfile.ListEthnicData = GetEthnicityDatabySchoolname(this.rpGeneric, sSchoolName);
            vmIndexSchoolProfile.ListNationalityData = GetNationalityDatabySchoolname(this.rpGeneric, sSchoolName);
            vmIndexSchoolProfile.ListSIMDData = GetSIMDDatabySchoolname(this.rpGeneric, sSchoolName, new List<string>(new string[] { "2012" }));
            vmIndexSchoolProfile.ListStdStageData = GetStudentStageDatabySchoolname(this.rpGeneric, sSchoolName);

            List<string> TempCode = new List<string>();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW SIMD_2012_decile FROM test_3 group by SIMD_2012_decile");

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        TempCode.Add(itemRow.ToString());
                    }
                }
            }


            vmIndexSchoolProfile.ListSIMDCode = TempCode;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW EthnicBackground FROM test_3 group by EthnicBackground");

            fooList = listResult.OfType<string>().ToList();

            vmIndexSchoolProfile.ListEthnicCode = fooList;

            vmIndexSchoolProfile.DicEthnicBG = GetDicEhtnicBG();


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW NationalIdentity FROM test_3 group by NationalIdentity");

            fooList = listResult.OfType<string>().ToList();
            vmIndexSchoolProfile.ListNationalityCode = fooList;
            vmIndexSchoolProfile.DicNational = GetDicNational();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM test_3 group by StudentStage");

            fooList = listResult.OfType<string>().ToList();
            vmIndexSchoolProfile.ListStageCode = fooList;

            return View("index", vmIndexSchoolProfile);



        }

        public ActionResult Compareable()
        {
            var vmIndex2SchoolProfile = new Index2SchoolProfileViewModel();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmIndex2SchoolProfile.ListSchoolNameData = fooList;
            vmIndex2SchoolProfile.ListSchoolNameData2 = fooList;
            var sSchoolName1 = Request["selectedschoolname"];

            var sSchoolName2 = Request["selectedschoolname2"];

            if (sSchoolName1 == null && sSchoolName2 ==null)
            {
                sSchoolName1 = fooList[0];
                sSchoolName2 = fooList[1];
            }

            // Data for sSchoolName1

            vmIndex2SchoolProfile.selectedschoolname = sSchoolName1;
            vmIndex2SchoolProfile.ListEthnicData = GetEthnicityDatabySchoolname(this.rpGeneric, sSchoolName1);
            vmIndex2SchoolProfile.ListNationalityData = GetNationalityDatabySchoolname(this.rpGeneric, sSchoolName1);
            vmIndex2SchoolProfile.ListSIMDData = GetSIMDDatabySchoolname(this.rpGeneric, sSchoolName1, new List<string>(new string[] { "2012" }));
            vmIndex2SchoolProfile.ListStdStageData = GetStudentStageDatabySchoolname(this.rpGeneric, sSchoolName1);

                // Data for sSchoolName2

            vmIndex2SchoolProfile.selectedschoolname2 = sSchoolName2;
            vmIndex2SchoolProfile.ListEthnicData2 = GetEthnicityDatabySchoolname(this.rpGeneric, sSchoolName2);
            vmIndex2SchoolProfile.ListNationalityData2 = GetNationalityDatabySchoolname(this.rpGeneric, sSchoolName2);
            vmIndex2SchoolProfile.ListSIMDData2 = GetSIMDDatabySchoolname(this.rpGeneric, sSchoolName2, new List<string>(new string[] { "2012" }));
            vmIndex2SchoolProfile.ListStdStageData2 = GetStudentStageDatabySchoolname(this.rpGeneric, sSchoolName2);


            List<string> TempCode = new List<string>();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW SIMD_2012_decile FROM test_3 group by SIMD_2012_decile");
            
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        TempCode.Add(itemRow.ToString());
                    }
                }
            }


            vmIndex2SchoolProfile.ListSIMDCode = TempCode;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW EthnicBackground FROM test_3 group by EthnicBackground");

            fooList = listResult.OfType<string>().ToList();

            vmIndex2SchoolProfile.ListEthnicCode = fooList;

            vmIndex2SchoolProfile.DicEthnicBG = GetDicEhtnicBG();


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW NationalIdentity FROM test_3 group by NationalIdentity");

            fooList = listResult.OfType<string>().ToList();
            vmIndex2SchoolProfile.ListNationalityCode = fooList;
            vmIndex2SchoolProfile.DicNational = GetDicNational();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM test_3 group by StudentStage");

            fooList = listResult.OfType<string>().ToList();
            vmIndex2SchoolProfile.ListStageCode = fooList;

            return View("index2", vmIndex2SchoolProfile);           
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