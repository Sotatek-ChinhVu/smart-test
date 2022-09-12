using UseCase.Core.Sync.Core;

namespace UseCase.TotalMoney.GetMaxMoney
{
    public class GetMaxMoneyInputData : IInputData<GetMaxMoneyOutputData>
    {
        public GetMaxMoneyInputData(long ptId, int hokenId, int futanKbn, int rate, int gendoGaku, int monthLimitFutan, int houbetsuNumber, int hokenName, int isLimitListSum)
        {
            PtId = ptId;
            HokenId = hokenId;
            FutanKbn = futanKbn;
            Rate = rate;
            GendoGaku = gendoGaku;
            MonthLimitFutan = monthLimitFutan;
            HoubetsuNumber = houbetsuNumber;
            HokenName = hokenName;
            IsLimitListSum = isLimitListSum;
        }

        public long PtId { get; private set; }
        public int HokenId { get; private set; }
        public int FutanKbn { get; private set; }
        public int Rate { get; private set; }
        public int GendoGaku { get; private set; }
        public int MonthLimitFutan { get; private set; }
        public int HoubetsuNumber { get; private set; }
        public int HokenName { get; private set; }
        public int IsLimitListSum { get; private set; }
    }
}
