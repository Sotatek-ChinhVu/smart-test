using UseCase.Core.Sync.Core;

namespace UseCase.KarteInfs.GetLists
{
    public class GetListKarteInfInputData : IInputData<GetListKarteInfOutputData>
    {
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public int IsDeleted { get; private set; }

        public GetListKarteInfInputData(long ptId, long raiinNo, int sinDate, int isDeleted)
        {
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            IsDeleted = isDeleted;
        }
    }
}
