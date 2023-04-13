using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.GetContainerMst
{
    public class GetContainerMstInputData : IInputData<GetContainerMstOutputData>
    {
        public GetContainerMstInputData(long raiinNo, int sinDate, long ptId, string eventCd)
        {
            RaiinNo = raiinNo;
            SinDate = sinDate;
            PtId = ptId;
            EventCd = eventCd;
        }

        public long RaiinNo { get; private set; }

        public int SinDate { get; private set; }

        public long PtId { get; private set; }

        public string EventCd { get; private set; }
    }
}
