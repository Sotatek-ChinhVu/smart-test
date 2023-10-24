using Domain.Models.SetGenerationMst;
using EmrCloudApi.Responses.MstItem.DiseaseNameMstSearch;

namespace EmrCloudApi.Responses.GetSendaiGeneration
{
    public class SetSendaiGenerationGetListResponse
    {
        public SetSendaiGenerationGetListResponse(List<SetSendaiGenerationModel> listData)
        {
            ListData = listData;
        }
        public List<SetSendaiGenerationModel> ListData { get; private set; }
    }
}
