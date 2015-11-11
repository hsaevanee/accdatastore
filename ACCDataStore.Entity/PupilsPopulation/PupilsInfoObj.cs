using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.PupilsPopulation
{
    public class PupilsInfoObj: BaseEntity
    {
        public virtual int id { get; set; }
        public virtual int dbmonth { get; set; }
        public virtual int dbyear { get; set; }
        public virtual string gender { get; set; }
        public virtual string post_in { get; set; }
        public virtual string post_out { get; set; }
        public virtual string postcode { get; set; }

    }
}
