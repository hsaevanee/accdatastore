using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.ScrumHub
{
    public class ScrumCourseModel : BaseEntity
    {

        public virtual int id { get; set; }
        public virtual string course_code { get; set; }
        public virtual string course_title { get; set; }
        public virtual string coordinator { get; set; }

    }
}
