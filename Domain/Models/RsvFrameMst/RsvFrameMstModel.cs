using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RsvFrameMst
{
    public class RsvFrameMstModel
    {
        public RsvFrameMstModel(int hpId, int rsvFrameId, int rsvGrpId, int sortKey, string rsvFrameName, int tantoId, int kaId, int makeRaiin, int isDeleted)
        {
            HpId = hpId;
            RsvFrameId = rsvFrameId;
            RsvGrpId = rsvGrpId;
            SortKey = sortKey;
            RsvFrameName = rsvFrameName;
            TantoId = tantoId;
            KaId = kaId;
            MakeRaiin = makeRaiin;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public int RsvFrameId { get; private set; }
        public int RsvGrpId { get; private set; }
        public int SortKey { get; private set; }
        public string RsvFrameName { get; private set; }
        public int TantoId { get; private set; }
        public int KaId { get; private set; }
        public int MakeRaiin { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
