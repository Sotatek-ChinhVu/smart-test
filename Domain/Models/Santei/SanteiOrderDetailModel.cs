namespace Domain.Models.Santei
{
    public class SanteiOrderDetailModel
    {
        public SanteiOrderDetailModel(long id, int hpId, int santeiGrpCd, int seqNo, string itemCd, double suryo, int addType)
        {
            Id = id;
            HpId = hpId;
            SanteiGrpCd = santeiGrpCd;
            SeqNo = seqNo;
            ItemCd = itemCd;
            Suryo = suryo;
            AddType = addType;
        }

        public long Id { get; private set; }

        public int HpId { get; private set; }

        public int SanteiGrpCd { get; private set; }

        public int SeqNo { get; private set; }

        public string ItemCd { get; private set; }

        public double Suryo { get; private set; }

        public int AddType { get; private set; }
    }
}
