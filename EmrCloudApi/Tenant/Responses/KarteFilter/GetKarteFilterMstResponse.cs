using UseCase.KarteFilter;

namespace EmrCloudApi.Tenant.Responses.KarteFilter;

public class GetKarteFilterMstResponse
{
    public List<KarteFilterMstModelOutputItem>? KarteFilters { get; set; }
    public GetKarteFilterMstResponse(List<KarteFilterMstModelOutputItem>? karteFilters)
    {
        KarteFilters = karteFilters;
    }
}
