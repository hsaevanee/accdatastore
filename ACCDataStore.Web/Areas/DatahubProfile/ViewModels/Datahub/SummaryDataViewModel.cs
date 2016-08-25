using ACCDataStore.Entity.DatahubProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class SummaryDataViewModel
    {
        public SummaryDataViewModel(SummaryData summaryData)
        {
            this.summaryData = summaryData;
        }

        public SummaryData summaryData;

        public int getAllPupils() 
        {
            return summaryData.allFemale + summaryData.allMale + summaryData.allUnspecified;
        }
        
        public int getAllPupils15()
        {
            return summaryData.all15Female + summaryData.all15Male + summaryData.all15Unspecified;
        }

        public int getAllPupils16()
        {
            return summaryData.all16Female + summaryData.all16Male + summaryData.all16Unspecified;
        }

        public int getAllPupils17()
        {
            return summaryData.all17Female + summaryData.all17Male + summaryData.all17Unspecified;
        }

        public int getAllPupils18()
        {
            return summaryData.all19Female + summaryData.all19Male + summaryData.all19Unspecified;
        }

        public int getAllPupils19()
        {
            return summaryData.all19Female + summaryData.all19Male + summaryData.all19Unspecified;
        }

        public int getAllPupilsInSchool()
        {
            return summaryData.schoolFemale + summaryData.schoolMale + summaryData.schoolUnspecified;
        }

        public int getAllPupilsInSchoolTransition()
        {
            return summaryData.schoolInTransitionFemale + summaryData.schoolInTransitionMale + summaryData.schoolInTransitionUnspecified;
        }

        public int getAllPupilsMovedOutwithScotland()
        {
            return summaryData.movedOutScotlandFemale + summaryData.movedOutScotlandMale + summaryData.movedOutScotlandUnspecified;
        }

        public int getAllPupilsInActivityAgreement()
        {
            return summaryData.activityAgreementFemale + summaryData.activityAgreementMale + summaryData.activityAgreementUnspecified;
        }

        public int getAllPupilsInEmployabilityFundStage2()
        {
            return summaryData.fundStage2Female + summaryData.fundStage2Male + summaryData.fundStage2Unspecified;
        }

        public int getAllPupilsInEmployabilityFundStage3()
        {
            return summaryData.fundStage4Female + summaryData.fundStage4Male + summaryData.fundStage4Unspecified;
        }

        public int getAllPupilsInEmployabilityFundStage4()
        {
            return summaryData.fundStage4Female + summaryData.fundStage4Male + summaryData.fundStage4Unspecified;
        }

        public int getAllPupilsInFullTimeEmployement()
        {
            return summaryData.fullTimeEmploymentFemale + summaryData.fullTimeEmploymentMale + summaryData.fundStage2Unspecified;
        }

        public int getAllPupilsInFurtherEducation()
        {
            return summaryData.furtherEducationFemale + summaryData.furtherEducationMale + summaryData.furtherEducationUnspecified;
        }

        public int getAllPupilsInHigherEducation()
        {
            return summaryData.higherEducationFemale + summaryData.higherEducationMale + summaryData.higherEducationUnspecified;
        }

        public int getAllPupilsInModernApprenship()
        {
            return summaryData.modernApprenticeshipFemale + summaryData.modernApprenticeshipMale + summaryData.modernApprenticeshipUnspecified;
        }

        public int getAllPupilsInPartTimeEmployment()
        {
            return summaryData.parttimeEmploymentFemale + summaryData.parttimeEmploymentMale + summaryData.parttimeEmploymentUnspecified;
        }

        public int getAllPupilsInPersonalSkillDevelopment()
        {
            return summaryData.personalDevelopmentFemale + summaryData.personalDevelopmentMale + summaryData.personalDevelopmentUnspecified;
        }

        public int getAllPupilsInSelfEmployed()
        {
            return summaryData.selfEmployedFemale + summaryData.selfEmployedMale + summaryData.selfEmployedUnspecified;
        }

        public int getAllPupilsInTraining()
        {
            return summaryData.trainingFemale + summaryData.trainingMale + summaryData.trainingUnspecified;
        }

        public int getAllPupilsInVoulanteerWork()
        {
            return summaryData.voluntaryWorkFemale + summaryData.voluntaryWorkMale + summaryData.voluntaryWorkUnspecified;
        }

        public int getAllPupilsInCustody()
        {
            return summaryData.custodyFemale + summaryData.custodyMale + summaryData.custodyUnspecified;
        }

        public int getAllPupilsInEconomicallyInactive()
        {
            return summaryData.economicallyInactiveFemale + summaryData.economicallyInactiveMale + summaryData.economicallyInactiveUnspecified;
        }

        public int getAllPupilsInUnavailableIllHealth()
        {
            return summaryData.illHealthFemale + summaryData.illHealthMale + summaryData.illHealthUnspecified;
        }

        public int getAllPupilsInUnemployed()
        {
            return summaryData.unemployedFemale + summaryData.unemployedMale + summaryData.unemployedUnspecified;
        }

        public int getAllPupilsInUnknown()
        {
            return summaryData.unknownFemale + summaryData.unknownMale + summaryData.unknownUnspecified;
        }
        
        public int getAllPupilsIncludingMovedOutwithScotland()
        {
            return getAllPupils() + getAllPupilsMovedOutwithScotland();
        }

        public double Percentage(int number)
        {
            // How are we calculating this?
            double result = (double)(number * 100) / getAllPupils();
            return double.IsNaN(result) ? 0.0 : result;

        }

        public double Participating()
        {
            return (double)(Percentage(
                getAllPupilsInSchool() +
                getAllPupilsInSchoolTransition() +
                getAllPupilsInActivityAgreement() +
                getAllPupilsInEmployabilityFundStage2() +
                getAllPupilsInEmployabilityFundStage3() +
                getAllPupilsInEmployabilityFundStage4() +
                getAllPupilsInFullTimeEmployement() +
                getAllPupilsInFurtherEducation() +
                getAllPupilsInHigherEducation() +
                getAllPupilsInModernApprenship() +
                getAllPupilsInPartTimeEmployment() +
                getAllPupilsInPersonalSkillDevelopment() +
                getAllPupilsInSelfEmployed() +
                getAllPupilsInTraining() +
                getAllPupilsInVoulanteerWork()
            ));
        }

        public double NotParticipating()
        {
            return (double)(Percentage(
                getAllPupilsInCustody()+
                getAllPupilsInEconomicallyInactive() +
                getAllPupilsInUnavailableIllHealth() +
                //Pupils unknown not in calculation in DatahubData
                getAllPupilsInUnknown() +
                getAllPupilsInUnemployed()
            ));
        }

        public object FormatNumber(int number)
        {
            if (number <= 10)
            {
                return "*";
            }
            else
            {
                return number;
            }

        }
    }
}