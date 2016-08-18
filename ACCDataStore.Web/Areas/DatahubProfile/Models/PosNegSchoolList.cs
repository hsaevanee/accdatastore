using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class PosNegSchoolList
    {
        public string name { get; set; }
        public int participating { get; set; }
        public int notParticipating { get; set; }
        public int unknown { get; set; }
    }
}