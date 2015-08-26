using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class School:BaseEntity
    {
        public string seedcode { get; set; }
        public string name { get; set; }

        public School(string seedcode, string name)
        {
            this.seedcode = seedcode;
            this.name = name;
        }
    }
}
