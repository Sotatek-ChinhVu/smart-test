using Interactor.MedicalExamination.HistoryCommon;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination;

public class GetDataPrintKarte2Interactor : IGetDataPrintKarte2InputPort
{
    private readonly IHistoryCommon _historyCommon;

    public GetDataPrintKarte2Interactor(IHistoryCommon historyCommon)
    {
        _historyCommon = historyCommon;
    }

    public GetMedicalExaminationHistoryOutputData Handle(GetDataPrintKarte2InputData inputData)
    {
        return _historyCommon.GetDataKarte2(inputData);
    }
}
