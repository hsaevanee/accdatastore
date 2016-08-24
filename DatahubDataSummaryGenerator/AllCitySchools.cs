using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore
{
    public class AllCitySchools
    {
        public virtual int id { get; set; }
        public virtual string seedCode { get; set; }
        public virtual string name { get; set; }
        public virtual string referenceCouncil { get; set; }
    }
}
