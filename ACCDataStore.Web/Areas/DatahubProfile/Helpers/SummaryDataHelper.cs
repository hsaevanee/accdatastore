using ACCDataStore.Entity.DatahubProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Helpers
{
    public class SummaryDataHelper
    {
        
        public SummaryDataHelper(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        private IGenericRepository2nd rpGeneric2nd;

        public SummaryDataViewModel GetSummaryDataForCouncil(string code, int month, int year)
        {
            //SummaryData result = new SummaryData();
            //switch (councilName.ToLower())
            //{
            //    case "aberdeen city":
            //        result = rpGeneric2nd.QueryOver<AberdeenSummary>().Where(x => x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            //        break;
            //    default:
            //        result = null;
            //        break;
            //}

            var result = rpGeneric2nd.QueryOver<AberdeenSummary>().Where(x => x.dataCode == code && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            return new SummaryDataViewModel(result);
        }

        public IList<SummaryDataViewModel> GetSummaryDataForAllDataZones(int month, int year)
        {
            var result = rpGeneric2nd.QueryOver<AberdeenSummary>().Where(x => x.type == "Data Zone" && x.dataMonth == month && x.dataYear == year).List<SummaryData>();
            return _CreateListOfViewModels(result);
        }

        public SummaryDataViewModel GetSummaryDataForSingleDataZone(string code, int month, int year)
        {
            var result = rpGeneric2nd.QueryOver<AberdeenSummary>().Where(x => x.type == "Data Zone" && x.dataCode == code && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            return new SummaryDataViewModel(result);
        }

        public SummaryDataViewModel GetSummaryDataForSingleIntermediateZone(string code, int month, int year)
        {
            var result = rpGeneric2nd.QueryOver<AberdeenSummary>().Where(x => x.type == "Intermediate Zone" && x.dataCode == code && x.dataMonth == month && x.dataYear == year).SingleOrDefault();
            return new SummaryDataViewModel(result);
        }

        public IList<SummaryDataViewModel> GetSummaryDataForAllIntermediateZones(int month, int year)
        {
            var result = rpGeneric2nd.QueryOver<AberdeenSummary>().Where(x => x.type == "Intermediate Zone" && x.dataMonth == month && x.dataYear == year).List<SummaryData>();

            return _CreateListOfViewModels(result);
        }

        public IList<SummaryDataViewModel> GetSummaryDataForAllSchools(string councilCode, int month, int year)
        {
            IList<SummaryData> result = new Collection<SummaryData>();
            IList<AllSchools> allSchoolsForCouncil = this.rpGeneric2nd.QueryOver<AllSchools>().Where(x => x.referenceCouncil == councilCode).List();
            foreach (var school in allSchoolsForCouncil)
            {
                // At this point we should know to witch council/city to point to [TODO]
                //var query = _SummaryDataQueryCouncilHelper("Aberdeen City");
                //Type type = typeof(AberdeenSummary);
                //Assembly n = type.Assembly;
                //System.Reflection.Assembly a = typeof(AberdeenSummary).Assembly;

                SummaryData currentSummary = GetSummaryDataForSingleSchool(school.seedCode, month, year);
                result.Add(currentSummary);
            }
            return _CreateListOfViewModels(result);
        }

        public SummaryData GetSummaryDataForSingleSchool(string seedCode, int month, int year ) 
        { 
            SummaryData currentSummary = (SummaryData) this.rpGeneric2nd.Query<AberdeenSummary>()
                                            .Where(x => x.type == "School" && x.dataCode == seedCode && x.dataMonth == month && x.dataYear == year)
                                            .SingleOrDefault();
            return currentSummary; 
        }

        // WIP
        private IQueryable<SummaryData> _SummaryDataQueryCouncilHelper(string councilName)
        {
            IQueryable<SummaryData> queryOver;
            switch (councilName.ToLower())
            {
                case "aberdeen city":
                    queryOver = (IQueryable<AberdeenSummary>) this.rpGeneric2nd.Query<AberdeenSummary>();
                    break;
                default:
                    queryOver = null;
                    break;
            }
            return queryOver;
        }

        private IList<SummaryDataViewModel> _CreateListOfViewModels(IList<SummaryData> summaryDataList)
        {
            IList<SummaryDataViewModel> summaryDataViewModelList = new Collection<SummaryDataViewModel>();
            IList<decimal> percentageData = new Collection<decimal>();

            foreach (SummaryData item in summaryDataList)
            {
                summaryDataViewModelList.Add(new SummaryDataViewModel(item));
            }
            return summaryDataViewModelList;
        }
    }
}