using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.PupilsPopulation.ViewModels.Pupils05
{
    public class Pupils05Data
    {
        public string datacode { get; set; } // store school code / datazone code
        public int allpupils05 { get; set; }
        public int allpupils04 { get; set; }
        public int allpupils03 { get; set; }
        public int allpupils02 { get; set; }
        public int allpupils01 { get; set; }
        public int allpupils00 { get; set; }


        public double Allpupilsbetween0to5()
        {
            return allpupils05 + allpupils04 + allpupils03 + allpupils02 + allpupils01 + allpupils00;
        }

        public double Allpupilsbetween0to4()
        {
            return allpupils04 + allpupils03 + allpupils02 + allpupils01 + allpupils00;
        }


        public double Allpupilsbetween0to3()
        {
            return allpupils03 + allpupils02 + allpupils01 + allpupils00;
        }


        public double Allpupilsbetween0to2()
        {
            return allpupils02 + allpupils01 + allpupils00;
        }


        public double Allpupilsbetween0to1()
        {
            return allpupils01 + allpupils00;
        }

        public double Allpupilsbetween0to0()
        {
            return allpupils00;
        }

        public double Percentage(double number, double total)
        {
            return (double)(number * 100) / total;
        }

    }
}