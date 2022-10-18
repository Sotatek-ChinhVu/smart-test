namespace Interactor.ExportPDF.Karte2;
public enum Karte2Status : byte
{
    Success = 1,
    Failed = 2,
    CanNotExportPdf = 3,
    InvalidHpId = 4,
    InvalidPtId = 5,
    InvalidSinDate = 6,
    InvalidUser = 7,
    InvalidDeleteCondition = 8,
    NoData = 9,
    InvalidUrl = 10,
    InvalidStartDate = 11,
    InvalidEndDate = 12,
}
