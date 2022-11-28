namespace EmrCloudApi.Responses.RaiinKubun
{
    public class GetColumnNameItem
    {
        public GetColumnNameItem(string field, string headerName)
        {
            Field = field;
            HeaderName = headerName;
        }

        public string Field { get; private set; }
        public string HeaderName { get; private set; }
    }
}
