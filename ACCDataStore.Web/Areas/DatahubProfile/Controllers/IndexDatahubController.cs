using ACCDataStore.Entity;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.DatahubProfile.Controllers
{
    public class IndexDatahubController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexDatahubController));

        private readonly IGenericRepository rpGeneric;

        public IndexDatahubController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
        // GET: DatahubProfile/IndexDatahub
        public ActionResult Index()
        {
            //var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmDatahubViewModel = new DatahubViewModel();
            var datahubAbdcitydata = new DatahubData(); 
            var listResultMSAccess = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();
            //var number = listResultMSAccess.Where(x => x.SDS_Client_Ref.Equals("")).Count();
            datahubAbdcitydata.allpupils = listResultMSAccess.Count(x => !x.SDS_Client_Ref.Equals(""));
            datahubAbdcitydata.allFemalepupils = listResultMSAccess.Count(x => x.Gender.ToLower().Equals("female"));
            datahubAbdcitydata.allMalepupils = listResultMSAccess.Count(x => x.Gender.ToLower().Equals("male"));
            datahubAbdcitydata.all16pupils = listResultMSAccess.Count(x => x.Age == 16);
            datahubAbdcitydata.all17pupils = listResultMSAccess.Count(x => x.Age == 17);
            datahubAbdcitydata.all18pupils = listResultMSAccess.Count(x => x.Age == 18);
            datahubAbdcitydata.all19pupils = listResultMSAccess.Count(x => x.Age == 19);
            datahubAbdcitydata.schoolpupils = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("school pupil"));
            datahubAbdcitydata.schoolpupilsintransition = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition"));
            datahubAbdcitydata.schoolpupilsmovedoutinscotland = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland"));
            datahubAbdcitydata.pupilsinAtivityAgreement = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("activity agreement"));
            datahubAbdcitydata.pupilsinEmployFundSt2 = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2"));
            datahubAbdcitydata.pupilsinEmployFundSt3 = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3"));
            datahubAbdcitydata.pupilsinEmployFundSt4 = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4"));
            datahubAbdcitydata.pupilsinFullTimeEmployed = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("full-time employment"));
            datahubAbdcitydata.pupilsinFurtherEdu = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("further education"));
            datahubAbdcitydata.pupilsinHigherEdu = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("higher education"));
            datahubAbdcitydata.pupilsinModernApprenship = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship"));
            datahubAbdcitydata.pupilsinPartTimeEmployed = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("part-time employment"));
            datahubAbdcitydata.pupilsinPersonalSkillDevelopment = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development"));
            datahubAbdcitydata.pupilsinSelfEmployed = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("self-employed"));
            datahubAbdcitydata.pupilsinTraining = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)"));
            datahubAbdcitydata.pupilsinVolunteerWork = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("voluntary work"));
            
            // Non positive 
            datahubAbdcitydata.AvgWeekssinceLastPositiveDestination = listResultMSAccess.Where(x => x.Weeks_since_last_Pos_Status != null).Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status));
            datahubAbdcitydata.pupilsinCustody = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("custody"));
            datahubAbdcitydata.pupilsinEconomically = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("economically inactive"));
            datahubAbdcitydata.pupilsinUnavailableillHealth = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health"));
            datahubAbdcitydata.pupilsinUnemployed = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("unemployed"));
            datahubAbdcitydata.pupilsinUnknown = listResultMSAccess.Count(x => x.Current_Status.ToLower().Equals("unknown"));
            vmDatahubViewModel.Aberdeencity = datahubAbdcitydata;
            vmDatahubViewModel.ListSchoolNameData = GetListSchoolname(rpGeneric);
            
            var sSchoolcode = Request["selectedschoolcode"];

            if (sSchoolcode != null) {
                //vmDatahubViewModel.selectedschoolname = vmDatahubViewModel.ListSchoolNameData.FirstOrDefault(x => x.seedcode.Equals(sSchoolcode)).Select(x => x.name).ToString();
                vmDatahubViewModel.SelectedSchool = GetDatahubdatabySchoolcode(rpGeneric, sSchoolcode);
            }
            return View("index", vmDatahubViewModel);
        }

        protected IList<School> GetListSchoolname(IGenericRepository rpGeneric)
        {
           List<School> temp = new List<School>();
           temp.Add(new School("5244439", "Aberdeen Grammar School"));
           temp.Add(new School("5235634", "Bridge Of Don Academy"));
           temp.Add(new School("5234034", "Bucksburn Academy"));
           temp.Add(new School("5248744", "Cordyce School"));
           temp.Add(new School("5235839", "Cults Academy"));
           temp.Add(new School("5243335", "Dyce Academy"));
           temp.Add(new School("5243238", "Harlaw Academy"));
           temp.Add(new School("5243432", "Hazlehead Academy"));
           temp.Add(new School("5244943", "Hazlewood School"));
           temp.Add(new School("5243831", "Kincorth Academy"));
           temp.Add(new School("5244234", "Northfield Academy"));
           temp.Add(new School("5246237", "Oldmachar Academy"));
           temp.Add(new School("5246431", "St Machar Academy"));
           temp.Add(new School("5244838", "Torry Academy"));
            return temp;

        }


        protected DatahubData GetDatahubdatabySchoolcode(IGenericRepository rpGeneric,string seedcode)
        {
            var datahubdata = new DatahubData();
            var listResultMSAccess = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();

            var n = listResultMSAccess.Count();
            
            var listdata = (from a in listResultMSAccess where a.SEED_Code!=null && a.SEED_Code.Equals(seedcode) select a).ToList();
            
            //var number = listResultMSAccess.Where(x => x.SDS_Client_Ref.Equals("")).Count();
            datahubdata.allpupils = listdata.Count(x => !x.SDS_Client_Ref.Equals(""));
            datahubdata.allFemalepupils = listdata.Count(x => x.Gender.ToLower().Equals("female"));
            datahubdata.allMalepupils = listdata.Count(x => x.Gender.ToLower().Equals("male"));
            datahubdata.all16pupils = listdata.Count(x => x.Age == 16);
            datahubdata.all17pupils = listdata.Count(x => x.Age == 17);
            datahubdata.all18pupils = listdata.Count(x => x.Age == 18);
            datahubdata.all19pupils = listdata.Count(x => x.Age == 19);
            datahubdata.schoolpupils = listdata.Count(x => x.Current_Status.ToLower().Equals("school pupil"));
            datahubdata.schoolpupilsintransition = listdata.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition"));
            datahubdata.schoolpupilsmovedoutinscotland = listdata.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland"));
            datahubdata.pupilsinAtivityAgreement = listdata.Count(x => x.Current_Status.ToLower().Equals("activity agreement"));
            datahubdata.pupilsinEmployFundSt2 = listdata.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2"));
            datahubdata.pupilsinEmployFundSt3 = listdata.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3"));
            datahubdata.pupilsinEmployFundSt4 = listdata.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4"));
            datahubdata.pupilsinFullTimeEmployed = listdata.Count(x => x.Current_Status.ToLower().Equals("full-time employment"));
            datahubdata.pupilsinFurtherEdu = listdata.Count(x => x.Current_Status.ToLower().Equals("further education"));
            datahubdata.pupilsinHigherEdu = listdata.Count(x => x.Current_Status.ToLower().Equals("higher education"));
            datahubdata.pupilsinModernApprenship = listdata.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship"));
            datahubdata.pupilsinPartTimeEmployed = listdata.Count(x => x.Current_Status.ToLower().Equals("part-time employment"));
            datahubdata.pupilsinPersonalSkillDevelopment = listdata.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development"));
            datahubdata.pupilsinSelfEmployed = listdata.Count(x => x.Current_Status.ToLower().Equals("self-employed"));
            datahubdata.pupilsinTraining = listdata.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)"));
            datahubdata.pupilsinVolunteerWork = listdata.Count(x => x.Current_Status.ToLower().Equals("voluntary work"));

            // Non positive 
            datahubdata.AvgWeekssinceLastPositiveDestination = listdata.Where(x => x.Weeks_since_last_Pos_Status != null).Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status));
            datahubdata.pupilsinCustody = listdata.Count(x => x.Current_Status.ToLower().Equals("custody"));
            datahubdata.pupilsinEconomically = listdata.Count(x => x.Current_Status.ToLower().Equals("economically inactive"));
            datahubdata.pupilsinUnavailableillHealth = listdata.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health"));
            datahubdata.pupilsinUnemployed = listdata.Count(x => x.Current_Status.ToLower().Equals("unemployed"));
            datahubdata.pupilsinUnknown = listdata.Count(x => x.Current_Status.ToLower().Equals("unknown"));
            return datahubdata;
        }
    }
}