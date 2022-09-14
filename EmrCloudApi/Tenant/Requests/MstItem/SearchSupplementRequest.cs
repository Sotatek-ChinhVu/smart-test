﻿namespace EmrCloudApi.Tenant.Requests.MstItem
{
    public class SearchSupplementRequest
    {
        public string SearchValue { get; set; } = String.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
