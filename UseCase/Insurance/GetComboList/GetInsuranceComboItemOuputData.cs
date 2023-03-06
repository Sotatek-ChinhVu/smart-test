namespace UseCase.Insurance.GetComboList
{
    public class GetInsuranceComboItemOuputData
    {
        public GetInsuranceComboItemOuputData(int hokenPid, string hokenName, bool isExpired, string displayRateOnly, bool isShaho, bool isKokuho)
        {
            HokenPid = hokenPid;
            HokenName = hokenName;
            IsExpired = isExpired;
            DisplayRateOnly = displayRateOnly;
            IsShaho = isShaho;
            IsKokuho = isKokuho;
        }

        public int HokenPid { get; private set; }

        public string HokenName { get; private set; }

        public bool IsExpired { get; private set; }

        public string DisplayRateOnly { get; private set; }

        public bool IsShaho { get; private set; }

        public bool IsKokuho { get; private set; }
    }
}
