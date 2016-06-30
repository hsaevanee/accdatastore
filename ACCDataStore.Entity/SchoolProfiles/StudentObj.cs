using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class StudentObj : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual int SeedCode { get; set; }
        public virtual string StudentID { get; set; }
        public virtual string ForeName { get; set; }
        public virtual string SurName { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Stage { get; set; }
        public virtual string SCN { get; set; }
        public virtual string EthnicBackground { get; set; }
        public virtual string NationalIdentity { get; set; }
        public virtual string FreeSchoolMeal { get; set; }
        public virtual string StdLookedAfter { get; set; }
        public virtual string LevelofEnglish { get; set; }
    }
}
