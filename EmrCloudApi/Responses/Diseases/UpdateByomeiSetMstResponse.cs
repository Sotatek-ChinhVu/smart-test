namespace EmrCloudApi.Responses.Diseases
{
    public class UpdateByomeiSetMstResponse
    {
        public UpdateByomeiSetMstResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
