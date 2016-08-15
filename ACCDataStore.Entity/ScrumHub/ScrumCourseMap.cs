using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.ScrumHub
{
   public class ScrumCourseMap : ClassMap<ScrumCourseModel>
    {
       public ScrumCourseMap() 
       {
           Table("scrumcourses");
           Id(x => x.id);
           Map(x => x.course_code);
           Map(x => x.course_title);
           Map(x => x.coordinator);
       }
    }
}
