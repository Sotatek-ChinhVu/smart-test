using Domain.Models.OrdInfs;
using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.GetValidJihiYobo
{
    public class GetValidJihiYoboInputData : IInputData<GetValidJihiYoboOutputData>
    {
        public GetValidJihiYoboInputData(int hpId, int syosaiKbn, int sinDate, List<OdrInfItemInputData> allOdrInf)
        {
            HpId = hpId;
            SyosaiKbn = syosaiKbn;
            SinDate = sinDate;
            AllOdrInf = allOdrInf;
        }

        public int HpId { get; private set; }
        public int SyosaiKbn { get; private set; }
        public int SinDate { get; private set; }
        public List<OdrInfItemInputData> AllOdrInf { get; private set; }
    }
}
