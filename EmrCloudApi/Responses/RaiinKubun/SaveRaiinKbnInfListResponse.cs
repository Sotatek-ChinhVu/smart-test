namespace EmrCloudApi.Responses.RaiinKubun
{
    public class SaveRaiinKbnInfListResponse
    {
        public SaveRaiinKbnInfListResponse(bool success)
        {
            Success = success;

        }

        public bool Success { get; private set; }
    }
}
