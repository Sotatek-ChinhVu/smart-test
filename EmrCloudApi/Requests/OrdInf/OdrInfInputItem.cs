﻿namespace EmrCloudApi.Requests.OrdInfs
{
    public class OdrInfInputItem
    {
        public int OdrKouiKbn { get; set; }
        public int InoutKbn { get; set; }
        public int DaysCnt { get; set; }
        public bool IsAutoAddItem { get; set; }
        public List<OdrInfDetailInputItem> OdrDetails { get; set; } = new();
    }
}
