using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchSupplement
{
    public class SearchSupplementOutputData : IOutputData
    {
        public SearchSupplementOutputData(List<SearchSupplementModel> searchSupplementResponse, int total, SearchSupplementStatus status)
        {
            SearchSupplementResponse = searchSupplementResponse;
            Total = total;
            Status = status;
        }

        public List<SearchSupplementModel> SearchSupplementResponse { get; private set; }
        public int Total { get; set; }
        public SearchSupplementStatus Status { get; private set; }
    }
}
