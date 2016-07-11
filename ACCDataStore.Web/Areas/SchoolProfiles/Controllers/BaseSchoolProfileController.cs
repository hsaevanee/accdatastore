﻿using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfiles;
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
                    listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from stage where Code LIKe 'S%'");
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
            return listdata;

        }

        protected List<Year> GetListYear()
        {
            List<Year> temp = new List<Year>();
            //temp.Add(new Year("2015"));
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
                //case "2015":
                //    listResult = rpGeneric2nd.FindAll<SchStudent2015>().ToList<StudentObj>();
                //    break;
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
            }
            else {
                listData = listResult;
            }
            return listData;

        }

        //create dataTable from list of DataSeries
        protected DataTable CreateDataTale(List<DataSeries> listobject, Dictionary<string, string> dictionary, string tabletitle, string showtype)
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
                dataTable.Columns.Add("Total", typeof(string));
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
                    temprowdata.Add(temp.checkSumCount.ToString());
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            else {
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
            




            return dataTable;
        }
        //data table for trending page
        protected DataTable CreateDataTale(List<List<DataSeries>> listobject, Dictionary<string, string> dictionary, string tabletitle, string showtype) 
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
 
        protected List<DataSeries> GetDataSeries(string datatitle, List<StudentObj> listPupilData, List<School> listSelectedSchool, Year iyear)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            //var listResultwithPercentage = null;
            double sum = 0.0;
            List<ObjectDetail> listResultwithPercentage = new List<ObjectDetail>();
            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select primary pupils for selected school
                listtempPupilData = listPupilData.Where(x => x.SeedCode.ToString().Equals(item.seedcode)).ToList();
                
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
                    case "freeschoolmeal":
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "stage":
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.StudentStage }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.StudentStage, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "freemeal":
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "lookafter":
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.StudentLookedAfter }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.StudentLookedAfter == null ? "" : y.Key.StudentLookedAfter, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;

                
                }

                listobject.Add(new DataSeries { school = item, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });
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
                case "freeschoolmeal":
                    listResultforAll = listPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
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
                case "lookafter":
                    listResultforAll = listPupilData.GroupBy(x => x.StudentLookedAfter).Select(y => new { Code = y.Key == null? "": y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
            
            }

            listobject.Add(new DataSeries { school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });

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
                    case "freeschoolmeal":
                        listResult = listPupilData.GroupBy(x => new { x.SeedCode, x.FreeSchoolMealRegistered }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.FreeSchoolMealRegistered, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
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
                case "freeschoolmeal":
                    listResultforAll = listPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                    //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;
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