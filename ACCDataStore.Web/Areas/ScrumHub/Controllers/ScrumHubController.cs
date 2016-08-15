using ACCDataStore.Entity.ScrumHub;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.ScrumHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.ScrumHub.Controllers
{
    public class ScrumHubController : Controller
    {
        private readonly IGenericRepository2nd rpGeneric2nd;
        private List<ScrumTable> listResult;
        private List<ScrumCourseModel> listCourses;
        // GET: ScrumHub/ScrumHub
        public ScrumHubController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd; //connect to accdatastore database in MySQL
            this.listResult = this.rpGeneric2nd.FindAll<ScrumTable>().ToList();
            this.listCourses = this.rpGeneric2nd.FindAll<ScrumCourseModel>().ToList();
        }
        /*public ActionResult Index()
        {
            //List<ScrumTable> listResult = this.rpGeneric2nd.FindAll<ScrumTable>().ToList();
            //List<ScrumCourseModel> listCourses = this.rpGeneric2nd.FindAll<ScrumCourseModel>().ToList();
            ScrumViewModel x = new ScrumViewModel();
            List<UserCourses> z = (from a in listResult join b in listCourses on a.id equals b.id select new UserCourses { id = a.id, firstname = a.firstname, lastname = a.lastname, age = a.age, course_code = b.course_code, course_title = b.course_title, coordinator = b.coordinator }).ToList();
            string query = "SELECT * FROM scrumtable";
            string query2 = "SELECT t1.id,t1.firstname,t1.lastname,t1.age,t2.course_code FROM scrumcourses t2, scrumtable t1 WHERE t1.id = t2.id";
            List<ScrumTable> newlist = new List<ScrumTable>();
            
            List<UserCourses> usercourselist = new List<UserCourses>();

            var results = rpGeneric2nd.FindByNativeSQL(query);
            var results2 = rpGeneric2nd.FindByNativeSQL(query2);

            foreach (var individual in results2) 
            {
                usercourselist.Add(new UserCourses { id = int.Parse(individual[0].ToString()), firstname = individual[1].ToString(), lastname = individual[2].ToString(), age = int.Parse(individual[3].ToString()), course_code = individual[4].ToString() });
            }

            foreach (var person in results)
            {
                newlist.Add(new ScrumTable { id = int.Parse(person[0].ToString()), firstname = person[1].ToString(), lastname = person[2].ToString(), age = int.Parse(person[3].ToString()) });
            }
            x.ListCourses = listCourses;
            //x.scrumlist = listResult.OrderBy(y => y.age).ToList();
            x.scrumlist = newlist;
            //x.joined = z;
            x.joined = usercourselist;

            return View("ScrumIndex", x);
        }*/
        public ActionResult Index()
        {
            ScrumViewModel x = new ScrumViewModel();
            x.scrumlist = listResult.OrderBy(y => y.age).ToList();
            x.ListCourses = listCourses;
            x.Meta1 = new ScrumTable();
            x.Meta2 = new ScrumCourseModel();
            return View("ScrumIndex", x);
        }

        public ActionResult Form1()
        {
            ScrumViewModel viewModel = new ScrumViewModel();
            List<string> scrumTable1Parameters = Request["table1"].Split(',').ToList();
            List<string> scrumTable2Parameters = Request["table2"].Split(',').ToList();
            List<UserCourses> scrumJoinTable = (from a in listResult join b in listCourses on a.id equals b.id select new UserCourses { id = a.id, firstname = a.firstname, lastname = a.lastname, age = a.age, course_code = b.course_code, course_title = b.course_title, coordinator = b.coordinator }).ToList();
            viewModel.joined = scrumJoinTable;
            viewModel.Meta1 = new ScrumTable();
            viewModel.Meta2 = new ScrumCourseModel();

            viewModel.paramlist = scrumTable1Parameters.Concat(scrumTable2Parameters).ToList();
            return View("ScrumIndex", viewModel);
        }

        public JsonResult scrumChart() 
        {
            object chartData = new object();
            chartData = new
                {people = listResult};
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
    }
}