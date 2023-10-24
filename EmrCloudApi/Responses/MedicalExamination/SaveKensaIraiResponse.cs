using UseCase.MedicalExamination.SaveKensaIrai;

namespace EmrCloudApi.Responses.MedicalExamination;

public class SaveKensaIraiResponse
{
    public SaveKensaIraiResponse(string message, List<KensaIraiReportItem> kensaIraiReportItemList)
    {
        Message = message;
        KensaIraiReportItemList = kensaIraiReportItemList;
    }

    public string Message { get; private set; }

    public List<KensaIraiReportItem> KensaIraiReportItemList { get; private set; }
}
