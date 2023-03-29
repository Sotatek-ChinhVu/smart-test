using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetOrderSheetGroup
{
    public class GetOrderSheetGroupInputData : IInputData<GetOrderSheetGroupOutputData>
    {
        public GetOrderSheetGroupInputData(int hpId, int userId, long ptId, bool selectDefOnLoad)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            SelectDefOnLoad = selectDefOnLoad;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public long PtId { get; private set; }

        public bool SelectDefOnLoad { get; private set; }
    }
}
