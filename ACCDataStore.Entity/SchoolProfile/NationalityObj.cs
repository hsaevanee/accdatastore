using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class NationalityObj : BaseEntity
    {
        public string IdentityCode { get; set; }
        public string IdentityName { get; set; }
        public double PercentageInSchool { get; set; }
        public double PercentageAllSchool { get; set; }
    }
}
