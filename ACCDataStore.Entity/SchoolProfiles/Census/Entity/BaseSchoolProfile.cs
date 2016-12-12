using ACCDataStore.Entity.RenderObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class BaseSchoolProfile
    {
        public Year YearInfo { get; set; }
        public List<GenericSchoolData> ListGenericSchoolData { get; set; }
    }
}
