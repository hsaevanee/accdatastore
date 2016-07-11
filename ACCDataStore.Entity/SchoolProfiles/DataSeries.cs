using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class DataSeries: ObjectDetail
    {
        public School school;
        public Year year;
        public List<ObjectDetail> listdataitems;
        public double checkSumPercentage;
        public int checkSumCount;
    }
}
