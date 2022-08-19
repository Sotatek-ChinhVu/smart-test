namespace EmrCloudApi.Tenant.Responses.InputItem
{
    public class UpdateAdoptedInputItemResponse
    {
        public UpdateAdoptedInputItemResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
