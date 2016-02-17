using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACCDataStore.Entity
{
    public class Rights : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual Users Users { get; set; }
        public virtual int SheetID { get; set; }
        public virtual int GroupID { get; set; }
        //public virtual Category Category { get; set; }
        //public virtual LocationInfo LocationInfo { get; set; }
        //public virtual SiteInfo SiteInfo { get; set; }
        //public virtual DeviceList DeviceList { get; set; }
        //public virtual DeviceParams DeviceParams { get; set; }
        public virtual RightsCode RightsCode { get; set; }
        public virtual bool RightValue { get; set; }

        public virtual int SiteID { get; set; }
        public virtual int DeviceParamID { get; set; }
        public virtual int RightCode { get; set; }
    }
}
