using ACCDataStore.Entity;
using ACCDataStore.Web.Areas.Authorisation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using ACCDataStore.Repository;
using System.Web.Security;

namespace ACCDataStore.Web.Areas.Authorisation.Controllers
{
    public class IndexAuthorisationController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexAuthorisationController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public IndexAuthorisationController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        // GET: Authorisation/IndexAuthorisation
        public ActionResult Index()
        {
            var vmIndex = new IndexViewModel();
            vmIndex.ApplicationName = HttpContext.Application["APP_NAME"] as string;
            vmIndex.ApplicationVersion = HttpContext.Application["APP_VERSION"] as string;
            return View(vmIndex);           
            //var listUsersFindAll = this.rpGeneric.FindAll<Users>();

            //// insert 1st db
            //var eUsersToInsert = new Users();
            //eUsersToInsert.UserName = "John1st";
            //eUsersToInsert.Password = "1234";
            //this.rpGeneric.SaveOrUpdate<Users>(eUsersToInsert);

        }

        public ActionResult Login(IndexViewModel vmIndex)
        {
            try
            {

                // user logined
                if (vmIndex.Username.Equals("demo", StringComparison.CurrentCultureIgnoreCase) && vmIndex.Password.Equals("demo", StringComparison.CurrentCultureIgnoreCase))
                {
                    // store user session for 'demo' user
                    var eUsers = new Users();
                    eUsers.UserID = 0; // just simulate
                    eUsers.UserName = "demo";
                    eUsers.IsAdministrator = false; // 'demo' is just normal user, not admin
                    Session["SessionUser"] = eUsers;
                    
                    return RedirectToAction("Index", "Index", new { Area = "", id = "" });
                }
                else if (vmIndex.Username.Equals("admin", StringComparison.CurrentCultureIgnoreCase) && vmIndex.Password.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
                {
                    // store user session for 'admin' user
                    var eUsers = new Users();
                    eUsers.UserID = 1; // just simulate
                    eUsers.UserName = "admin";
                    eUsers.IsAdministrator = true; // 'admin' is admin user
                    Session["SessionUser"] = eUsers;
                    //ValidateUser("admin", "admin");
                    return RedirectToAction("Index", "Index", new { Area = "", id = "" });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Logout(IndexViewModel vmIndex)
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }

        private bool ValidateUser(string userName, string password)
        {

            bool isValid = false;

            var listResultMySQL2 = this.rpGeneric2nd.FindAll<Users>();

            var listResultMySQL21 = this.rpGeneric2nd.Find<Users>(" from Users where Users.UserName = :userName ", new string[] { "userName" }, new object[] { "Jan" });



            //Users eUsers = this.rpGeneric2nd.Find<Users>(" from Users where Users.UserName = :userName ", new string[] { "userName" }, new object[] { userName }).FirstOrDefault(); 
 



            return isValid;
        }

    }
}