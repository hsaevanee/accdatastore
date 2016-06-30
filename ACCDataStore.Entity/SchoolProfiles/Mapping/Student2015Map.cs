using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Mapping
{
    public class Student2015Map : ClassMap<Student2015>
    {
        public Student2015Map(){
            Table("test_student_2015");
            Id(x => x.ID).Column("ID");
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.ForeName);
            Map(x => x.SurName);
            Map(x => x.PostCode);
            Map(x => x.Stage);
            Map(x => x.SCN);
            Map(x => x.EthnicBackground);
            Map(x => x.NationalIdentity);
            Map(x => x.FreeSchoolMeal);
            Map(x => x.StdLookedAfter);
            Map(x => x.LevelofEnglish);
        }


    }
}
