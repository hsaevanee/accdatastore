using ACCDataStore.Entity.RenderObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class CityChartData: ChartData
    {
        public BaseRenderObject ChartFSMPrimary { get; set; }
        public BaseRenderObject ChartFSMSecondary { get; set; }
        public BaseRenderObject ChartFSMSpecial { get; set; }
        public BaseRenderObject ChartFSMCity { get; set; }
    }
}
