using Domain.Models.RaiinListSetting;

namespace EmrCloudApi.Responses.RaiinListSetting
{
    public class GetFilingcategoryResponse
    {
        public GetFilingcategoryResponse(List<FilingCategoryModel> data)
        {
            Data = data;
        }

        public List<FilingCategoryModel> Data { get; private set; }
    }
}
