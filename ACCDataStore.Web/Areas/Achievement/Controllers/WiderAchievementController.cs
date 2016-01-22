using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.Achievement.ViewModels.WiderAchievement;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.Achievement.Controllers
{
    public class WiderAchievementController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(WiderAchievementController));

        private readonly IGenericRepository rpGeneric;
        private readonly IGenericRepository3nd rpGeneric3nd;

        public WiderAchievementController(IGenericRepository rpGeneric, IGenericRepository3nd rpGeneric3nd)
        {
            this.rpGeneric = rpGeneric;
            this.rpGeneric3nd = rpGeneric3nd;
        }

        public ActionResult IndexSummary(string schoolsubmitButton, string awardsubmitButton)
        {
            //var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmWiderAchievement = new WiderAchievementViewModel();

            vmWiderAchievement.ListWiderAchievementData = GetWiderAchievementdata(this.rpGeneric);

            return View("IndexSummary", vmWiderAchievement);
        }

        public ActionResult Index(string schoolsubmitButton, string awardsubmitButton)
        {
            //var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmWiderAchievement = new WiderAchievementViewModel();
            vmWiderAchievement.Listschoolname = GetListschoolname(this.rpGeneric);
            vmWiderAchievement.Listawardname = GetListAwardname(this.rpGeneric);

            vmWiderAchievement.ListWiderAchievementData = GetWiderAchievementdata(this.rpGeneric);

            List<WiderAchievementObj> temp = new List<WiderAchievementObj>();

            if (schoolsubmitButton != null)
            {
                var sSchoolname = Request["selectedschoolname"];
                vmWiderAchievement.selectedschoolname = sSchoolname;
                if (sSchoolname != null)
                {
                    List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
                    if (listdata != null)
                    {
                        temp = (from a in listdata where a.centre.Equals(sSchoolname) select a).ToList();

                    }

                }
            }
            if (awardsubmitButton != null)
            {
                var sAwardname = Request["selectedawardname"];
                vmWiderAchievement.selectedawardname = sAwardname;
                if (sAwardname != null)
                {
                    List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
                    if (listdata != null)
                    {
                        temp = (from a in listdata where a.awardname.Equals(sAwardname) select a).ToList();

                    }

                }
            }

            vmWiderAchievement.Listresults = temp;

            return View("index", vmWiderAchievement);
        }

        protected List<WiderAchievementObj> GetWiderAchievementdata(IGenericRepository rpGeneric)
        {
            Console.Write("GetWiderAchievementdata in BaseSchoolProfileController==> ");

            List<WiderAchievementObj> listdata = rpGeneric.FindAll<WiderAchievementObj>().ToList();
            List<WiderAchievementObj> temp = new List<WiderAchievementObj>();

            if (listdata != null)
            {
                //temp = (from a in listdata where a.schoolname.Equals(sSchoolname) select a).ToList();
                temp = listdata.GroupBy(a => new { a.age_range, a.awardname }).Select(x => new WiderAchievementObj
                {
                    age_range = x.Key.age_range,
                    awardname = x.Key.awardname,
                    award2013 = x.Sum(y => y.award2013),
                    award2014 = x.Sum(y => y.award2014),
                    award2015 = x.Sum(y => y.award2015),
                }).ToList();

            }

            temp = temp.OrderByDescending(x => x.age_range).ThenBy(x => x.awardname).ToList();

            return temp;
        }

        protected List<School> GetListschoolname(IGenericRepository rpGeneric)
        {
            List<School> temp = new List<School>();
            var listdata = rpGeneric.FindSingleColumnByNativeSQL("Select distinct schoolname from WiderAchievementdata");
            if (listdata != null)
            {
                foreach (var item in listdata)
                {
                    if (item != null)
                    {
                        temp.Add(new School(item.ToString(), item.ToString()));
                    }

                }
            }
            return temp;

        }

        protected List<string> GetListAwardname(IGenericRepository rpGeneric)
        {
            List<string> temp = new List<string>();
            var listdata = rpGeneric.FindSingleColumnByNativeSQL("Select distinct awardname from WiderAchievementdata");
            if (listdata != null)
            {
                foreach (var item in listdata)
                {
                    if (item != null)
                    {
                        temp.Add(item.ToString());
                    }

                }
            }
            return temp;

        }
    }
}