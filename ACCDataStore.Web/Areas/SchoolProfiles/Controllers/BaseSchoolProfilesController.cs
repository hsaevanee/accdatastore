using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using ClosedXML.Excel;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class BaseSchoolProfilesController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(BaseSchoolProfilesController));

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

            dictionary.Add("99", "N/A");

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

            switch (sSchoolType)
            {
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

        protected Dictionary<string, string> GetDicSIMDDecile()
        {

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "1");
            dictionary.Add("2", "2");
            dictionary.Add("3", "3");
            dictionary.Add("4", "4");
            dictionary.Add("5", "5");
            dictionary.Add("6", "6");
            dictionary.Add("7", "7");
            dictionary.Add("8", "8");
            dictionary.Add("9", "9");
            dictionary.Add("10", "10");
            dictionary.Add("99", "99");
            return dictionary;

        }

        protected List<School> GetListSchool(IGenericRepository2nd rpGeneric2nd, string sSchoolType)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select sed_no, name, hmie_report, school_website,revisec_capacity,hmie_date,budget from View_Costcentre_nine where ClickNGo !=0 AND schoolType_id = " + sSchoolType);

            //var listResult = this.rpGeneric3nd.FindAll<Costcentre>().ToList();

            List<School> listdata = new List<School>();
            School temp = null;

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        temp = new School(itemRow[0].ToString(), itemRow[1].ToString());
                        temp.hmie_report = itemRow[2] != null ? itemRow[2].ToString() : "";
                        temp.website_link = itemRow[3] != null ? itemRow[3].ToString() : "";
                        temp.schoolCapacity = Convert.ToInt32(itemRow[4]);
                        temp.hmieLastReport = itemRow[5] != null ? Convert.ToDateTime(itemRow[5].ToString()) : (DateTime?)null;
                        temp.costperpupil = Convert.ToDouble(itemRow[6]);
                        listdata.Add(temp);
                    }
                }
            }
            return listdata.OrderBy(x => x.name).ToList();

        }

        protected List<Year> GetListYear()
        {
            List<Year> temp = new List<Year>();
            temp.Add(new Year("2011"));
            temp.Add(new Year("2012"));
            temp.Add(new Year("2013"));
            temp.Add(new Year("2014"));
            temp.Add(new Year("2015"));
            temp.Add(new Year("2016"));
            return temp;

        }

        protected virtual List<ViewObj> GetListViewObj(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string datatitle)
        {
            List<ViewObj> listResult = new List<ViewObj>();
            string query = "";
            switch (datatitle)
            {
                case "eal":
                    query = "Select * from summary_levelofenglish where schooltype = " + sSchoolType;
                    break;
                case "ethnicbackground":
                    query = "Select * from summary_ethnicbackground where schooltype = " + sSchoolType;
                    break;
                case "stage":
                    query = "Select * from summary_studentstage where schooltype = " + sSchoolType;
                    break;
                case "nationality":
                    query = "Select * from summary_nationality where schooltype = " + sSchoolType;
                    break;
                case "needtype":
                    //to calculate IEP CSP
                    query = "Select * from summary_studentneed where schooltype = " + sSchoolType;
                    break;
                case "lookedafter":
                    //to calculate IEP CSP
                    query = "Select * from summary_studentlookedafter where schooltype = " + sSchoolType;
                    break;
                case "simd":
                    //to calculate IEP CSP
                    query = "Select * from summary_simd where schooltype = " + sSchoolType;
                    break;
                case "attendance":
                    //to calculate IEP CSP
                    query = "Select * from summary_attendance where schooltype = " + sSchoolType;
                    break;
                case "schoolroll":
                    //to calculate IEP CSP
                    query = "Select * from summary_schoolroll where schooltype = " + sSchoolType;
                    break;
            }

            var listtemp = rpGeneric2nd.FindByNativeSQL(query);
            foreach (var itemrow in listtemp)
            {
                if (itemrow != null)
                {
                    ViewObj temp = new ViewObj();
                    temp.year = new Year(itemrow[0].ToString());
                    temp.seedcode = itemrow[1].ToString();
                    temp.schooltype = itemrow[2].ToString();
                    temp.code = itemrow[3].ToString();
                    temp.count = Convert.ToInt32(itemrow[4].ToString());
                    listResult.Add(temp);
                }
            }


            return listResult;

        }

        //Historical NationalityData
        protected List<NationalityIdentity> GetHistoricalNationalityData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<NationalityIdentity> listNationalityIdentity = new List<NationalityIdentity>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            NationalityIdentity NationalityIdentity = new NationalityIdentity();

            Dictionary<string, string> DictNationality = GetDicNationalIdenity(rpGeneric2nd);

            foreach (var item in DictNationality)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "nationality");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && !x.code.Equals("08")).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Sum(x => x.count),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    NationalityIdentity = new NationalityIdentity();
                    NationalityIdentity.YearInfo = year;
                    NationalityIdentity.ListGenericSchoolData = groupedList;
                    listNationalityIdentity.Add(NationalityIdentity);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictNationality[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    NationalityIdentity = new NationalityIdentity();
                    NationalityIdentity.YearInfo = year;
                    NationalityIdentity.ListGenericSchoolData = groupedList;
                    listNationalityIdentity.Add(NationalityIdentity);
                }

            }

            return listNationalityIdentity.OrderBy(x => x.YearInfo.year).ToList();
        }

        // NationalityIdentity Chart
        protected ColumnCharts GetChartNationalityIdentity(List<SPSchool> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Nationality - Census " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% of pupils";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].NationalityIdentity.ListGenericSchoolData.Select(x => x.Name).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.NationalityIdentity.ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }

            eColumnCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        //Historical EthnicBackground data
        protected List<Ethnicbackground> GetHistoricalEthnicData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<Ethnicbackground> listEthnicbackground = new List<Ethnicbackground>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            Ethnicbackground Ethnicbackground = new Ethnicbackground();

            Dictionary<string, string> DictNationality = GetDicEhtnicBG(rpGeneric2nd);

            foreach (var item in DictNationality)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "ethnicbackground");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    Ethnicbackground = new Ethnicbackground();
                    Ethnicbackground.YearInfo = year;
                    Ethnicbackground.ListGenericSchoolData = groupedList;
                    listEthnicbackground.Add(Ethnicbackground);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictNationality[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    Ethnicbackground = new Ethnicbackground();
                    Ethnicbackground.YearInfo = year;
                    Ethnicbackground.ListGenericSchoolData = groupedList;
                    listEthnicbackground.Add(Ethnicbackground);
                }

            }

            return listEthnicbackground.OrderBy(x => x.YearInfo.year).ToList(); ;
        }
        
        //Historical Level of English data
        protected List<LevelOfEnglish> GetHistoricalEALData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<LevelOfEnglish> listLevelOfEnglish = new List<LevelOfEnglish>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            LevelOfEnglish LevelOfEnglish = new LevelOfEnglish();

            Dictionary<string, string> DictEnglisheLevel = GetDicEnglisheLevel(rpGeneric2nd);

            foreach (var item in DictEnglisheLevel)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "eal");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictEnglisheLevel[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LevelOfEnglish = new LevelOfEnglish();
                    LevelOfEnglish.YearInfo = year;
                    LevelOfEnglish.ListGenericSchoolData = groupedList.OrderBy(x => x.Name).ToList();
                    listLevelOfEnglish.Add(LevelOfEnglish);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictEnglisheLevel[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LevelOfEnglish = new LevelOfEnglish();
                    LevelOfEnglish.YearInfo = year;
                    LevelOfEnglish.ListGenericSchoolData = groupedList.OrderBy(x => x.Name).ToList();
                    listLevelOfEnglish.Add(LevelOfEnglish);
                }

            }

            return listLevelOfEnglish.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        // Level of English Chart
        protected ColumnCharts GetChartLevelofEnglish(List<SPSchool> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Level of English - Census " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% of pupils";
            //eColumnCharts.yAxis.min = 0;
            //eColumnCharts.yAxis.max = 100;
            //eColumnCharts.yAxis.tickInterval = 20;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].LevelOfEnglish.ListGenericSchoolData.Select(x => x.Name).ToList();

                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.LevelOfEnglish.ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }
            eColumnCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };
            eColumnCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // Level of English Chart by catagories
        protected ColumnCharts GetChartLevelofEnglishbyCatagories(List<SPSchool> listSchool, Year selectedyear) // query from database and return charts object
        {
            List<GenericSchoolData> temp = new List<GenericSchoolData>();
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Level of English - Census " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% of pupils";
            //eColumnCharts.yAxis.min = 0;
            //eColumnCharts.yAxis.max = 100;
            //eColumnCharts.yAxis.tickInterval = 20;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool.Select(x => x.SchoolName).ToList();
                foreach (var edata in listSchool[0].LevelOfEnglish.ListGenericSchoolData)
                {
                    temp = new List<GenericSchoolData>();
                    foreach (var eSchool in listSchool)
                    {
                        temp.AddRange(eSchool.LevelOfEnglish.ListGenericSchoolData);
                    }
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = edata.Name,
                        data = temp.Where(x => x.Name.Equals(edata.Name)).Select(x => (float?)Convert.ToDouble(x.Percent)).ToList()

                    });
                }
            }

            eColumnCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        //Historical Looked After data
        protected List<LookedAfter> GetHistoricalLookedAfterData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<LookedAfter> listLookedAfter = new List<LookedAfter>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            LookedAfter LookedAfter = new LookedAfter();

            Dictionary<string, string> DictLookedAfter = GetDicLookAfter();

            foreach (var item in DictLookedAfter)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "lookedafter");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictLookedAfter[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LookedAfter = new LookedAfter();
                    LookedAfter.YearInfo = year;
                    LookedAfter.GenericSchoolData = new GenericSchoolData()
                    {
                        Code = "1&2",
                        Name = "LookedafterPupils",
                        Value = "",
                        count = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.count).Sum(),
                        sum = total,
                        Percent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.Percent).Sum(),
                        sPercent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => Convert.ToDouble(x.sPercent)).Sum().ToString()
                    };
                    LookedAfter.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => x.Code).ToList(); ;
                    listLookedAfter.Add(LookedAfter);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictLookedAfter[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LookedAfter = new LookedAfter();
                    LookedAfter.YearInfo = year;
                    LookedAfter.GenericSchoolData = new GenericSchoolData()
                    {
                        Code = "1&2",
                        Name = "LookedafterPupils",
                        Value = "",
                        count = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.count).Sum(),
                        sum = total,
                        Percent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.Percent).Sum(),
                        sPercent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => Convert.ToDouble(x.sPercent)).Sum().ToString()
                    };
                    LookedAfter.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => x.Code).ToList(); ;
                    listLookedAfter.Add(LookedAfter);
                }

            }

            return listLookedAfter.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        // Looked After Chart
        protected ColumnCharts GetChartLookedAfter(List<SPSchool> listSchool) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Looked After Children ";
            eColumnCharts.yAxis.title.text = "% of pupils Looked After";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].listLookedAfter.Select(x => x.YearInfo.academicyear).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.listLookedAfter.Select(x => (float?)Convert.ToDouble(x.ListGenericSchoolData.Select(y => Convert.ToDouble(y.sPercent)).Sum())).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }

            eColumnCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };


            eColumnCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        //Historical SIMD data
        protected List<SPSIMD> GetHistoricalSIMDData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<SPSIMD> listSIMD = new List<SPSIMD>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            SPSIMD SPSIMD = new SPSIMD();

            Dictionary<string, string> DictSIMD = GetDicSIMDDecile();

            foreach (var item in DictSIMD)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "simd");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    if (listresult != null && listresult.Count > 0)
                    {
                        int total = listresult.Select(s => s.count).Sum();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictSIMD[y.Key.ToString()],
                            count = y.Select(a => a.count).Sum(),
                            sum = total,
                            Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                            sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        SPSIMD = new SPSIMD();
                        SPSIMD.YearInfo = year;
                        SPSIMD.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => Convert.ToInt16(x.Code)).ToList();
                        listSIMD.Add(SPSIMD);
                    }
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    if (listresult != null && listresult.Count > 0)
                    {
                        int total = listresult.Select(s => s.count).Sum();
                        var groupedList = listresult.Select(y => new GenericSchoolData
                        {
                            Code = y.code,
                            Name = DictSIMD[y.code],
                            count = y.count,
                            sum = total,
                            Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                            sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        SPSIMD = new SPSIMD();
                        SPSIMD.YearInfo = year;
                        SPSIMD.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => Convert.ToInt16(x.Code)).ToList();
                        listSIMD.Add(SPSIMD);
                    }

                }

            }

            return listSIMD.OrderBy(x => x.YearInfo.year).ToList();
        }

        // SIMD Chart
        protected ColumnCharts GetChartSIMDDecile(List<SPSchool> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Scottish Index of Multiple Deprivation 2016/2017";
            eColumnCharts.yAxis.title.text = "% of pupils in Each Decile";
            //eColumnCharts.yAxis.min = 0;
            //eColumnCharts.yAxis.max = 100;
            //eColumnCharts.yAxis.tickInterval = 20;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SIMD.ListGenericSchoolData.Select(x => "Decile " + x.Name).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.SIMD.ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }

            eColumnCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        //Historical Attendance Data
        protected List<SPAttendance> GetHistoricalAttendanceData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, School school, List<Year> listyear)
        {
            List<SPAttendance> listAttendance = new List<SPAttendance>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            SPAttendance SPAttendance = new SPAttendance();

            Dictionary<string, string> DictAttendance = GetDicAttendance(rpGeneric2nd);

            foreach (var item in DictAttendance)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "attendance");

            if (school.seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    if (listresult.Count > 0)
                    {
                        tempdata = new List<GenericSchoolData>();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictAttendance[y.Key.ToString()],
                            count = y.Sum(x => x.count)
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        int possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();
                        int sum = groupedList.Where(x => x.Code.Equals("10") || x.Code.Equals("11") || x.Code.Equals("12")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });
                        int sumUnauthorised = groupedList.Where(x => x.Code.StartsWith("3")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = sumUnauthorised,
                            sum = possibledays,
                            Percent = sumUnauthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumUnauthorised * 100.00F / possibledays), 1).ToString()
                        });

                        int sumAuthorised = groupedList.Where(x => x.Code.StartsWith("2") || x.Code.Equals("13")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = sumAuthorised,
                            sum = possibledays,
                            Percent = sumAuthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumAuthorised * 100.00F / possibledays), 1).ToString()
                        });

                        sum = groupedList.Where(x => x.Code.Equals("40")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = sumAuthorised + sumUnauthorised,
                            sum = possibledays,
                            Percent = (sumAuthorised + sumUnauthorised) * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber(((sumAuthorised + sumUnauthorised) * 100.00F / possibledays), 1).ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);

                    }
                    else
                    {
                        tempdata = new List<GenericSchoolData>();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);
                    }
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(school.seedcode)).ToList();
                    if (listresult.Count > 0)
                    {
                        tempdata = new List<GenericSchoolData>();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictAttendance[y.Key.ToString()],
                            count = y.Sum(x => x.count)
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        int possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();
                        int sum = groupedList.Where(x => x.Code.Equals("10") || x.Code.Equals("11") || x.Code.Equals("12")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });
                        int sumUnauthorised = groupedList.Where(x => x.Code.StartsWith("3")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = sumUnauthorised,
                            sum = possibledays,
                            Percent = sumUnauthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumUnauthorised * 100.00F / possibledays), 1).ToString()
                        });

                        int sumAuthorised = groupedList.Where(x => x.Code.StartsWith("2") || x.Code.Equals("13")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = sumAuthorised,
                            sum = possibledays,
                            Percent = sumAuthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumAuthorised * 100.00F / possibledays), 1).ToString()
                        });

                        sum = groupedList.Where(x => x.Code.Equals("40")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = sumAuthorised + sumUnauthorised,
                            sum = possibledays,
                            Percent = (sumAuthorised + sumUnauthorised) * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber(((sumAuthorised + sumUnauthorised) * 100.00F / possibledays), 1).ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);

                    }
                    else
                    {
                        tempdata = new List<GenericSchoolData>();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);
                    }

                }

            }

            return listAttendance.OrderBy(x => x.YearInfo.year).ToList();
        }

        // Attendance Chart
        protected SplineCharts GetChartAttendance(List<SPSchool> listSchool, string ssubject) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = ssubject;
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            //finding subject index to query data from list
            string[] arraySubject = { "Attendance", "Unauthorised Absence", "Authorised Absence", "Absense due to Exclusion", "Total Absence" };
            int indexsubject = Array.FindIndex(arraySubject, item => item.Equals(ssubject));

            if (listSchool != null && listSchool.Count > 0)
            {

                foreach (var eSchool in listSchool)
                {
                    var listSeries = eSchool.listAttendance.Select(x => x.ListGenericSchoolData[indexsubject].sPercent.Equals("n/a")? null : (float?)float.Parse(x.ListGenericSchoolData[indexsubject].sPercent)).ToList();
                    //Select(x => float.Parse(x.sPercent) == 0 ? null : (float?)float.Parse(x.sPercent)).ToList()
                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName,
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor],
                        lineWidth = 2,
                        data = listSeries,
                        visible = true
                    });
                    indexColor++;
                }

                eSplineCharts.xAxis.categories = listSchool[0].listAttendance.Select(x => x.YearInfo.year).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = " % " + ssubject };
            }

            eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
            {
                enabled = true
            };

            eSplineCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        //Historical Exclusion Data
        protected List<SPExclusion> GetHistoricalExclusionData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, School school, List<Year> listyear)
        {
            List<SPExclusion> listExclusion = new List<SPExclusion>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>() { new GenericSchoolData("0", "Not Removed From Register"), new GenericSchoolData("1", "Removed From Register") };
            SPExclusion SPExclusion = new SPExclusion();
            GenericSchoolData tempobj = new GenericSchoolData();
            string queryExclusion, querySchoolRoll = "";
            int schoolroll = 0;

            foreach (Year year in listyear)
            {
                if (school.seedcode.Equals("1002"))
                {
                    queryExclusion = "SELECT Year, 1002, Code, sum(Count), sum(Sum)  FROM summary_exclusion where  SchoolType=" + sSchoolType + " and year = " + year.year + " group by Year, Code";
                    querySchoolRoll = "Select Year, sum(Count) from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year;
                }
                else
                {
                    queryExclusion = "SELECT Year, Seedcode, Code, sum(Count), sum(Sum)  FROM summary_exclusion where  SchoolType= " + sSchoolType + "  and year = " + year.year + " and seedcode = " + school.seedcode + " group by Year, Code;";
                    querySchoolRoll = "Select Year, Count from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year + " and seedcode = " + school.seedcode;
                }

                var listResult = rpGeneric2nd.FindByNativeSQL(queryExclusion);
                if (listResult != null)
                {
                    tempdata = new List<GenericSchoolData>();
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tempobj = new GenericSchoolData(itemRow[2].ToString(), itemRow[2].ToString().Equals("0") ? "Not Removed From Register" : "Removed From Register");
                            tempobj.count = Convert.ToInt16(itemRow[3].ToString());
                            tempobj.sum = Convert.ToInt16(itemRow[4].ToString());
                            tempobj.Percent = Convert.ToInt16(itemRow[3].ToString());
                            tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                            tempdata.Add(tempobj);
                        }
                    }
                    var listResultSchoolRoll = rpGeneric2nd.FindByNativeSQL(querySchoolRoll);
                    if (listResultSchoolRoll != null)
                    {
                        foreach (var itemRow in listResultSchoolRoll)
                        {
                            if (itemRow != null)
                            {
                                schoolroll = Convert.ToInt16(itemRow[1].ToString());
                            }
                        }
                    }

                    tempdata.AddRange(foo.Where(x => tempdata.All(p1 => !p1.Code.Equals(x.Code))));
                    SPExclusion = new SPExclusion();
                    SPExclusion.YearInfo = new Year(year.year);
                    tempobj = new GenericSchoolData("2", "Number of days per 1000 pupils lost to exclusions");
                    tempobj.count = tempdata.Sum(x => x.sum);  //Sum length of exclusion
                    tempobj.sum = schoolroll;   //school Roll
                    tempobj.Percent = schoolroll==0? 0.00F: tempobj.count / 2.0F / schoolroll * 1000.0F;
                    tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                    //tempdata.Add(tempobj);
                    SPExclusion.ListGenericSchoolData = new List<GenericSchoolData>() { tempdata.Where(x => x.Code.Equals("1")).First(), tempobj };
                    listExclusion.Add(SPExclusion);
                }

            }



            return listExclusion.OrderBy(x => x.YearInfo.year).ToList();
        }


        // Exclusion Chart
        protected SplineCharts GetChartExclusion(List<SPSchool> listSchool, string dataset) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = dataset;
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            //finding subject index to query data from list
            string[] arraySubject = { "Number of Removals from the Register", "Number of Days Lost Per 1000 Pupils Through Exclusions" };
            int indexsubject = Array.FindIndex(arraySubject, item => item.Equals(dataset));

            if (listSchool != null && listSchool.Count > 0)
            {

                foreach (var eSchool in listSchool)
                {
                    var listSeries = eSchool.listExclusion.Select(x => (float?)float.Parse(x.ListGenericSchoolData[indexsubject].sPercent)).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName,
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor],
                        lineWidth = 2,
                        data = listSeries,
                        visible = true
                    });
                    indexColor++;
                }

                eSplineCharts.xAxis.categories = listSchool[0].listExclusion.Select(x => x.YearInfo.year).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = dataset };
                //eSplineCharts.yAxis.min = 0;
                //eSplineCharts.yAxis.max = 10;
                //eSplineCharts.yAxis.tickInterval = 1;
            }

            eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
            {
                enabled = true
            };

            eSplineCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        //Historical StudentNeed
        protected List<StudentNeed> GetHistoricalStudentNeed(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, SchoolRoll schoolroll, List<Year> listyear)
        {
            StudentNeed StudentNeed = new StudentNeed(); ;
            List<StudentNeed> listStudentNeed = new List<StudentNeed>();

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "needtype");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    StudentNeed = new StudentNeed();
                    StudentNeed.year = year;
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int totalcount = listresult.Where(x => x.code.Equals("02")).Select(x => x.count).Sum();
                    StudentNeed.IEP = new GenericSchoolData()
                    {
                        Code = "02",
                        Name = "IEP",
                        count = totalcount,
                        sum = schoolroll.schoolroll,
                        Percent = schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("01")).Select(x => x.count).Sum();
                    StudentNeed.CSP = new GenericSchoolData()
                    {
                        Code = "01",
                        Name = "CSP",
                        count = totalcount,
                        sum = schoolroll.schoolroll,
                        Percent = schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.00F), 1).ToString()
                    };
                    listStudentNeed.Add(StudentNeed);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    StudentNeed = new StudentNeed();
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    StudentNeed.year = year;
                    int totalcount = listresult.Where(x => x.code.Equals("02")).Select(x => x.count).Sum();
                    StudentNeed.IEP = new GenericSchoolData()
                    {
                        Code = "02",
                        Name = "IEP",
                        count = totalcount,
                        sum = schoolroll.schoolroll,
                        Percent = schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("01")).Select(x => x.count).Sum();
                    StudentNeed.CSP = new GenericSchoolData()
                    {
                        Code = "01",
                        Name = "CSP",
                        count = totalcount,
                        sum = schoolroll.schoolroll,
                        Percent = schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((schoolroll.schoolroll != 0 ? (totalcount * 100.00F / schoolroll.schoolroll) : 0.00F), 1).ToString()
                    };
                    listStudentNeed.Add(StudentNeed);
                }

            }

            return listStudentNeed.OrderBy(x => x.year.year).ToList();
        }

        // StudentNeed Chart
        protected ColumnCharts GetChartStudentNeedIEP(List<SPSchool> listSchool) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Pupils with an IEP ";
            eColumnCharts.yAxis.title.text = "% of pupils with an IEP";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].listStudentNeed.Select(x => x.year.academicyear).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.listStudentNeed.Select(x => (float?)Convert.ToDouble(x.IEP.sPercent)).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }

            eColumnCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };
            eColumnCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // StudentNeed Chart
        protected ColumnCharts GetChartStudentNeedCSP(List<SPSchool> listSchool) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Pupils with a CSP ";
            eColumnCharts.yAxis.title.text = "% of pupils with a CSP";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].listStudentNeed.Select(x => x.year.academicyear).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.listStudentNeed.Select(x => (float?)Convert.ToDouble(x.CSP.sPercent)).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }
            eColumnCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        //Get SchoolRoll data
        protected SPSchoolRollForecast GetSchoolRollForecastData(IGenericRepository2nd rpGeneric2nd, School school)
        {

            SPSchoolRollForecast SchoolRollForecast = new SPSchoolRollForecast();
            List<GenericSchoolData> tempdataActualnumber = new List<GenericSchoolData>();
            List<GenericSchoolData> tempdataForecastnumber = new List<GenericSchoolData>();

            //get actual number 
            var listResult = rpGeneric2nd.FindByNativeSQL("Select * from schoolrollforecast where seedcode = " + school.seedcode);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        for (int i = 3; i <= 17; i++)
                        {
                            if (i <= 10)
                            {
                                tempdataActualnumber.Add(new GenericSchoolData(new Year((i + 2005).ToString()).academicyear, NumberFormatHelper.ConvertObjectToFloat(itemRow[i])));
                                tempdataForecastnumber.Add(new GenericSchoolData(new Year((i + 2005).ToString()).academicyear.ToString(), 0F));

                            }
                            else
                            {
                                tempdataForecastnumber.Add(new GenericSchoolData(new Year((i + 2005).ToString()).academicyear, NumberFormatHelper.ConvertObjectToFloat(itemRow[i])));
                                tempdataActualnumber.Add(new GenericSchoolData(new Year((i + 2005).ToString()).academicyear, 0F));

                            }
                        }
                    }
                }
            }

            SchoolRollForecast.ListActualSchoolRoll = tempdataActualnumber;
            SchoolRollForecast.ListForecastSchoolRoll = tempdataForecastnumber;

            return SchoolRollForecast;
        }
       
        // SchoolRoll Forecast Chart
        protected SplineCharts GetChartSchoolRollForecast(List<SPSchool> listSchool) // query from database and return charts object
        {

            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = " School Roll ";
            eSplineCharts.yAxis.title.text = "Number of Pupils";
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();
            //finding subject index to query data from list

            if (listSchool != null && listSchool.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchool[0].SchoolRollForecast.ListActualSchoolRoll.Select(x => x.Code).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "Number of Pupils" };

                foreach (var eSchool in listSchool)
                {
                    if (!eSchool.SeedCode.Equals("1002"))
                    {
                        var listSeriesActual = eSchool.SchoolRollForecast.ListActualSchoolRoll.Select(x => float.Parse(x.sPercent) == 0 ? null : (float?)float.Parse(x.sPercent)).ToList();

                        eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                        {
                            name = eSchool.SchoolName,
                            color = "#24CBE5",
                            lineWidth = 2,
                            data = listSeriesActual,
                            visible = true
                        });


                        var listSeriesForeCast = eSchool.SchoolRollForecast.ListForecastSchoolRoll.Select(x => float.Parse(x.sPercent) == 0 ? null : (float?)float.Parse(x.sPercent)).ToList();

                        eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                        {
                            name = "Forecast (2015 based)",
                            color = "#f969e8",
                            lineWidth = 2,
                            data = listSeriesForeCast,
                            visible = true
                        });

                    }


                }
            }

            eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
            {
                enabled = true
            };

            eSplineCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };
            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        //Historical StudentStage data
        protected List<StudentStage> GetHistoricalStudentStageData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<StudentStage> listStudentStage = new List<StudentStage>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            StudentStage StudentStage = new StudentStage();

            Dictionary<string, string> DictStage = GetDicStage(rpGeneric2nd, sSchoolType);

            foreach (var item in DictStage)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "stage");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictStage[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    StudentStage = new StudentStage();
                    StudentStage.YearInfo = year;
                    StudentStage.ListGenericSchoolData = groupedList;
                    StudentStage.totalschoolroll = total;
                    listStudentStage.Add(StudentStage);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictStage[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    StudentStage = new StudentStage();
                    StudentStage.YearInfo = year;
                    StudentStage.ListGenericSchoolData = groupedList;
                    StudentStage.totalschoolroll = total;
                    listStudentStage.Add(StudentStage);
                }

            }

            return listStudentStage.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical Free School Meal Registered data
        protected List<FreeSchoolMeal> GetHistoricalFSMDataPrimary(IGenericRepository2nd rpGeneric2nd, string seedcode, List<Year> listyear)
        {
            List<FreeSchoolMeal> listFSM = new List<FreeSchoolMeal>();
            List<GenericSPFSM> tempdata = new List<GenericSPFSM>();
            FreeSchoolMeal SPFSM = new FreeSchoolMeal();
            GenericSPFSM temp;
            //List<Year> listyear = new List<Year>() { new Year("2015"), new Year("2016") };

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    SPFSM = new FreeSchoolMeal();
                    var listResult = rpGeneric2nd.FindByNativeSQL("SELECT year, StudentStage, code, sum(count) FROM accdatastore.summary_fsm where SchoolType=2 and Year = " + year.year + " group by year, StudentStage, code");
                    if (listResult != null)
                    {
                        tempdata = new List<GenericSPFSM>();
                        foreach (var itemRow in listResult)
                        {
                            if (itemRow != null)
                            {
                                temp = new GenericSPFSM(itemRow[1].ToString(), itemRow[2].ToString(), Convert.ToInt16(itemRow[3].ToString()));
                                tempdata.Add(temp);
                            }
                        }
                        int totalschoolroll = tempdata.Select(x => x.count).Sum();

                        if (Convert.ToInt32(year.year)>=2015)
                        {
                            var groupedList = tempdata.Where(x => x.Code.Equals("1") && (x.Studentstage.Equals("P4") || x.Studentstage.Equals("P5") || x.Studentstage.Equals("P6") || x.Studentstage.Equals("P7"))).Select(y => new GenericSPFSM
                            {
                                Code = y.Code,
                                Studentstage = y.Studentstage,
                                count = y.count,
                                sum = totalschoolroll,
                                Percent = (y.count * 100.00F / tempdata.Select(x => x.count).Sum()),
                                sPercent = NumberFormatHelper.FormatNumber((y.count * 100.00F / tempdata.Select(x => x.count).Sum()), 1).ToString()

                            }).ToList();
                            SPFSM.year = year;
                            SPFSM.GenericSchoolData = new GenericSchoolData()
                            {
                                Code = "1",
                                count = groupedList.Select(x => x.count).Sum(),
                                Value = "",
                                sum = totalschoolroll,
                                Name = "Free School Meals Registered",
                                Percent = groupedList.Select(x => x.Percent).Sum(),
                                sPercent = NumberFormatHelper.FormatNumber(groupedList.Select(x => Convert.ToDouble(x.sPercent)).Sum(), 1).ToString()
                            };

                        }else{
                            var groupedList = tempdata.Where(x => x.Code.Equals("1")).Select(y => new GenericSPFSM
                            {
                                Code = y.Code,
                                Studentstage = y.Studentstage,
                                count = y.count,
                                sum = totalschoolroll,
                                Percent = (y.count * 100.00F / tempdata.Select(x => x.count).Sum()),
                                sPercent = NumberFormatHelper.FormatNumber((y.count * 100.00F / tempdata.Select(x => x.count).Sum()), 1).ToString()

                            }).ToList();
                            SPFSM.year = year;
                            SPFSM.GenericSchoolData = new GenericSchoolData()
                            {
                                Code = "1",
                                count = groupedList.Select(x => x.count).Sum(),
                                Value = "",
                                sum = totalschoolroll,
                                Name = "Free School Meals Registered",
                                Percent = groupedList.Select(x => x.Percent).Sum(),
                                sPercent = NumberFormatHelper.FormatNumber(groupedList.Select(x => Convert.ToDouble(x.sPercent)).Sum(), 1).ToString()
                            };
                        }
                    }
                    listFSM.Add(SPFSM);
                }

            }
            else
            {
                foreach (Year year in listyear)
                {

                    SPFSM = new FreeSchoolMeal();
                    var listResult = rpGeneric2nd.FindByNativeSQL("SELECT year, StudentStage, code, sum(count) FROM accdatastore.summary_fsm where SchoolType=2 and Year = " + year.year + " and seedcode = " + seedcode + " group by year, StudentStage, code");
                    if (listResult != null)
                    {
                        tempdata = new List<GenericSPFSM>();
                        foreach (var itemRow in listResult)
                        {
                            if (itemRow != null)
                            {
                                temp = new GenericSPFSM(itemRow[1].ToString(), itemRow[2].ToString(), Convert.ToInt16(itemRow[3].ToString()));
                                tempdata.Add(temp);
                            }
                        }
                        int totalschoolroll = tempdata.Select(x => x.count).Sum();
                        if (Convert.ToInt32(year.year) >= 2015)
                        {
                            var groupedList = tempdata.Where(x => x.Code.Equals("1") && (x.Studentstage.Equals("P4") || x.Studentstage.Equals("P5") || x.Studentstage.Equals("P6") || x.Studentstage.Equals("P7"))).Select(y => new GenericSPFSM
                            {
                                Code = y.Code,
                                Studentstage = y.Studentstage,
                                count = y.count,
                                sum = totalschoolroll,
                                Percent = (y.count * 100.00F / tempdata.Select(x => x.count).Sum()),
                                sPercent = NumberFormatHelper.FormatNumber((y.count * 100.00F / tempdata.Select(x => x.count).Sum()), 1).ToString()

                            }).ToList();
                            SPFSM.year = year;
                            SPFSM.GenericSchoolData = new GenericSchoolData()
                            {
                                Code = "1",
                                count = groupedList.Select(x => x.count).Sum(),
                                Value = "",
                                sum = totalschoolroll,
                                Name = "Free School Meals Registered",
                                Percent = groupedList.Select(x => x.Percent).Sum(),
                                sPercent = NumberFormatHelper.FormatNumber(groupedList.Select(x => Convert.ToDouble(x.sPercent)).Sum(), 1).ToString()
                            };
                        
                        }else{
                            var groupedList = tempdata.Where(x => x.Code.Equals("1")).Select(y => new GenericSPFSM
                            {
                                Code = y.Code,
                                Studentstage = y.Studentstage,
                                count = y.count,
                                sum = totalschoolroll,
                                Percent = (y.count * 100.00F / tempdata.Select(x => x.count).Sum()),
                                sPercent = NumberFormatHelper.FormatNumber((y.count * 100.00F / tempdata.Select(x => x.count).Sum()), 1).ToString()

                            }).ToList();
                            SPFSM.year = year;
                            SPFSM.GenericSchoolData = new GenericSchoolData()
                            {
                                Code = "1",
                                count = groupedList.Select(x => x.count).Sum(),
                                Value = "",
                                sum = totalschoolroll,
                                Name = "Free School Meals Registered",
                                Percent = groupedList.Select(x => x.Percent).Sum(),
                                sPercent = NumberFormatHelper.FormatNumber(groupedList.Select(x => Convert.ToDouble(x.sPercent)).Sum(), 1).ToString()
                            };
                        }




                        

                        //SPFSM.ListGenericSchoolData = groupedList;
                    }
                    listFSM.Add(SPFSM);
                }

            }

            return listFSM.OrderBy(x => x.year.year).ToList();
        }

        //Historical Free School Meal Registered data
        protected List<FreeSchoolMeal> GetHistoricalFSMDataSecondary(IGenericRepository2nd rpGeneric2nd, string seedcode, List<Year> listyear)
        {
            List<FreeSchoolMeal> listFSM = new List<FreeSchoolMeal>();
            List<GenericSPFSM> tempdata = new List<GenericSPFSM>();
            FreeSchoolMeal SPFSM = new FreeSchoolMeal();
            GenericSPFSM temp;

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    SPFSM = new FreeSchoolMeal();
                    var listResult = rpGeneric2nd.FindByNativeSQL("SELECT year, StudentStage, code, sum(count) FROM accdatastore.summary_fsm where SchoolType=3 and Year = " + year.year + " group by year, StudentStage, code");
                    if (listResult != null)
                    {
                        tempdata = new List<GenericSPFSM>();
                        foreach (var itemRow in listResult)
                        {
                            if (itemRow != null)
                            {
                                temp = new GenericSPFSM(itemRow[1].ToString(), itemRow[2].ToString(), Convert.ToInt16(itemRow[3].ToString()));
                                tempdata.Add(temp);
                            }
                        }
                        int totalschoolroll = tempdata.Select(x => x.count).Sum();

                        var groupedList = tempdata.Where(x => x.Code.Equals("1")).Select(y => new GenericSPFSM
                        {
                            Code = y.Code,
                            Studentstage = y.Studentstage,
                            count = y.count,
                            sum = totalschoolroll,
                            Percent = (y.count * 100.00F / tempdata.Select(x => x.count).Sum()),
                            sPercent = NumberFormatHelper.FormatNumber((y.count * 100.00F / tempdata.Select(x => x.count).Sum()), 1).ToString()

                        }).ToList();
                        SPFSM.year = year;
                        SPFSM.GenericSchoolData = new GenericSchoolData()
                        {
                            Code = "1",
                            count = groupedList.Select(x => x.count).Sum(),
                            Value = "",
                            sum = totalschoolroll,
                            Name = "Free School Meals Registered",
                            Percent = groupedList.Select(x => x.Percent).Sum(),
                            sPercent = NumberFormatHelper.FormatNumber(groupedList.Select(x => Convert.ToDouble(x.sPercent)).Sum(), 1).ToString()
                        };
                        //SPFSM.ListGenericSchoolData = groupedList;
                    }
                    listFSM.Add(SPFSM);
                }

            }
            else
            {
                foreach (Year year in listyear)
                {
                    SPFSM = new FreeSchoolMeal();
                    var listResult = rpGeneric2nd.FindByNativeSQL("SELECT year, StudentStage, code, sum(count) FROM accdatastore.summary_fsm where SchoolType=3 and Year = " + year.year + " and seedcode = " + seedcode + " group by year, StudentStage, code");
                    if (listResult != null)
                    {
                        tempdata = new List<GenericSPFSM>();
                        foreach (var itemRow in listResult)
                        {
                            if (itemRow != null)
                            {
                                temp = new GenericSPFSM(itemRow[1].ToString(), itemRow[2].ToString(), Convert.ToInt16(itemRow[3].ToString()));
                                tempdata.Add(temp);
                            }
                        }
                        int totalschoolroll = tempdata.Select(x => x.count).Sum();
                        var groupedList = tempdata.Where(x => x.Code.Equals("1")).Select(y => new GenericSPFSM
                        {
                            Code = y.Code,
                            Studentstage = y.Studentstage,
                            count = y.count,
                            sum = totalschoolroll,
                            Percent = (y.count * 100.00F / tempdata.Select(x => x.count).Sum()),
                            sPercent = NumberFormatHelper.FormatNumber((y.count * 100.00F / tempdata.Select(x => x.count).Sum()), 1).ToString()

                        }).ToList();
                        SPFSM.year = year;
                        SPFSM.GenericSchoolData = new GenericSchoolData()
                        {
                            Code = "1",
                            count = groupedList.Select(x => x.count).Sum(),
                            Value = "",
                            sum = totalschoolroll,
                            Name = "Free School Meals Registered",
                            Percent = groupedList.Select(x => x.Percent).Sum(),
                            sPercent = NumberFormatHelper.FormatNumber(groupedList.Select(x => Convert.ToDouble(x.sPercent)).Sum(), 1).ToString()
                        };

                        //SPFSM.ListGenericSchoolData = groupedList;
                    }
                    listFSM.Add(SPFSM);
                }

            }

            return listFSM.OrderBy(x => x.year.year).ToList();
        }

        //Historical Free School Meal Registered data
        protected List<FreeSchoolMeal> GetHistoricalFSMDataSpecial(IGenericRepository2nd rpGeneric2nd, string seedcode, List<Year> listyear)
        {
            List<FreeSchoolMeal> listFSM = new List<FreeSchoolMeal>();
            List<GenericSPFSM> tempdata = new List<GenericSPFSM>();
            FreeSchoolMeal SPFSM = new FreeSchoolMeal();
            GenericSPFSM temp;

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    SPFSM = new FreeSchoolMeal();
                    var listResult = rpGeneric2nd.FindByNativeSQL("SELECT year, StudentStage, code, sum(count) FROM accdatastore.summary_fsm where SchoolType=4 and Year = " + year.year + " group by year, StudentStage, code");
                    if (listResult != null)
                    {
                        tempdata = new List<GenericSPFSM>();
                        foreach (var itemRow in listResult)
                        {
                            if (itemRow != null)
                            {
                                temp = new GenericSPFSM(itemRow[1].ToString(), itemRow[2].ToString(), Convert.ToInt16(itemRow[3].ToString()));
                                tempdata.Add(temp);
                            }
                        }
                        int totalschoolroll = tempdata.Select(x => x.count).Sum();

                        var groupedList = tempdata.Where(x => x.Code.Equals("1")).Select(y => new GenericSPFSM
                        {
                            Code = y.Code,
                            Studentstage = y.Studentstage,
                            count = y.count,
                            sum = totalschoolroll,
                            Percent = (y.count * 100.00F / tempdata.Select(x => x.count).Sum()),
                            sPercent = NumberFormatHelper.FormatNumber((y.count * 100.00F / tempdata.Select(x => x.count).Sum()), 1).ToString()

                        }).ToList();
                        SPFSM.year = year;
                        SPFSM.GenericSchoolData = new GenericSchoolData()
                        {
                            Code = "1",
                            count = groupedList.Select(x => x.count).Sum(),
                            Value = "",
                            sum = totalschoolroll,
                            Name = "Free School Meals Registered",
                            Percent = groupedList.Select(x => x.Percent).Sum(),
                            sPercent = NumberFormatHelper.FormatNumber(groupedList.Select(x => Convert.ToDouble(x.sPercent)).Sum(), 1).ToString()
                        };
                        //SPFSM.ListGenericSchoolData = groupedList;
                    }
                    listFSM.Add(SPFSM);
                }

            }
            else
            {
                foreach (Year year in listyear)
                {
                    SPFSM = new FreeSchoolMeal();
                    var listResult = rpGeneric2nd.FindByNativeSQL("SELECT year, StudentStage, code, sum(count) FROM accdatastore.summary_fsm where SchoolType=4 and Year = " + year.year + " and seedcode = " + seedcode + " group by year, StudentStage, code");
                    if (listResult != null)
                    {
                        tempdata = new List<GenericSPFSM>();
                        foreach (var itemRow in listResult)
                        {
                            if (itemRow != null)
                            {
                                temp = new GenericSPFSM(itemRow[1].ToString(), itemRow[2].ToString(), Convert.ToInt16(itemRow[3].ToString()));
                                tempdata.Add(temp);
                            }
                        }
                        int totalschoolroll = tempdata.Select(x => x.count).Sum();
                        var groupedList = tempdata.Where(x => x.Code.Equals("1")).Select(y => new GenericSPFSM
                        {
                            Code = y.Code,
                            Studentstage = y.Studentstage,
                            count = y.count,
                            sum = totalschoolroll,
                            Percent = (y.count * 100.00F / tempdata.Select(x => x.count).Sum()),
                            sPercent = NumberFormatHelper.FormatNumber((y.count * 100.00F / tempdata.Select(x => x.count).Sum()), 1).ToString()

                        }).ToList();
                        SPFSM.year = year;
                        SPFSM.GenericSchoolData = new GenericSchoolData()
                        {
                            Code = "1",
                            count = groupedList.Select(x => x.count).Sum(),
                            Value = "",
                            sum = totalschoolroll,
                            Name = "Free School Meals Registered",
                            Percent = groupedList.Select(x => x.Percent).Sum(),
                            sPercent = NumberFormatHelper.FormatNumber(groupedList.Select(x => Convert.ToDouble(x.sPercent)).Sum(), 1).ToString()
                        };

                        //SPFSM.ListGenericSchoolData = groupedList;
                    }
                    listFSM.Add(SPFSM);
                }

            }

            return listFSM.OrderBy(x => x.year.year).ToList();
        }

        protected string[] ExportDataToXLSX(string tablename, List<SPSchool> listSchool)
        {
            var workbook = new XLWorkbook();
            var dtNow = DateTime.Now.ToString("yyyyMMdd_HHmmss");
 
           // var dtResult = ProcessExportDataFormat(listReportData, listDeviceParams, nAggregateType, eUserDataTableHdr);

            var sWorksheetName = "test";
            var worksheet = workbook.Worksheets.Add(sWorksheetName);

            worksheet.Cell(1, 1).Value = "Test";
            //worksheet.Cell(2, 1).Value = eDeviceList.NameWithDesc;
            //worksheet.Cell(3, 1).Value = sDateTimeFrom + " - " + sDateTimeTo + " " + "[" + GetEnumDescription((AGGREGATE_TYPE)nAggregateType) + "]";
            //worksheet.Cell(4, 1).InsertTable(dtResult);

            //var nLastCellColumn = dtResult.Columns.Count;
            //worksheet.Range(1, 1, 1, nLastCellColumn).Merge();
            //worksheet.Range(2, 1, 2, nLastCellColumn).Merge();
            //worksheet.Range(3, 1, 3, nLastCellColumn).Merge();
            //worksheet.Range(4, 1, 4, nLastCellColumn).Merge();

            //worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            worksheet.Tables.FirstOrDefault().ShowAutoFilter = false;
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var sFileName = "test.xlsx";
            var sFilePath = HttpContext.Server.MapPath(@"~\download\" + sFileName);
            workbook.SaveAs(sFilePath);

            return new string[] { "download/" + sFileName, sFileName };
        }

        //private DataTable ProcessExportDataFormat(IList<object[]> listData, IList<DeviceParams> listDeviceParams, int nAggregateType, UserDataTableHdr eUserDataTableHdr)
        //{
        //    var dataResult = new DataTable();
        //    try
        //    {
        //        if (listData != null && listData.Count > 0)
        //        {
        //            var dicDIColumn = new Dictionary<int, DeviceParams>();
        //            if (listDeviceParams != null && listDeviceParams.Count > 0)
        //            {
        //                var dicDeviceParams = Session["SessionDicDeviceParams"] as Dictionary<string, DeviceParams>;
        //                for (var i = 0; i < listDeviceParams.Count + 1; i++)
        //                {
        //                    dataResult.Columns.Add(i == 0 ? "Date Time" : listDeviceParams[i - 1].FldName);
        //                }
        //            }
        //            else
        //            {
        //                foreach (var itemColumn in listData[0])
        //                {
        //                    dataResult.Columns.Add();
        //                }
        //            }
        //            foreach (var oRow in listData)
        //            {
        //                // date time
        //                oRow[0] = ProcessDataFormatDateTime(Convert.ToDateTime(oRow[0]), nAggregateType, eUserDataTableHdr);
        //                dataResult.Rows.Add(oRow);
        //            }
        //        }
        //        else
        //        {
        //            if (listDeviceParams != null && listDeviceParams.Count > 0)
        //            {
        //                for (var i = 0; i < listDeviceParams.Count + 1; i++)
        //                {
        //                    dataResult.Columns.Add(i == 0 ? "Date Time" : listDeviceParams[i - 1].FldName);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex, ex.Message);
        //    }
        //    return dataResult;
        //}
    }
}