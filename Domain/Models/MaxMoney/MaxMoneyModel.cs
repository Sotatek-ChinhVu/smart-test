namespace Domain.Models.MaxMoney
{
    public class MaxMoneyModel
    {
       public MaxMoneyModel(int kohiId, int gendoGaku, int remainGendoGaku, int rate, string houbetu, string hokenName, int sinDateYM, int futanKbn, int monthLimitFutan, int isLimitListSum, string displaySinDateYM, IEnumerable<LimitListModel> listLimits)
        {
            KohiId = kohiId;
            GendoGaku = gendoGaku;
            RemainGendoGaku = remainGendoGaku;
            Rate = rate;
            Houbetu = houbetu;
            HokenName = hokenName;
            SinDateYM = sinDateYM;
            FutanKbn = futanKbn;
            MonthLimitFutan = monthLimitFutan;
            IsLimitListSum = isLimitListSum;
            DisplaySinDateYM = displaySinDateYM;
            ListLimits = listLimits;
        }

        public int KohiId { get; private set; }
        public int GendoGaku { get; private set; }
        public int RemainGendoGaku { get; private set; }
        public int Rate { get; private set; }
        public string Houbetu { get; private set; }
        public string HokenName { get; private set; }
        public int SinDateYM { get; private set; }
        public int FutanKbn { get; private set; }
        public int MonthLimitFutan { get; private set; }
        public int IsLimitListSum { get; private set; }
        public string DisplaySinDateYM { get; private set; }
        public bool IsLimitMaxMoney
        {
            get => FutanKbn == 1 && MonthLimitFutan == 0;
        }
        public string HeaderText
        {
            get => Houbetu + " " + HokenName;
        }
        public bool IsToltalGakuDisplay
        {
            get => IsLimitListSum == 1;
        }
        public IEnumerable<LimitListModel> ListLimits { get;private set; }
    }
}
