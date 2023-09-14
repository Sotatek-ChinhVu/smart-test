using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetKensaStdMstModelsResponse
    {
        public GetKensaStdMstModelsResponse(List<KensaStdMstModel> kensaStdMsts) 
        {
            KensaStdMsts = kensaStdMsts;
        }

        public List<KensaStdMstModel> KensaStdMsts { get; private set; }
    }
}
