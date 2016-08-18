using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.MonthOnMonthOverview.Mapping
{
    public class Month1Map: ClassMap<Month1>
    {
         public Month1Map() 
        {
            Table("datahubdata_jan");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);
   
        }
  
    }
    public class Month2Map : ClassMap<Month2>
    {
        public Month2Map()
        {
            Table("datahubdata_feb");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month3Map : ClassMap<Month3>
    {
        public Month3Map()
        {
            Table("datahubdata_march");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month4Map : ClassMap<Month4>
    {
        public Month4Map()
        {
            Table("datahubdata_april");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month5Map : ClassMap<Month5>
    {
        public Month5Map()
        {
            Table("datahubdata_may");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month6Map : ClassMap<Month6>
    {
        public Month6Map()
        {
            Table("datahubdata_june");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month7Map : ClassMap<Month7>
    {
        public Month7Map()
        {
            Table("datahubdata_july");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month8Map : ClassMap<Month8>
    {
        public Month8Map()
        {
            Table("datahubdata_aug");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month9Map : ClassMap<Month9>
    {
        public Month9Map()
        {
            Table("datahubdata_sept");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month10Map : ClassMap<Month10>
    {
        public Month10Map()
        {
            Table("datahubdata_oct");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month11Map : ClassMap<Month11>
    {
        public Month11Map()
        {
            Table("datahubdata_nov");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
    public class Month12Map : ClassMap<Month12>
    {
        public Month12Map()
        {
            Table("datahubdata_dec");
            Id(x => x.Id).Column("ID");
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.Date_of_Birth);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
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
            Map(x => x.Current_Status);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
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
            Map(x => x.ASN);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN_Source);
            Map(x => x.CSP);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan_Source);

        }

    }
   
    
}
