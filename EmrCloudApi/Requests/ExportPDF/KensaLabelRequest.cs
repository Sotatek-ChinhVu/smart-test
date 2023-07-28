﻿using Reporting.KensaLabel.Model;

namespace EmrCloudApi.Requests.ExportPDF
{
    public class KensaLabelRequest : ReportRequestBase
    {
        public long PtId { get; set; }

        public long RaiinNo { get; set; }

        public int SinDate { get; set; }

        public string ItemCd { get; set; }

        public string ContainerName { get; set; }

        public long ContainerCd { get; set; }

        public int Count { get; set; }

        public string PrinterName { get; set; }

        public int InoutKbn { get; set; }

        public int OdrKouiKbn { get; set; }
    }
}
