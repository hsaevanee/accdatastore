using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class StudentSIMDMap : ClassMap<StudentSIMD>
    {
        public StudentSIMDMap() {
            Table("test 3");
            Id(x => x.ID).Column("Id");
            Map(x => x.SchName).Column("Name");
            Map(x => x.Test2_Postcode).Column("Test2_Postcode");
            Map(x => x.Gender).Column("Gender");
            Map(x => x.StudentStage);
            Map(x => x.StudentStatus);
            Map(x => x.DateOfBirth);
            Map(x => x.AdmissionDate);
            Map(x => x.EthnicBackground);
            Map(x => x.NationalIdentity);
            Map(x => x.AsylumStatus);
            Map(x => x.FreeSchoolMealRegistered);
            Map(x => x.BaseSchoolCode);
            Map(x => x.ParentLaCode);
            Map(x => x.ModeOfAttendance);
            Map(x => x.IepIndicator);
            Map(x => x.RonIndicator);
            Map(x => x.MainstreamIntegration);
            Map(x => x.AttendanceSsu);
            Map(x => x.MainDifficultyInLearning);
            Map(x => x.CSPIndicator);
            Map(x => x.AdditionalSupportText);
            Map(x => x.DeclaredDisabled);
            Map(x => x.AssessedDisabled);
            Map(x => x.PhysicalAdaptation);
            Map(x => x.CurriculumAdaptation);
            Map(x => x.CommunicationAdaptation);
            Map(x => x.DisabilityText);
            Map(x => x.GaelicEducation);
            Map(x => x.LevelOfEnglish);
            Map(x => x.Literacy_Primary).Column("Literacy Primary");
            Map(x => x.Reading);
            Map(x => x.Writing);
            Map(x => x.L_and_T).Column("L and T");
            Map(x => x.Numeracy_Primary).Column("Numeracy Primary");
            Map(x => x.NMM);
            Map(x => x.SPM);
            Map(x => x.IH);
            Map(x => x.Adm_date).Column("Adm date");
            Map(x => x.Lv_Date).Column("Lv Date");
            Map(x => x.In_Care_Curre).Column("In Care Curre");
            Map(x => x.In_whilst_at).Column("In whilst at");
            Map(x => x.In_care_locati).Column("In care locati");
            Map(x => x.CityandShire_Postcode).Column("City&Shire_Postcode");
            Map(x => x.DataZone);
            Map(x => x.SIMD_2012_rank).Column("SIMD 2012 rank");
            Map(x => x.SIMD_2012_quintile).Column("SIMD 2012 quintile");
            Map(x => x.SIMD_2012_decile).Column("SIMD 2012 decile");
            Map(x => x.SIMD_2012_vigintile).Column("SIMD 2012 vigintile");
            Map(x => x.SIMD_2009_rank).Column("SIMD 2009 rank");
            Map(x => x.SIMD_2009_quintile).Column("SIMD 2009 quintile");
            Map(x => x.SIMD_2009_decile).Column("SIMD 2009 decile");
            Map(x => x.SIMD_2009_vigintile).Column("SIMD 2009 vigintile");
            Map(x => x.Datazone_Population_2010).Column("Datazone Population (2010)");
            Map(x => x.CHP_Population_Weighted_Vigintile_2012).Column("CHP Population Weighted Vigintile 2012");
            Map(x => x.HB_Population_Weighted_Vigintile_2012).Column("HB Population Weighted Vigintile 2012");
            Map(x => x.Scotland_Population_Weighted_Vigintile_2012).Column("Scotland Population Weighted Vigintile 2012");
            Map(x => x.IntZone);
            Map(x => x.IntZone_Name);
            Map(x => x.LA_Name);
            Map(x => x.CHP_Name);
            Map(x => x.HB_Code);
            Map(x => x.UR6_Desc);
            Map(x => x.SplitInd);
        }
    }
}
