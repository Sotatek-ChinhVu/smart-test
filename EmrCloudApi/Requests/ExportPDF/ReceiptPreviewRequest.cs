﻿namespace EmrCloudApi.Requests.ExportPDF
{
    public class ReceiptPreviewRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeikyuYm { get; set; }

        public int SinYm { get; set; }

        public int HokenId { get; set; }
    }
}
