﻿using Domain.Models.HokenMst;

namespace EmrCloudApi.Tenant.Responses.HokenMst
{
    public class GetDetailHokenMstResponse
    {
        public GetDetailHokenMstResponse(HokenMasterModel data)
        {
            Data = data;
        }

        public HokenMasterModel Data { get; set; }
    }
}
