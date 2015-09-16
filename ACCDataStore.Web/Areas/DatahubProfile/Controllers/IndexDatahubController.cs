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

            vmDatahubViewModel.Aberdeencity = GetDatahubdatabySchoolcode(rpGeneric, null);
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
            var listdata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();

            if (seedcode != null)
            {
                listdata = (from a in listdata where a.SEED_Code != null && a.SEED_Code.Equals(seedcode) select a).ToList();
            }
            //var number = listResultMSAccess.Where(x => x.SDS_Client_Ref.Equals("")).Count();
            //Opportunities for all
            datahubdata.allpupils = listdata.Count(x => !x.SDS_Client_Ref.Equals(""));
            datahubdata.allFemalepupils = listdata.Count(x => x.Gender.ToLower().Equals("female"));
            datahubdata.allMalepupils = listdata.Count(x => x.Gender.ToLower().Equals("male"));
            datahubdata.all16pupils = listdata.Count(x => x.Age == 16);
            datahubdata.all17pupils = listdata.Count(x => x.Age == 17);
            datahubdata.all18pupils = listdata.Count(x => x.Age == 18);
            datahubdata.all19pupils = listdata.Count(x => x.Age == 19);

            // Current positive
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
            datahubdata.AvgWeekssinceLastPositiveDestination = listdata.Where(x => x.Weeks_since_last_Pos_Status != null).DefaultIfEmpty().Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status));
            datahubdata.pupilsinCustody = listdata.Count(x => x.Current_Status.ToLower().Equals("custody"));
            datahubdata.pupilsinEconomically = listdata.Count(x => x.Current_Status.ToLower().Equals("economically inactive"));
            datahubdata.pupilsinUnavailableillHealth = listdata.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health"));
            datahubdata.pupilsinUnemployed = listdata.Count(x => x.Current_Status.ToLower().Equals("unemployed"));
            datahubdata.pupilsinUnknown = listdata.Count(x => x.Current_Status.ToLower().Equals("unknown"));
            return datahubdata;
        }
    }
}