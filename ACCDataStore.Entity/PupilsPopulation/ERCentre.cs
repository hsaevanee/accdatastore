using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.PupilsPopulation
{
    public class ERCentre : BaseEntity
    {
        public string providerType { get; set; }
        public string registrationName { get; set; }
        public string registrationNo { get; set; }
        public string postcode { get; set; }
        public ERCentre()
        {
 

        }
    }
}
