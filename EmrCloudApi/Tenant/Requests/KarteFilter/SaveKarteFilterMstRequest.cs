﻿using UseCase.KarteFilter.SaveListKarteFilter;

namespace EmrCloudApi.Tenant.Requests.KarteFilter;

public class SaveKarteFilterMstRequest
{
    public List<SaveKarteFilterMstModelInputItem> KarteFilters { get; set; } = new List<SaveKarteFilterMstModelInputItem>();
}
