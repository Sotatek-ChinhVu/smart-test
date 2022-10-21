﻿using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetLastRaiinInfsResponse
{
    public GetLastRaiinInfsResponse(List<ReceptionModel> data)
    {
        Data = data;
    }

    public List<ReceptionModel> Data { get; private set; }
}
