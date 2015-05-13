using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class EthnicObj : BaseEntity
    {
        public EthnicObj(string sethiniccode,string sethinicname, double sdata, double sdata2)
        {
            this.EthinicCode = sethiniccode;
            this.EthinicName = sethinicname;
            this.PercentageInSchool = sdata;
            this.PercentageAllSchool= sdata2;
        }

        public EthnicObj()
        {
            this.EthinicCode = null;
            this.EthinicName = null;
            this.PercentageInSchool = 0;
            this.PercentageAllSchool = 0;
        }
        public string EthinicCode { get; set; }
        public string EthinicName { get; set; }
        public double PercentageInSchool { get; set; }
        public double PercentageAllSchool { get; set; }
    }
}
