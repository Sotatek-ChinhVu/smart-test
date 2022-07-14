using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientInfor
{
    public class PatientInfor
    {
        public HpId HpId { get; private set;}
        public PtId PtId { get; private set;}
        public ReferenceNo ReferenceNo { get; private set;}
        public SeqNo SeqNo { get; private set;}
        public PtNum PtNum { get; private set;}
        public KanaName KanaName { get; private set;}
        public KanjiName KanjiName { get; private set;}
        public PatientInfor(HpId hpId, PtId ptId, ReferenceNo referenceNo, SeqNo seqNo, PtNum ptNum, KanaName kanaName, KanjiName kanjiName)
        {
            HpId = hpId;
            PtId = ptId;
            ReferenceNo = referenceNo;
            SeqNo = seqNo;
            PtNum = ptNum;
            KanaName = kanaName;
            KanjiName = kanjiName;
        }

        public PatientInfor(int hpId, long ptId, long referenceNo, long seqNo, long ptNum, string kanaName, string kanjiName)
        {
            HpId = HpId.From(hpId);
            PtId = PtId.From(ptId);
            ReferenceNo = ReferenceNo.From(referenceNo);
            SeqNo = SeqNo.From(seqNo);
            PtNum = PtNum.From(ptNum);
            KanaName = KanaName.From(kanaName);
            KanjiName = KanjiName.From(kanjiName);
        }
    }
}
