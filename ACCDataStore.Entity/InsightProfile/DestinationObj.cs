using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.InsightProfile
{
   public class DestinationObj : BaseEntity
    {
       public string objname { get; set; }
       public string year { get; set; }
       public string destiationcode { get; set; }
       public string gender{ get; set; }
       public double number { get; set; }

       public DestinationObj(string objname,string year,string gender,string code, double number) {
           this.objname = objname;
           this.year = year;
           this.destiationcode = code;
           this.gender = gender;
           this.number = number;
       
       }
    }
}
