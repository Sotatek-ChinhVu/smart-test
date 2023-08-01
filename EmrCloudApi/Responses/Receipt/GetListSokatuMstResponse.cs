using Domain.Models.Receipt;

namespace EmrCloudApi.Responses.Receipt
{
    public class GetListSokatuMstResponse
    {
        public GetListSokatuMstResponse(List<SokatuMstModel> sokatuMstModels)
        {
            SokatuMstModels = sokatuMstModels;
        }

        public List<SokatuMstModel> SokatuMstModels { get; private set; }
    }
}
