using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Data;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.WiderAchievement;
using Common.Logging;
using ACCDataStore.Repository;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class WiderAchievementController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(WiderAchievementController));

        private readonly IGenericRepository rpGeneric;

        public WiderAchievementController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        // GET: SchoolProfile/WiderAchievement
        //public ActionResult Index()
        //{
        //    string filePath = "C:\\data\\WiderAchievementData.xlsx";
        //    DataTable dt1 = new DataTable();
        //    //using (XLWorkbook workBook = new XLWorkbook(filePath))
        //    //{
        //    //    //Read the first Sheet from Excel file.
        //    //    IXLWorksheet workSheet = workBook.Worksheet(1);

        //    //    //Loop through the Worksheet rows.
        //    //    bool firstRow = true;
        //    //    foreach (IXLRow row in workSheet.Rows())
        //    //    {
        //    //        //Use the first row to add columns to DataTable.
        //    //        if (firstRow)
        //    //        {
        //    //            foreach (IXLCell cell in row.Cells())
        //    //            {
        //    //                dt.Columns.Add(cell.Value.ToString());
        //    //            }
        //    //            firstRow = false;
        //    //        }
        //    //        else
        //    //        {
        //    //            //Add rows to DataTable.
        //    //            dt.Rows.Add();
        //    //            int i = 0;
        //    //            foreach (IXLCell cell in row.Cells())
        //    //            {
        //    //                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
        //    //                i++;
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    using (XLWorkbook workBook = new XLWorkbook(filePath)){
        //        IXLWorksheet workSheet = workBook.Worksheet(1);
        //        //adding columns
        //        dt1.Columns.Add(workSheet.Row(3).Cell(1).Value.ToString());
        //        dt1.Columns.Add(workSheet.Row(3).Cell(2).Value.ToString());
        //        dt1.Columns.Add(workSheet.Row(3).Cell(3).Value.ToString());
        //        dt1.Columns.Add(workSheet.Row(3).Cell(4).Value.ToString());
        //        dt1.Columns.Add(workSheet.Row(3).Cell(5).Value.ToString());

        //        //adding table1
                

        //        for (int i = 4; i <= 26; i++ )
        //        {
        //            DataRow dataRow = dt1.NewRow();
        //            IXLRow row = workSheet.Row(i);
        //            for (int j = 0; j <= 4; j++)
        //            {
        //                dataRow[j] = (workSheet.Row(i).Cell(j+1).Value.ToString());
        //            }
        //            dt1.Rows.Add(dataRow);
        //        }
        //        DataRow dataRow1 = dt1.NewRow();
        //        dataRow1[0] = ("");
        //        dataRow1[1] = (workSheet.Row(31).Cell(2).Value.ToString());
        //        dataRow1[2] = (workSheet.Row(31).Cell(3).Value.ToString());
        //        dataRow1[3] = (workSheet.Row(31).Cell(4).Value.ToString());
        //        dataRow1[4] = (workSheet.Row(31).Cell(5).Value.ToString());
        //        dt1.Rows.Add(dataRow1);
        //    }


        //    DataTable dt2 = new DataTable();
        //    using (XLWorkbook workBook = new XLWorkbook(filePath))
        //    {
        //        IXLWorksheet workSheet = workBook.Worksheet(1);
        //        //adding columns
        //        dt2.Columns.Add(workSheet.Row(3).Cell(7).Value.ToString());
        //        dt2.Columns.Add(workSheet.Row(3).Cell(8).Value.ToString());
        //        dt2.Columns.Add(workSheet.Row(3).Cell(9).Value.ToString());
        //        dt2.Columns.Add(workSheet.Row(3).Cell(10).Value.ToString());
        //        dt2.Columns.Add(workSheet.Row(3).Cell(11).Value.ToString());

        //        //adding table1


        //        for (int i = 4; i <= 29; i++)
        //        {
        //            DataRow dataRow = dt2.NewRow();
        //            IXLRow row = workSheet.Row(i);
        //            for (int j = 0; j <= 4; j++)
        //            {
        //                dataRow[j] = (workSheet.Row(i).Cell(j + 7).Value.ToString());
        //            }
        //            dt2.Rows.Add(dataRow);
        //        }
        //        DataRow dataRow1 = dt2.NewRow();
        //        dataRow1[0] = ("");
        //        dataRow1[1] = (workSheet.Row(31).Cell(8).Value.ToString());
        //        dataRow1[2] = (workSheet.Row(31).Cell(9).Value.ToString());
        //        dataRow1[3] = (workSheet.Row(31).Cell(10).Value.ToString());
        //        dataRow1[4] = (workSheet.Row(31).Cell(11).Value.ToString());
        //        dt2.Rows.Add(dataRow1);
        //    }

        //    var vmWiderAchievement = new WiderAchievementViewModel();
        //    vmWiderAchievement.dtTable1 = dt1;

        //    vmWiderAchievement.dtTable2 = dt2;
        //    return View(vmWiderAchievement);
        //}


        public ActionResult Index(string schoolsubmitButton, string awardsubmitButton)
        {
            //var eGeneralSettings = TS.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmWiderAchievement = new WiderAchievementViewModel();
            vmWiderAchievement.Listschoolname = GetListschoolname(this.rpGeneric);
            vmWiderAchievement.Listawardname = GetListAwardname(this.rpGeneric);

            List<WiderAchievementObj> temp = new List<WiderAchievementObj>();

            if (schoolsubmitButton != null)
            {
                var sSchoolname= Request["selectedschoolname"];
                vmWiderAchievement.selectedschoolname = sSchoolname;
                if (sSchoolname != null)
                {
                    List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
                    if (listdata != null) {
                        temp = (from a in listdata where a.schoolname.Equals(sSchoolname) select a).ToList();
                      
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