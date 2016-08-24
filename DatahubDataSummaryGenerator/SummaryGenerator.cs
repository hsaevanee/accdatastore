using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatahubDataSummaryGenerator
{
    class SummaryGenerator
    {
        private static ISessionFactory CreateSessionFactory()
        {
            //var connectionString = "server=localhost;user id=root;password=toor;persist security info=True;database=simd";
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(
                        c => c.FromConnectionStringWithKey("ConnectionString")
                    )
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.BaseEntity>())
                .BuildSessionFactory();
        }

        protected static SummaryData CalculateSummaryData(IList<DatahubDataObj> tableData, string datacode, string name, int type, int month, int year)
        {
            SummaryData summaryData = new SummaryData();
            summaryData.name = name;
            summaryData.dataCode = datacode;
            summaryData.dataMonth = month;
            summaryData.dataYear = year;
            summaryData.type = type;

            summaryData.allFemale = tableData.Count(x => x.Gender.ToLower().Equals("female"));
            summaryData.allMale = tableData.Count(x =>  x.Gender.ToLower().Equals("male"));
            summaryData.allUnspecified = tableData.Count(x =>  x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all15Female = tableData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("female"));
            summaryData.all15Male = tableData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("male"));
            summaryData.all15Unspecified = tableData.Count(x => x.Age == 15 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all16Female = tableData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("female"));
            summaryData.all16Male = tableData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("male"));
            summaryData.all16Unspecified = tableData.Count(x => x.Age == 16 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all17Female = tableData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("female"));
            summaryData.all17Male = tableData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("male"));
            summaryData.all17Unspecified = tableData.Count(x => x.Age == 17 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all18Female = tableData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("female"));
            summaryData.all18Male = tableData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("male"));
            summaryData.all18Unspecified = tableData.Count(x => x.Age == 18 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.all19Female = tableData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("female"));
            summaryData.all19Male = tableData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("male"));
            summaryData.all19Unspecified = tableData.Count(x => x.Age == 19 && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.activityAgreementFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("female"));
            summaryData.activityAgreementMale = tableData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("male"));
            summaryData.activityAgreementUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.schoolFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("female"));
            summaryData.schoolMale = tableData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("male"));
            summaryData.schoolUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("school pupil") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.schoolInTransitionFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("female"));
            summaryData.schoolInTransitionMale = tableData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("male"));
            summaryData.schoolInTransitionUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("school pupil - in transition") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.activityAgreementFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("female"));
            summaryData.activityAgreementMale = tableData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("male"));
            summaryData.activityAgreementUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("activity agreement") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage2Female = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage2Male = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage2Unspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 2") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage3Female = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage3Male = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage3Unspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 3") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fundStage4Female = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("female"));
            summaryData.fundStage4Male = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("male"));
            summaryData.fundStage4Unspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("employability fund stage 4") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.fullTimeEmploymentFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("female"));
            summaryData.fullTimeEmploymentMale = tableData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("male"));
            summaryData.fullTimeEmploymentUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("full-time employment") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.furtherEducationFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("female"));
            summaryData.furtherEducationMale = tableData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("male"));
            summaryData.furtherEducationUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("further education") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.higherEducationFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("female"));
            summaryData.higherEducationMale = tableData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("male"));
            summaryData.higherEducationUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("higher education") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.modernApprenticeshipFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("female"));
            summaryData.modernApprenticeshipMale = tableData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("male"));
            summaryData.modernApprenticeshipUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("modern apprenticeship") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.parttimeEmploymentFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("female"));
            summaryData.parttimeEmploymentMale = tableData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("male"));
            summaryData.parttimeEmploymentUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("part-time employment") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.personalDevelopmentFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("female"));
            summaryData.personalDevelopmentMale = tableData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("male"));
            summaryData.personalDevelopmentUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("personal/ skills development") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.selfEmployedFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("female"));
            summaryData.selfEmployedMale = tableData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("male"));
            summaryData.selfEmployedUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("self-employed") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.trainingFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("female"));
            summaryData.trainingMale = tableData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("male"));
            summaryData.trainingUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("training (non ntp)") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.voluntaryWorkFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("female"));
            summaryData.voluntaryWorkMale = tableData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("male"));
            summaryData.voluntaryWorkUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("voluntary work") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.custodyFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("female"));
            summaryData.custodyMale = tableData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("male"));
            summaryData.custodyUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("custody") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.economicallyInactiveFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("female"));
            summaryData.economicallyInactiveMale = tableData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("male"));
            summaryData.economicallyInactiveUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("economically inactive") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.illHealthFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("female"));
            summaryData.illHealthMale = tableData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("male"));
            summaryData.illHealthUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("unavailable - ill health") && x.Gender.ToLower().Equals("information not yet obtained"));

            summaryData.unemployedFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("female"));
            summaryData.unemployedMale = tableData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("male"));
            summaryData.unemployedUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("unemployed") && x.Gender.ToLower().Equals("information not yet obtained"));


            summaryData.unknownFemale = tableData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("female"));
            summaryData.unknownMale = tableData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("male"));
            summaryData.unknownUnspecified = tableData.Count(x => x.Current_Status.ToLower().Equals("unknown") && x.Gender.ToLower().Equals("information not yet obtained"));

            var temp = tableData.Where(x => x.Weeks_since_last_Pos_Status != null).DefaultIfEmpty().ToList();
            summaryData.AvgWeek = temp.First() == null ? 0.0 : temp.Average(x => Convert.ToInt16(x.Weeks_since_last_Pos_Status ?? "0"));

            return summaryData;
        }
        static void Main(string[] args)
        {
            var sessionFactory = CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    var str = session.Get<DataZoneObj>("S01000001").Reference_Parent;
                    // insert magic code
                    IList<DatahubDataObj> datahubStudentData = session.QueryOver<DatahubDataObj>().Where(x => x.Data_Month == 08 && x.Data_Year == 2016).List<DatahubDataObj>();
                    var temp = CalculateSummaryData(datahubStudentData, "S12000033", "Aberdeen", 1, 08, 2016);
                    
                    
                    foreach (var property in temp.GetType().GetProperties())
                    {
                        Console.WriteLine(property.Name + " : " + property.GetValue(temp).ToString());
                    }                    
                    Console.WriteLine();
                }
            }
            Console.WriteLine("rdy");
            Console.ReadKey();
        }
    }
}
