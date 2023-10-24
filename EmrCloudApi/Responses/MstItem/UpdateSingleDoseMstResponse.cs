namespace EmrCloudApi.Responses.MstItem
{
    public class UpdateSingleDoseMstResponse
    {
        public UpdateSingleDoseMstResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
