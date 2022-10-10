namespace EmrCloudApi.Tenant.Requests.StickyNote
{
    public class GetStickyNoteRequest
    {
        public int HpId { get; set; }
        public int PtId { get; set; }
        public int IsDeleted { get; set; }
    }
}
