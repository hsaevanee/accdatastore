using ACCDataStore.Entity.SchoolProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class ObjectDetail: BaseEntity

    {
        public List<StudentObj> liststudents;
        public string itemcode; // Code 
        public int count; //length of pupils list
        public double percentage;
        public double percentageFemale;
        public double percentageMale;
    }
}
