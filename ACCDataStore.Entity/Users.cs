using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class Users : BaseEntity
    {
        public virtual int UserID { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FullName { get; set; }
        public virtual bool IsAdministrator { get; set; }
        public virtual bool IsScoolAdministrator { get; set; }
        public virtual bool IsDataHubAdministrator { get; set; }
        public virtual bool IsPublicAdministrator { get; set; }
    }
}
