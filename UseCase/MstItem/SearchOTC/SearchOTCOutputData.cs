using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchOTC
{
    public class SearchOTCOutputData : IOutputData
    {
        public SearchOTCOutputData(List<SearchOTCModel> searchOTCModels, SearchOTCStatus status)
        {
            SearchOTCResponse = searchOTCModels;
            Status = status;
        }

        public List<SearchOTCModel> SearchOTCResponse { get; private set; }

        public SearchOTCStatus Status { get; private set; }
    }
}
