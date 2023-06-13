﻿namespace EmrCloudApi.Requests.OrdInfs
{
    public class KarteItem
    {
        public long RaiinNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public string Text { get; set; } = string.Empty;
        public int IsDeleted { get; set; }
        public string RichText { get; set; } = string.Empty;
    }
}
