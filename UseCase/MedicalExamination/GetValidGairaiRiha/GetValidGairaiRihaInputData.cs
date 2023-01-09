using Domain.Models.OrdInfs;
using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.GetValidGairaiRiha
{
    public class GetValidGairaiRihaInputData : IInputData<GetValidGairaiRihaOutputData>
    {
        public GetValidGairaiRihaInputData(int hpId, int ptId, long raiinNo, int sinDate, int syosaiKbn, List<OdrInfItemInputData> allOdrInf)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            SyosaiKbn = syosaiKbn;
            AllOdrInf = allOdrInf;
        }

        public int HpId { get; private set; }
        public int PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public int SyosaiKbn { get; private set; }
        public List<OdrInfItemInputData> AllOdrInf { get; private set; }
    }
}
