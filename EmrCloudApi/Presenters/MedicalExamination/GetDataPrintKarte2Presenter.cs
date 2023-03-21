using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Presenters.MedicalExamination;

public class GetDataPrintKarte2Presenter : IGetDataPrintKarte2OutputPort
{
    public Response<GetDataPrintKarte2Response> Result { get; private set; } = new();

    public void Complete(GetMedicalExaminationHistoryOutputData outputData)
    {
        Result.Data = new GetDataPrintKarte2Response(outputData.RaiinfList, outputData.Karte2Input);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetMedicalExaminationHistoryStatus status) => status switch
    {
        GetMedicalExaminationHistoryStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
