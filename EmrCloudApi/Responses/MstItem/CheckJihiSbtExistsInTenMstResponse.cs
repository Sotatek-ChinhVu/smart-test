namespace EmrCloudApi.Responses.MstItem
{
    public class CheckJihiSbtExistsInTenMstResponse
    {
        public CheckJihiSbtExistsInTenMstResponse(bool status)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}
