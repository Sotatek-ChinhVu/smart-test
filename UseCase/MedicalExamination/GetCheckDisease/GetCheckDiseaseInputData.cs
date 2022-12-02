using Domain.Models.Diseases;
using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.GetCheckDisease
{
    public class GetCheckDiseaseInputData : IInputData<GetCheckDiseaseOutputData>
    {
        public GetCheckDiseaseInputData(int hpId, int sinDate, List<PtDiseaseModel> todayByomeis, List<OdrInfItemInputData> todayOdrs)
        {
            HpId = hpId;
            SinDate = sinDate;
            TodayByomeis = todayByomeis;
            TodayOdrs = todayOdrs;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public List<PtDiseaseModel> TodayByomeis { get; private set; }

        public List<OdrInfItemInputData> TodayOdrs { get; private set; }

    }
}
