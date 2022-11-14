using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.NextOrder
{
    public class RsvkrtKarteInfModel
    {
        public RsvkrtKarteInfModel(int hpId, long ptId, int sinDate, long raiinNo, int karteKbn, long seqNo, string text, string richText, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            KarteKbn = karteKbn;
            SeqNo = seqNo;
            Text = text;
            RichText = richText;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int KarteKbn { get; private set; }

        public long SeqNo { get; private set; }

        public string Text { get; private set; }

        public string RichText { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
