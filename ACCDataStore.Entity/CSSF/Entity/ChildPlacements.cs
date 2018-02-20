using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.CSSF
{
    public class ChildPlacements : BaseEntity
    {
         public virtual int Id { get; set; }
         public virtual string client_id {get;set;}
         public virtual string dob { get; set; }
         public virtual string gender { get; set; }
         public virtual string placement_id { get; set; }
         public virtual string placement_name { get; set; }
         public virtual string placement_started { get; set; }
         public virtual string placement_ended { get; set; }
         public virtual string placement_code { get; set; }
         public virtual int Lacode { get; set; }
         public virtual string LAType { get; set; }
        

    }
}
