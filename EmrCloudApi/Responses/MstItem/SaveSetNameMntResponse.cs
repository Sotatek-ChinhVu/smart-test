namespace EmrCloudApi.Responses.MstItem
{
    public class SaveSetNameMntResponse
    {
        public SaveSetNameMntResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
