using UseCase.Diseases.Validation;

namespace EmrCloudApi.Responses.Diseases
{
    public class ValidationPtDiseaseListResponse
    {
        public ValidationPtDiseaseListResponse(ValidationPtDiseaseListStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public ValidationPtDiseaseListStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
