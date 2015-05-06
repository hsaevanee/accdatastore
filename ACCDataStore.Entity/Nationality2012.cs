using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class Nationality2012 : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual string NationalIdentity { get; set; }
        public virtual string Gender { get; set; }
    }
}
