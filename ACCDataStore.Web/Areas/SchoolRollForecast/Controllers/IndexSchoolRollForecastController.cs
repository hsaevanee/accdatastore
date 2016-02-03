using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Data;
using Common.Logging;
using ACCDataStore.Repository;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity;
using System.IO;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.WiderAchievement;

namespace ACCDataStore.Web.Areas.SchoolRollForecast.Controllers
{
    public class IndexSchoolRollForecastController : Controller
    {
        // GET: SchoolRollForecast/IndexSchoolRollForecast
        public ActionResult Index()
        {
            var vmWiderAchievement = new WiderAchievementViewModel();
            string filePath = "C:\\data\\WiderAchievementData.xlsx";
            DataTable dt1 = new DataTable();


            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);
                //adding columns
                dt1.Columns.Add(workSheet.Row(3).Cell(1).Value.ToString());
                dt1.Columns.Add(workSheet.Row(3).Cell(2).Value.ToString());
                dt1.Columns.Add(workSheet.Row(3).Cell(3).Value.ToString());
                dt1.Columns.Add(workSheet.Row(3).Cell(4).Value.ToString());
                dt1.Columns.Add(workSheet.Row(3).Cell(5).Value.ToString());

                //adding table1


                for (int i = 4; i <= 26; i++)
                {
                    DataRow dataRow = dt1.NewRow();
                    IXLRow row = workSheet.Row(i);
                    for (int j = 0; j <= 4; j++)
                    {
                        dataRow[j] = (workSheet.Row(i).Cell(j + 1).Value.ToString());
                    }
                    dt1.Rows.Add(dataRow);
                }
                DataRow dataRow1 = dt1.NewRow();
                dataRow1[0] = ("");
                dataRow1[1] = (workSheet.Row(31).Cell(2).Value.ToString());
                dataRow1[2] = (workSheet.Row(31).Cell(3).Value.ToString());
                dataRow1[3] = (workSheet.Row(31).Cell(4).Value.ToString());
                dataRow1[4] = (workSheet.Row(31).Cell(5).Value.ToString());
                dt1.Rows.Add(dataRow1);
            }

            vmWiderAchievement.dtTable1 = dt1;

            DataTable dt2 = new DataTable();
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);
                //adding columns
                dt2.Columns.Add(workSheet.Row(3).Cell(7).Value.ToString());
                dt2.Columns.Add(workSheet.Row(3).Cell(8).Value.ToString());
                dt2.Columns.Add(workSheet.Row(3).Cell(9).Value.ToString());
                dt2.Columns.Add(workSheet.Row(3).Cell(10).Value.ToString());
                dt2.Columns.Add(workSheet.Row(3).Cell(11).Value.ToString());

                //adding table1


                for (int i = 4; i <= 29; i++)
                {
                    DataRow dataRow = dt2.NewRow();
                    IXLRow row = workSheet.Row(i);
                    for (int j = 0; j <= 4; j++)
                    {
                        dataRow[j] = (workSheet.Row(i).Cell(j + 7).Value.ToString());
                    }
                    dt2.Rows.Add(dataRow);
                }
                DataRow dataRow1 = dt2.NewRow();
                dataRow1[0] = ("");
                dataRow1[1] = (workSheet.Row(31).Cell(8).Value.ToString());
                dataRow1[2] = (workSheet.Row(31).Cell(9).Value.ToString());
                dataRow1[3] = (workSheet.Row(31).Cell(10).Value.ToString());
                dataRow1[4] = (workSheet.Row(31).Cell(11).Value.ToString());
                dt2.Rows.Add(dataRow1);
            }


            vmWiderAchievement.dtTable2 = dt2;
            return View("Home", vmWiderAchievement);
        }

        // GET: SchoolProfile/WiderAchievement
        public DataTable ReadDataFromExcel()
        {
            string filePath = "C:\\data\\WiderAchievementData.xlsx";
            DataTable dt1 = new DataTable();
           

            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);
                //adding columns
                dt1.Columns.Add(workSheet.Row(3).Cell(1).Value.ToString());
                dt1.Columns.Add(workSheet.Row(3).Cell(2).Value.ToString());
                dt1.Columns.Add(workSheet.Row(3).Cell(3).Value.ToString());
                dt1.Columns.Add(workSheet.Row(3).Cell(4).Value.ToString());
                dt1.Columns.Add(workSheet.Row(3).Cell(5).Value.ToString());

                //adding table1


                for (int i = 4; i <= 26; i++)
                {
                    DataRow dataRow = dt1.NewRow();
                    IXLRow row = workSheet.Row(i);
                    for (int j = 0; j <= 4; j++)
                    {
                        dataRow[j] = (workSheet.Row(i).Cell(j + 1).Value.ToString());
                    }
                    dt1.Rows.Add(dataRow);
                }
                DataRow dataRow1 = dt1.NewRow();
                dataRow1[0] = ("");
                dataRow1[1] = (workSheet.Row(31).Cell(2).Value.ToString());
                dataRow1[2] = (workSheet.Row(31).Cell(3).Value.ToString());
                dataRow1[3] = (workSheet.Row(31).Cell(4).Value.ToString());
                dataRow1[4] = (workSheet.Row(31).Cell(5).Value.ToString());
                dt1.Rows.Add(dataRow1);
            }


            DataTable dt2 = new DataTable();
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);
                //adding columns
                dt2.Columns.Add(workSheet.Row(3).Cell(7).Value.ToString());
                dt2.Columns.Add(workSheet.Row(3).Cell(8).Value.ToString());
                dt2.Columns.Add(workSheet.Row(3).Cell(9).Value.ToString());
                dt2.Columns.Add(workSheet.Row(3).Cell(10).Value.ToString());
                dt2.Columns.Add(workSheet.Row(3).Cell(11).Value.ToString());

                //adding table1


                for (int i = 4; i <= 29; i++)
                {
                    DataRow dataRow = dt2.NewRow();
                    IXLRow row = workSheet.Row(i);
                    for (int j = 0; j <= 4; j++)
                    {
                        dataRow[j] = (workSheet.Row(i).Cell(j + 7).Value.ToString());
                    }
                    dt2.Rows.Add(dataRow);
                }
                DataRow dataRow1 = dt2.NewRow();
                dataRow1[0] = ("");
                dataRow1[1] = (workSheet.Row(31).Cell(8).Value.ToString());
                dataRow1[2] = (workSheet.Row(31).Cell(9).Value.ToString());
                dataRow1[3] = (workSheet.Row(31).Cell(10).Value.ToString());
                dataRow1[4] = (workSheet.Row(31).Cell(11).Value.ToString());
                dt2.Rows.Add(dataRow1);
            }
            return dt2;
        }
    }
}