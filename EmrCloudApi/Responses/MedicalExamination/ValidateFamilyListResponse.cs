using UseCase.Family;

namespace EmrCloudApi.Responses.MedicalExamination;

public class ValidateFamilyListResponse
{
    public ValidateFamilyListResponse(ValidateFamilyListStatus status, string validationMessage)
    {
        Status = status;
        ValidationMessage = validationMessage;
    }

    public ValidateFamilyListStatus Status { get; private set; }

    public string ValidationMessage { get; private set; }
}
