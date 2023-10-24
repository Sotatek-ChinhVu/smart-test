using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListKensaIjiSettingResponse
    {
        public GetListKensaIjiSettingResponse(List<KensaIjiSettingModel> data)
        {
            Data = data;
        }

        public List<KensaIjiSettingModel> Data { get; private set; }
    }
}
