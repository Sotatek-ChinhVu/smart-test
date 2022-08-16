using UseCase.KarteFilter.GetListKarteFilter;

namespace EmrCloudApi.Tenant.Responses.KarteFilter;

public class GetKarteFilterMstResponse
{
    public List<GetKarteFilterMstModelOutputItem>? KarteFilters { get; private set; }
    public GetKarteFilterMstResponse(List<GetKarteFilterMstModelOutputItem>? karteFilters)
    {
        KarteFilters = karteFilters;
    }
}
