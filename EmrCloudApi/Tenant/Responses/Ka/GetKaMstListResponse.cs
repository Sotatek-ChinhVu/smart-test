﻿using Domain.Models.Ka;

namespace EmrCloudApi.Tenant.Responses.Ka;

public class GetKaMstListResponse
{
    public GetKaMstListResponse(List<KaMstModel> departments)
    {
        Departments = departments;
    }

    public List<KaMstModel> Departments { get; private set; }
}
