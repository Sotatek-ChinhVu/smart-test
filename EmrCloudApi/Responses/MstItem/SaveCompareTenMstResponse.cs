namespace EmrCloudApi.Responses.MstItem
{
    public class SaveCompareTenMstResponse
    {
        public SaveCompareTenMstResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
