using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class SIMDObj:BaseEntity
    {
        public SIMDObj(string sSIMDcode, double sdata, double sdata2)
        {
            this.SIMDCode = sSIMDcode;
            //this.SIMDName = sSIMDname;
            this.PercentageInSchool = sdata;
            this.PercentageAllSchool= sdata2;
        }

        public SIMDObj()
        {
            this.SIMDCode = null;
            //this.SIMDName = null;
            this.PercentageInSchool = 0;
            this.PercentageAllSchool = 0;
        }
        public string SIMDCode { get; set; }
        //public string SIMDName { get; set; }
        public double PercentageInSchool { get; set; }
        public double PercentageAllSchool { get; set; }
    }
    
}
