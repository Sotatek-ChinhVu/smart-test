namespace DevExpress.Response.Karte1;

public enum Karte1Status : byte
{
    Success = 1,
    PtInfNotFould = 2,
    HokenNotFould = 3,
    InvalidHpId = 4,
    InvalidSindate = 5,
    Failed = 6,
    CanNotExportPdf = 7,
}
