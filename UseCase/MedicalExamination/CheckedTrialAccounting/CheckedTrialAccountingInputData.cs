using Domain.Models.OrdInfs;
using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetCheckedOrder;

namespace UseCase.MedicalExamination.CheckedTrialAccounting
{
    public class CheckedTrialAccountingInputData : IInputData<CheckedTrialAccountingOutputData>
    {
        public CheckedTrialAccountingInputData(int hpId, int userId, int sinDate, int tantoId, int kaId, List<OdrInfItem> odrInfItems)
        {
            HpId = hpId;
            UserId = userId;
            SinDate = sinDate;
            TantoId = tantoId;
            KaId = kaId;
            OdrInfItems = odrInfItems;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public int TantoId { get; private set; }
        public int KaId { get; private set; }
        public List<OrdInfModel> OdrInfItems { get; private set; }
    }
}