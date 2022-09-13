using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchOTC
{
    public class SearchOTCOutputData : IOutputData
    {
        public SearchOTCOutputData(List<OtcItemModel> searchOTCResponse, int total, SearchOTCStatus status)
        {
            SearchOTCResponse = searchOTCResponse;
            Total = total;
            Status = status;
        }

        public List<OtcItemModel> SearchOTCResponse { get; private set; }
        public int Total { get; set; }
        public SearchOTCStatus Status { get; private set; }
    }
}
