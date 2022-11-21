using Domain.Models.SetMst;

namespace EmrCloudApi.Tenant.Responses.SetMst
{
    public class GetSetMstToolTipResponse
    {
        public GetSetMstToolTipResponse(SetMstTooltipModel data)
        {
            Data = data;
        }

        public SetMstTooltipModel Data { get; private set; }
    }
}