using ACCDataStore.Entity;
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

            var dicNational = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dicNational.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dicNational;

        }

        protected Dictionary<string, string> GetDicNationalIdenity(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from nationality");

            var dicNational = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dicNational.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dicNational;

        }

        protected Dictionary<string, string> GetDicEnglisheLevel(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from englishlevel");

            var dicNational = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dicNational.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dicNational;

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

            temp.Add(new Year("2013"));
            temp.Add(new Year("2014"));
            temp.Add(new Year("2015"));
            return temp;

        }

        protected List<StudentObj> GetListAllPupils(IGenericRepository2nd rpGeneric2nd, Year year, string sSchoolType)
        {
            List<StudentObj> listResult = new List<StudentObj>();
            switch (year.year)
            {
                case "2013":
                    listResult = rpGeneric2nd.FindAll<Student2013>().ToList<StudentObj>();
                    break;
                case "2014":
                    listResult = rpGeneric2nd.FindAll<Student2014>().ToList<StudentObj>();
                    break;
                case "2015":
                    listResult = rpGeneric2nd.FindAll<Student2015>().ToList<StudentObj>();
                    break;
            }
                         
            List<StudentObj> listData = new List<StudentObj>();
            if (sSchoolType.Equals("2"))
            {
                //select only primary pupils
                listData = listResult.Where(x => x.Stage.StartsWith("P")).ToList();
            }
            else if (sSchoolType.Equals("3"))
            {
                //select only secondary pupils
                listData = listResult.Where(x => x.Stage.StartsWith("S")).ToList();
            }
            else {
                listData = listResult;
            }
            return listData;

        }
        protected DataTable CreateDataTale(List<DataSeries> listobject, Dictionary<string, string> dictionary, string tabletitle)
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(tabletitle, typeof(string));
            foreach (var temp in listobject)
                {
                    dataTable.Columns.Add(temp.school.name, typeof(string));
                }
            List<string> temprowdata;
            string tempdataitem;

            foreach (var key in dictionary)
            {
                temprowdata = new List<string>();
                temprowdata.Add(key.Value);
                foreach (var temp in listobject)
                {
                        List<ObjectDetail> listtemp = temp.listdataitems;
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0.00" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().percentage.ToString("0.00");
                        temprowdata.Add(tempdataitem);

                }
                dataTable.Rows.Add(temprowdata.ToArray());
            }
            return dataTable;
        }

        protected DataTable CreateDataTale(List<List<DataSeries>> listobject, Dictionary<string, string> dictionary, string tabletitle) 
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(tabletitle, typeof(string));

            DataColumn col;

            foreach (var item in listobject)
            {
                
                foreach (var temp in item)
                {
                    col = new DataColumn();
                    //dataTable.Columns.Add(temp.school.name, typeof(string));
                    //col.Caption = "Your Caption";
                    col.ColumnName = temp.school.name + temp.year.year;
                    col.DataType = System.Type.GetType("System.String");
                    dataTable.Columns.Add(col);
                }

            }



            List<string> temprowdata;
            string tempdataitem;

            foreach (var key in dictionary)
            {
                temprowdata = new List<string>();
                temprowdata.Add(key.Value);
                foreach (var item in listobject)
                {
                    foreach (var temp in item)
                    {
                        List<ObjectDetail> listtemp = temp.listdataitems;
                        tempdataitem = listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault() == null ? "0.00" : listtemp.Where(x => x.itemcode.Equals(key.Key)).FirstOrDefault().percentage.ToString("0.00");
                        temprowdata.Add(tempdataitem);
                    }
                }
                dataTable.Rows.Add(temprowdata.ToArray());
            }
            return dataTable;
        }
        //not in use
        protected List<DataSeries> GetEthnicBackgroundDataSeries(List<StudentObj> listPupilData, List<School> listSelectedSchool)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            //var listResultwithPercentage = null;
            double sum=0.0;

            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select primary pupils for selected school
                listtempPupilData = listPupilData.Where(x => x.SeedCode.ToString().Equals(item.seedcode)).ToList();
                var listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.EthnicBackground }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), EthnicBackgroundcode = y.Key.EthnicBackground, list = y.ToList(), count = y.ToList().Count() }).ToList();
                sum = (double)listResult.Select(r => r.count).Sum();
                var listResultwithPercentage = listResult.Select(y => new ObjectDetail{ itemcode = y.EthnicBackgroundcode, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                listobject.Add(new DataSeries { school = item, listdataitems = listResultwithPercentage});
           }

            // get data for all primary schools
            var listResultforAll = listPupilData.GroupBy(x => x.EthnicBackground).Select(y => new { EthnicBackgroundcode = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
            //calculate the total number of pupils in Aberdeen
            sum = (double)listResultforAll.Select(r => r.count).Sum();
            var listResultwithPercentage2 = listResultforAll.Select(y => new ObjectDetail { itemcode = y.EthnicBackgroundcode, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
            listobject.Add(new DataSeries{ school = new School("Aberdeen City","Aberdeen City"), listdataitems = listResultwithPercentage2});
            return listobject;
        }
        //not in use
        protected List<DataSeries> GetNationalIdentityDataSeries(List<StudentObj> listPupilData, List<School> listSelectedSchool)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            //var listResultwithPercentage = null;
            double sum = 0.0;

            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select primary pupils for selected school
                listtempPupilData = listPupilData.Where(x => x.SeedCode.ToString().Equals(item.seedcode)).ToList();
                var listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.NationalIdentity }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), NationalIdentitycode = y.Key.NationalIdentity, list = y.ToList(), count = y.ToList().Count() }).ToList();
                sum = (double)listResult.Select(r => r.count).Sum();
                var listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.NationalIdentitycode, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                listobject.Add(new DataSeries { school = item, listdataitems = listResultwithPercentage });
            }

            // get data for all primary schools
            var listResultforAll = listPupilData.GroupBy(x => x.NationalIdentity).Select(y => new { NationalIdentitycode = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
            //calculate the total number of pupils in Aberdeen
            sum = (double)listResultforAll.Select(r => r.count).Sum();
            var listResultwithPercentage2 = listResultforAll.Select(y => new ObjectDetail { itemcode = y.NationalIdentitycode, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
            listobject.Add(new DataSeries { school = new School("Aberdeen City", "Aberdeen City"), listdataitems = listResultwithPercentage2 });

            return listobject;
        }

        //not in use
        protected List<DataSeries> GetEngishLevelDataSeries(List<StudentObj> listPupilData, List<School> listSelectedSchool)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            //var listResultwithPercentage = null;
            double sum = 0.0;

            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select primary pupils for selected school
                listtempPupilData = listPupilData.Where(x => x.SeedCode.ToString().Equals(item.seedcode)).ToList();
                var listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.LevelofEnglish }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), LevelofEnglishcode = y.Key.LevelofEnglish, list = y.ToList(), count = y.ToList().Count() }).ToList();
                sum = (double)listResult.Select(r => r.count).Sum();
                var listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.LevelofEnglishcode, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                listobject.Add(new DataSeries { school = item, listdataitems = listResultwithPercentage });
            }

            // get data for all primary schools
            var listResultforAll = listPupilData.GroupBy(x => x.LevelofEnglish).Select(y => new { LevelofEnglishcode = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
            //calculate the total number of pupils in Aberdeen
            sum = (double)listResultforAll.Select(r => r.count).Sum();
            var listResultwithPercentage2 = listResultforAll.Select(y => new ObjectDetail { itemcode = y.LevelofEnglishcode, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
            listobject.Add(new DataSeries { school = new School("Aberdeen City", "Aberdeen City"), listdataitems = listResultwithPercentage2 });
            return listobject;
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
                        listResult = listtempPupilData.GroupBy(x => new { x.SeedCode, x.LevelofEnglish }).Select(y => new { SeedCode = y.Key.SeedCode.ToString(), Code = y.Key.LevelofEnglish, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        sum = (double)listResult.Select(r => r.count).Sum();
                        listResultwithPercentage = listResult.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                
                }
                
                listobject.Add(new DataSeries { school = item, year = iyear, listdataitems = listResultwithPercentage });
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
                    listResultforAll = listPupilData.GroupBy(x => x.LevelofEnglish).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                            //calculate the total number of pupils in Aberdeen
                    sum = (double)listResultforAll.Select(r => r.count).Sum();
                    listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                    break;

            
            }


            listobject.Add(new DataSeries { school = new School("Aberdeen City", "Aberdeen City"),year = iyear, listdataitems = listResultwithPercentage });

            return listobject;
        }
    }
}