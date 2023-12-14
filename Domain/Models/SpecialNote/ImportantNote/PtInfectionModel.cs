﻿using System.Text.Json.Serialization;

namespace Domain.Models.SpecialNote.ImportantNote
{
    public class PtInfectionModel
    {
        [JsonConstructor]
        public PtInfectionModel(int hpId, long ptId, long seqNo, int sortNo, string byomeiCd, string byotaiCd, string byomei, int startDate, string cmt, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            SortNo = sortNo;
            ByomeiCd = byomeiCd;
            ByotaiCd = byotaiCd;
            Byomei = byomei;
            StartDate = startDate;
            Cmt = cmt;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public int SortNo { get; private set; }

        public string ByomeiCd { get; private set; }

        public string ByotaiCd { get; private set; }

        public string Byomei { get; private set; }

        public int StartDate { get; private set; }

        public string Cmt { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
