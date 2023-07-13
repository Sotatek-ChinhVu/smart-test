using Domain.Models.FlowSheet;

namespace EmrCloudApi.Responses.FlowSheet.GetListFlowSheetDto
{
    public class RaiinListInfDto
    {
        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public RaiinListInfDto(RaiinListInfModel model)
        {
            GrpId = model.GrpId;
            KbnCd = model.KbnCd;
        }
    }
}
