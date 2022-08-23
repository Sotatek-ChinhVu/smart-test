﻿namespace Domain.Models.PtCmtInf
{
    public class PtCmtInfModel
    {
        public PtCmtInfModel(int hpId, long ptId, int seqNo, string text, int isDeleted, long id)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            Text = text;
            IsDeleted = isDeleted;
            Id = id;
        }

        public PtCmtInfModel()
        {
            Text = string.Empty;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SeqNo { get; private set; }

        public string Text { get; private set; }

        public int IsDeleted { get; private set; }

        public long Id { get; private set; }

    }
}
