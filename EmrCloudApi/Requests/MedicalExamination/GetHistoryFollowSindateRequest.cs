﻿namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetHistoryFollowSindateRequest
    {
        public long PtId { get; set; }

        public int UserId { get; set; }

        public int SinDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public int DeleteConditon { get; set; }

        public int IsShowApproval { get; set; }

        public long RaiinNo { get; set; }

        public byte Flag { get; set; }
    }
}