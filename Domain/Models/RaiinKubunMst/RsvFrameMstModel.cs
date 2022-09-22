namespace Domain.Models.RaiinKubunMst
{
    public class RsvFrameMstModel
    {
        public RsvFrameMstModel(int rsvGrpId, int rsvFrameId, int sortKey, string rsvFrameName, int tantoId, int kaId, int makeRaiin, int isDeleted)
        {
            RsvGrpId = rsvGrpId;
            RsvFrameId = rsvFrameId;
            SortKey = sortKey;
            RsvFrameName = rsvFrameName;
            TantoId = tantoId;
            KaId = kaId;
            MakeRaiin = makeRaiin;
            IsDeleted = isDeleted;
        }

        public int RsvGrpId { get; private set; }

        public int RsvFrameId { get; private set; }

        public int SortKey { get; private set; }

        public string RsvFrameName { get; private set; }

        public int TantoId { get; private set; }

        public int KaId { get; private set; }

        public int MakeRaiin { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
