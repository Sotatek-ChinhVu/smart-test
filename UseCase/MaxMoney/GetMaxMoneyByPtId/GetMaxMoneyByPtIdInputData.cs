using Helper.Common;
using Helper.Extension;
using UseCase.Core.Sync.Core;

namespace UseCase.MaxMoney.GetMaxMoneyByPtId
{
    public class GetMaxMoneyByPtIdInputData : IInputData<GetMaxMoneyByPtIdOutputData>
    {
        public GetMaxMoneyByPtIdInputData(int hpId, long ptId, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
        }

        public int HpId { get; private set }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int SinDateY => CIUtil.Copy(SinDate.AsString(), 1, 4).AsInteger();

        public int SinDateM => CIUtil.Copy(SinDate.AsString(), 5, 2).AsInteger();

        public int SinYM => CIUtil.Copy(SinDate.AsString(), 1, 6).AsInteger();
    }
}
