using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.KensaHistory
{
    public class GetKensaInfDetailByIraiCdResponse
    {
        public GetKensaInfDetailByIraiCdResponse(List<ListKensaInfDetailItemModel> data)
        {
            Data = data;
        }

        public List<ListKensaInfDetailItemModel> Data { get; private set; }
    }
}
