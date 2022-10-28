using UseCase.Core.Sync.Core;
using UseCase.KarteInfs.GetListColumnName;

namespace UseCase.RaiinKubunMst.GetListColumnName
{
    public class GetColumnNameListOutputData : IOutputData
    {
        public List<string> ColumnName { get; private set; }

        public GetColumnNameListStatus Status { get; private set; }

        public GetColumnNameListOutputData(GetColumnNameListStatus status, List<string> columnName)
        {
            ColumnName = columnName;
            Status = status;
        }
    }
}
