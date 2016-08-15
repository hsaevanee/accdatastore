using ACCDataStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Entity.ScrumHub
{
    public class ScrumHubModel : BaseEntity
    {
        public virtual int id {get; set;}
        public virtual string firstname { get; set; }
        public virtual string lastname { get; set; }
        public virtual int age { get; set; }
    }
    public class ScrumTable : ScrumHubModel { }
}