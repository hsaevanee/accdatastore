using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.MySQL
{
    public class LA100Pupils : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual int scn { get; set; }
        public virtual int year { get; set; }
        public virtual string centre { get; set; }
        public virtual int stage { get; set; }
        public virtual int gender { get; set; }
        public virtual int ethnicity { get; set; }
        public virtual string age_group { get; set; }
        public virtual string lac_group { get; set; }
        public virtual string eal_group { get; set; }
        public virtual string asn_group { get; set; }
        public virtual string vc_asn_and_mainstream_integration_group { get; set; }
        public virtual string simd_decile { get; set; }
        public virtual string simd_vigintile { get; set; }
        public virtual string pupil_points_group { get; set; }
        public virtual string leaver_group { get; set; }
        public virtual string leaver_destination_group { get; set; }
        public virtual string destination { get; set; }
        public virtual string leaver_centre { get; set; }
        public virtual string vc_stage_and_winter_leaver_group { get; set; }
        public virtual string vc_leaver_stage_group { get; set; }
        public virtual string highest_scqf_course_to_date { get; set; }
        public virtual string highest_lit_scqf_level_to_date { get; set; }
        public virtual string highest_num_scqf_level_to_date { get; set; }
        public virtual string annual_total_tariff_points { get; set; }
        public virtual string cumulative_total_tariff_points { get; set; }
        public virtual LA100Schools Schools { get; set; }
    }
}
