using UseCase.Diseases.Upsert;

namespace EmrCloudApi.Responses.Diseases
{
    public class UpsertPtDiseaseListResponse
    {
        public UpsertPtDiseaseListResponse(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; private set; }
    }

    public class UpsertPtDiseaseListMedicalResponse
    {
        public UpsertPtDiseaseListMedicalResponse(UpsertPtDiseaseListStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public UpsertPtDiseaseListStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
