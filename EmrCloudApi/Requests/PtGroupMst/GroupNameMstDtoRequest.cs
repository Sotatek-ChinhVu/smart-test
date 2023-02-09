namespace EmrCloudApi.Requests.PtGroupMst
{
    public class GroupNameMstDtoRequest
    {
        public int GrpId { get; set; }

        public int SortNo { get; set; }

        public string GrpName { get; set; } = string.Empty;

        public List<GroupItemDtoRequest> GroupItems { get; set; } = new List<GroupItemDtoRequest>();
    }
}
