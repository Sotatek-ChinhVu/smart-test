﻿namespace Reporting.Calculate.Requests
{
    public class CalculateOneRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int SeikyuUp { get; set; }

        public string Prefix { get; set; } = string.Empty;
    }
}
