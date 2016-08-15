﻿using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.Census;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class BaseSchoolProfileController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(BaseSchoolProfileController));
        // GET: SchoolProfiles/BaseSchoolProfile
        protected Dictionary<string, string> GetDicEhtnicBG(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from ethnicbackground");

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicNationalIdenity(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from nationality");

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicFreeSchoolMeal()
        {
          
            var dictionary = new Dictionary<string, string>();

 
            dictionary.Add("1", "Pupils registered as entitled to free school meals");
 
            return dictionary;

        }

        protected Dictionary<string, string> GetDicLookAfter()
        {

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("01", "Looked After At Home");

            dictionary.Add("02", "Looked After Away From Home");

            return dictionary;

        }

        protected Dictionary<string, string> GetDicEnglisheLevel(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from englishlevel");

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicStage(IGenericRepository2nd rpGeneric2nd, string sSchoolType)
        {
            dynamic listResult = null;

            switch(sSchoolType){
                case "2":
                    listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from stage where Code LIKe 'P%'");
                    break;
                case "3":
                    listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from stage where Code in ('S1','S2','S3','S4','S5','S6')");
                    break;
                case "4":
                    listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from stage where Code in ('SP')");
                    break;
            }

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicAttendance(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from attendancecodes");

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected List<School> GetListSchool(IGenericRepository2nd rpGeneric2nd, string sSchoolType)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select sed_no, name from View_Costcentre where ClickNGo !=0 AND schoolType_id = " + sSchoolType);

            //var listResult = this.rpGeneric3nd.FindAll<Costcentre>().ToList();

            List<School> listdata = new List<School>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        listdata.Add(new School(itemRow[0].ToString(), itemRow[1].ToString()));
                    }
                }
            }
            return listdata.OrderBy( x=>x.name).ToList();

        }

        protected List<Year> GetListYear()
        {
            List<Year> temp = new List<Year>();
            temp.Add(new Year("2015"));
            temp.Add(new Year("2014"));
            temp.Add(new Year("2013"));
            temp.Add(new Year("2012"));
            temp.Add(new Year("2011"));
            temp.Add(new Year("2010"));
            temp.Add(new Year("2009"));
            temp.Add(new Year("2008"));
            return temp;

        }

        protected List<StudentObj> GetListAllPupils(IGenericRepository2nd rpGeneric2nd, Year year, string sSchoolType)
        {
            List<StudentObj> listResult = new List<StudentObj>();
            switch (year.year)
            {
                case "2008":
                    //listResult = rpGeneric2nd.Find<SchStudent2008>(" from SchStudent2008 where SchStudent2008.StudentStatus = :studentStatus ", new string[] { "studentStatus" }, new object[] { "01" }).ToList<StudentObj>();
                    listResult = rpGeneric2nd.FindAll<SchStudent2008>().ToList<StudentObj>();
                    break;
                case "2009":
                    listResult = rpGeneric2nd.FindAll<SchStudent2009>().ToList<StudentObj>();
                    break;
                case "2010":
                    listResult = rpGeneric2nd.FindAll<SchStudent2010>().ToList<StudentObj>();
                    break;
                case "2011":
                    listResult = rpGeneric2nd.FindAll<SchStudent2011>().ToList<StudentObj>();
                    break;
                case "2012":
                    listResult = rpGeneric2nd.FindAll<SchStudent2012>().ToList<StudentObj>();
                    break;
                case "2013":
                    listResult = rpGeneric2nd.FindAll<SchStudent2013>().ToList<StudentObj>();
                    break;
                case "2014":
                    listResult = rpGeneric2nd.FindAll<SchStudent2014>().ToList<StudentObj>();
                    break;
                case "2015":
                    listResult = rpGeneric2nd.FindAll<SchStudent2015>().ToList<StudentObj>();
                    break;
            }
                         
            List<StudentObj> listData = new List<StudentObj>();
            if (sSchoolType.Equals("2"))
            {
                //select only primary pupils
                listData = listResult.Where(x => (x.StudentStage.StartsWith("P")) && (x.StudentStatus.Equals("01"))).ToList();
            }
            else if (sSchoolType.Equals("3"))
            {
                //select only secondary pupils
                listData = listResult.Where(x => (x.StudentStage.StartsWith("S")) && (x.StudentStatus.Equals("01"))).ToList();
                listData = listData.Where(x => !x.StudentStage.Equals("SP")).ToList(); // excluded special schoool

            }
            else if (sSchoolType.Equals("4"))
            {
                //select only special pupils
                listData = listResult.Where(x => (x.StudentStage.Equals("SP")) && (x.StudentStatus.Equals("01"))).ToList();
            }
            else if (sSchoolType.Equals("5")) // pupils for aberdeen city
            {
                //select all pupils in Aberdeen
                listData = listResult.Where(x => x.StudentStatus.Equals("01")).ToList(); 
            }
 
            return listData;

        }


        //create dataTable from list of DataSeries
        protected DataTable CreateDataTable(List<DataSeries> listobject, Dictionary<string, string> dictionary, string tabletitle, string showtype)
        {

            DataTable dataTable = new DataTable();           
            List<string> temprowdata;
            string tempdataitem;

            if (showtype.Equals("number"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                foreach (var key in dictionary)
                {
                    dataTable.Columns.Add(key.Value, typeof(string));
                }
                //dataTable.Columns.Add("Total", typeof(string));
                //display number
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    foreach (var key in dictionary)
                    {
                        List<ObjectDetail> listtemp = temp.listdataitems;
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().count.ToString();
                        temprowdata.Add(tempdataitem);

                    }
                    //temprowdata.Add(temp.checkSumCount.ToString());
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            else if (showtype.Equals("percentage"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                foreach (var key in dictionary)
                {
                    dataTable.Columns.Add(key.Value, typeof(string));
                }
                //dataTable.Columns.Add("Total", typeof(string));
                //display percentage
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    foreach (var key in dictionary)
                    {
                        List<ObjectDetail> listtemp = temp.listdataitems;
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0.00" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().percentage.ToString("0.00");
                        temprowdata.Add(tempdataitem);

                    }
                    //temprowdata.Add(temp.checkSumPercentage.ToString());
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            else if (showtype.Equals("no+%"))
            {
                //this case show count and percentage
                dataTable.Columns.Add(tabletitle, typeof(string));
                foreach (var key in dictionary)
                {
                    dataTable.Columns.Add(key.Value, typeof(string)); 
                    dataTable.Columns.Add(key.Value + " (%)", typeof(string));
                }
                //dataTable.Columns.Add("Total", typeof(string));
                //display percentage
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    foreach (var key in dictionary)
                    {
                        List<ObjectDetail> listtemp = temp.listdataitems;
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().count.ToString();
                        temprowdata.Add(tempdataitem);
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0.00" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().percentage.ToString("0.00");
                        temprowdata.Add(tempdataitem);

                    }
                    //temprowdata.Add(temp.checkSumPercentage.ToString());
                    dataTable.Rows.Add(temprowdata.ToArray());
                }
            
            
            }

            return dataTable;
        }

        protected DataTable CreateDataTaleWithTotal(List<DataSeries> listobject, Dictionary<string, string> dictionary, string tabletitle, string showtype)
        {
            // create data table with count total data show in each row
            DataTable dataTable = new DataTable();           
            List<string> temprowdata;
            string tempdataitem;

  
            if (showtype.Equals("number"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                foreach (var key in dictionary)
                {
                    dataTable.Columns.Add(key.Value, typeof(string));
                }
                dataTable.Columns.Add("Total", typeof(string));

                int sum = 0;
                //display number
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    sum = 0;
                    foreach (var key in dictionary)
                    {
                        List<ObjectDetail> listtemp = temp.listdataitems;
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().count.ToString();
                        temprowdata.Add(tempdataitem);
                        sum = sum + Convert.ToInt32(tempdataitem);

                    }
                    temprowdata.Add(sum.ToString(""));
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            else if (showtype.Equals("percentage"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                foreach (var key in dictionary)
                {
                    dataTable.Columns.Add(key.Value, typeof(string));
                }
                dataTable.Columns.Add("Total", typeof(string));
                //display percentage

                double sum = 0;

                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    sum = 0;
                    foreach (var key in dictionary)
                    {
                        List<ObjectDetail> listtemp = temp.listdataitems;
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0.00" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().percentage.ToString("0.00");
                        temprowdata.Add(tempdataitem);
                        sum = sum + Double.Parse(tempdataitem);
                    }
                    temprowdata.Add(sum.ToString("0.00"));
                    dataTable.Rows.Add(temprowdata.ToArray());
                }
            
            }
            else if (showtype.Equals("no+%"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                foreach (var key in dictionary)
                {
                    dataTable.Columns.Add(key.Value, typeof(string));
                    dataTable.Columns.Add(key.Value+ " (%)", typeof(string));
                }
                dataTable.Columns.Add("Total", typeof(string));
                dataTable.Columns.Add("Total(%)", typeof(string));
                //display percentage

                double sum = 0;
                double sumP = 0;
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    sum = 0;
                    sumP = 0;
                    foreach (var key in dictionary)
                    {
                        List<ObjectDetail> listtemp = temp.listdataitems;
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0.00" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().count.ToString("0.00");
                        temprowdata.Add(tempdataitem);
                        sum = sum + Double.Parse(tempdataitem);
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0.00" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().percentage.ToString("0.00");
                        temprowdata.Add(tempdataitem);
                        sumP = sumP + Double.Parse(tempdataitem);
                    }
                    temprowdata.Add(sum.ToString("0.00"));
                    temprowdata.Add(sumP.ToString("0.00"));
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            return dataTable;
        }

        protected DataTable CreateDataTable(List<DataSeries> listobject, string tabletitle, string showtype)
        {

            DataTable dataTable = new DataTable();
            List<string> temprowdata;
 

            if (showtype.Equals("number"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                foreach (var item in listobject[0].listdataitems)
                {
                    dataTable.Columns.Add(item.itemcode, typeof(string));
                }
                //dataTable.Columns.Add("Total", typeof(string));
                //display number
                //adding row data
                foreach (var item in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(item.school.name);
                    foreach (var temp in item.listdataitems)
                    {
                        temprowdata.Add(Double.IsNaN(temp.count) ? "na" : temp.count.ToString("0"));
                    }
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            else if (showtype.Equals("percentage"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                foreach (var item in listobject[0].listdataitems)
                {
                    dataTable.Columns.Add(item.itemcode, typeof(string));
                }
                //dataTable.Columns.Add("Total", typeof(string));
                //display number
                //adding row data
                foreach (var item in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(item.school.name);
                    foreach (var temp in item.listdataitems)
                    {
                        temprowdata.Add(Double.IsNaN(temp.percentage) ? "na" : temp.percentage.ToString("0.00"));
                    }
                    dataTable.Rows.Add(temprowdata.ToArray());
                }


            }
           
            return dataTable;
        }

        //data table for trending page
        protected DataTable CreateDataTable(List<List<DataSeries>> listobject, Dictionary<string, string> dictionary, string tabletitle, string showtype) 
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(tabletitle, typeof(string));

            List<string> temprowdata;
            string tempdataitem;

            if (showtype.Equals("number"))
            {
                //add column
                foreach (var key in dictionary)
                {
                    dataTable.Columns.Add(key.Value, typeof(string));
                }
                dataTable.Columns.Add("Total", typeof(string));
                //add row
                foreach (var item in listobject)
                {
                    foreach (var temp in item)
                    {
                        temprowdata = new List<string>();
                        temprowdata.Add(temp.school.name + temp.year.year);
                        foreach (var key in dictionary)
                        {
                            List<ObjectDetail> listtemp = temp.listdataitems;
                            tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().count.ToString();
                            temprowdata.Add(tempdataitem);

                        }
                        temprowdata.Add(temp.checkSumCount.ToString());
                        dataTable.Rows.Add(temprowdata.ToArray());
                    }

                }
            }
            else {

                foreach (var key in dictionary)
                {
                    dataTable.Columns.Add(key.Value, typeof(string));
                }
                foreach (var item in listobject)
                {
                    foreach (var temp in item)
                    {
                        temprowdata = new List<string>();
                        temprowdata.Add(temp.school.name + temp.year.year);
                        foreach (var key in dictionary)
                        {
                            List<ObjectDetail> listtemp = temp.listdataitems;
                            tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0.00" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().percentage.ToString("0.00");
                            temprowdata.Add(tempdataitem);

                        }
                        dataTable.Rows.Add(temprowdata.ToArray());
                    }

                }
            }



            return dataTable;
        }
 
        protected List<DataSeries> GetDataSeries(string datatitle, List<StudentObj> listPupilData, List<School> listSelectedSchool, Year iyear, string schooltype)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            List<StudentObj> listtempPupilDataP4P7 = new List<StudentObj>();
            //var listResultwithPercentage = null;
            double sum = 0.0;
            List<ObjectDetail> listResultwithPercentage = new List<ObjectDetail>();
            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select primary pupils for selected school
                listtempPupilData = listPupilData.Where(x => x.SeedCode.ToString().Equals(item.seedcode)).ToList();
                listtempPupilDataP4P7 = (from a in listtempPupilData where a.StudentStage.Equals("P4") || a.StudentStage.Equals("P5") || a.StudentStage.Equals("P6") || a.StudentStage.Equals("P7") select a).ToList<StudentObj>();
                switch(datatitle){
               
                    case "nationality":
                        var listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.NationalIdentity }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.NationalIdentity, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "ethnicity":
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.EthnicBackground }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.EthnicBackground, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "englishlevel":
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.LevelOfEnglish }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.LevelOfEnglish, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    //case "freeschoolmeal":
                    //    listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //    sum = (double)listResult.Select(r => r.count).Sum();
                    //    listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    //    break;
                    case "stage":
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.StudentStage }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.StudentStage, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "freemeal":
                        if (schooltype.Equals("2"))
                        {
                            //select only pupils between stage 4 and 7                          
                            //listResult = temp.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                            var listResultP4P7 = listtempPupilDataP4P7.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();
                            listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();

                            sum = (double)listResult.Select(r => r.count).Sum();
                            listResultwithPercentage = listResultP4P7.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        }
                        else{
                            listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();
                            sum = (double)listResult.Select(r => r.count).Sum();
                            listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        }
                        break;
                    case "lookafter":
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.StudentLookedAfter }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.StudentLookedAfter == null ? "" : y.Key.StudentLookedAfter, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;

                
                }

                listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = item, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });
            }

            // get data for all primary schools
            switch (datatitle) {

                case "nationality":
                    var listResultforAll = listPupilData.GroupBy(x => x.NationalIdentity).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
                case "ethnicity":
                    listResultforAll = listPupilData.GroupBy(x => x.EthnicBackground).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
                case "englishlevel":
                    listResultforAll = listPupilData.GroupBy(x => x.LevelOfEnglish).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
                //case "freeschoolmeal":
                //    listResultforAll = listPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                //    //calculate the total number of pupils in Aberdeen
                //    sum = (double)listResultforAll.Select(r => r.count).Sum();
                //    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                //    break;
                case "stage":
                    listResultforAll = listPupilData.GroupBy(x => x.StudentStage).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
                case "freemeal":
                    if (schooltype.Equals("2")) {
                        //select only pupils between stage 4 and 7
                        var temp = (from a in listPupilData where a.StudentStage.Equals("P4") || a.StudentStage.Equals("P5") || a.StudentStage.Equals("P6") || a.StudentStage.Equals("P7") select a).ToList();
                        var listResultforP4P7 = temp.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        listResultforAll = listPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();

                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforP4P7.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    }
                    else
                    {
                        listResultforAll = listPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    }
                    break;
                case "lookafter":
                    listResultforAll = listPupilData.GroupBy(x => x.StudentLookedAfter).Select(y => new { Code = y.Key == null? "": y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
            
            }

            School school = new School("Aberdeen City", "Aberdeen City"); ;

            if (schooltype.Equals("2"))
            {
                school = new School("Aberdeen Primary Schools", "Aberdeen Primary Schools");
            }
            else if (schooltype.Equals("3"))
            {
                school = new School("Aberdeen Secondary Schools", "Aberdeen Secondary Schools");
            }
            else if (schooltype.Equals("4"))
            {
                school = new School("Aberdeen Special Schools", "Aberdeen Special Schools");
            }


            listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = school, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });

            return listobject;
        }

        

        protected DataSeries GetDataSeriesBySchool(string datatitle, List<StudentObj> listPupilData, School school, Year iyear)
        {
            double sum = 0.0;
            DataSeries dataobj = new DataSeries();
            List<ObjectDetail> listResultwithPercentage = new List<ObjectDetail>();
            //calculate individual school
            
                switch (datatitle)
                {

                    case "nationality":
                        var listResult = listPupilData.GroupBy(x => new { x.SeedCode, x.NationalIdentity }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.NationalIdentity, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "ethnicity":
                        listResult = listPupilData.GroupBy(x => new { x.SeedCode, x.EthnicBackground }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.EthnicBackground, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "englishlevel":
                        listResult = listPupilData.GroupBy(x => new { x.SeedCode, x.LevelOfEnglish }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.LevelOfEnglish, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    //case "freeschoolmeal":
                    //    listResult = listPupilData.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //    sum = (double)listResult.Select(r => r.count).Sum();
                    //    listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    //    break;
                    case "stage":
                        listResult = listPupilData.GroupBy(x => new { x.SeedCode, x.StudentStage }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.StudentStage, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "freemeal":
                        listResult = listPupilData.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;

                }

            dataobj.school = school;
            dataobj.year = iyear;
            dataobj.listdataitems= listResultwithPercentage;
            dataobj.checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum();
            dataobj.checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum();

            return dataobj;
        }
        protected DataSeries GetDataSeriesByAberdeenCity(string datatitle, List<StudentObj> listPupilData, Year iyear)
        {
            double sum = 0.0;
            DataSeries dataobj = new DataSeries();
            List<ObjectDetail> listResultwithPercentage = new List<ObjectDetail>();
            //calculate individual school

            // get data for all primary schools
            switch (datatitle)
            {

                case "nationality":
                    var listResultforAll = listPupilData.GroupBy(x => x.NationalIdentity).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
                case "ethnicity":
                    listResultforAll = listPupilData.GroupBy(x => x.EthnicBackground).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
                case "englishlevel":
                    listResultforAll = listPupilData.GroupBy(x => x.LevelOfEnglish).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
                //case "freeschoolmeal":
                //    listResultforAll = listPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                //    //calculate the total number of pupils in Aberdeen
                //    sum = (double)listResultforAll.Select(r => r.count).Sum();
                //    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                //    break;
                case "stage":
                    listResultforAll = listPupilData.GroupBy(x => x.StudentStage).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
                case "freemeal":
                    listResultforAll = listPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;

            }

            dataobj.school = new School("Aberdeen City", "Aberdeen City");
            dataobj.year = iyear;
            dataobj.listdataitems = listResultwithPercentage;
            dataobj.checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum();
            dataobj.checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum();

            return dataobj;
        }

        protected List<AaeAttendanceObj> GetAaeAttendanceLists(IGenericRepository2nd rpGeneric2nd, string sSchoolType, Year year, List<School> schools, List<StudentObj> listAllPupils)
        {
            List<AaeAttendanceObj> listResult = new List<AaeAttendanceObj>();
            //List<AaeAttendanceObjT> listResult2 = new List<AaeAttendanceObjT>();
            switch (year.year)
            {
                case "2008":
                    listResult = rpGeneric2nd.FindAll<AaeAttendance2008>().ToList<AaeAttendanceObj>();
                    break;
                case "2009":
                    listResult = rpGeneric2nd.FindAll<AaeAttendance2009>().ToList<AaeAttendanceObj>();
                    break;
                case "2010":
                    listResult = rpGeneric2nd.FindAll<AaeAttendance2010>().ToList<AaeAttendanceObj>();
                    break;
                case "2011":
                    listResult = rpGeneric2nd.FindAll<AaeAttendance2011>().ToList<AaeAttendanceObj>();
                    break;
                case "2012":
                    listResult = rpGeneric2nd.FindAll<AaeAttendance2012>().ToList<AaeAttendanceObj>();
                    break;
                case "2013":
                    listResult = rpGeneric2nd.FindAll<AaeAttendance2013>().ToList<AaeAttendanceObj>();
                    break;
                case "2014":
                    listResult = rpGeneric2nd.FindAll<AaeAttendance2014>().ToList<AaeAttendanceObj>();
                    break;
                case "2015":
                    listResult = new List<AaeAttendanceObj>();
                    break;
            }

            //select only pupils with status 01

            List<AaeAttendanceObj> listData = new List<AaeAttendanceObj>();
            if (sSchoolType.Equals("2"))
            {
                //select only primary pupils
                listData = listResult.Where(x => (x.StudentStage.StartsWith("P"))).ToList();
            }
            else if (sSchoolType.Equals("3"))
            {
                //select only secondary pupils
                listData = listResult.Where(x => (x.StudentStage.StartsWith("S"))).ToList();
                listData = listData.Where(x => !x.StudentStage.Equals("SP")).ToList(); // excluded special schoool

            }
            else if (sSchoolType.Equals("4"))
            {
                //select only special pupils
                listData = listResult.Where(x => (x.StudentStage.Equals("SP"))).ToList();
            }
            else if (sSchoolType.Equals("5")) // pupils for aberdeen city
            {
                //select all pupils in Aberdeen
                listData = listResult.ToList();
            }

            return listData;


        }

        protected List<DataSeries> GetAaeAttendanceDataSeries(string datatitle, List<AaeAttendanceObj> listPupilData, List<School> listSelectedSchool, Year iyear, string schooltype)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<AaeAttendanceObj> listtempPupilData = new List<AaeAttendanceObj>();
            //var listResultwithPercentage = null;
            double sum = 0, sumUnauthorisedAb = 0, sumAuthorised = 0, sumAttendance = 0, sumAbExclusion = 0;
            List<ObjectDetail> listResultwithPercentage = null;
            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                sum = 0;
                sumUnauthorisedAb = 0;
                sumAuthorised = 0;
                sumAttendance = 0;
                sumAbExclusion = 0;

                listResultwithPercentage = new List<ObjectDetail>();
                //select primary pupils for selected school
                listtempPupilData = listPupilData.Where(x => x.SeedCode.ToString().Equals(item.seedcode)).ToList();

                switch (datatitle)
                {
                    case "attendance":
                        sum = (double)listtempPupilData.Where(x => x.AttendanceCode.StartsWith("01")).Select(r => r.Total).Sum() - listtempPupilData.Where(x => x.AttendanceCode.Equals("02")).Select(r => r.Total).Sum();
                        sumAttendance = (double)listtempPupilData.Where(x => x.AttendanceCode.Equals("10")).Select(r => r.Total).Sum() + (double)listtempPupilData.Where(x => x.AttendanceCode.Equals("11")).Select(r => r.Total).Sum() +listtempPupilData.Where(x => x.AttendanceCode.Equals("12")).Select(r => r.Total).Sum() ;
                        // including code 30/31/32/33
                        sumUnauthorisedAb = (double)listtempPupilData.Where(x => x.AttendanceCode.StartsWith("3")).Select(r => r.Total).Sum();
                        // including code 11-13/20-24
                        sumAuthorised =  listtempPupilData.Where(x => x.AttendanceCode.Equals("13")).Select(r => r.Total).Sum() + listtempPupilData.Where(x => x.AttendanceCode.StartsWith("2")).Select(r => r.Total).Sum();
                        // including code 40
                        sumAbExclusion = (double)listtempPupilData.Where(x => x.AttendanceCode.Equals("40")).Select(r => r.Total).Sum();
                        //adding to list
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Attendance", count = (int)sumAttendance, percentage = sum != 0 ? (sumAttendance / sum) * 100 : 0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Authorised Absence", count = (int)sumAuthorised, percentage = sum != 0 ? (sumAuthorised / sum) * 100 : 0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Unauthorised Absence", count = (int)sumUnauthorisedAb, percentage = sum != 0 ? (sumUnauthorisedAb / sum) *100 : 0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Absense due to exclusion", count = (int)sumAbExclusion, percentage = sum != 0 ? (sumAbExclusion / sum) * 100 : 0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Total Absence", count = (int)(sumAuthorised + sumUnauthorisedAb), percentage = sum != 0 ? (sumAuthorised + sumUnauthorisedAb) / sum * 100 : 0 });

                        //listtempPupilData.Select(y => new ObjectDetail { itemcode = y.AttendanceCode, count = y.Total, percentage = sum != 0 ? (y.Total / sum) * 100 : 0 }).ToList();
                        break;
                }

                listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = item, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });
            }
            //reset list
            sum = 0;
            sumUnauthorisedAb = 0;
            sumAuthorised = 0;
            sumAbExclusion = 0;
            sumAttendance = 0;
            listResultwithPercentage = new List<ObjectDetail>();
            // get data for all primary schools
            switch (datatitle)
            {
                case "attendance":
                    sumAttendance = (double)listPupilData.Where(x => x.AttendanceCode.StartsWith("10")).Select(r => r.Total).Sum() +(double)listPupilData.Where(x => x.AttendanceCode.Equals("11")).Select(r => r.Total).Sum() + listPupilData.Where(x => x.AttendanceCode.Equals("12")).Select(r => r.Total).Sum();

                    sum = (double)listPupilData.Where(x => x.AttendanceCode.StartsWith("01")).Select(r => r.Total).Sum() - listPupilData.Where(x => x.AttendanceCode.Equals("02")).Select(r => r.Total).Sum();
                    // including code 30/31/32/33
                    sumUnauthorisedAb = (double)listPupilData.Where(x => x.AttendanceCode.StartsWith("3")).Select(r => r.Total).Sum();
                    // including code 11-13/20-24
                    sumAuthorised = listPupilData.Where(x => x.AttendanceCode.Equals("13")).Select(r => r.Total).Sum() + listPupilData.Where(x => x.AttendanceCode.StartsWith("2")).Select(r => r.Total).Sum();
                    sumAbExclusion = (double)listPupilData.Where(x => x.AttendanceCode.Equals("40")).Select(r => r.Total).Sum();

                    listResultwithPercentage.Add(new ObjectDetail { itemcode = "Attendance", count = (int)sumAttendance, percentage = sum != 0 ? (sumAttendance / sum) * 100 : 0 });
                    listResultwithPercentage.Add(new ObjectDetail { itemcode = "Authorised Absence", count = (int)sumAuthorised, percentage = sum != 0 ? (sumAuthorised / sum) * 100 : 0 });
                    listResultwithPercentage.Add(new ObjectDetail { itemcode = "Unauthorised Absence", count = (int)sumUnauthorisedAb, percentage = sum != 0 ? (sumUnauthorisedAb / sum) * 100 : 0 });
                    listResultwithPercentage.Add(new ObjectDetail { itemcode = "Absense due to exclusion", count = (int)sumAbExclusion, percentage = sum != 0 ? (sumAbExclusion / sum) * 100 : 0 });
                    listResultwithPercentage.Add(new ObjectDetail { itemcode = "Total Absence", count = (int)(sumAuthorised + sumUnauthorisedAb), percentage = sum != 0 ? (sumAuthorised + sumUnauthorisedAb) / sum * 100 : 0 });

                    break;
            }


            School school = new School("Aberdeen City", "Aberdeen City");;

            if (schooltype.Equals("2"))
            {
                school = new School("Aberdeen Primary Schools", "Aberdeen Primary Schools");
            }
            else if (schooltype.Equals("3"))
            {
                school = new School("Aberdeen Secondary Schools", "Aberdeen Secondary Schools");
            }
            else if (schooltype.Equals("4"))
            {
                school = new School("Aberdeen Special Schools", "Aberdeen Special Schools");
             }

            listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = school, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });

            return listobject;
        }

        protected List<ExclusionStudentObj> GetListExclusionPupils(IGenericRepository2nd rpGeneric2nd, Year year, string sSchoolType)
        {
            List<ExclusionStudentObj> listResult = new List<ExclusionStudentObj>();
            switch (year.year)
            {
                case "2008":
                    //listResult = rpGeneric2nd.Find<SchStudent2008>(" from SchStudent2008 where SchStudent2008.StudentStatus = :studentStatus ", new string[] { "studentStatus" }, new object[] { "01" }).ToList<StudentObj>();
                    listResult = rpGeneric2nd.FindAll<ExclusionStudent2008>().ToList<ExclusionStudentObj>();
                    break;
                case "2009":
                    listResult = rpGeneric2nd.FindAll<ExclusionStudent2009>().ToList<ExclusionStudentObj>();
                    break;
                case "2010":
                    listResult = rpGeneric2nd.FindAll<ExclusionStudent2010>().ToList<ExclusionStudentObj>();
                    break;
                case "2011":
                    listResult = rpGeneric2nd.FindAll<ExclusionStudent2011>().ToList<ExclusionStudentObj>();
                    break;
                case "2012":
                    listResult = rpGeneric2nd.FindAll<ExclusionStudent2012>().ToList<ExclusionStudentObj>();
                    break;
                case "2013":
                    listResult = rpGeneric2nd.FindAll<ExclusionStudent2013>().ToList<ExclusionStudentObj>();
                    break;
                case "2014":
                    listResult = rpGeneric2nd.FindAll<ExclusionStudent2014>().ToList<ExclusionStudentObj>();
                    break;
                case "2015":
                    listResult = new List<ExclusionStudentObj>();
                    break;
            }

            List<ExclusionStudentObj> listData = new List<ExclusionStudentObj>();
            if (sSchoolType.Equals("2"))
            {
                //select only primary pupils
                listData = listResult.Where(x => (x.StudentStage.StartsWith("P")) && (x.StudentStatus.Equals("01"))).ToList();
            }
            else if (sSchoolType.Equals("3"))
            {
                //select only secondary pupils
                listData = listResult.Where(x => (x.StudentStage.StartsWith("S")) && (x.StudentStatus.Equals("01"))).ToList();
                listData = listData.Where(x => !x.StudentStage.Equals("SP")).ToList(); // excluded special schoool

            }
            else if (sSchoolType.Equals("4"))
            {
                //select only special pupils
                listData = listResult.Where(x => (x.StudentStage.Equals("SP")) && (x.StudentStatus.Equals("01"))).ToList();
            }
            else if (sSchoolType.Equals("5")) // pupils for aberdeen city
            {
                //select all pupils in Aberdeen
                listData = listResult.Where(x => x.StudentStatus.Equals("01")).ToList();
            }

            return listData;

        }

        protected List<DataSeries> GetExclusionDataSeries(string datatitle, List<ExclusionStudentObj> listPupilData, List<School> listSelectedSchool, Year iyear, string schooltype)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<ExclusionStudentObj> listtempPupilData = new List<ExclusionStudentObj>();
            //var listResultwithPercentage = null;
            double sum0 = 0, sum1 = 0, sumLength = 0;
            List<ObjectDetail> listResultwithPercentage = null;
            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                sum0 = 0;
                sum1 = 0;
                sumLength = 0;

                listResultwithPercentage = new List<ObjectDetail>();
                //select primary pupils for selected school
                listtempPupilData = listPupilData.Where(x => x.SeedCode.ToString().Equals(item.seedcode)).ToList();

                switch (datatitle)
                {
                    case "exclusion":
                        //sum0 = (double)listtempPupilData.Select(x => x.RemovedFromRegister.Equals("0")).Count();
                        sum0 = (double)listtempPupilData.Count(x => x.RemovedFromRegister.Equals("0"));
                        sum1 = (double)listtempPupilData.Count(x => x.RemovedFromRegister.Equals("1"));
                        // including code 30/31/32/33
                        sumLength = (double)listtempPupilData.Where(x => x.LengthOfExclusion !=0).Select(r => r.LengthOfExclusion).Sum();
                        //adding to list
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Temporary Exclusions", count = (int)sum0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Removals from the Register", count = (int)sum1 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Days lost per 1000 pupils", count = (int)(sumLength != 0 ? (sumLength / 2 ) / 1000 : 0 )});
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Total Exclusions", count = (int)(sum0+sum1) });

                        //listtempPupilData.Select(y => new ObjectDetail { itemcode = y.AttendanceCode, count = y.Total, percentage = sum != 0 ? (y.Total / sum) * 100 : 0 }).ToList();
                        break;
                }

                listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = item, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });
            }
            //reset list
                sum0 = 0;
                sum1 = 0;
                sumLength = 0;
            listResultwithPercentage = new List<ObjectDetail>();
            // get data for all primary schools
            switch (datatitle)
            {
                case "exclusion":
                    sum0 = (double)listPupilData.Count(x => x.RemovedFromRegister.Equals("0"));
                    sum1 = (double)listPupilData.Count(x => x.RemovedFromRegister.Equals("1"));
                        // including code 30/31/32/33
                    sumLength = (double)listPupilData.Where(x => x.LengthOfExclusion != 0).Select(r => r.LengthOfExclusion).Sum();
                        //adding to list
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Temporary Exclusions", count = (int)sum0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Removals from the Register", count = (int)sum1 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Days lost per 1000 pupils", count = (int)(sumLength != 0 ? (sumLength / 2 ) / 1000 : 0 )});
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Total Exclusions", count = (int)(sum0+sum1) });

                    break;
            }


            School school = new School("Aberdeen City", "Aberdeen City"); ;

            if (schooltype.Equals("2"))
            {
                school = new School("Aberdeen Primary Schools", "Aberdeen Primary Schools");
            }
            else if (schooltype.Equals("3"))
            {
                school = new School("Aberdeen Secondary Schools", "Aberdeen Secondary Schools");
            }
            else if (schooltype.Equals("4"))
            {
                school = new School("Aberdeen Special Schools", "Aberdeen Special Schools");
            }

            listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = school, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });

            return listobject;
        }
        
        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }
    }
}