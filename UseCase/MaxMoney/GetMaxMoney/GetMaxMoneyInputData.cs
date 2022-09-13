using Helper.Common;
using Helper.Extension;
using UseCase.Core.Sync.Core;

namespace UseCase.MaxMoney.GetMaxMoney
{
    public class GetMaxMoneyInputData : IInputData<GetMaxMoneyOutputData>
    {
        public GetMaxMoneyInputData(long ptId, int hpId, int hokenKohiId, int sinDate)
        {
            PtId = ptId;
            HpId = hpId;
            HokenKohiId = hokenKohiId;
            SinDate = sinDate;
        }

        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int HokenKohiId { get; private set; } //PtKohi
        public int SinDate { get; private set; }
        public int SinDateY
        {
            get => CIUtil.Copy(SinDate.AsString(), 1, 4).AsInteger();
        }
        public int SinDateM
        {
            get => CIUtil.Copy(SinDate.AsString(), 5, 2).AsInteger();
        }
        public int SinYM
        {
            get => CIUtil.Copy(SinDate.AsString(), 1, 6).AsInteger();
        }
    }
}
