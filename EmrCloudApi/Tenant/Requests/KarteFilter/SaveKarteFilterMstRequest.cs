using UseCase.KarteFilter.SaveListKarteFilter;

namespace EmrCloudApi.Tenant.Requests.KarteFilter;

public class SaveKarteFilterMstRequest
{
    public List<SaveKarteFilterMstInputItem> KarteFilters { get; set; } = new List<SaveKarteFilterMstInputItem>();
}
