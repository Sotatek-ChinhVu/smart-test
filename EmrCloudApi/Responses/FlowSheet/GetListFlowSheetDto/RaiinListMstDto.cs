using Domain.Models.RaiinListMst;

namespace EmrCloudApi.Responses.FlowSheet.GetListFlowSheetDto
{
    public class RaiinListMstDto
    {
        public RaiinListMstDto(RaiinListMstModel model)
        {
            GrpId = model.GrpId;
            GrpName = model.GrpName;
            SortNo = model.SortNo;
            DetailList = model.RaiinListDetailsList.Select(r => new RaiinListDetailDto(r.GrpId, r.KbnCd, r.SortNo, r.KbnName, r.ColorCd, r.IsDeleted, r.IsOnlySwapSortNo)).ToList();
            IsDeleted = model.IsDeleted;
        }

        public int GrpId { get; private set; }

        public string GrpName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public List<RaiinListDetailDto> DetailList { get; private set; }
    }
}
