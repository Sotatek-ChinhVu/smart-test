namespace UseCase.HokenMst.GetHokenMst
{
    public class HokenMstItem
    {
        public HokenMstItem(string selectedValueMaster, string displayTextMasterWithoutHokenNo)
        {
            SelectedValueMaster = selectedValueMaster;
            DisplayTextMasterWithoutHokenNo = displayTextMasterWithoutHokenNo;
        }

        public string SelectedValueMaster { get; private set; }

        public string DisplayTextMasterWithoutHokenNo { get; private set; }
    }
}
