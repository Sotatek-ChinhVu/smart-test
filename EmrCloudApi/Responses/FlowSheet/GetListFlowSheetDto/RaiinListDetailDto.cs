using Domain.Models.RaiinListSetting;

namespace EmrCloudApi.Responses.FlowSheet.GetListFlowSheetDto
{
    public class RaiinListDetailDto
    {
        public RaiinListDetailDto(int grpId, int kbnCd, int sortNo, string kbnName, string colorCd, int isDeleted, bool isOnlySwapSortNo)
        {
            GrpId = grpId;
            KbnCd = kbnCd;
            SortNo = sortNo;
            KbnName = kbnName;
            ColorCd = colorCd;
            IsDeleted = isDeleted;
            IsOnlySwapSortNo = isOnlySwapSortNo;
        }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public int SortNo { get; private set; }

        public string KbnName { get; private set; }

        public string ColorCd { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsOnlySwapSortNo { get; private set; }
    }
}
