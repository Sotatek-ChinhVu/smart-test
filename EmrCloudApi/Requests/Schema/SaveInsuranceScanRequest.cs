﻿namespace EmrCloudApi.Requests.Schema
{
    public class SaveInsuranceScanRequest
    {
        public long PtId { get; set; }

        public int HokenGrp { get; set; }

        public int HokenId { get; set; }

        public string UrlOldImage { get; set; } = string.Empty;
    }
}
