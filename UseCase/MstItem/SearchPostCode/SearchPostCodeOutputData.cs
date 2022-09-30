using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchPostCode
{
    public class SearchPostCodeOutputData : IOutputData
    {
        public SearchPostCodeOutputData(List<PostCodeMstModel> postCodeMstModels, SearchPostCodeStatus status)
        {
            PostCodeMstModels = postCodeMstModels;
            Status = status;
        }

        public List<PostCodeMstModel> PostCodeMstModels { get; private set; }

        public SearchPostCodeStatus Status { get; private set; }
    }
}
