﻿namespace CalculateService.Requests
{
    public class GetSinMeiListRequest
    {
        public List<long> RaiinNoList { get; set; } = new List<long>();

        public int SinDate { get; set; }

        public long PtId { get; set; }

        public int HpId { get; set; }

        public int SinYm { get; set; }

        public int HokenId { get; set; }

        public int SeikyuYm { get; set; }

        /*Mode = 3 Kaikei, 21 AccountingCard, 2 ReceCheck*/
        public int SinMeiMode { get; set; }
    }
}
