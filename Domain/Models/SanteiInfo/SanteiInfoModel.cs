namespace Domain.Models.SanteiInfo
{
    public class SanteiInfoModel
    {
        public SanteiInfoModel(int hpId, long ptId, string itemCd, int seqNo, int alertDays, int alertTerm, long id, IEnumerable<SanteiInfoDetailModel>? santeiInfoDetailModel)
        {
            HpId = hpId;
            PtId = ptId;
            ItemCd = itemCd;
            SeqNo = seqNo;
            AlertDays = alertDays;
            AlertTerm = alertTerm;
            Id = id;
            SanteiInfoDetailModel = santeiInfoDetailModel;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public string ItemCd { get; private set; }
        public int SeqNo { get; private set; }
        public int AlertDays { get; private set; }
        public int AlertTerm { get; private set; }
        public long Id { get; private set; }
        public IEnumerable<SanteiInfoDetailModel>? SanteiInfoDetailModel { get; private set; }
    }
}
