namespace Domain.Models.SpecialNote.PatientInfo
{
    public class SeikaturekiInfModel
    {
        public SeikaturekiInfModel(long id, int hpId, long ptId, long seqNo, string text)
        {
            Id = id;
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            Text = text;
        }

        public SeikaturekiInfModel()
        {
            Text = string.Empty;
        }


        public long Id { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public string Text { get; private set; }
    }
}
