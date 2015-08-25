using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.MySQL
{
    public class LA100Schools : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual string SeedCode { get; set; }
        public virtual string SchoolName { get; set; }
    }
}
