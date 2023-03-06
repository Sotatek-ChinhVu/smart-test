namespace EmrCloudApi.Responses.Accounting
{
    public class RecaculateTestResponse
    {
        public RecaculateTestResponse(string result)
        {
            Result = result;
        }
        public string Result { get; set; }
    }
}