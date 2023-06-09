namespace EmrCloudApi.Requests.RaiinListSetting
{
    public class RaiinListMstDto
    {
        public RaiinListMstDto(int grpId, string grpName, int sortNo, int isDeleted, List<RaiinListDetailDto> raiinListDetailsList)
        {
            GrpId = grpId;
            GrpName = grpName;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            RaiinListDetailsList = raiinListDetailsList;
        }

        public int GrpId { get; private set; }

        public string GrpName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public List<RaiinListDetailDto> RaiinListDetailsList { get; private set; }
    }
}
