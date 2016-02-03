using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class EthnicBackgroundObj:BaseEntity
    {
        public virtual double id { get; set; }
        public virtual string code { get; set; }
        public virtual string value { get; set; }
        public virtual string ScotXedcode { get; set; }
    }
}
