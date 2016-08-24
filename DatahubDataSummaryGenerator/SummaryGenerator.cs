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
            //var connectionString = "server=localhost;user id=root;password=toor;persist security info=True;database=simd";
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(
                        c => c.FromConnectionStringWithKey("ConnectionString")
                    )
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SummaryGenerator>())
                .BuildSessionFactory();
        }

        protected static SummaryData CalculateSummaryData(IList<DatahubDataObj> tableData, string datacode, string name, int type, int month, int year)
        {
            tableData = tableData.Where(x => x.Data_Month == month && x.Data_Year == year).ToList();

            SummaryData summaryData = new SummaryData();
            summaryData.name = name;
            summaryData.dataCode = datacode;
            summaryData.dataMonth = month;
            summaryData.dataYear = year;
            summaryData.type = type;

            summaryData.allFemale = tableData.Count(x => x.Gender.ToLower().Equals("female"));
            summaryData.allMale = tableData.Count(x => x.Gender.ToLower().Equals("male"));
            summaryData.allUnspecified = tableData.Count(x => x.Gender.ToLower().Equals("information not yet obtained"));

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
                    CreateOneMonthEntry(session, datahubStudentDataAllPeriods, 08, 2016);
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
            Console.WriteLine("rdy");
            Console.ReadKey();
        }

        protected static void CreateOneMonthEntry(ISession session, IList<DatahubDataObj> allStudents, int month, int year)
        {

            List<SummaryData> SummaryEntries = new List<SummaryData>();       
            IList<AllCitySchools> AllSchools = session.QueryOver<AllCitySchools>().List<AllCitySchools>();
            foreach (AllCitySchools school in AllSchools)
            {
                SummaryEntries.Add(CalculateSummaryData(allStudents, school.seedCode, school.name, 4, month, year));
            }

            foreach (var property in SummaryEntries[0].GetType().GetProperties())
            {
                Console.WriteLine(property.Name + " : " + property.GetValue(SummaryEntries[0]).ToString());
            }

            //foreach (SummaryData entry in SummaryEntries)
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        session.Save(entry);
            //        transaction.Commit();
            //    }
            //}
        }
    }

    public class monthYear
    {
        public int Data_Month { get; set; }
        public int Data_Year { get; set; }
    }

    public class DatahubDataMap : ClassMap<DatahubDataObj>
    {
        public DatahubDataMap()
        {
            Table("datahubdata_aberdeen");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
            Map(x => x.Date_of_Birth);
            Map(x => x.Age);
            Map(x => x.Gender);
            Map(x => x.SDS_Client_Ref);
            Map(x => x.Scottish_Candidate_Number);
            Map(x => x.Statutory_Leave_Date);
            Map(x => x.SEED_Code);
            Map(x => x.School_Name);
            Map(x => x.School_MIS_Reference);
            Map(x => x.Start_Date);
            Map(x => x.Anticipated_School_Leaving_Date);
            Map(x => x.Actual_Date_Left_School);
            Map(x => x.School_Year_Group);
            Map(x => x.School_Roll_Status_Code);
            Map(x => x.School_History_Source);
            Map(x => x.Preferred_Occupation);
            Map(x => x.Preferred_Occupation_Source);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
            Map(x => x.Current_Status);
            Map(x => x.Status_Source);
            Map(x => x.Conditional_Status);
            Map(x => x.Status_Start_Date);
            Map(x => x.Organisation_Name);
            Map(x => x.Course_Title);
            Map(x => x.Course_Level);
            Map(x => x.Employer_Name);
            Map(x => x.Job_Title);
            Map(x => x.End_Date);
            Map(x => x.Weeks_since_last_Pos_Status);
            Map(x => x.Last_Positive_Status);
            Map(x => x.Last_Engagement_with_SDS);
            Map(x => x.Benefit_Types);
            Map(x => x.Benefit_Source);
            Map(x => x.Looked_After_Status);
            Map(x => x.Looked_After_Source);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN);
            Map(x => x.ASN_Source);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP);
            Map(x => x.CSP_Source);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Childs_Plan_Source);
            Map(x => x.Data_Month);
            Map(x => x.Data_Year);
        }
    }

    public class DatahubDataObj
    {
        public virtual int Id { get; set; }
        public virtual string Cohort { get; set; }
        public virtual string Forename { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Preferred_Forename { get; set; }
        public virtual string CSS_Address { get; set; }
        public virtual string CSS_Postcode { get; set; }
        public virtual string LA_Address { get; set; }
        public virtual string LA_Postcode { get; set; }
        public virtual string Telephone_Number { get; set; }
        public virtual DateTime Date_of_Birth { get; set; }
        public virtual int Age { get; set; }
        public virtual string Gender { get; set; }
        public virtual string SDS_Client_Ref { get; set; }
        public virtual string Scottish_Candidate_Number { get; set; }
        public virtual DateTime Statutory_Leave_Date { get; set; }
        public virtual string SEED_Code { get; set; }
        public virtual string School_Name { get; set; }
        public virtual string School_MIS_Reference { get; set; }
        public virtual DateTime Start_Date { get; set; }
        public virtual DateTime Anticipated_School_Leaving_Date { get; set; }
        public virtual DateTime Actual_Date_Left_School { get; set; }
        public virtual string School_Year_Group { get; set; }
        public virtual string School_Roll_Status_Code { get; set; }
        public virtual string School_History_Source { get; set; }
        public virtual string Preferred_Occupation { get; set; }
        public virtual string Preferred_Occupation_Source { get; set; }
        public virtual string Preferred_Route { get; set; }
        public virtual string Preferred_Route_Source { get; set; }
        public virtual string Current_Status { get; set; }
        public virtual string Status_Source { get; set; }
        public virtual string Conditional_Status { get; set; }
        public virtual string Status_Start_Date { get; set; }
        public virtual string Organisation_Name { get; set; }
        public virtual string Course_Title { get; set; }
        public virtual string Course_Level { get; set; }
        public virtual string Employer_Name { get; set; }
        public virtual string Job_Title { get; set; }
        public virtual string End_Date { get; set; }
        public virtual string Weeks_since_last_Pos_Status { get; set; }
        public virtual string Last_Positive_Status { get; set; }
        public virtual DateTime Last_Engagement_with_SDS { get; set; }
        public virtual string Benefit_Types { get; set; }
        public virtual string Benefit_Source { get; set; }
        public virtual string Looked_After_Status { get; set; }
        public virtual string Looked_After_Source { get; set; }
        public virtual string Young_Carer { get; set; }
        public virtual string Young_Carer_Source { get; set; }
        public virtual string ASN { get; set; }
        public virtual string ASN_Source { get; set; }
        public virtual string IEP { get; set; }
        public virtual string IEP_Source { get; set; }
        public virtual string CSP { get; set; }
        public virtual string CSP_Source { get; set; }
        public virtual string Transition_Plan { get; set; }
        public virtual string Transition_Plan_Source { get; set; }
        public virtual string Childs_Plan { get; set; }
        public virtual string Childs_Plan_Source { get; set; }
        public virtual int Data_Month { get; set; }
        public virtual int Data_Year { get; set; }
    }
}
