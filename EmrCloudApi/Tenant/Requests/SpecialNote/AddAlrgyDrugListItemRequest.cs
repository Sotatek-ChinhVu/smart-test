using System.ComponentModel;

namespace EmrCloudApi.Tenant.Requests.SpecialNote
{
    public class AddAlrgyDrugListItemRequest
    {
        public long PtId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string DrugName { get; set; } = string.Empty;

        [DefaultValue (0)]
        public int StartDate { get; set; }

        [DefaultValue(99999999)]
        public int EndDate { get; set; }

        public string Cmt { get; set; } = string.Empty;
    }
}
