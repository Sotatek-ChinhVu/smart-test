using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.KarteInfs.GetLists
{
    public class GetListKarteInfInputData : IInputData<GetListKarteInfOutputData>
    {
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public bool IsDeleted { get; private set; }

        public GetListKarteInfInputData(long ptId, long raiinNo, int sinDate, bool isDeleted)
        {
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            IsDeleted = isDeleted;
        }
    }
}
