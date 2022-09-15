namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class UpdateAdoptedTenItemResponse
    {
        public UpdateAdoptedTenItemResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
