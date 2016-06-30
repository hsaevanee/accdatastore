using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Mapping
{
    public class Student2013Map : ClassMap<Student2013>
    {
        public Student2013Map(){
            Table("test_student_2013");
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
