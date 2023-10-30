using Interactor.MedicalExamination.KensaIraiCommon;
using UseCase.MedicalExamination.SaveKensaIrai;

namespace Interactor.MedicalExamination;

public class SaveKensaIraiInteractor : ISaveKensaIraiInputPort
{
    private readonly IKensaIraiCommon _kensaIraiCommon;

    public SaveKensaIraiInteractor(IKensaIraiCommon kensaIraiCommon)
    {
        _kensaIraiCommon = kensaIraiCommon;
    }

    public SaveKensaIraiOutputData Handle(SaveKensaIraiInputData inputData)
    {
        return _kensaIraiCommon.SaveKensaIraiAction(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);
    }
}
