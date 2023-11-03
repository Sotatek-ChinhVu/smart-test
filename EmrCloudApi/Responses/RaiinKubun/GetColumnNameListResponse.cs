namespace EmrCloudApi.Responses.RaiinKubun
{
    public class GetColumnNameListResponse
    {
        public GetColumnNameListResponse(List<GetColumnNameItem> columnNames)
        {
            ColumnNames = columnNames;
        }

        public List<GetColumnNameItem> ColumnNames { get; private set; } = new();
    }
}
