﻿namespace EmrCloudApi.Tenant.Requests.PtKyuseiInf
{
    public class GetPtKyuseiInfRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
