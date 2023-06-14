﻿namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetMedicalExaminationHistoryRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int DeleteCondition { get; set; }
        public int UserId { get; set; }
        public long FilterId { get; set; }
        public int IsShowApproval { get; set; }
        public List<RaiinBookmarked> RaiinBookmarked { get; set; } = new();
    }

    public class RaiinBookmarked
    {
        public long RaiinNo { get; set; }
        public bool Flag { get; set; }
    }
}
