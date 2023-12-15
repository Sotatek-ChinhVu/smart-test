namespace UseCase.LastDayInformation.SaveSettingLastDayInfoList
{
    public class SaveSettingLastDayInfoListInputItem
    {
        public SaveSettingLastDayInfoListInputItem(int grpId, int sortNo, string grpName, int isDeleted, List<SaveOdrDateDetailInputItem> saveOdrDateDetailItems)
        {
            GrpId = grpId;
            SortNo = sortNo;
            GrpName = grpName;
            IsDeleted = isDeleted;
            SaveOdrDateDetailItems = saveOdrDateDetailItems;
        }

        public int GrpId {  get; private set; }

        public int SortNo { get; private set; }

        public string GrpName { get; private set; }

        public int IsDeleted { get; private set; }

        public List<SaveOdrDateDetailInputItem> SaveOdrDateDetailItems { get; private set; }
    }
}
