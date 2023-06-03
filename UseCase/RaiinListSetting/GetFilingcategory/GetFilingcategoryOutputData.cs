using Domain.Models.RaiinListSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.GetFilingcategory
{
    public class GetFilingcategoryOutputData : IOutputData
    {
        public GetFilingcategoryOutputData(GetFilingcategoryStatus status, List<FilingCategoryModel> data)
        {
            Status = status;
            Data = data;
        }

        public GetFilingcategoryStatus Status { get; private set; }

        public List<FilingCategoryModel> Data { get; private set; }
    }
}
