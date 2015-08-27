using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class Year: BaseEntity
    {
        public string year { get; set; }
        public string academicyear { get; set; }
        //public string isSelected { get; set; }

        public Year(string year, string academicyear)
        {
            this.year = year;
            this.academicyear = academicyear;
            //this.isSelected = "checked";
        }
    }
}
