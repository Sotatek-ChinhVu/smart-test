using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaSetDetail;

namespace EmrCloudApi.Responses.KensaHistory
{
    public class GetListKensaCmtMstResponse
    {
        public GetListKensaCmtMstResponse(List<KensaCmtMstModel> data)
        {
            Data = data;
        }

        public List<KensaCmtMstModel> Data { get; private set; }
    }
}
