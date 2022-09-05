namespace EmrCloudApi.Tenant.Responses.SetMst
{
    public class CopyPasteSetMstResponse
    {
        public CopyPasteSetMstResponse(bool status)
        {
            Status = status;
        }
        public bool Status { get; private set; } = false;
    }
}
