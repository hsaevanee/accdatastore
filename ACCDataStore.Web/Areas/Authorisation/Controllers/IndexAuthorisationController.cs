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
using System.Security.Cryptography;

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
            var sKey = "yoursecretkey";
            var sTextRaw = "password";
            var sTextEncrypt = EncryptString(sTextRaw, sKey);
            var sTextDecrypt = DecryptString(sTextEncrypt, sKey);


            return View("Login",vmIndex);           
            //var listUsersFindAll = this.rpGeneric.FindAll<Users>();

            //// insert 1st db
            //var eUsersToInsert = new Users();
            //eUsersToInsert.UserName = "John1st";
            //eUsersToInsert.Password = "1234";
            //this.rpGeneric.SaveOrUpdate<Users>(eUsersToInsert);

            // how to use encryptstring and decryptstring
            //var sKey = "yoursecretkey";
            //var sTextRaw = "password";
            //var sTextEncrypt = EncryptString(sTextRaw, sKey);
            //var sTextDecrypt = DecryptString(sTextEncrypt, sKey);

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
                else if (vmIndex.Username.Equals("UserSchoolProfile", StringComparison.CurrentCultureIgnoreCase) && vmIndex.Password.Equals("schoolprofile", StringComparison.CurrentCultureIgnoreCase))
                {
                    // store user session for 'admin' user
                    var eUsers = new Users();
                    eUsers.UserID = 1; // just simulate
                    eUsers.UserName = "UserSchoolProfile";
                    eUsers.IsAdministrator = true; // 'admin' is admin user
                    Session["SessionUser"] = eUsers;
                    //ValidateUser("admin", "admin");
                    return RedirectToAction("Index", "Index", new { Area = "", id = "" });
                }
                else if (vmIndex.Username.Equals("admin", StringComparison.CurrentCultureIgnoreCase) && vmIndex.Password.Equals("adminhatai1232498!", StringComparison.CurrentCultureIgnoreCase))
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
                    return RedirectToAction("Index", "IndexAuthorisation", new { Area = "Authorisation", id = "" });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return RedirectToAction("Index", "IndexAuthorisation", new { Area = "Authorisation", id = "" });
            }
        }

        public ActionResult Logout(IndexViewModel vmIndex)
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }

        public ActionResult Register(IndexViewModel vmIndex)
        {
            try
            {

                return RedirectToAction("Index");
             }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return RedirectToAction("Index");
            }

        }

        private bool ValidateUser(string userName, string password)
        {

            bool isValid = false;

            var listResultMySQL2 = this.rpGeneric2nd.FindAll<Users>();

            var listResultMySQL21 = this.rpGeneric2nd.Find<Users>(" from Users where Users.UserName = :userName ", new string[] { "userName" }, new object[] { "Jan" });



            //Users eUsers = this.rpGeneric2nd.Find<Users>(" from Users where Users.UserName = :userName ", new string[] { "userName" }, new object[] { userName }).FirstOrDefault(); 

            return isValid;
        }

        static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

    }
}