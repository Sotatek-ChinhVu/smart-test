using Domain.Models.KarteFilterMst;
using UseCase.KarteFilter.GetListKarteFilter;

namespace EmrCloudApi.Responses.KarteFilter;

public class GetKarteFilterMstResponse
{
    public List<KarteFilterMstModel> KarteFilters { get; private set; }

    public GetKarteFilterMstResponse(List<KarteFilterMstModel> karteFilters)
    {
        KarteFilters = karteFilters;
    }
}
