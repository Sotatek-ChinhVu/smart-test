using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDosageDrugList
{
    public class SearchSupplementOutputData : IOutputData
    {
        public SearchSupplementOutputData(List<SearchSupplementBaseModel> searchSupplementResponse, int total, SearchSupplementStatus status)
        {
            SearchSupplementResponse = searchSupplementResponse;
            Total = total;
            Status = status;
        }

        public List<SearchSupplementBaseModel> SearchSupplementResponse { get; private set; }
        public int Total { get; set; }
        public SearchSupplementStatus Status { get; private set; }
    }
}
