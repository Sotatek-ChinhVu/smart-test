namespace Domain.Models.RsvGrpMst
{
    public class RsvGrpMstModel
    {
        public RsvGrpMstModel(int hpId, int rsvGrpId, int sortKey, string rsvGrpName, int isDeleted)
        {
            HpId = hpId;
            RsvGrpId = rsvGrpId;
            SortKey = sortKey;
            RsvGrpName = rsvGrpName;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public int RsvGrpId { get; private set; }
        public int SortKey { get; private set; }
        public string RsvGrpName { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
