namespace EmrCloudApi.Tenant.Requests.SetMst
{
    public class CopyPasteSetMstRequest
    {
        public int HpId { get; set; }

        public int UserId { get; set; }

        public int CopySetCd { get; set; }

        public int PasteSetCd { get; set; }
    }
}
