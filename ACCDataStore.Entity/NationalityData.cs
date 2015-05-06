using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class NationalityData : BaseEntity
    {
        public string Identity { get; set; }
        public int TotalNumber { get; set; }
    }
}
