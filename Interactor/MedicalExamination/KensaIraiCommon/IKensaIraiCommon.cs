using UseCase.MedicalExamination.SaveKensaIrai;

namespace Interactor.MedicalExamination.KensaIraiCommon;

public interface IKensaIraiCommon
{
    SaveKensaIraiOutputData SaveKensaIraiAction(int hpId, int userId, long ptId, int sinDate, long raiinNo);
}
