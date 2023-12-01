namespace UseCase.LastDayInformation.SaveSettingLastDayInfoList
{
    public class SaveOdrDateDetailInputItem
    {
        public int GrpId { get; set; }

        public int SeqNo { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public int IsDeleted { get; set; }

        public int SortNo { get; set; }
    }
}
