﻿using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MySQL
{
    public class AberdeenSummaryMap : ClassMap<AberdeenSummary>
    {
        public AberdeenSummaryMap()
        {
            Table("datahubdata_summary_aberdeen");
            Id(x => x.id).Column("id");
            Map(x => x.name);
            Map(x => x.dataCode);
            Map(x => x.allFemale);
            Map(x => x.allUnspecified);
            Map(x => x.all15Male);
            Map(x => x.all15Female);
            Map(x => x.all15Unspecified);
            Map(x => x.all16Male);
            Map(x => x.all16Female);
            Map(x => x.all16Unspecified);
            Map(x => x.all17Male);
            Map(x => x.all17Female);
            Map(x => x.all17Unspecified);
            Map(x => x.all18Male);
            Map(x => x.all18Female);
            Map(x => x.all18Unspecified);
            Map(x => x.all19Male);
            Map(x => x.all19Female);
            Map(x => x.all19Unspecified);
            Map(x => x.schoolMale);
            Map(x => x.schoolFemale);
            Map(x => x.schoolUnspecified);
            Map(x => x.schoolInTransitionMale);
            Map(x => x.schoolInTransitionFemale);
            Map(x => x.schoolInTransitionUnspecified);
            Map(x => x.activityAgreementMale);
            Map(x => x.activityAgreementFemale);
            Map(x => x.activityAgreementUnspecified);
            Map(x => x.fundStage2Male);
            Map(x => x.fundStage2Female);
            Map(x => x.fundStage2Unspecified);
            Map(x => x.fundStage3Male);
            Map(x => x.fundStage3Female);
            Map(x => x.fundStage3Unspecified);
            Map(x => x.fundStage4Male);
            Map(x => x.fundStage4Female);
            Map(x => x.fundStage4Unspecified);
            Map(x => x.fullTimeEmploymentMale);
            Map(x => x.fullTimeEmploymentFemale);
            Map(x => x.fullTimeEmploymentUnspecified);
            Map(x => x.furtherEducationMale);
            Map(x => x.furtherEducationFemale);
            Map(x => x.furtherEducationUnspecified);
            Map(x => x.higherEducationMale);
            Map(x => x.higherEducationFemale);
            Map(x => x.higherEducationUnspecified);
            Map(x => x.modernApprenticeshipMale);
            Map(x => x.modernApprenticeshipFemale);
            Map(x => x.modernApprenticeshipUnspecified);
            Map(x => x.parttimeEmploymentMale);
            Map(x => x.parttimeEmploymentFemale);
            Map(x => x.parttimeEmploymentUnspecified);
            Map(x => x.personalDevelopmentMale);
            Map(x => x.personalDevelopmentFemale);
            Map(x => x.personalDevelopmentUnspecified);
            Map(x => x.selfEmployedMale);
            Map(x => x.selfEmployedFemale);
            Map(x => x.selfEmployedUnspecified);
            Map(x => x.trainingMale);
            Map(x => x.trainingFemale);
            Map(x => x.trainingUnspecified);
            Map(x => x.voluntaryWorkMale);
            Map(x => x.voluntaryWorkFemale);
            Map(x => x.voluntaryWorkUnspecified);
            Map(x => x.AvgWeek);
            Map(x => x.custodyMale);
            Map(x => x.custodyFemale);
            Map(x => x.custodyUnspecified);
            Map(x => x.economicallyInactiveMale);
            Map(x => x.economicallyInactiveFemale);
            Map(x => x.economicallyInactiveUnspecified);
            Map(x => x.illHealthMale);
            Map(x => x.illHealthFemale);
            Map(x => x.illHealthUnspecified);
            Map(x => x.unemployedMale);
            Map(x => x.unemployedFemale);
            Map(x => x.unemployedUnspecified);
            Map(x => x.unknownMale);
            Map(x => x.unknownFemale);
            Map(x => x.unknownUnspecified);
            Map(x => x.type);
            Map(x => x.dataMonth);
            Map(x => x.dataYear);
        }
    }
}
