using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class EthnicBackgroundObj:BaseEntity
    {
        public virtual int id { get; set; }
        public virtual string code { get; set; }
        public virtual string name { get; set; }
        public virtual string scotXedCode { get; set; }

    }
}
