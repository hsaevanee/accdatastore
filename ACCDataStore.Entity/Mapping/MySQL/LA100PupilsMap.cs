using ACCDataStore.Entity.MySQL;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MySQL
{
    public class LA100PupilsMap: ClassMap<LA100Pupils>
    {   
        public LA100PupilsMap()
        {
            Id(x => x.ID);
            Map(x => x.scn);
            Map(x => x.year);
            Map(x => x.centre);
            Map(x => x.stage);
            Map(x => x.gender);
            Map(x => x.ethnicity);
            Map(x => x.age_group);
            Map(x => x.lac_group);
            Map(x => x.asn_group);
            Map(x => x.vc_asn_and_mainstream_integration_group);
            Map(x => x.simd_decile);
            Map(x => x.simd_vigintile);
            Map(x => x.pupil_points_group);
            Map(x => x.leaver_group);
            Map(x => x.leaver_destination_group);
            Map(x => x.destination);
            Map(x => x.leaver_centre);
            Map(x => x.vc_stage_and_winter_leaver_group);
            Map(x => x.vc_leaver_stage_group);
            Map(x => x.highest_scqf_course_to_date);
            Map(x => x.highest_lit_scqf_level_to_date);
            Map(x => x.highest_num_scqf_level_to_date);
            Map(x => x.annual_total_tariff_points);
            Map(x => x.cumulative_total_tariff_points);
        }
    }
}
