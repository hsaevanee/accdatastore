using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolRollForecast
{
    public class SchoolRollForecastObj : BaseEntity
    {
        private const double p1 = 0.17;
        private const double p2 = 0.16;
        private const double p3 = 0.16;
        private const double p4 = 0.14;
        private const double p5 = 0.13;
        private const double p6 = 0.12;
        private const double p7 = 0.12;
        private const double pAF14 = 0.985406;
        private const double pAF15 = 0.979817;
        private const double pAF16 = 0.968061;
        private const double pAF17 = 0.969960;
        private const double pAF18 = 0.972025;
        private const double pAF19 = 0.972390;
        private const double pAF20 = 0.964445;
        private const double pAF21 = 0.967585;

        public List<double> ListP1Input { get; set; }
        public List<double> ListPupilsHhld { get; set; }
        public List<double> ListHousing { get; set; }
        public List<double> ListParentsCharter { get; set; }
        public List<double> ListP1 { get; set; }
        public List<double> ListP2 { get; set; }
        public List<double> ListP3 { get; set; }
        public List<double> ListP4 { get; set; }
        public List<double> ListP5 { get; set; }
        public List<double> ListP6 { get; set; }
        public List<double> ListP7 { get; set; }
        public List<double> ListTotalRoll { get; set; }
        public List<double> ListMaxCap { get; set; }
        public List<double> ListTotRollFunctWCap { get; set; }
        public List<double> ListTotRollFunctWCapPer { get; set; }

    }
}
