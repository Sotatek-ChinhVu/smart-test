namespace Domain.Models.RaiinKubunMst
{
    public class RsvGrpMstModel
    {
        public RsvGrpMstModel(int rsvGrpId, int sortKey, string rsvGrpName, int isDeleted)
        {
            RsvGrpId = rsvGrpId;
            SortKey = sortKey;
            RsvGrpName = rsvGrpName;
            IsDeleted = isDeleted;
        }

        public int RsvGrpId { get; private set; }

        public int SortKey { get; private set; }

        public string RsvGrpName { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
