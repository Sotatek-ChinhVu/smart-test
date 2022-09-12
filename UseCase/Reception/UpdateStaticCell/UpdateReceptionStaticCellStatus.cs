namespace UseCase.Reception.UpdateStaticCell;

public enum UpdateReceptionStaticCellStatus
{
    RaiinInfNotFound = 0,
    RaiinInfUpdated,
    RaiinCmtUpdated,
    PatientCmtUpdated,
    InvalidHpId,
    InvalidSinDate,
    InvalidRaiinNo,
    InvalidPtId,
    InvalidUketukeSbtId,
    InvalidTantoId,
    InvalidKaId,
    InvalidCellName
}
