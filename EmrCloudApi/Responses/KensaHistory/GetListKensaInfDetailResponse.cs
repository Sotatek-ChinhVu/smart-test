using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.KensaHistory
{
    public class GetListKensaInfDetailResponse
    {
        public GetListKensaInfDetailResponse(List<ListKensaInfDetailModel> data)
        {
            Data = data;
        }

        public List<ListKensaInfDetailModel> Data { get; private set; }
    }
}
