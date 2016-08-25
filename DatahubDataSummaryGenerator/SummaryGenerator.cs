using ACCDataStore;
using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatahubDataSummaryGenerator
{
    class SummaryGenerator
    {
        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(
                        c => c.FromConnectionStringWithKey("ConnectionString")
                    )
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.DatahubProfile.AberdeenSummaryMap>())
                .BuildSessionFactory();
        }

        private static void SaveRowForEntity(ISession session, object entity)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    session.Save(entity);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    transaction.Rollback();
                    throw;
                }
            }
        }

        protected static AberdeenSummary CalculateSummaryData(IList<DatahubDataObj> studentData, string datacode, string name, string type, int month, int year)
        {
            // TODO: remove year month and work only with subsetStudent for a given year and month
            // studentData = studentData.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();
            
            AberdeenSummary summaryData = new AberdeenSummary();
            
            summaryData.name = name;
            summaryData.dataCode = datacode;
            summaryData.dataMonth = month;
            summaryData.dataYear = year;
            summaryData.type = type;

            summaryData.movedOutScotlandFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals("female"));
            summaryData.movedOutScotlandMale = studentData.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals("male"));
            summaryData.movedOutScotlandUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("moved outwith scotland") && x.Gender.ToLower().Equals("information not yet obtained"));

            studentData = studentData.Where(x => !(x.Current_Status.ToLower().Equals("moved outwith scotland"))).ToList();

            summaryData.allFemale = studentData.Count(x => x.Gender.ToLower().Equals("female"));
            summaryData.allMale = studentData.Count(x => x.Gender.ToLower().Equals("male"));
            summaryData.allUnspecified = studentData.Count(x => x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all15Female = studentData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("female"));
            summaryData.all15Male = studentData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("male"));
            summaryData.all15Unspecified = studentData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all16Female = studentData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("female"));
            summaryData.all16Male = studentData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("male"));
            summaryData.all16Unspecified = studentData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all17Female = studentData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("female"));
            summaryData.all17Male = studentData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("male"));
            summaryData.all17Unspecified = studentData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all18Female = studentData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("female"));
            summaryData.all18Male = studentData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("male"));
            summaryData.all18Unspecified = studentData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all19Female = studentData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("female"));
            summaryData.all19Male = studentData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("male"));
            summaryData.all19Unspecified = studentData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.activityAgreementFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("female"));
            summaryData.activityAgreementMale = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("male"));
            summaryData.activityAgreementUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.schoolFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("female"));
            summaryData.schoolMale = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("male"));
            summaryData.schoolUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.schoolInTransitionFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("female"));
            summaryData.schoolInTransitionMale = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("male"));
            summaryData.schoolInTransitionUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.activityAgreementFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("female"));
            summaryData.activityAgreementMale = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("male"));
            summaryData.activityAgreementUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage2Female = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage2Male = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage2Unspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage3Female = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage3Male = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage3Unspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage4Female = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage4Male = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage4Unspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fullTimeEmploymentFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("female"));
            summaryData.fullTimeEmploymentMale = studentData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("male"));
            summaryData.fullTimeEmploymentUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.furtherEducationFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("female"));
            summaryData.furtherEducationMale = studentData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("male"));
            summaryData.furtherEducationUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.higherEducationFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("female"));
            summaryData.higherEducationMale = studentData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("male"));
            summaryData.higherEducationUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.modernApprenticeshipFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("female"));
            summaryData.modernApprenticeshipMale = studentData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("male"));
            summaryData.modernApprenticeshipUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.parttimeEmploymentFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("female"));
            summaryData.parttimeEmploymentMale = studentData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("male"));
            summaryData.parttimeEmploymentUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.personalDevelopmentFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("female"));
            summaryData.personalDevelopmentMale = studentData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("male"));
            summaryData.personalDevelopmentUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.selfEmployedFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("female"));
            summaryData.selfEmployedMale = studentData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("male"));
            summaryData.selfEmployedUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.trainingFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("female"));
            summaryData.trainingMale = studentData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("male"));
            summaryData.trainingUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.voluntaryWorkFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("female"));
            summaryData.voluntaryWorkMale = studentData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("male"));
            summaryData.voluntaryWorkUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.custodyFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("female"));
            summaryData.custodyMale = studentData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("male"));
            summaryData.custodyUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.economicallyInactiveFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("female"));
            summaryData.economicallyInactiveMale = studentData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("male"));
            summaryData.economicallyInactiveUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.illHealthFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("female"));
            summaryData.illHealthMale = studentData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("male"));
            summaryData.illHealthUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.unemployedFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("female"));
            summaryData.unemployedMale = studentData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("male"));
            summaryData.unemployedUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("information not yet obtained"));


            summaryData.unknownFemale = studentData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("female"));
            summaryData.unknownMale = studentData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("male"));
            summaryData.unknownUnspecified = studentData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("information not yet obtained"));

            var temp = studentData.Where(x => x.Weeks_since_last_Pos_Status != null).DefaultIfEmpty().ToList();
            summaryData.AvgWeek = temp.First() == null ? 0.0 : temp.Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status ?? "0"));

            return summaryData;
        }

        static void Main(string[] args)
        {
            var sessionFactory = CreateSessionFactory();
            using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    //var str = session.Get<DataZoneObj>("S01000001").Reference_Parent;
                    //// insert magic code
                    //IList<DatahubDataObj> datahubStudentData = session.QueryOver<DatahubDataObj>().Where(x => x.Data_Month == 08 && x.Data_Year == 2016 && x.Current_Status != "Moved Outwith Scotland").List<DatahubDataObj>();
                    ////datahubStudentData = datahubStudentData.Where(x => );
                    //var temp = CalculateSummaryData(datahubStudentData, "S12000033", "Aberdeen", 1, 08, 2016);
                    //foreach (var property in temp.GetType().GetProperties())
                    //{
                    //    Console.WriteLine(property.Name + " : " + property.GetValue(temp).ToString());
                    //}
                    //Console.WriteLine();

                    IList<DatahubDataObj> datahubStudentDataAllPeriods = session.QueryOver<DatahubDataObj>().List<DatahubDataObj>();
                    IList<DatahubDataObj> result = getSubsetStudentsByZone(session, datahubStudentDataAllPeriods, "intermediate zone","S02000024", 08, 2016);
                    IList<DatahubDataObj> result2 = getSubsetStudentsByZone(session, datahubStudentDataAllPeriods, "data zone", "S01000011", 08, 2016);

                    //CreateOneMonthEntry(session, datahubStudentDataAllPeriods, 08, 2016);
                    //var newssss = session.CreateSQLQuery("SELECT DISTINCT `accdatastore`.`datahubdata_aberdeen`.`Data_Month`, `accdatastore`.`datahubdata_aberdeen`.`Data_Year` from `accdatastore`.`datahubdata_aberdeen` WHERE not(Current_Status = 'Moved Outwith Scotland')");
                    //var nezzzz = datahubStudentDataAllPeriods.Select(x => new { x.Data_Month, x.Data_Year }).Distinct().ToList();
                    //IList<monthYear> monthYearData = datahubStudentDataAllPeriods.Select(x => new monthYear { month = x.Data_Month, Data_Year = x.Data_Year }).Distinct().ToList();

                    //var monthList = datahubStudentData.Select(x => x.Data_Month).Distinct().ToList();
                    //var yearList = datahubStudentData.Select(x => x.Data_Year).Distinct().ToList();
                    //IList<monthYear> monthYearData = new Collection<monthYear>();
                    //foreach (var item in monthList)
                    //{
                    //    monthYearData.Add(new monthYear { month = item, year = yearList[monthList.IndexOf(item)] });
                    //}
                    Console.WriteLine();
                }
            }
            Console.WriteLine("Operation completed.");
            Console.ReadKey();
        }

        private static IList<DatahubDataObj> getSubsetStudentsByCouncil (IList<DatahubDataObj> allStudents, int month, int year)
        {
            IList<DatahubDataObj> subsetStudents = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();
            return subsetStudents;
        }

        private static IList<DatahubDataObj> getSubsetStudentsBySchool (IList<DatahubDataObj> allStudents, string seedCode, int month, int year)
        {
            IList<DatahubDataObj> subsetStudents = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year && x.SEED_Code != null && x.SEED_Code.Equals(seedCode)).ToList();
            return subsetStudents;
        }

        private static IList<DatahubDataObj> getSubsetStudentsByZone (ISession session, IList<DatahubDataObj> allStudents, string zonetype, string zonecode, int month, int year)
        {
            // This method retrieves a subset of Datahub Student Data by their corresponding neighbourhood and a specific period in time (month, year)
            // We first fetch all the post codes that correspond to that neighbourhood
            // Then we filther the Datahub Student Data list by the specific period (month, year)
            // Finally we join the two lists and return a list of Datahub Student Data

            IList<NeighbourhoodObj> postCodes = new Collection<NeighbourhoodObj>();
            IList<DatahubDataObj> subsetStudents = new Collection<DatahubDataObj>();

            switch (zonetype.ToLower())
            {
                case "intermediate zone":
                    postCodes = session.QueryOver<NeighbourhoodObj>().Where(x => x.IntDataZone == zonecode).List();
                    break;
                case "data zone":
                    postCodes = session.QueryOver<NeighbourhoodObj>().Where(x => x.DataZone == zonecode).List();
                    break;
                default:
                    postCodes = null;
                    break;
            }

            try
            {
                IList<DatahubDataObj> subsetStudentsThisPeriod = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();
                subsetStudents = (from s in subsetStudentsThisPeriod
                                    join p in postCodes
                                    on s.CSS_Postcode equals p.CSS_Postcode
                                    select s).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            
            return subsetStudents;
        }

        //private static IList<DatahubDataObj> getSubsetStudents(ISession session, IList<DatahubDataObj> allStudents, string subsetType, int month, int year)
        //{
        //    IList<DatahubDataObj> subsetStudents = new Collection<DatahubDataObj>();

        //    switch (subsetType.ToLower())
        //    {
        //        case "council":
        //            subsetStudents = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();
        //            break;
        //        case "intermediate zone":
        //            break;
        //        case "data zone":
        //            break;
        //        case "school":
        //            subsetStudents = allStudents.Where(x => x.Data_Month == month && x.Data_Year == year && x.SEED_Code!=null && x.SEED_Code).ToList();
        //            break;
        //    }

        //    return subsetStudents;
        //}

        protected static void CreateOneMonthEntry(ISession session, IList<DatahubDataObj> allStudents, string type, int month, int year)
        {
            IList<DatahubDataObj> subsetStudents = new Collection<DatahubDataObj>();

            switch (type.ToLower())
            {
                case "council":
                    break;
                case "intermediate zone":
                    break;
                case "data zone":
                    break;
                case "school":
                    break;
            }

            //List<SummaryData> SummaryEntries = new List<SummaryData>();       
            IList<AllSchools> AllSchools = session.QueryOver<AllSchools>().List<AllSchools>();
            foreach (AllSchools school in AllSchools)
            {
                IList<DatahubDataObj> allStudentsThisSchool = allStudents.Where(x => x.SEED_Code!=null && x.SEED_Code.Equals(school.seedCode)).ToList();
                AberdeenSummary currentSummary = CalculateSummaryData(allStudentsThisSchool, school.seedCode, school.name, "School", month, year);
                //SaveRowForEntity(session, currentSummary);
            }
            
            //foreach (var property in SummaryEntries[0].GetType().GetProperties())
            //{
            //    Console.WriteLine(property.Name + " : " + property.GetValue(SummaryEntries[0]).ToString());
            //}
        }
    }

    public class monthYear
    {
        public int month { get; set; }
        public int year { get; set; }
    }

    
}
