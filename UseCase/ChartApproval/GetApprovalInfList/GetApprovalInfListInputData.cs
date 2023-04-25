using UseCase.Core.Sync.Core;

namespace UseCase.ChartApproval.GetApprovalInfList
{
    public class GetApprovalInfListInputData : IInputData<GetApprovalInfListOutputData>
    {
        public GetApprovalInfListInputData(int hpId, int startDate, int endDate, int kaId, int tantoId, bool confirmStartDateEndDate)
        {
            HpId = hpId;
            StartDate = startDate;
            EndDate = endDate;
            KaId = kaId;
            TantoId = tantoId;
            ConfirmStartDateEndDate = confirmStartDateEndDate;
        }

        public int HpId { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int KaId { get; private set; }

        public int TantoId { get; private set; }

        public bool ConfirmStartDateEndDate { get; private set; }
    }
}