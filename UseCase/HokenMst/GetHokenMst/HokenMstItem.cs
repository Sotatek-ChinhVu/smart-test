namespace UseCase.HokenMst.GetHokenMst
{
    public class HokenMstItem
    {
        public HokenMstItem(string selectedValueMaster, string displayTextMasterWithoutHokenNo, string houbetu, string houbetuDisplayText, int hokenNumber, int hokenEdaNo)
        {
            SelectedValueMaster = selectedValueMaster;
            DisplayTextMasterWithoutHokenNo = displayTextMasterWithoutHokenNo;
            Houbetu = houbetu;
            HoubetuDisplayText = houbetuDisplayText;
            HokenNumber = hokenNumber;
            HokenEdaNo = hokenEdaNo;
        }

        public string SelectedValueMaster { get; private set; }

        public string DisplayTextMasterWithoutHokenNo { get; private set; }

        public string Houbetu { get; private set; }

        public string HoubetuDisplayText { get; private set; }

        public int HokenNumber { get; private set; }

        public int HokenEdaNo { get; private set; }
    }
}
