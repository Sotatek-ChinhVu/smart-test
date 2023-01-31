using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetWarningMemo
{
    public class GetWarningMemoOutputData : IOutputData
    {
        public GetWarningMemoOutputData(string warningMemo)
        {
            WarningMemo = warningMemo;
        }

        public string WarningMemo { get; private set; }
    }
}
