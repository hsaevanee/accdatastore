using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class BaseSchoolProfileController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(BaseSchoolProfileController));

        //private readonly IGenericRepository rpGeneric;

        //public BaseSchoolProfileController(IGenericRepository rpGeneric)
        //{
        //    this.rpGeneric = rpGeneric;
        //}

        //public BaseSchoolProfileController()
        //{
        //   // this.rpGeneric = rpGeneric;
        //}

        protected List<EthnicObj> GetEthnicityDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname)
        {
            Console.Write("GetEthnicityData in BaseSchoolProfileController==> ");

            List<EthnicObj> listDataseries = new List<EthnicObj>();
            List<EthnicObj> listtemp = new List<EthnicObj>();
            List<EthnicObj> listtemp1 = new List<EthnicObj>();
            EthnicObj tempEthnicObj = new EthnicObj();

            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select EthnicBackground,Gender,(Count(EthnicBackground)* 100 / (Select Count(*) From test_3))  From test_3  Group By EthnicBackground, Gender ");
            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Ethniccode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Ethniccode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempEthnicObj = new EthnicObj();
                        foreach (var itemRow in templist2)
                        {
                            tempEthnicObj.EthinicCode = Convert.ToString(itemRow[0]);
                            tempEthnicObj.EthinicName = GetDicEhtnicBG().ContainsKey(tempEthnicObj.EthinicCode) ? GetDicEhtnicBG()[tempEthnicObj.EthinicCode] : "NO NAME";

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempEthnicObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempEthnicObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempEthnicObj);
                    }
                }
            }


            //% for specific schoolname
            string query = " Select EthnicBackground,Gender, (Count(EthnicBackground)* 100 /";
            query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            query += " From test_3 where Name in ('" + mSchoolname + " ') Group By EthnicBackground, Gender ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                // need to select only the Ethniccode that appear for this specific school
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                //foreach (var Ethniccode in DistinctItems)
                //{
                //    tempEthnicObj = listtemp.Find(x => x.EthinicCode.Equals(Ethniccode.Key));
                //    if (tempEthnicObj != null)
                //        listDataseries.Add(tempEthnicObj);
                //}


                foreach (var itemRow in listResult)
                {
                    var x = (from a in listtemp where a.EthinicCode.Equals(Convert.ToString(itemRow[0])) select a).ToList();
                    if (x.Count != 0)
                    {
                        tempEthnicObj = x[0];
                        if ("F".Equals(Convert.ToString(itemRow[1])))
                        {
                            tempEthnicObj.PercentageFemaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        else
                        {
                            tempEthnicObj.PercentageMaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        //listDataseries.Add(tempEthnicObj);
                    }
                }
            }

            foreach (var itemRow in listtemp)
            {
                tempEthnicObj = itemRow;
                tempEthnicObj.PercentageInSchool = tempEthnicObj.PercentageFemaleInSchool + tempEthnicObj.PercentageMaleInSchool;
                tempEthnicObj.PercentageAllSchool = tempEthnicObj.PercentageFemaleAllSchool + tempEthnicObj.PercentageMaleAllSchool;
            }

            return listtemp;
        }

        protected Dictionary<string, string> GetDicEhtnicBG()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "White – Scottish");
            dicNational.Add("02", "African – African / Scottish / British");
            dicNational.Add("03", "Caribbean or Black – Caribbean / British / Scottish");
            dicNational.Add("05", "Asian – Indian/British/Scottish");
            dicNational.Add("06", "Asian – Pakistani / British / Scottish");
            dicNational.Add("07", "Asian –Bangladeshi / British / Scottish");
            dicNational.Add("08", "Asian – Chinese / British / Scottish");
            dicNational.Add("09", "White – Other");
            dicNational.Add("10", "Not Disclosed");
            dicNational.Add("12", "Mixed or multiple ethnic groups");
            dicNational.Add("17", "Asian – Other");
            dicNational.Add("19", "White – Gypsy/Traveller");
            dicNational.Add("21", "White – Other British");
            dicNational.Add("22", "White – Irish");
            dicNational.Add("23", "White – Polish");
            dicNational.Add("24", "Caribbean or Black – Other");
            dicNational.Add("25", "African – Other");
            dicNational.Add("27", "Other – Arab");
            dicNational.Add("98", "Not Known");
            dicNational.Add("99", "Other – Other");
            return dicNational;
        }

        protected Dictionary<string, string> GetDicGender()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("F", "Female");
            dicNational.Add("M", "Male");
            return dicNational;
        }

        protected Dictionary<string, string[]> GetDicGenderWithSelected(List<string> listSelectedGender)
        {
            var dicNational = new Dictionary<string, string[]>();
            if (listSelectedGender != null)
            {
                if (listSelectedGender.FirstOrDefault(x => x.Equals("F")) != null)
                {
                    dicNational.Add("F", new string[] { "Female", "checked" });
                }
                else
                {
                    dicNational.Add("F", new string[] { "Female", "" });
                }
                if (listSelectedGender.FirstOrDefault(x => x.Equals("M")) != null)
                {
                    dicNational.Add("M", new string[] { "Male", "checked" });
                }
                else
                {
                    dicNational.Add("M", new string[] { "Male", "" });
                }
            }
            else
            {
                dicNational.Add("F", new string[] { "Female", "checked" });
                dicNational.Add("M", new string[] { "Male", "checked" });
            }
            return dicNational;
        }

        protected List<NationalityObj> GetNationalityDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname)
        {
            Console.Write("GetNationalityData ==> ");

            List<NationalityObj> listDataseries = new List<NationalityObj>();
            List<NationalityObj> listtemp = new List<NationalityObj>();
            NationalityObj tempNationalObj = new NationalityObj();


            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select NationalIdentity,Gender, (Count(NationalIdentity)* 100 / (Select Count(*) From test_3))  From test_3  Group By NationalIdentity, Gender ");
            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        tempNationalObj = new NationalityObj();
            //        tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
            //        tempNationalObj.IdentityName = GetDicNational().ContainsKey(tempNationalObj.IdentityCode) ? GetDicNational()[tempNationalObj.IdentityCode] : "NO NAME";
            //        tempNationalObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);
            //        listtemp.Add(tempNationalObj);
            //    }
            //}
            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Ethniccode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Ethniccode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempNationalObj = new NationalityObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
                            tempNationalObj.IdentityName = GetDicEhtnicBG().ContainsKey(tempNationalObj.IdentityCode) ? GetDicEhtnicBG()[tempNationalObj.IdentityCode] : "NO NAME";

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempNationalObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempNationalObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempNationalObj);
                    }
                }
            }

            //% for specific schoolname
            string query = " Select NationalIdentity, Gender, (Count(NationalIdentity)* 100 /";
            query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            query += " From test_3 where Name in ('" + mSchoolname + " ') Group By NationalIdentity, Gender ";

            listResult = rpGeneric.FindByNativeSQL(query);

            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        tempNationalObj = listtemp.Find(x => x.IdentityCode.Equals(Convert.ToString(itemRow[0])));
            //        tempNationalObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);

            //        listDataseries.Add(tempNationalObj);

            //    }
            //}


            //return listDataseries;
            if (listResult != null)
            {
                // need to select only the Ethniccode that appear for this specific school
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Ethniccode in DistinctItems)
                {
                    tempNationalObj = listtemp.Find(x => x.IdentityCode.Equals(Ethniccode.Key));
                    if (tempNationalObj != null)
                        listDataseries.Add(tempNationalObj);
                }


                foreach (var itemRow in listResult)
                {
                    var x = (from a in listtemp where a.IdentityCode.Equals(Convert.ToString(itemRow[0])) select a).ToList();
                    if (x.Count != 0)
                    {
                        tempNationalObj = x[0];
                        if ("F".Equals(Convert.ToString(itemRow[1])))
                        {
                            tempNationalObj.PercentageFemaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        else
                        {
                            tempNationalObj.PercentageMaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        //listDataseries.Add(tempEthnicObj);
                    }
                }
            }

            foreach (var itemRow in listDataseries)
            {
                tempNationalObj = itemRow;
                tempNationalObj.PercentageInSchool = tempNationalObj.PercentageFemaleInSchool + tempNationalObj.PercentageMaleInSchool;
                tempNationalObj.PercentageAllSchool = tempNationalObj.PercentageFemaleAllSchool + tempNationalObj.PercentageMaleAllSchool;
            }

            return listDataseries;
        }

        protected Dictionary<string, string> GetDicNational()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "Scottish");
            dicNational.Add("02", "English");
            dicNational.Add("03", "Northern Irish");
            dicNational.Add("04", "Welsh");
            dicNational.Add("05", "British");
            dicNational.Add("99", "Other");
            dicNational.Add("10", "Not Disclosed");
            dicNational.Add("98", "Not Known");
            return dicNational;
        }

        protected List<SIMDObj> GetSIMDDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname, List<string> myear)
        {
            Console.Write("GetSIMDDatabySchoolname in BaseSchoolProfileController ==> ");

            List<SIMDObj> listDataseries = new List<SIMDObj>();
            List<SIMDObj> listtemp = new List<SIMDObj>();
            SIMDObj tempSIMDObj = new SIMDObj();
            
            //should loop through myear

            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select SIMD_2012_decile, (Count(SIMD_2012_decile)* 100 / (Select Count(*) From test_3))  From test_3  Group By SIMD_2012_decile ");
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempSIMDObj = new SIMDObj();
                    tempSIMDObj.SIMDCode = Convert.ToString(itemRow[0]);
                    tempSIMDObj.PercentageAllSchool2012 = Convert.ToDouble(itemRow[1]);
                    listtemp.Add(tempSIMDObj);
                }
            }

            listResult = rpGeneric.FindByNativeSQL("Select SIMD_2009_decile, (Count(SIMD_2009_decile)* 100 / (Select Count(*) From test_3))  From test_3  Group By SIMD_2009_decile ");
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempSIMDObj = listtemp.Find(x => x.SIMDCode.Equals(Convert.ToString(itemRow[0])));
                    tempSIMDObj.PercentageAllSchool2009 = Convert.ToDouble(itemRow[1]);
                }
            }

            string query = " Select SIMD_2012_decile, (Count(SIMD_2012_decile)* 100 /";
            query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            query += " From test_3 where Name in ('" + mSchoolname + " ') Group By SIMD_2012_decile ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempSIMDObj = listtemp.Find(x => x.SIMDCode.Equals(Convert.ToString(itemRow[0])));
                    tempSIMDObj.PercentageInSchool2012 = Convert.ToDouble(itemRow[1]);



                }
            }

            query = " Select SIMD_2009_decile, (Count(SIMD_2009_decile)* 100 /";
            query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            query += " From test_3 where Name in ('" + mSchoolname + " ') Group By SIMD_2009_decile ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempSIMDObj = listtemp.Find(x => x.SIMDCode.Equals(Convert.ToString(itemRow[0])));
                    tempSIMDObj.PercentageInSchool2009 = Convert.ToDouble(itemRow[1]);



                }
            }







            //% for All school
            //var listResult = rpGeneric.FindByNativeSQL("Select SIMD_2012_decile, (Count(SIMD_2012_decile)* 100 / (Select Count(*) From test_3))  From test_3  Group By SIMD_2012_decile ");
            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        tempSIMDObj = new SIMDObj();
            //        tempSIMDObj.SIMDCode = Convert.ToString(itemRow[0]);
            //        tempSIMDObj.PercentageAllSchool2012 = Convert.ToDouble(itemRow[1]);
            //        listtemp.Add(tempSIMDObj);
            //    }
            //}


            ////% for specific schoolname
            //string query = " Select SIMD_2012_decile, (Count(SIMD_2012_decile)* 100 /";
            //query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            //query += " From test_3 where Name in ('" + mSchoolname + " ') Group By SIMD_2012_decile ";

            //listResult = rpGeneric.FindByNativeSQL(query);
            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        tempSIMDObj = listtemp.Find(x => x.SIMDCode.Equals(Convert.ToString(itemRow[0])));
            //        tempSIMDObj.PercentageInSchool2012 = Convert.ToDouble(itemRow[1]);

            //        listDataseries.Add(tempSIMDObj);

            //    }
            //}
            return listtemp;
        }

        protected List<StdStageObj> GetStudentStageDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname)
        {
            Console.Write("GetStdStageDatabySchoolname in BaseSchoolProfileController==> ");

            List<StdStageObj> listDataseries = new List<StdStageObj>();
            List<StdStageObj> listtemp = new List<StdStageObj>();
            List<StdStageObj> listtemp1 = new List<StdStageObj>();
            StdStageObj tempStdStageObj = new StdStageObj();

            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select StudentStage,Gender,(Count(StudentStage)* 100 / (Select Count(*) From test_3))  From test_3  Group By StudentStage, Gender ");
            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var StdStagecode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(StdStagecode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempStdStageObj = new StdStageObj();
                        foreach (var itemRow in templist2)
                        {
                            tempStdStageObj.StageCode = Convert.ToString(itemRow[0]);                            

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempStdStageObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempStdStageObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempStdStageObj);
                    }
                }
            }


            //% for specific schoolname
            string query = " Select StudentStage,Gender, (Count(StudentStage)* 100 /";
            query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            query += " From test_3 where Name in ('" + mSchoolname + " ') Group By StudentStage, Gender ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                // need to select only the Ethniccode that appear for this specific school
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var StdStagecode in DistinctItems)
                {
                    tempStdStageObj = listtemp.Find(x => x.StageCode.Equals(StdStagecode.Key));
                    if (tempStdStageObj != null)
                        listDataseries.Add(tempStdStageObj);
                }


                foreach (var itemRow in listResult)
                {
                    var x = (from a in listtemp where a.StageCode.Equals(Convert.ToString(itemRow[0])) select a).ToList();
                    if (x.Count != 0)
                    {
                        tempStdStageObj = x[0];
                        if ("F".Equals(Convert.ToString(itemRow[1])))
                        {
                            tempStdStageObj.PercentageFemaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        else
                        {
                            tempStdStageObj.PercentageMaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        //listDataseries.Add(tempEthnicObj);
                    }
                }
            }

            foreach (var itemRow in listDataseries)
            {
                tempStdStageObj = itemRow;
                tempStdStageObj.PercentageInSchool = tempStdStageObj.PercentageFemaleInSchool + tempStdStageObj.PercentageMaleInSchool;
                tempStdStageObj.PercentageAllSchool = tempStdStageObj.PercentageFemaleAllSchool + tempStdStageObj.PercentageMaleAllSchool;
            }

            return listDataseries;
        }


    }
}