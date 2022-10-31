using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.GetListColumnName
{
    public class GetColumnNameListOutputData : IOutputData
    {
        public List<string> ColumnNames { get; private set; }

        public GetColumnNameListStatus Status { get; private set; }

        public GetColumnNameListOutputData(GetColumnNameListStatus status, List<string> columnName)
        {
            ColumnNames = columnName;
            Status = status;
        }
    }
}
