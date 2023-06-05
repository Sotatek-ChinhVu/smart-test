namespace EmrCloudApi.Responses.NextOrder
{
    public class CheckNextOrdHaveOrdResponse
    {
        public CheckNextOrdHaveOrdResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
