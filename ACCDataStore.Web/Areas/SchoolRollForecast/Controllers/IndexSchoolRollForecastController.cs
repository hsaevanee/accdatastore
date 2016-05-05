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

        List<double> ListP1Input = new List<double>();
        public List<double> ListHousing = new List<double>();
        List<double> ListParentsCharter = new List<double>();
        List<double> ListPupilsHhld = new List<double>();
        List<double> ListP1 = new List<double>();
        List<double> ListP2 = new List<double>();
        List<double> ListP3 = new List<double>();
        List<double> ListP4 = new List<double>();
        List<double> ListP5 = new List<double>();
        List<double> ListP6 = new List<double>();
        List<double> ListP7 = new List<double>();
        List<double> ListS1 = new List<double>();
        List<double> ListS2 = new List<double>();
        List<double> ListS3 = new List<double>();
        List<double> ListS4 = new List<double>();
        List<double> ListS5 = new List<double>();
        List<double> ListS6 = new List<double>();
        List<double> ListTotalRoll = new List<double>();
        List<double> ListMaxCap = new List<double>();
        List<double> ListTotRollFunctWCap = new List<double>();
        List<double> ListTotalRollFunctWCapPer = new List<double>();
        //private List<double> P1Input;

        // GET: SchoolRollForecast/IndexSchoolRollForecast
        public ActionResult Index(string schoolsubmitButton)
        {
            var vmSchoolRollForecastViewModel = new SchoolRollForecastViewModel();
            SchoolRollForecastObj schObj = new SchoolRollForecastObj();

            var sSchoolName = Request["selectedschoolname"];

            string filePath = "C:\\data\\schools\\primary\\abbotswell.xlsx";

            if (sSchoolName != null)
            {
                filePath = sSchoolName;
            }
            //string filePath = "C:\\data\\BonAccord.xlsx";

            List<double> ListP1Input = new List<double>();
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //input sheet =1, Granite city = 2 bon accord = 3 etc
                IXLWorksheet workSheet = workBook.Worksheet(1);
                int autonumber = 5;
                for (int i = 2; i <= 13; i++)
                {
                    ListP1Input.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP1Input = ListP1Input;

                //List<double> ListPupilsHhld = new List<double>();
                autonumber = 6;
                for (int i = 2; i <= 13; i++)
                {
                    ListPupilsHhld.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListPupilsHhld = ListPupilsHhld;

                //List<double> ListHousing = new List<double>();
                autonumber = 7;
                for (int i = 2; i <= 13; i++)
                {
                    ListHousing.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListHousing = ListHousing;

                //List<double> ListParentsCharter = new List<double>();
                autonumber = 8;
                for (int i = 2; i <= 13; i++)
                {
                    ListParentsCharter.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListParentsCharter = ListParentsCharter;

                //List<double> ListP1 = new List<double>();
                autonumber = 10;
                for (int i = 2; i <= 13; i++)
                {
                    ListP1.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP1 = ListP1;

                //List<double> ListP2 = new List<double>();
                autonumber = 11;
                for (int i = 2; i <= 13; i++)
                {
                    ListP2.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP2 = ListP2;

                //List<double> ListP3 = new List<double>();
                autonumber = 12;
                for (int i = 2; i <= 13; i++)
                {
                    ListP3.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP3 = ListP3;

                //List<double> ListP4 = new List<double>();
                autonumber = 13;
                for (int i = 2; i <= 13; i++)
                {
                    ListP4.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP4 = ListP4;

                //List<double> ListP5 = new List<double>();
                autonumber = 14;
                for (int i = 2; i <= 13; i++)
                {
                    ListP5.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP5 = ListP5;

                //List<double> ListP6 = new List<double>();
                autonumber = 15;
                for (int i = 2; i <= 13; i++)
                {
                    ListP6.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP6 = ListP6;

                // List<double> ListP7 = new List<double>();
                autonumber = 16;
                for (int i = 2; i <= 13; i++)
                {
                    ListP7.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP7 = ListP7;

                // List<double> ListTotalRoll = new List<double>();
                autonumber = 17;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotalRoll.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotalRoll = ListTotalRoll;

                // List<double> ListMaxCap = new List<double>();
                autonumber = 18;
                for (int i = 2; i <= 13; i++)
                {
                    ListMaxCap.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListMaxCap = ListMaxCap;

                //List<double> ListTotRollFunctWCap = new List<double>();
                autonumber = 19;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotRollFunctWCap.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotRollFunctWCap = ListTotRollFunctWCap;

                //List<double> ListTotRollFunctWCapPer = new List<double>();
                autonumber = 20;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotalRollFunctWCapPer.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotRollFunctWCapPer = ListTotalRollFunctWCapPer;

                vmSchoolRollForecastViewModel.schObj = schObj;

                vmSchoolRollForecastViewModel.listSchoolname = GetListSchoolName();
                vmSchoolRollForecastViewModel.selectedschoolname = sSchoolName;

                Session["vmSchoolRollForecastViewModel"] = vmSchoolRollForecastViewModel;

            }
            return View("Home", vmSchoolRollForecastViewModel);
        }

        // Do not touch this jan will kill you :P
        public ActionResult IndexPrimarySchool(string schoolsubmitButton)
        {
            var vmSchoolRollForecastViewModel = new SchoolRollForecastViewModel();
            SchoolRollForecastObj schObj = new SchoolRollForecastObj();

            var sSchoolName = Request["selectedschoolname"];

            string filePath = "C:\\data\\BonAccord.xlsx";

            if (sSchoolName != null)
            {
                filePath = sSchoolName;
            }
            //string filePath = "C:\\data\\Duthie.xlsx";

            List<double> ListP1Input = new List<double>();
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //input sheet =1, Granite city = 2 bon accord = 3 etc
                IXLWorksheet workSheet = workBook.Worksheet(1);
                int autonumber = 5;
                for (int i = 2; i <= 13; i++)
                {
                    ListP1Input.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP1Input = ListP1Input;

                //List<double> ListPupilsHhld = new List<double>();
                autonumber = 6;
                for (int i = 2; i <= 13; i++)
                {
                    ListPupilsHhld.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListPupilsHhld = ListPupilsHhld;

                //List<double> ListHousing = new List<double>();
                autonumber = 7;
                for (int i = 2; i <= 13; i++)
                {
                    ListHousing.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListHousing = ListHousing;

                //List<double> ListParentsCharter = new List<double>();
                autonumber = 8;
                for (int i = 2; i <= 13; i++)
                {
                    ListParentsCharter.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListParentsCharter = ListParentsCharter;

                //List<double> ListP1 = new List<double>();
                autonumber = 10;
                for (int i = 2; i <= 13; i++)
                {
                    ListP1.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP1 = ListP1;

                //List<double> ListP2 = new List<double>();
                autonumber = 11;
                for (int i = 2; i <= 13; i++)
                {
                    ListP2.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP2 = ListP2;

                //List<double> ListP3 = new List<double>();
                autonumber = 12;
                for (int i = 2; i <= 13; i++)
                {
                    ListP3.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP3 = ListP3;

                //List<double> ListP4 = new List<double>();
                autonumber = 13;
                for (int i = 2; i <= 13; i++)
                {
                    ListP4.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP4 = ListP4;

                //List<double> ListP5 = new List<double>();
                autonumber = 14;
                for (int i = 2; i <= 13; i++)
                {
                    ListP5.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP5 = ListP5;

                //List<double> ListP6 = new List<double>();
                autonumber = 15;
                for (int i = 2; i <= 13; i++)
                {
                    ListP6.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP6 = ListP6;

                // List<double> ListP7 = new List<double>();
                autonumber = 16;
                for (int i = 2; i <= 13; i++)
                {
                    ListP7.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListP7 = ListP7;

                // List<double> ListTotalRoll = new List<double>();
                autonumber = 17;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotalRoll.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotalRoll = ListTotalRoll;

                // List<double> ListMaxCap = new List<double>();
                autonumber = 18;
                for (int i = 2; i <= 13; i++)
                {
                    ListMaxCap.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListMaxCap = ListMaxCap;

                //List<double> ListTotRollFunctWCap = new List<double>();
                autonumber = 19;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotRollFunctWCap.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotRollFunctWCap = ListTotRollFunctWCap;

                //List<double> ListTotRollFunctWCapPer = new List<double>();
                autonumber = 20;
                for (int i = 2; i <= 13; i++)
                {
                    ListTotalRollFunctWCapPer.Add(workSheet.Row(autonumber).Cell(i).IsEmpty() ? 0.00 : workSheet.Row(autonumber).Cell(i).GetDouble());
                }
                schObj.ListTotRollFunctWCapPer = ListTotalRollFunctWCapPer;

                vmSchoolRollForecastViewModel.schObj = schObj;

                vmSchoolRollForecastViewModel.listSchoolname = GetListSchoolName();
                Session["vmSchoolRollForecastViewModel"] = vmSchoolRollForecastViewModel;

            }
            return View("Home", vmSchoolRollForecastViewModel);
        }


        protected List<School> GetListSchoolName()
        {
            List<School> temp = new List<School>();


            string filePath = "C:\\data\\schools\\primary\\";


            temp.Add(new School("AbbotsWell Primary (KincorthAcademy)", filePath+"abbotswell.xlsx"));
            temp.Add(new School("Airyhall Primary (Hazlehead Academy)", filePath+"airyhall.xlsx"));
            temp.Add(new School("Ashley Road Primary (Aberdeen Grammar)", filePath + "ashleyrd.xlsx"));
            temp.Add(new School("Braehead Primary (BOD Academy)", filePath + "braehead.xlsx"));
            temp.Add(new School("Bramblebrae Primary (Northfield Academy)", filePath + "bramblebrae.xlsx"));
            temp.Add(new School("Brimmond Primary (Bucksburn Academy)", filePath + "brimmond.xlsx"));
            temp.Add(new School("Broomhill Primary (Harlaw Academy)", filePath + "broomhill.xlsx"));
            temp.Add(new School("Charleston Primary (Kincorth Academy)", filePath + "charleston.xlsx"));
            temp.Add(new School("Cornhill Primary (St Machar Academy)", filePath + "cornhill.xlsx"));
            temp.Add(new School("Culter Primary (Cults Academy)", filePath + "culter.xlsx"));
            temp.Add(new School("Cults Primary (Cults Academy)", filePath + "cults.xlsx"));
            temp.Add(new School("Danestone Primary (Old Machar Academy)", filePath + "danestone.xlsx"));
            temp.Add(new School("Dyce Primary (Dyce Academy)", filePath + "dyce.xlsx"));
            temp.Add(new School("Fernielea Primary (HazleHead Academy)", filePath + "fernielea.xlsx"));
            temp.Add(new School("FerryHill Primary (Harlaw Academy)", filePath + "ferryhill.xlsx"));
            temp.Add(new School("Forehill Primary (Old Machar Academy)", filePath + "forehill.xlsx"));
            temp.Add(new School("Gilcomstoun Primary (Aberdeen Grammar)", filePath + "gilcomstoun.xlsx"));
            temp.Add(new School("Glashieburn Primary (Old Machar Academy)", filePath + "glashieburn.xlsx"));
            temp.Add(new School("Greenbrae Primary (Old Machar Academy)", filePath + "greenbrae.xlsx"));
            temp.Add(new School("Hanover Primary (Harlaw Academy)", filePath + "hanover.xlsx"));
            temp.Add(new School("Hazlehead Primary (Hazlehead Academy)", filePath + "hazlehead.xlsx"));
            temp.Add(new School("Heathryburn Primary (Northfield Academy)", filePath + "heathryburn.xlsx"));
            temp.Add(new School("Holy Family RC Primary (Northfield Academy)", filePath + "holyfamily.xlsx"));
            temp.Add(new School("Kaimhill Primary (Harlaw Academy)", filePath + "kaimhill.xlsx"));
            temp.Add(new School("Kingsford Primary (Hazlehead Academy)", filePath + "kingsford.xlsx"));
            temp.Add(new School("Kingswells Primary (Bucksburn Academy)", filePath + "kingswells.xlsx"));
            temp.Add(new School("Kirkhill Primary (Kincorth Academy)", filePath + "kirkhill.xlsx"));
            temp.Add(new School("Kittybrewster Primary (St Machar Academy)", filePath + "kittybrewster.xlsx"));
            temp.Add(new School("Loirston Primary (Kincorth Academy)", filePath + "loirston.xlsx"));
            temp.Add(new School("Manor Park Primary (Northfield/St Machar Academy)", filePath + "manorpark.xlsx"));
            temp.Add(new School("Middleton Park Primary (Old Machar Academy)", filePath + "middletonpark.xlsx"));
            temp.Add(new School("Mile End Primary (Aberdeen Grammar)", filePath + "mileend.xlsx"));
            temp.Add(new School("Milltimber Primary (Cults Academy)", filePath + "milltimber.xlsx"));
            temp.Add(new School("Muirfield Primary (Northfield Academy)", filePath + "muirfield.xlsx"));
            temp.Add(new School("QuarryHill Primary (Northfield Academy)", filePath + "quarryhill.xlsx"));
            temp.Add(new School("Riverbank Primary (St Machar Academy)", filePath + "riverbank.xlsx"));
            temp.Add(new School("Scotstown Primary (BOD Academy)", filePath + "scotstown.xlsx"));
            temp.Add(new School("Seaton Primary (St Machar Academy)", filePath + "seaton.xlsx"));
            temp.Add(new School("Skene Square Primary (Aberdeen Grammar)", filePath + "skene.xlsx"));
            temp.Add(new School("Stoneywood Primary (Bucksburn Academy)", filePath + "stoneywood.xlsx"));
            temp.Add(new School("St Josephs RC (Aberdeen Grammar)", filePath + "stjosephs.xlsx"));
            temp.Add(new School("St Peters RC (St Machar Academy)", filePath + "stpeters.xlsx"));
            temp.Add(new School("Sunnybank Primary (St Machar Academy)", filePath + "sunnybank.xlsx"));
            temp.Add(new School("Tullos Primary (Torry Academy)", filePath + "tullos.xlsx"));
            temp.Add(new School("Walker Road Primary (Torry Academy)", filePath + "walker.xlsx"));
            temp.Add(new School("Westpark Primary (Northfield Academy)", filePath + "westpark.xlsx"));
            temp.Add(new School("Woodside Primary (St Machar Academy)", filePath + "woodside.xlsx"));
            temp.Add(new School("Balmedie Primary (BOD Academy)", filePath + "balmedie.xlsx"));
            temp.Add(new School("Maryculter/Lairhillak Primary (Cults Academy)", filePath + "maryculter.xlsx"));
            temp.Add(new School("Newmacher Primary (Dyce Academy)", filePath + "newmacher.xlsx"));




















            return temp;

        }
        public ActionResult Calculate(string calculateButton, SchoolRollForecastViewModel schoorollforecastmodel)
        {
            var vmSchoolRollForecastViewModel = new SchoolRollForecastViewModel();

            var vmtempViewModel = Session["vmSchoolRollForecastViewModel"] as SchoolRollForecastViewModel;

            SchoolRollForecastObj schObj = vmtempViewModel.schObj;

            //var sSchoolName = vmtempViewModel.selectedschoolname;

            SchoolRollForecastObj tempObj = schObj;

            List<double> ListHousing = new List<double>();

            if (calculateButton != null)
            {
                if (Request["housing"] != null)
                {
                    ListHousing = Request["housing"].Split(',').Select(Double.Parse).ToList();

                }
            }
            List<double> ListP1Input = new List<double>();

            if (calculateButton != null)
            {
                if (Request["P1Input"] != null)
                {
                    ListP1Input = Request["P1Input"].Split(',').Select(Double.Parse).ToList();

                }
            }
            List<double> ListParentsCharter = new List<double>();

            if (calculateButton != null)
            {
                if (Request["ParentsCharter"] != null)
                {
                    ListParentsCharter = Request["ParentsCharter"].Split(',').Select(Double.Parse).ToList();

                }
            }
            List<double> ListPupilsHhld = new List<double>();

            if (calculateButton != null)
            {
                if (Request["Pupils/Hhld"] != null)
                {
                    ListPupilsHhld = Request["Pupils/Hhld"].Split(',').Select(Double.Parse).ToList();

                }
            }
            //bon accord
            //2014 Year
            double P1Input2014 = schObj.ListP1Input[4];
            double P1PupilsPrevious2013 = schObj.ListP1[3];
            double P2PupilsPrevious2013 = schObj.ListP2[3];
            double P3PupilsPrevious2013 = schObj.ListP3[3];
            double P4PupilsPrevious2013 = schObj.ListP4[3];
            double P5PupilsPrevious2013 = schObj.ListP5[3];
            double P6PupilsPrevious2013 = schObj.ListP6[3];
            double P7PupilsPrevious2013 = schObj.ListP7[3];
            double PupilHhld = schObj.ListPupilsHhld[4];
            double ParentsCharter = schObj.ListParentsCharter[4];
            double housing_current = schObj.ListHousing[4];
            double housing_previous = schObj.ListHousing[3];
            double primclassP1 = 0.17;
            double primclassP2 = 0.16;
            double primclassP3 = 0.16;
            double primclassP4 = 0.14;
            double primclassP5 = 0.13;
            double primclassP6 = 0.12;
            double primclassP7 = 0.12;
            double propadjustfact2014 = 0.985405562058004;
            // MaxCapacity = 200;
            //double 2014total = ListTotalRoll[4];



            //2014
            tempObj.ListP1[4] = (P1Input2014 + ParentsCharter + ((housing_previous + housing_current) / 2) * PupilHhld * primclassP1) * propadjustfact2014;

            tempObj.ListP2[4] = (P1PupilsPrevious2013 + ((housing_previous + housing_current) / 2) * PupilHhld * primclassP2) * propadjustfact2014;

            tempObj.ListP3[4] = (P2PupilsPrevious2013 + ((housing_previous + housing_current) / 2) * PupilHhld * primclassP3) * propadjustfact2014;

            tempObj.ListP4[4] = (P3PupilsPrevious2013 + ((housing_previous + housing_current) / 2) * PupilHhld * primclassP4) * propadjustfact2014;

            tempObj.ListP5[4] = (P4PupilsPrevious2013 + ((housing_previous + housing_current) / 2) * PupilHhld * primclassP5) * propadjustfact2014;

            tempObj.ListP6[4] = (P5PupilsPrevious2013 + ((housing_previous + housing_current) / 2) * PupilHhld * primclassP6) * propadjustfact2014;

            tempObj.ListP7[4] = (P6PupilsPrevious2013 + ((housing_previous + housing_current) / 2) * PupilHhld * primclassP7) * propadjustfact2014;

            tempObj.ListTotalRoll[4] = (schObj.ListP1[4] + schObj.ListP2[4] + schObj.ListP3[4] + schObj.ListP4[4] + schObj.ListP5[4] + schObj.ListP6[4] + schObj.ListP7[4]);

            tempObj.ListMaxCap[4] = 200;

            tempObj.ListTotRollFunctWCap[4] = (schObj.ListTotalRoll[4] - schObj.ListMaxCap[4]);

            tempObj.ListTotRollFunctWCapPer[4] = (schObj.ListTotalRoll[4] / schObj.ListMaxCap[4]);



            //2015 figures

            double P1Input2015 = schObj.ListP1Input[5];
            double P1PupilsPrevious2014 = schObj.ListP1[4];
            double P2PupilsPrevious2014 = schObj.ListP2[4];
            double P3PupilsPrevious2014 = schObj.ListP3[4];
            double P4PupilsPrevious2014 = schObj.ListP4[4];
            double P5PupilsPrevious2014 = schObj.ListP5[4];
            double P6PupilsPrevious2014 = schObj.ListP6[4];
            double P7PupilsPrevious2014 = schObj.ListP7[4];
            double PupilHhld2015 = schObj.ListPupilsHhld[5];
            double ParentsCharter2015 = schObj.ListParentsCharter[5];
            double housing_current2015 = schObj.ListHousing[5];
            double housing_previous2015 = schObj.ListHousing[4];
            double propadjustfact2015 = 0.979816501586464;

            tempObj.ListP1[5] = (P1Input2015 + ParentsCharter2015 + ((housing_previous2015 + housing_current2015) / 2) * PupilHhld2015 * primclassP1) * propadjustfact2014;

            tempObj.ListP2[5] = (P1PupilsPrevious2014 + ((housing_previous2015 + housing_current2015) / 2) * PupilHhld2015 * primclassP2) * propadjustfact2015;

            tempObj.ListP3[5] = (P2PupilsPrevious2014 + ((housing_previous2015 + housing_current2015) / 2) * PupilHhld2015 * primclassP3) * propadjustfact2015;

            tempObj.ListP4[5] = (P3PupilsPrevious2014 + ((housing_previous2015 + housing_current2015) / 2) * PupilHhld2015 * primclassP4) * propadjustfact2015;

            tempObj.ListP5[5] = (P4PupilsPrevious2014 + ((housing_previous2015 + housing_current2015) / 2) * PupilHhld2015 * primclassP5) * propadjustfact2015;

            tempObj.ListP6[5] = (P5PupilsPrevious2014 + ((housing_previous2015 + housing_current2015) / 2) * PupilHhld2015 * primclassP6) * propadjustfact2015;

            tempObj.ListP7[5] = (P6PupilsPrevious2014 + ((housing_previous2015 + housing_current2015) / 2) * PupilHhld2015 * primclassP7) * propadjustfact2015;

            tempObj.ListTotalRoll[5] = (schObj.ListP1[5] + schObj.ListP2[5] + schObj.ListP3[5] + schObj.ListP4[5] + schObj.ListP5[5] + schObj.ListP6[5] + schObj.ListP7[5]);

            tempObj.ListMaxCap[5] = 200;

            tempObj.ListTotRollFunctWCap[5] = (schObj.ListTotalRoll[4] - schObj.ListMaxCap[5]);

            tempObj.ListTotRollFunctWCapPer[5] = (schObj.ListTotalRoll[4] / schObj.ListMaxCap[5]);

            //2016 figuress

            double P1Input2016 = schObj.ListP1Input[6];
            double P1PupilsPrevious2015 = schObj.ListP1[5];
            double P2PupilsPrevious2015 = schObj.ListP2[5];
            double P3PupilsPrevious2015 = schObj.ListP3[5];
            double P4PupilsPrevious2015 = schObj.ListP4[5];
            double P5PupilsPrevious2015 = schObj.ListP5[5];
            double P6PupilsPrevious2015 = schObj.ListP6[5];
            double P7PupilsPrevious2015 = schObj.ListP7[5];
            double PupilHhld2016 = schObj.ListPupilsHhld[6];
            double ParentsCharter2016 = schObj.ListParentsCharter[6];
            double housing_current2016 = schObj.ListHousing[6];
            double housing_previous2016 = schObj.ListHousing[5];
            double propadjustfact2016 = 0.968060566478151;
            double birthRateFactor2016 = 1.005182363;

            tempObj.ListP1[6] = ((P1PupilsPrevious2015 - ParentsCharter2016) * birthRateFactor2016 + ParentsCharter2016 + ((housing_previous2016 + housing_current2016) / 2) * PupilHhld2016 * primclassP1) * propadjustfact2016;

            tempObj.ListP2[6] = (P1PupilsPrevious2015 + ((housing_previous2016 + housing_current2016) / 2) * PupilHhld2016 * primclassP2) * propadjustfact2016;

            tempObj.ListP3[6] = (P2PupilsPrevious2015 + ((housing_previous2016 + housing_current2016) / 2) * PupilHhld2016 * primclassP3) * propadjustfact2016;

            tempObj.ListP4[6] = (P3PupilsPrevious2015 + ((housing_previous2016 + housing_current2016) / 2) * PupilHhld2016 * primclassP4) * propadjustfact2016;

            tempObj.ListP5[6] = (P4PupilsPrevious2015 + ((housing_previous2016 + housing_current2016) / 2) * PupilHhld2016 * primclassP5) * propadjustfact2016;

            tempObj.ListP6[6] = (P5PupilsPrevious2015 + ((housing_previous2016 + housing_current2016) / 2) * PupilHhld2016 * primclassP6) * propadjustfact2016;

            tempObj.ListP7[6] = (P6PupilsPrevious2015 + ((housing_previous2016 + housing_current2016) / 2) * PupilHhld2016 * primclassP7) * propadjustfact2016;

            tempObj.ListTotalRoll[6] = (schObj.ListP1[6] + schObj.ListP2[6] + schObj.ListP3[6] + schObj.ListP4[6] + schObj.ListP5[6] + schObj.ListP6[6] + schObj.ListP7[6]);

            tempObj.ListMaxCap[6] = 200;

            tempObj.ListTotRollFunctWCap[6] = (schObj.ListTotalRoll[5] - schObj.ListMaxCap[6]);

            tempObj.ListTotRollFunctWCapPer[6] = (schObj.ListTotalRoll[5] / schObj.ListMaxCap[6]);

            //2017 figures
            double P1Input2017 = schObj.ListP1Input[7];
            double P1PupilsPrevious2016 = schObj.ListP1[6];
            double P2PupilsPrevious2016 = schObj.ListP2[6];
            double P3PupilsPrevious2016 = schObj.ListP3[6];
            double P4PupilsPrevious2016 = schObj.ListP4[6];
            double P5PupilsPrevious2016 = schObj.ListP5[6];
            double P6PupilsPrevious2016 = schObj.ListP6[6];
            double P7PupilsPrevious2016 = schObj.ListP7[6];
            double PupilHhld2017 = schObj.ListPupilsHhld[7];
            double ParentsCharter2017 = schObj.ListParentsCharter[7];
            double housing_current2017 = schObj.ListHousing[7];
            double housing_previous2017 = schObj.ListHousing[6];
            double propadjustfact2017 = 0.969959540743594;
            double birthRateFactor2017 = 1.010696867;


            tempObj.ListP1[7] = ((P1PupilsPrevious2016 - ParentsCharter2017) * birthRateFactor2017 + ParentsCharter2017 + ((housing_previous2017 + housing_current2017) / 2) * PupilHhld2017 * primclassP1) * propadjustfact2017;

            tempObj.ListP2[7] = (P1PupilsPrevious2016 + ((housing_previous2017 + housing_current2017) / 2) * PupilHhld2017 * primclassP2) * propadjustfact2017;

            tempObj.ListP3[7] = (P2PupilsPrevious2016 + ((housing_previous2017 + housing_current2017) / 2) * PupilHhld2017 * primclassP3) * propadjustfact2017;

            tempObj.ListP4[7] = (P3PupilsPrevious2016 + ((housing_previous2017 + housing_current2017) / 2) * PupilHhld2017 * primclassP4) * propadjustfact2017;

            tempObj.ListP5[7] = (P4PupilsPrevious2016 + ((housing_previous2017 + housing_current2017) / 2) * PupilHhld2017 * primclassP5) * propadjustfact2017;

            tempObj.ListP6[7] = (P5PupilsPrevious2016 + ((housing_previous2017 + housing_current2017) / 2) * PupilHhld2017 * primclassP6) * propadjustfact2017;

            tempObj.ListP7[7] = (P6PupilsPrevious2016 + ((housing_previous2017 + housing_current2017) / 2) * PupilHhld2017 * primclassP7) * propadjustfact2017;

            tempObj.ListTotalRoll[7] = (schObj.ListP1[7] + schObj.ListP2[7] + schObj.ListP3[7] + schObj.ListP4[7] + schObj.ListP5[7] + schObj.ListP6[7] + schObj.ListP7[7]);

            tempObj.ListMaxCap[7] = 200;

            tempObj.ListTotRollFunctWCap[7] = (schObj.ListTotalRoll[6] - schObj.ListMaxCap[7]);

            tempObj.ListTotRollFunctWCapPer[7] = (schObj.ListTotalRoll[6] / schObj.ListMaxCap[7]);

            //2018 figures

            double P1Input2018 = schObj.ListP1Input[8];
            double P1PupilsPrevious2017 = schObj.ListP1[7];
            double P2PupilsPrevious2017 = schObj.ListP2[7];
            double P3PupilsPrevious2017 = schObj.ListP3[7];
            double P4PupilsPrevious2017 = schObj.ListP4[7];
            double P5PupilsPrevious2017 = schObj.ListP5[7];
            double P6PupilsPrevious2017 = schObj.ListP6[7];
            double P7PupilsPrevious2017 = schObj.ListP7[7];
            double PupilHhld2018 = schObj.ListPupilsHhld[8];
            double ParentsCharter2018 = schObj.ListParentsCharter[8];
            double housing_current2018 = schObj.ListHousing[8];
            double housing_previous2018 = schObj.ListHousing[7];
            double propadjustfact2018 = 0.97202537700975;
            double birthRateFactor2018 = 1.02517364;


            tempObj.ListP1[8] = ((P1PupilsPrevious2017 - ParentsCharter2018) * birthRateFactor2018 + ParentsCharter2018 + ((housing_previous2018 + housing_current2018) / 2) * PupilHhld2018 * primclassP1) * propadjustfact2018;

            tempObj.ListP2[8] = (P1PupilsPrevious2017 + ((housing_previous2018 + housing_current2018) / 2) * PupilHhld2018 * primclassP2) * propadjustfact2018;

            tempObj.ListP2[8] = (P1PupilsPrevious2017 + ((housing_previous2018 + housing_current2018) / 2) * PupilHhld2018 * primclassP2) * propadjustfact2018;

            tempObj.ListP3[8] = (P2PupilsPrevious2017 + ((housing_previous2018 + housing_current2018) / 2) * PupilHhld2018 * primclassP3) * propadjustfact2018;

            tempObj.ListP4[8] = (P3PupilsPrevious2017 + ((housing_previous2018 + housing_current2018) / 2) * PupilHhld2018 * primclassP4) * propadjustfact2017;

            tempObj.ListP5[8] = (P4PupilsPrevious2017 + ((housing_previous2018 + housing_current2018) / 2) * PupilHhld2018 * primclassP5) * propadjustfact2018;

            tempObj.ListP6[8] = (P5PupilsPrevious2017 + ((housing_previous2018 + housing_current2018) / 2) * PupilHhld2018 * primclassP6) * propadjustfact2018;

            tempObj.ListP7[8] = (P6PupilsPrevious2017 + ((housing_previous2018 + housing_current2018) / 2) * PupilHhld2018 * primclassP7) * propadjustfact2018;

            tempObj.ListTotalRoll[8] = (schObj.ListP1[8] + schObj.ListP2[8] + schObj.ListP3[8] + schObj.ListP4[8] + schObj.ListP5[8] + schObj.ListP6[8] + schObj.ListP7[8]);

            tempObj.ListMaxCap[7] = 200;

            tempObj.ListTotRollFunctWCap[8] = (schObj.ListTotalRoll[7] - schObj.ListMaxCap[8]);

            tempObj.ListTotRollFunctWCapPer[8] = (schObj.ListTotalRoll[7] / schObj.ListMaxCap[8]);

            //2019 figures
            double P1Input2019 = schObj.ListP1Input[9];
            double P1PupilsPrevious2018 = schObj.ListP1[8];
            double P2PupilsPrevious2018 = schObj.ListP2[8];
            double P3PupilsPrevious2018 = schObj.ListP3[8];
            double P4PupilsPrevious2018 = schObj.ListP4[8];
            double P5PupilsPrevious2018 = schObj.ListP5[8];
            double P6PupilsPrevious2018 = schObj.ListP6[8];
            double P7PupilsPrevious2018 = schObj.ListP7[8];
            double PupilHhld2019 = schObj.ListPupilsHhld[9];
            double ParentsCharter2019 = schObj.ListParentsCharter[9];
            double housing_current2019 = schObj.ListHousing[9];
            double housing_previous2019 = schObj.ListHousing[8];
            double propadjustfact2019 = 0.972389778542098;
            double birthRateFactor2019 = 1.012702558;


            tempObj.ListP1[9] = ((P1PupilsPrevious2018 - ParentsCharter2019) * birthRateFactor2019 + ParentsCharter2019 + ((housing_previous2019 + housing_current2019) / 2) * PupilHhld2019 * primclassP1) * propadjustfact2019;

            tempObj.ListP2[9] = (P1PupilsPrevious2018 + ((housing_previous2019 + housing_current2019) / 2) * PupilHhld2019 * primclassP2) * propadjustfact2019;

            tempObj.ListP2[9] = (P1PupilsPrevious2018 + ((housing_previous2019 + housing_current2019) / 2) * PupilHhld2019 * primclassP2) * propadjustfact2019;

            tempObj.ListP3[9] = (P2PupilsPrevious2018 + ((housing_previous2019 + housing_current2019) / 2) * PupilHhld2019 * primclassP3) * propadjustfact2019;

            tempObj.ListP4[9] = (P3PupilsPrevious2018 + ((housing_previous2019 + housing_current2019) / 2) * PupilHhld2019 * primclassP4) * propadjustfact2019;

            tempObj.ListP5[9] = (P4PupilsPrevious2018 + ((housing_previous2019 + housing_current2019) / 2) * PupilHhld2019 * primclassP5) * propadjustfact2019;

            tempObj.ListP6[9] = (P5PupilsPrevious2018 + ((housing_previous2019 + housing_current2019) / 2) * PupilHhld2019 * primclassP6) * propadjustfact2019;

            tempObj.ListP7[9] = (P6PupilsPrevious2018 + ((housing_previous2019 + housing_current2019) / 2) * PupilHhld2019 * primclassP7) * propadjustfact2019;

            tempObj.ListTotalRoll[9] = (schObj.ListP1[9] + schObj.ListP2[9] + schObj.ListP3[9] + schObj.ListP4[9] + schObj.ListP5[9] + schObj.ListP6[9] + schObj.ListP7[9]);

            tempObj.ListMaxCap[7] = 200;

            tempObj.ListTotRollFunctWCap[9] = (schObj.ListTotalRoll[8] - schObj.ListMaxCap[9]);

            tempObj.ListTotRollFunctWCapPer[9] = (schObj.ListTotalRoll[8] / schObj.ListMaxCap[9]);

            //2020 figures

            double P1Input2020 = schObj.ListP1Input[10];
            double P1PupilsPrevious2019 = schObj.ListP1[9];
            double P2PupilsPrevious2019 = schObj.ListP2[9];
            double P3PupilsPrevious2019 = schObj.ListP3[9];
            double P4PupilsPrevious2019 = schObj.ListP4[9];
            double P5PupilsPrevious2019 = schObj.ListP5[9];
            double P6PupilsPrevious2019 = schObj.ListP6[9];
            double P7PupilsPrevious2019 = schObj.ListP7[9];
            double PupilHhld2020 = schObj.ListPupilsHhld[10];
            double ParentsCharter2020 = schObj.ListParentsCharter[10];
            double housing_current2020 = schObj.ListHousing[10];
            double housing_previous2020 = schObj.ListHousing[9];
            double propadjustfact2020 = 0.964444503941792;
            double birthRateFactor2020 = 1.011106417;


            tempObj.ListP1[10] = ((P1PupilsPrevious2019 - ParentsCharter2020) * birthRateFactor2020 + ParentsCharter2020 + ((housing_previous2020 + housing_current2020) / 2) * PupilHhld2020 * primclassP1) * propadjustfact2020;

            tempObj.ListP2[10] = (P1PupilsPrevious2019 + ((housing_previous2020 + housing_current2020) / 2) * PupilHhld2020 * primclassP2) * propadjustfact2020;

            tempObj.ListP2[10] = (P1PupilsPrevious2019 + ((housing_previous2020 + housing_current2020) / 2) * PupilHhld2020 * primclassP2) * propadjustfact2020;

            tempObj.ListP3[10] = (P2PupilsPrevious2019 + ((housing_previous2020 + housing_current2020) / 2) * PupilHhld2020 * primclassP3) * propadjustfact2020;

            tempObj.ListP4[10] = (P3PupilsPrevious2019 + ((housing_previous2020 + housing_current2020) / 2) * PupilHhld2020 * primclassP4) * propadjustfact2020;

            tempObj.ListP5[10] = (P4PupilsPrevious2019 + ((housing_previous2020 + housing_current2020) / 2) * PupilHhld2020 * primclassP5) * propadjustfact2020;

            tempObj.ListP6[10] = (P5PupilsPrevious2019 + ((housing_previous2020 + housing_current2020) / 2) * PupilHhld2020 * primclassP6) * propadjustfact2020;

            tempObj.ListP7[10] = (P6PupilsPrevious2019 + ((housing_previous2020 + housing_current2020) / 2) * PupilHhld2020 * primclassP7) * propadjustfact2020;

            tempObj.ListTotalRoll[10] = (schObj.ListP1[10] + schObj.ListP2[10] + schObj.ListP3[10] + schObj.ListP4[10] + schObj.ListP5[10] + schObj.ListP6[10] + schObj.ListP7[10]);

            tempObj.ListMaxCap[7] = 200;

            tempObj.ListTotRollFunctWCap[10] = (schObj.ListTotalRoll[9] - schObj.ListMaxCap[10]);

            tempObj.ListTotRollFunctWCapPer[10] = (schObj.ListTotalRoll[9] / schObj.ListMaxCap[10]);

            //2021 figures
            double P1Input2021 = schObj.ListP1Input[11];
            double P1PupilsPrevious2020 = schObj.ListP1[10];
            double P2PupilsPrevious2020 = schObj.ListP2[10];
            double P3PupilsPrevious2020 = schObj.ListP3[10];
            double P4PupilsPrevious2020 = schObj.ListP4[10];
            double P5PupilsPrevious2020 = schObj.ListP5[10];
            double P6PupilsPrevious2020 = schObj.ListP6[10];
            double P7PupilsPrevious2020 = schObj.ListP7[10];
            double PupilHhld2021 = schObj.ListPupilsHhld[11];
            double ParentsCharter2021 = schObj.ListParentsCharter[11];
            double housing_current2021 = schObj.ListHousing[11];
            double housing_previous2021 = schObj.ListHousing[10];
            double propadjustfact2021 = 0.967584648504448;
            double birthRateFactor2021 = 1.007846664;


            tempObj.ListP1[11] = ((P1PupilsPrevious2020 - ParentsCharter2021) * birthRateFactor2021 + ParentsCharter2021 + ((housing_previous2021 + housing_current2021) / 2) * PupilHhld2021 * primclassP1) * propadjustfact2021;

            tempObj.ListP2[11] = (P1PupilsPrevious2020 + ((housing_previous2021 + housing_current2021) / 2) * PupilHhld2021 * primclassP2) * propadjustfact2021;

            tempObj.ListP2[11] = (P1PupilsPrevious2020 + ((housing_previous2021 + housing_current2021) / 2) * PupilHhld2021 * primclassP2) * propadjustfact2021;

            tempObj.ListP3[11] = (P2PupilsPrevious2020 + ((housing_previous2021 + housing_current2021) / 2) * PupilHhld2021 * primclassP3) * propadjustfact2021;

            tempObj.ListP4[11] = (P3PupilsPrevious2020 + ((housing_previous2021 + housing_current2021) / 2) * PupilHhld2021 * primclassP4) * propadjustfact2021;

            tempObj.ListP5[11] = (P4PupilsPrevious2020 + ((housing_previous2021 + housing_current2021) / 2) * PupilHhld2021 * primclassP5) * propadjustfact2021;

            tempObj.ListP6[11] = (P5PupilsPrevious2020 + ((housing_previous2021 + housing_current2021) / 2) * PupilHhld2021 * primclassP6) * propadjustfact2021;

            tempObj.ListP7[11] = (P6PupilsPrevious2020 + ((housing_previous2021 + housing_current2021) / 2) * PupilHhld2021 * primclassP7) * propadjustfact2021;

            tempObj.ListTotalRoll[11] = (schObj.ListP1[11] + schObj.ListP2[11] + schObj.ListP3[11] + schObj.ListP4[11] + schObj.ListP5[11] + schObj.ListP6[11] + schObj.ListP7[11]);

            tempObj.ListMaxCap[7] = 200;

            tempObj.ListTotRollFunctWCap[11] = (schObj.ListTotalRoll[10] - schObj.ListMaxCap[11]);

            tempObj.ListTotRollFunctWCapPer[11] = (schObj.ListTotalRoll[10] / schObj.ListMaxCap[11]);

            ////2014 Granite City

            //double S1 = schObj.ListS1[4];
            //double S2 = schObj.ListS2[4];
            //double S3 = schObj.ListS3[4];
            //double S4 = schObj.ListS4[4];
            //double S5 = schObj.ListS5[4];
            //double S6 = schObj.ListS6[4];
            //double ACPupilsPerHousehold = schObj.ListPupilsHhld[4];
            //double ACHousing2014 = schObj.ListHousing[4];
            //double ACParentsCharter2014 = schObj.ListParentsCharter[4];
            //double ACPropadjustFact2014 = 0.979545;
            //double ACSecClass2014 = 0.25;
            //double ACStayonRatio2014 = 75;
            //double ACStayonRatio22014 = 50;

            //tempObj.ListS1[4] = (ACParentsCharter2014 * (20 + 20 + 20)) + ((ListHousing[3] + ListHousing[4]) / 2 * ListPupilsHhld[4] * ACSecClass2014) * ACPropadjustFact2014;

            //tempObj.ListS2[4] = (ListS1[3] + ((ListHousing[3] + ListHousing[4]) / 2) * ListPupilsHhld[4] * ACSecClass2014) * ACPropadjustFact2014;

            //tempObj.ListS3[4] = (ListS2[3] + ((ListHousing[3] + ListHousing[4]) / 2) * ListPupilsHhld[4] * ACSecClass2014) * ACPropadjustFact2014;

            //tempObj.ListS4[4] = (ListS3[4] + ((ListHousing[3] + ListHousing[4]) / 2) * ListPupilsHhld[4] * ACSecClass2014) * ACPropadjustFact2014;

            //tempObj.ListS5[4] = ListS4[3] * ACStayonRatio2014;

            //tempObj.ListS6[4] = ListS5[3] * ACStayonRatio22014;

            //tempObj.ListMaxCap[4] = 200;

            //tempObj.ListTotRollFunctWCap[4] = (schObj.ListTotalRoll[3] - schObj.ListMaxCap[4]);

            //tempObj.ListTotRollFunctWCapPer[4] = (schObj.ListTotalRoll[3] / schObj.ListMaxCap[4]);

            ////2015
            //double S12015 = schObj.ListS1[5];
            //double S22015 = schObj.ListS2[5];
            //double S32015 = schObj.ListS3[5];
            //double S42015 = schObj.ListS4[5];
            //double S52015 = schObj.ListS5[5];
            //double S62015 = schObj.ListS6[5];
            //double ACPupilsPerHousehold2015 = schObj.ListPupilsHhld[5];
            //double ACHousing2015 = schObj.ListPupilsHhld[5];
            //double ACParentsCharter2015 = schObj.ListParentsCharter[5];
            //double ACPropAdjustFact2015 = 0.979545;
            //double ACSecClass2015 = 0.25;
            //double ACStayOnRatio2015 = 75;
            //double ACStayOnRatio22015 = 50;

            //tempObj.ListS1[5] =  (ACParentsCharter2015 * (19.6 + 19.6 + 19.6)) + ((ListHousing[4] + ListHousing[5]) / 2 * ListPupilsHhld[5] * ACSecClass2015) * ACPropAdjustFact2015;

            //tempObj.ListS2[5] = (ListS1[4] + ((ListHousing[4] + ListHousing[5]) / 2) * ListPupilsHhld[5] * ACSecClass2015) * ACPropAdjustFact2015;

            //tempObj.ListS3[5] = (ListS2[4] + ((ListHousing[4] + ListHousing[5]) / 2) * ListPupilsHhld[5] * ACSecClass2015) * ACPropAdjustFact2015;

            //tempObj.ListS4[5] = (ListS3[4] + ((ListHousing[4] + ListHousing[5]) / 2) * ListPupilsHhld[5] * ACSecClass2015) * ACPropAdjustFact2015;

            //tempObj.ListS5[5] = ListS4[4] * ACStayOnRatio2015;

            //tempObj.ListS6[5] = ListS5[4] * ACStayOnRatio22015;

            //tempObj.ListMaxCap[5] = 200;

            //tempObj.ListTotRollFunctWCap[5] = (schObj.ListTotalRoll[4] - schObj.ListMaxCap[5]);

            //tempObj.ListTotRollFunctWCapPer[5] = (schObj.ListTotalRoll[4] / schObj.ListMaxCap[5]);

            ////2016
            //double S12016 = schObj.ListS1[6];
            //double S22016 = schObj.ListS2[6];
            //double S32016 = schObj.ListS3[6];
            //double S42016 = schObj.ListS4[6];
            //double S52016 = schObj.ListS5[6];
            //double S62016 = schObj.ListS6[6];
            //double ACPupilsPerHousehold2016 = schObj.ListPupilsHhld[6];
            //double ACHousing2016 = schObj.ListPupilsHhld[6];
            //double ACParentsCharter2016 = schObj.ListParentsCharter[6];
            //double ACPropAdjustFact2016 = 0.970621;
            //double ACSecClass2016 = 0.25;
            //double ACStayOnRatio2016 = 75;
            //double ACStayOnRatio22016 = 50;

            //tempObj.ListS1[6] =  (ACParentsCharter2016 * (19.1 + 19.1 + 19.1)) + ((ListHousing[5] + ListHousing[6]) / 2 * ListPupilsHhld[6] * ACSecClass2016) * ACPropAdjustFact2016;

            //tempObj.ListS2[6] = (ListS1[5] + ((ListHousing[5] + ListHousing[6]) / 2) * ListPupilsHhld[6] * ACSecClass2016) * ACPropAdjustFact2016;

            //tempObj.ListS3[6] = (ListS2[5] + ((ListHousing[5] + ListHousing[6]) / 2) * ListPupilsHhld[6] * ACSecClass2016) * ACPropAdjustFact2016;

            //tempObj.ListS4[6] = (ListS3[5] + ((ListHousing[5] + ListHousing[6]) / 2) * ListPupilsHhld[6] * ACSecClass2016) * ACPropAdjustFact2016;

            //tempObj.ListS5[6] = ListS4[5] * ACStayOnRatio2016;

            //tempObj.ListS6[6] = ListS5[5] * ACStayOnRatio22016;

            //tempObj.ListMaxCap[6] = 200;

            //tempObj.ListTotRollFunctWCap[6] = (schObj.ListTotalRoll[5] - schObj.ListMaxCap[6]);

            //tempObj.ListTotRollFunctWCapPer[6] = (schObj.ListTotalRoll[5] / schObj.ListMaxCap[6]);

            // //2017
            //double S12017 = schObj.ListS1[7];
            //double S22017 = schObj.ListS2[7];
            //double S32017 = schObj.ListS3[7];
            //double S42017 = schObj.ListS4[7];
            //double S52017 = schObj.ListS5[7];
            //double S62017 = schObj.ListS6[7];
            //double ACPupilsPerHousehold2017 = schObj.ListPupilsHhld[7];
            //double ACHousing2017 = schObj.ListPupilsHhld[7];
            //double ACParentsCharter2017 = schObj.ListParentsCharter[7];
            //double ACPropAdjustFact2017 = 0.965379;
            //double ACSecClass2017 = 0.25;
            //double ACStayOnRatio2017 = 75;
            //double ACStayOnRatio22017 = 50;

            //tempObj.ListS1[7] = (ACParentsCharter2017 * (19.1 + 19.1 + 19.1)) + ((ListHousing[6] + ListHousing[7]) / 2 * ListPupilsHhld[7] * ACSecClass2017) * ACPropAdjustFact2017;

            //tempObj.ListS2[7] = (ListS1[6] + ((ListHousing[6] + ListHousing[7]) / 2) * ListPupilsHhld[7] * ACSecClass2017) * ACPropAdjustFact2017;

            //tempObj.ListS3[7] = (ListS2[6] + ((ListHousing[6] + ListHousing[7]) / 2) * ListPupilsHhld[7] * ACSecClass2017) * ACPropAdjustFact2017;

            //tempObj.ListS4[7] = (ListS3[6] + ((ListHousing[6] + ListHousing[7]) / 2) * ListPupilsHhld[7] * ACSecClass2017) * ACPropAdjustFact2017;

            //tempObj.ListS5[7] = ListS4[6] * ACStayOnRatio2017;

            //tempObj.ListS6[7] = ListS6[6] * ACStayOnRatio22017;

            //tempObj.ListMaxCap[7] = 200;

            //tempObj.ListTotRollFunctWCap[7] = (schObj.ListTotalRoll[6] - schObj.ListMaxCap[7]);

            //tempObj.ListTotRollFunctWCapPer[7] = (schObj.ListTotalRoll[6] / schObj.ListMaxCap[7]);


            ////2018
            //double S12018 = schObj.ListS1[8];
            //double S22018 = schObj.ListS2[8];
            //double S32018 = schObj.ListS3[8];
            //double S42018 = schObj.ListS4[8];
            //double S52018 = schObj.ListS5[8];
            //double S62018 = schObj.ListS6[8];
            //double ACPupilsPerHousehold2018 = schObj.ListPupilsHhld[8];
            //double ACHousing2018 = schObj.ListPupilsHhld[8];
            //double ACParentsCharter2018 = schObj.ListParentsCharter[8];
            //double ACPropAdjustFact2018 = 0.980609;
            //double ACSecClass2018 = 0.25;
            //double ACStayOnRatio2018 = 75;
            //double ACStayOnRatio22018 = 50;

            //tempObj.ListS1[8] = (ACParentsCharter2018 * (19.5 + 19.1 + 19.1)) + ((ListHousing[7] + ListHousing[8]) / 2 * ListPupilsHhld[8] * ACSecClass2018) * ACPropAdjustFact2018;

            //tempObj.ListS2[8] = (ListS1[7] + ((ListHousing[7] + ListHousing[8]) / 2) * ListPupilsHhld[8] * ACSecClass2018) * ACPropAdjustFact2018;

            //tempObj.ListS3[8] = (ListS2[7] + ((ListHousing[7] + ListHousing[8]) / 2) * ListPupilsHhld[8] * ACSecClass2018) * ACPropAdjustFact2018;

            //tempObj.ListS4[8] = (ListS3[7] + ((ListHousing[7] + ListHousing[8]) / 2) * ListPupilsHhld[8] * ACSecClass2018) * ACPropAdjustFact2018;

            //tempObj.ListS5[8] = ListS4[7] * ACStayOnRatio2018;

            //tempObj.ListS6[8] = ListS6[7] * ACStayOnRatio22018;

            //tempObj.ListMaxCap[8] = 200;

            //tempObj.ListTotRollFunctWCap[8] = (schObj.ListTotalRoll[7] - schObj.ListMaxCap[8]);

            //tempObj.ListTotRollFunctWCapPer[8] = (schObj.ListTotalRoll[7] / schObj.ListMaxCap[8]);


            ////2019
            //double S12019 = schObj.ListS1[9];
            //double S22019 = schObj.ListS2[9];
            //double S32019 = schObj.ListS3[9];
            //double S42019 = schObj.ListS4[9];
            //double S52019 = schObj.ListS5[9];
            //double S62019 = schObj.ListS6[9];
            //double ACPupilsPerHousehold2019 = schObj.ListPupilsHhld[9];
            //double ACHousing2019 = schObj.ListPupilsHhld[9];
            //double ACParentsCharter2019 = schObj.ListParentsCharter[9];
            //double ACPropAdjustFact2019 = 0.978155;
            //double ACSecClass2019 = 0.25;
            //double ACStayOnRatio2019 = 75;
            //double ACStayOnRatio22019 = 50;


            //tempObj.ListS1[9] = (ACParentsCharter2019 * (19.5 + 19.1 + 19.1)) + ((ListHousing[8] + ListHousing[9]) / 2 * ListPupilsHhld[9] * ACSecClass2019) * ACPropAdjustFact2019;

            //tempObj.ListS2[9] = (ListS1[8] + ((ListHousing[8] + ListHousing[9]) / 2) * ListPupilsHhld[9] * ACSecClass2019) * ACPropAdjustFact2019;

            //tempObj.ListS3[9] = (ListS2[8] + ((ListHousing[8] + ListHousing[9]) / 2) * ListPupilsHhld[9] * ACSecClass2019) * ACPropAdjustFact2019;

            //tempObj.ListS4[9] = (ListS3[8] + ((ListHousing[8] + ListHousing[9]) / 2) * ListPupilsHhld[9] * ACSecClass2019) * ACPropAdjustFact2019;

            //tempObj.ListS5[9] = ListS4[8] * ACStayOnRatio2019;

            //tempObj.ListS6[9] = ListS5[8] * ACStayOnRatio22019;

            //tempObj.ListMaxCap[9] = 200;

            //tempObj.ListTotRollFunctWCap[9] = (schObj.ListTotalRoll[8] - schObj.ListMaxCap[9]);

            //tempObj.ListTotRollFunctWCapPer[9] = (schObj.ListTotalRoll[8] / schObj.ListMaxCap[9]);


            ////2020
            //double S12020 = schObj.ListS1[10];
            //double S22020 = schObj.ListS2[10];
            //double S32020 = schObj.ListS3[10];
            //double S42020 = schObj.ListS4[10];
            //double S52020 = schObj.ListS5[10];
            //double S62020 = schObj.ListS6[10];
            //double ACPupilsPerHousehold2020 = schObj.ListPupilsHhld[10];
            //double ACHousing2020 = schObj.ListPupilsHhld[10];
            //double ACParentsCharter2020 = schObj.ListParentsCharter[10];
            //double ACPropAdjustFact2020 = 0.971013;
            //double ACSecClass2020 = 0.25;
            //double ACStayOnRatio2020 = 75;
            //double ACStayOnRatio22020 = 50;

            //tempObj.ListS1[10] = (ACParentsCharter2019 * (19.5 + 19.1 + 19.1)) + ((ListHousing[9] + ListHousing[10]) / 2 * ListPupilsHhld[10] * ACSecClass2020) * ACPropAdjustFact2020;

            //tempObj.ListS2[10] = (ListS1[9] + ((ListHousing[9] + ListHousing[10]) / 2) * ListPupilsHhld[10] * ACSecClass2020) * ACPropAdjustFact2020;

            //tempObj.ListS3[10] = (ListS2[9] + ((ListHousing[9] + ListHousing[10]) / 2) * ListPupilsHhld[10] * ACSecClass2020) * ACPropAdjustFact2020;

            //tempObj.ListS4[10] = (ListS3[9] + ((ListHousing[9] + ListHousing[10]) / 2) * ListPupilsHhld[10] * ACSecClass2020) * ACPropAdjustFact2020;

            //tempObj.ListS5[10] = ListS4[9] * ACStayOnRatio2020;

            //tempObj.ListS6[10] = ListS5[9] * ACStayOnRatio22020;

            //tempObj.ListMaxCap[10] = 200;

            //tempObj.ListTotRollFunctWCap[10] = (schObj.ListTotalRoll[9] - schObj.ListMaxCap[10]);

            //tempObj.ListTotRollFunctWCapPer[10] = (schObj.ListTotalRoll[9] / schObj.ListMaxCap[10]);



            ////2021
            //double S12021 = schObj.ListS1[11];
            //double S22021 = schObj.ListS2[11];
            //double S32021 = schObj.ListS3[11];
            //double S42021 = schObj.ListS4[11];
            //double S52021 = schObj.ListS5[11];
            //double S62021 = schObj.ListS6[11];
            //double ACPupilsPerHousehold2021 = schObj.ListPupilsHhld[11];
            //double ACHousing2021 = schObj.ListPupilsHhld[11];
            //double ACParentsCharter2021 = schObj.ListParentsCharter[11];
            //double ACPropAdjustFact2021 = 0.974821;
            //double ACSecClass2021 = 0.25;
            //double ACStayOnRatio2021 = 75;
            //double ACStayOnRatio22021 = 50;

            //tempObj.ListS1[11] = (ACParentsCharter2021 * (19.5 + 19.1 + 19.1)) + ((ListHousing[10] + ListHousing[11]) / 2 * ListPupilsHhld[11] * ACSecClass2021) * ACPropAdjustFact2021;

            //tempObj.ListS2[11] = (ListS1[10] + ((ListHousing[10] + ListHousing[11]) / 2) * ListPupilsHhld[11] * ACSecClass2021) * ACPropAdjustFact2021;

            //tempObj.ListS3[11] = (ListS2[10] + ((ListHousing[10] + ListHousing[11]) / 2) * ListPupilsHhld[11] * ACSecClass2021) * ACPropAdjustFact2021;

            //tempObj.ListS4[11] = (ListS3[10] + ((ListHousing[10] + ListHousing[11]) / 2) * ListPupilsHhld[11] * ACSecClass2021) * ACPropAdjustFact2021;

            //tempObj.ListS5[11] = ListS4[10] * ACStayOnRatio2021;

            //tempObj.ListS6[11] = ListS5[10] * ACStayOnRatio22021;

            //tempObj.ListMaxCap[11] = 200;

            //tempObj.ListTotRollFunctWCap[11] = (schObj.ListTotalRoll[10] - schObj.ListMaxCap[11]);

            //tempObj.ListTotRollFunctWCapPer[11] = (schObj.ListTotalRoll[10] / schObj.ListMaxCap[11]);

            tempObj.ListHousing = ListHousing;
            tempObj.ListP1Input = ListP1Input;
            tempObj.ListParentsCharter = ListParentsCharter;
            tempObj.ListPupilsHhld = ListPupilsHhld;

            vmSchoolRollForecastViewModel.schObj = tempObj;
            vmSchoolRollForecastViewModel.selectedschoolname = vmtempViewModel.selectedschoolname;
            vmSchoolRollForecastViewModel.listSchoolname = GetListSchoolName();
            return View("Home", vmSchoolRollForecastViewModel);
        }
    }
}


