﻿using UseCase.Core.Sync.Core;

namespace UseCase.KarteInf.GetList
{
    public class GetListKarteInfOuputItem : IOutputData
    {
        public GetListKarteInfOuputItem(int hpId, long raiinNo, int karteKbn, long seqNo, long ptId, int sinDate, string text, int isDeleted, string richText, bool isAutoFill)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            KarteKbn = karteKbn;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            Text = text;
            IsDeleted = isDeleted;
            RichText = richText;
            IsAutoFill = isAutoFill;
        }

        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public int KarteKbn { get; private set; }
        public long SeqNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; private set; }
        public int IsDeleted { get; private set; }
        public string RichText { get; private set; }
        public bool IsAutoFill { get; private set; }
    }
}
