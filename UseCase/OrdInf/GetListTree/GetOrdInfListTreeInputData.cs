using Domain.CommonObject;
using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetListTrees
{
    public class GetOrdInfListTreeInputData : IInputData<GetOrdInfListTreeOutputData>
    {
        public PtId PtId { get; private set; }
        public HpId HpId { get; private set; }
        public RaiinNo RaiinNo { get; private set; }
        public SinDate SinDate { get; private set; }

        public GetOrdInfListTreeInputData(long ptId, int hpId, long raiinNo, int sinDate)
        {
            PtId = PtId.From(ptId);
            HpId = HpId.From(hpId);
            RaiinNo = RaiinNo.From(raiinNo);
            SinDate = SinDate.From(sinDate);
        }
    }
}
