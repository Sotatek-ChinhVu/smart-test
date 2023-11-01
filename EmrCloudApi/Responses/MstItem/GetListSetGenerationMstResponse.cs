using Domain.Models.SetGenerationMst;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListSetGenerationMstResponse
    {
        public GetListSetGenerationMstResponse(List<ListSetGenerationMstModel> data)
        {
            Data = data;
        }

        public List<ListSetGenerationMstModel> Data { get; private set; }
    }
}
