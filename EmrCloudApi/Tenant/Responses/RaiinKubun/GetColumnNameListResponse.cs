namespace EmrCloudApi.Tenant.Responses.RaiinKubun
{
    public class GetColumnNameListResponse
    {
        public GetColumnNameListResponse(List<string> columnNames)
        {
            ColumnNames = columnNames;
        }

        public List<string> ColumnNames { get; private set; } = new();
    }
}
