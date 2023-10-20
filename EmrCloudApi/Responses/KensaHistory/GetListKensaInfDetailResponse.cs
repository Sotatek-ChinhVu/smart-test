using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.KensaHistory
{
    public class GetListKensaInfDetailResponse
    {
        public GetListKensaInfDetailResponse(ListKensaInfDetailModel data)
        {
            Data = data;
        }

        public ListKensaInfDetailModel Data { get; private set; }
    }
}
