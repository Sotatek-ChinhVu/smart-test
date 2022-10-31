using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchPostCode
{
    public class SearchPostCodeOutputData : IOutputData
    {
        public SearchPostCodeOutputData(int totalCount, List<PostCodeMstModel> postCodeMstModels, SearchPostCodeStatus status)
        {
            TotalCount = totalCount;
            PostCodeMstModels = postCodeMstModels;
            Status = status;
        }

        public int TotalCount { get; private set; }
        public List<PostCodeMstModel> PostCodeMstModels { get; private set; }

        public SearchPostCodeStatus Status { get; private set; }
    }
}
