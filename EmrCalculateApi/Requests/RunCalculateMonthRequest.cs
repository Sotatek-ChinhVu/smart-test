﻿namespace EmrCalculateApi.Requests
{
    public class RunCalculateMonthRequest
    {
        public int HpId { get; set; }
        
        public int SeikyuYm { get; set; }
        
        public List<long> PtIds { get; set; } = new List<long>();

        public string PreFix { get; set; } = string.Empty;

        public string HostName { get; set; } = string.Empty;

        public string UniqueKey { get; set; } = string.Empty;
    }
}
