using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetCheckDisease
{
    public class GetCheckDiseaseOutputData : IOutputData
    {
        public GetCheckDiseaseOutputData(List<GetCheckDiseaseItemOutputData> checkDiseaseItemOutputDatas, GetCheckDiseaseStatus status)
        {
            CheckDiseaseItemOutputDatas = checkDiseaseItemOutputDatas;
            Status = status;
        }

        public List<GetCheckDiseaseItemOutputData> CheckDiseaseItemOutputDatas { get; private set; }
        public GetCheckDiseaseStatus Status { get; private set; }
    }
}
