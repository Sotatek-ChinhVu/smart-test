namespace EmrCloudApi.Requests.RaiinListSetting
{
    public class RaiinListDetailDto
    {
        public RaiinListDetailDto(int grpId, int kbnCd, int sortNo, string kbnName, string colorCd, int isDeleted, bool isOnlySwapSortNo, List<RaiinListDocDto> raiinListDoc, List<RaiinListItemDto> raiinListItem, List<RaiinListFileDto> raiinListFile, KouiKbnCollectionDto kouCollection)
        {
            GrpId = grpId;
            KbnCd = kbnCd;
            SortNo = sortNo;
            KbnName = kbnName;
            ColorCd = colorCd;
            IsDeleted = isDeleted;
            IsOnlySwapSortNo = isOnlySwapSortNo;
            RaiinListDoc = raiinListDoc;
            RaiinListItem = raiinListItem;
            RaiinListFile = raiinListFile;
            KouCollection = kouCollection;
        }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public int SortNo { get; private set; }

        public string KbnName { get; private set; }

        public string ColorCd { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsOnlySwapSortNo { get; private set; }

        public List<RaiinListDocDto> RaiinListDoc { get; private set; }

        public List<RaiinListItemDto> RaiinListItem { get; private set; }

        public List<RaiinListFileDto> RaiinListFile { get; private set; }

        public KouiKbnCollectionDto KouCollection { get; private set; }
    }
}
