namespace EmrCloudApi.Requests.PtGroupMst
{
    public class GroupItemDtoRequest
    {
        public int GrpId { get; set; } 

        public string GrpCode { get; set; } = string.Empty;

        public long SeqNo { get; set; }

        public string GrpCodeName { get; set; } = string.Empty;

        public int SortNo { get; set; }
    }
}
