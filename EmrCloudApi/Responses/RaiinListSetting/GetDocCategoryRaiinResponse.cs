using Domain.Models.Document;

namespace EmrCloudApi.Responses.RaiinListSetting
{
    public class GetDocCategoryRaiinResponse
    {
        public GetDocCategoryRaiinResponse(List<DocCategoryModel> data)
        {
            Data = data;
        }

        public List<DocCategoryModel> Data { get; private set; }
    }
}
