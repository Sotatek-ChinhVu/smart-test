namespace EmrCloudApi.Tenant.Requests.SpecialNote
{
    public class AddAlrgyDrugListItem
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeqNo { get; set; }

        public int SortNo { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string DrugName { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string Cmt { get; set; } = string.Empty;

        public int IsDeleted { get; set; }
    }
}
