namespace UseCase.SpecialNote.AddAlrgyDrugList
{
    public class AddAlrgyDrugListItemInputData
    {
        public AddAlrgyDrugListItemInputData(long ptId, int sortNo, string itemCd, string drugName, int startDate, int endDate, string cmt)
        {
            PtId = ptId;
            SortNo = sortNo;
            ItemCd = itemCd;
            DrugName = drugName;
            StartDate = startDate;
            EndDate = endDate;
            Cmt = cmt;
        }

        public long PtId { get; private set; }

        public int SortNo { get; private set; }

        public string ItemCd { get; private set; }

        public string DrugName { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string Cmt { get; private set; }
    }
}
