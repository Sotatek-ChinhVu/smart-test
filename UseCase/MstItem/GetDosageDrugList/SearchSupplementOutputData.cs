using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDosageDrugList
{
    public class SearchSupplementOutputData : IOutputData
    {
        public SearchSupplementOutputData(List<SearchSupplementModel> searchSupplements, SearchSupplementStatus status)
        {
            SearchSupplementResponse = searchSupplements;
            Status = status;
        }

        public List<SearchSupplementModel> SearchSupplementResponse { get; private set; }

        public SearchSupplementStatus Status { get; private set; }
    }
}
