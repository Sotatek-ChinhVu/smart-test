using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.GetListColumnName
{
    public class GetColumnNameListInputData : IInputData<GetColumnNameListOutputData>
    {
        public GetColumnNameListInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
