namespace UseCase.HokenMst.GetHokenMst
{
    public class HokenMstItem
    {
        public HokenMstItem(string houbetu, string houbetuDisplayText, int hokenNumber, int hokenEdaNo)
        {
            Houbetu = houbetu;
            HoubetuDisplayText = houbetuDisplayText;
            HokenNumber = hokenNumber;
            HokenEdaNo = hokenEdaNo;
        }

        public string Houbetu { get; private set; }

        public string HoubetuDisplayText { get; private set; }

        public int HokenNumber { get; private set; }

        public int HokenEdaNo { get; private set; }
    }
}
