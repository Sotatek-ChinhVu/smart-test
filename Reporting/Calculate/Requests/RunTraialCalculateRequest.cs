﻿using Reporting.Calculate.Ika.Models;

namespace Reporting.Calculate.Requests
{
    public class RunTraialCalculateRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public List<OrderInfo> OrderInfoList { get; set; } = new List<OrderInfo>();

        public ReceptionModel Reception { get; set; }

        public bool CalcFutan { get; set; }
    }
}
