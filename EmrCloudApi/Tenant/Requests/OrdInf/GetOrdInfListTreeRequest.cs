﻿namespace EmrCloudApi.Tenant.Requests.OrdInfs
{
    public class GetOrdInfListTreeRequest
    {
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
