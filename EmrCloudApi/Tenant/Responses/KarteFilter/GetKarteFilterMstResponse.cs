﻿using UseCase.KarteFilter.GetListKarteFilter;

namespace EmrCloudApi.Tenant.Responses.KarteFilter;

public class GetKarteFilterMstResponse
{
    public List<KarteFilterMstOutputItem> KarteFilters { get; private set; }

    public GetKarteFilterMstResponse(List<KarteFilterMstOutputItem> karteFilters)
    {
        KarteFilters = karteFilters;
    }
}
