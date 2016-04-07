using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Data;
using Common.Logging;
using ACCDataStore.Repository;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity;
using System.IO;
using ACCDataStore.Web.Areas.SchoolRollForecast.ViewModels;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using ACCDataStore.Entity.SchoolRollForecast;


namespace ACCDataStore.Web.Areas.SchoolRollForecast.Controllers
{
    public class IndexSchoolRollForecastController : Controller
    {
        // GET: SchoolRollForecast/IndexSchoolRollForecast
        public ActionResult Index()
        {
            var vmSchoolRollForecastViewModel = new SchoolRollForecastViewModel();
            SchoolRollForecastObj schObj = new SchoolRollForecastObj();
                 
            string filePath = "C:\\data\\BonAccord.xlsx";
            List<double> ListP1Input = new List<double>();
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);
                int autonumber = 5;
                for (int i = 2; i <= 13; i++)
                {
                    ListP1Input.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP1Input = ListP1Input;

                List<double> ListPupilsHhld = new List<double>();
                autonumber = 6;
                for (int i = 2; i <= 13; i++)
                {
                    ListPupilsHhld.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListPupilsHhld = ListPupilsHhld;

                List<double> ListHousing = new List<double>();
                autonumber = 7;
                for (int i = 2; i <= 13; i++)
                {
                    ListHousing.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListHousing = ListHousing;

                List<double> ListParentsCharter = new List<double>();
                autonumber = 8;
                for (int i = 2; i <= 13; i++)
                {
                    ListParentsCharter.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListParentsCharter = ListParentsCharter;

                List<double> ListP1 = new List<double>();
                autonumber = 10;
                for (int i = 2; i <= 13; i++)
                {
                    ListP1.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP1 = ListP1;

                List<double> ListP2 = new List<double>();
                autonumber = 11;
                for (int i = 2; i <= 13; i++)
                {
                    ListP2.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP2 = ListP2;

                List<double> ListP3 = new List<double>();
                autonumber = 12;
                for (int i = 2; i <= 13; i++)
                {
                    ListP3.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP3 = ListP3;

                List<double> ListP4 = new List<double>();
                autonumber = 13;
                for (int i = 2; i <= 13; i++)
                {
                    ListP4.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP4 = ListP4;

                List<double> ListP5 = new List<double>();
                autonumber = 14;
                for (int i = 2; i <= 13; i++)
                {
                    ListP5.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP5 = ListP5;

                List<double> ListP6 = new List<double>();
                autonumber = 15;
                for (int i = 2; i <= 13; i++)
                {
                    ListP6.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP6 = ListP6;

                List<double> ListP7 = new List<double>();
                autonumber = 16;
                for (int i = 2; i <= 13; i++)
                {
                    ListP7.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP7 = ListP7;

                List<double> ListTotalRoll = new List<double>();
                autonumber = 17;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotalRoll.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotalRoll = ListTotalRoll;

                List<double> ListMaxCap = new List<double>();
                autonumber = 18;
                for (int i = 2; i <= 13; i++)
                {
                    ListMaxCap.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListMaxCap = ListMaxCap;

                List<double> ListTotRollFunctWCap = new List<double>();
                autonumber = 19;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotRollFunctWCap.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotRollFunctWCap = ListTotRollFunctWCap;

                List<double> ListTotRollFunctWCapPer = new List<double>();
                autonumber = 20;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotRollFunctWCapPer.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotRollFunctWCapPer = ListTotRollFunctWCapPer;

                vmSchoolRollForecastViewModel.schObj = schObj;

                Session["vmSchoolRollForecastViewModel"] = schObj;

            }
            return View("Home", vmSchoolRollForecastViewModel);
        }

        public ActionResult Calculate(string calculateButton)
        {
            var vmSchoolRollForecastViewModel = new SchoolRollForecastViewModel();

            SchoolRollForecastObj schObj = Session["vmSchoolRollForecastViewModel"] as SchoolRollForecastObj;

            if (calculateButton != null)
            {           
                if (Request["housing"] != null)
                {
                    var housinglist = Request["housing"].Split(',').Select(Double.Parse).ToList();
                }
            }

            vmSchoolRollForecastViewModel.schObj = schObj;
            return View("Home", vmSchoolRollForecastViewModel);
        }
    }
}