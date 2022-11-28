using UseCase.MedicalExamination.GetCheckDisease;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetCheckDiseaseResponse
    {
        public GetCheckDiseaseResponse(List<GetCheckDiseaseItemOutputData> checkDiseaseItemOutputDatas)
        {
            CheckDiseaseItemOutputDatas = checkDiseaseItemOutputDatas;
        }

        public List<GetCheckDiseaseItemOutputData> CheckDiseaseItemOutputDatas { get; private set; }
    }
}
