using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SaveKensaIrai;

public class SaveKensaIraiOutputData : IOutputData
{
    public SaveKensaIraiOutputData(string message, List<KensaIraiReportItem> kensaIraiReportItemList, SaveKensaIraiStatus status)
    {
        Message = message;
        KensaIraiReportItemList = kensaIraiReportItemList;
        Status = status;
    }

    public SaveKensaIraiOutputData()
    {
        Message = string.Empty;
        KensaIraiReportItemList = new();
        Status = SaveKensaIraiStatus.Failed;
    }

    public string Message { get; private set; }

    public SaveKensaIraiStatus Status { get; private set; }

    public List<KensaIraiReportItem> KensaIraiReportItemList { get; private set; }
}
