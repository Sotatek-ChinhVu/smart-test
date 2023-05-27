using Domain.Models.Document;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.GetDocCategory
{
    public class GetDocCategoryRaiinOutputData : IOutputData
    {
        public GetDocCategoryRaiinOutputData(GetDocCategoryRaiinStatus status, List<DocCategoryModel> data)
        {
            Status = status;
            Data = data;
        }

        public GetDocCategoryRaiinStatus Status { get; private set; }

        public List<DocCategoryModel> Data { get; private set; }
    }
}
