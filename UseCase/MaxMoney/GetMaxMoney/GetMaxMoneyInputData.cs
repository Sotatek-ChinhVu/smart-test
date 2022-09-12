using Helper.Common;
using Helper.Extension;
using UseCase.Core.Sync.Core;

namespace UseCase.MaxMoney.GetMaxMoney
{
    public class GetMaxMoneyInputData : IInputData<GetMaxMoneyOutputData>
    {
        public GetMaxMoneyInputData(long ptId, int hpId, int hokenKohiId, int rate, int gendoGaku, int sinDate, int futanKbn, int monthLimitFutan, string houbetsuNumber, string hokenName, int isLimitListSum, int futanRate, int limitFutan)
        {
            PtId = ptId;
            HpId = hpId;
            HokenKohiId = hokenKohiId;
            Rate = rate;
            GendoGaku = gendoGaku;
            SinDate = sinDate;
            FutanKbn = futanKbn;
            MonthLimitFutan = monthLimitFutan;
            HoubetsuNumber = houbetsuNumber;
            HokenName = hokenName;
            IsLimitListSum = isLimitListSum;
            FutanRate = futanRate;
            LimitFutan = limitFutan;
        }

        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int HokenKohiId { get; private set; }
        public int Rate { get; set; }
        public int GendoGaku { get; set; } //PtKohi
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

        #region By hoken mst
        public int FutanKbn { get; private set; }
        public int MonthLimitFutan { get; private set; }
        public string HoubetsuNumber { get; private set; }
        public string HokenName { get; private set; }
        public int IsLimitListSum { get; private set; }
        public int FutanRate { get; private set; }
        public int LimitFutan { get; private set; }
        #endregion
    }
}
