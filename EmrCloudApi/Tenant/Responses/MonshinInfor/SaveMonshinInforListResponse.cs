namespace EmrCloudApi.Tenant.Responses.MonshinInfor
{
    public class SaveMonshinInforListResponse
    {
        public SaveMonshinInforListResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
