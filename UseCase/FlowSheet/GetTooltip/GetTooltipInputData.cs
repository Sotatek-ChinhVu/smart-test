using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.GetTooltip
{
    public class GetTooltipInputData : IInputData<GetTooltipOutputData>
    {
        public GetTooltipInputData(int hpId, long ptId, int sinDate, int startDate, int endDate)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

    }
}
