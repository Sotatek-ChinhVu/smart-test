using Helper.Extension;

namespace Domain.Models.MaxMoney
{
    public class MaxMoneyInfoHokenModel
    {
        public MaxMoneyInfoHokenModel(int hokenKohiId, int rate, int sinYM, int futanKbn, int monthLimitFutan, int gendoGaku, string houbetsu, string hokenName, int isLimitListSum, int moneyLimitListFlag, int futanRate, int limitFutan)
        {
            HokenKohiId = hokenKohiId;
            Rate = rate;
            SinYM = sinYM;
            FutanKbn = futanKbn;
            MonthLimitFutan = monthLimitFutan;
            GendoGaku = gendoGaku;
            Houbetsu = houbetsu;
            HokenName = hokenName;
            IsLimitListSum = isLimitListSum;
            MoneyLimitListFlag = moneyLimitListFlag;
            FutanRate = futanRate;
            LimitFutan = limitFutan;
        }

        public int HokenKohiId { get; private set; }
        public int Rate { get; private set; }
        public int SinYM { get; private set; }
        public int FutanKbn { get; private set; }
        public int MonthLimitFutan { get; private set; }
        public int GendoGaku { get; private set; }
        public string Houbetsu { get; private set; }
        public string HokenName { get; private set; }
        public int IsLimitListSum { get; private set; }
        public int MoneyLimitListFlag { get; private set; }
        public int FutanRate { get; private set; }
        public int LimitFutan { get; private set; }
        public string DisplaySinDateYM
        {
            get => (SinYM / 100).AsString() + "/" + ((SinYM % 100 < 10) ? ("0" + (SinYM % 100).AsString()) : (SinYM % 100).AsString());
        }
        public bool IsLimitMaxMoney
        {
            get => FutanKbn == 1 && MonthLimitFutan == 0;
        }
        public bool IsTotalGakuDisplay
        {
            get => IsLimitListSum == 1;
        }
    }
}
