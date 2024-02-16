﻿namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class Yousiki1InfRequest
    {

        public long PtId { get; set; }

        public int SinYm { get; set; }

        public int DataType { get; set; }

        public int Status { get; set; }

        public int IsDeleted { get; set; }

        public List<CategoryRequest> CategoryRequests { get; set; } = new();

        public int SeqNo { get; set; }

        public TabYousikiRequest TabYousikiRequest { get; set; } = new();
    }
}