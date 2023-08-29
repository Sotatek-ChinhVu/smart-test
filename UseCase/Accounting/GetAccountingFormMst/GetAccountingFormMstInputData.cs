using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetAccountingFormMst
{
    public class GetAccountingFormMstInputData : IInputData<GetAccountingFormMstOutputData>
    {
        public GetAccountingFormMstInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
