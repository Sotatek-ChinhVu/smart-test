using Helper.Extension;

namespace Domain.Models.Receipt.Recalculation;

public class ReceRecalculationModel
{
    public ReceRecalculationModel(int seikyuYm, int rousaiSaigaiKbn, int isPaperRece, int birthday, long ptId, long ptNum, int sinYm, string hokenHoubetu, string kohi1Houbetu, string kohi2Houbetu, string kohi3Houbetu, string kohi4Houbetu, int hokenKbn, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, int hokenStartDate, int kohi1StartDate, int kohi2StartDate, int kohi3StartDate, int kohi4StartDate, int hokenEndDate, int kohi1EndDate, int kohi2EndDate, int kohi3EndDate, int kohi4EndDate, bool isHokenConfirmed, bool isKohi1Confirmed, bool isKohi2Confirmed, bool isKohi3Confirmed, bool isKohi4Confirmed, int rousaiSyobyoDate, bool isRosai, int isTester, int latestHokenConfirmedDate, int latestKohi1ConfirmedDate, int latestKohi2ConfirmedDate, int latestKohi3ConfirmedDate, int latestKohi4ConfirmedDate)
    {
        SeikyuYm = seikyuYm;
        RousaiSaigaiKbn = rousaiSaigaiKbn;
        IsPaperRece = isPaperRece;
        Birthday = birthday;
        PtId = ptId;
        PtNum = ptNum;
        SinYm = sinYm;
        HokenHoubetu = hokenHoubetu;
        Kohi1Houbetu = kohi1Houbetu;
        Kohi2Houbetu = kohi2Houbetu;
        Kohi3Houbetu = kohi3Houbetu;
        Kohi4Houbetu = kohi4Houbetu;
        HokenKbn = hokenKbn;
        HokenId = hokenId;
        Kohi1Id = kohi1Id;
        Kohi2Id = kohi2Id;
        Kohi3Id = kohi3Id;
        Kohi4Id = kohi4Id;
        HokenStartDate = hokenStartDate;
        Kohi1StartDate = kohi1StartDate;
        Kohi2StartDate = kohi2StartDate;
        Kohi3StartDate = kohi3StartDate;
        Kohi4StartDate = kohi4StartDate;
        HokenEndDate = hokenEndDate;
        Kohi1EndDate = kohi1EndDate;
        Kohi2EndDate = kohi2EndDate;
        Kohi3EndDate = kohi3EndDate;
        Kohi4EndDate = kohi4EndDate;
        IsHokenConfirmed = isHokenConfirmed;
        IsKohi1Confirmed = isKohi1Confirmed;
        IsKohi2Confirmed = isKohi2Confirmed;
        IsKohi3Confirmed = isKohi3Confirmed;
        IsKohi4Confirmed = isKohi4Confirmed;
        RousaiSyobyoDate = rousaiSyobyoDate;
        IsRosai = isRosai;
        IsTester = isTester;
        LatestHokenConfirmedDate = latestHokenConfirmedDate;
        LatestKohi1ConfirmedDate = latestKohi1ConfirmedDate;
        LatestKohi2ConfirmedDate = latestKohi2ConfirmedDate;
        LatestKohi3ConfirmedDate = latestKohi3ConfirmedDate;
        LatestKohi4ConfirmedDate = latestKohi4ConfirmedDate;
    }

    public int SeikyuYm { get; private set; }

    public int RousaiSaigaiKbn { get; private set; }

    public int IsPaperRece { get; private set; }

    public int Birthday { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public int SinYm { get; private set; }

    public string HokenHoubetu { get; private set; }

    public string Kohi1Houbetu { get; private set; }

    public string Kohi2Houbetu { get; private set; }

    public string Kohi3Houbetu { get; private set; }

    public string Kohi4Houbetu { get; private set; }

    #region Hoken
    public int HokenKbn { get; private set; }

    public int HokenId { get; private set; }

    public int Kohi1Id { get; private set; }

    public int Kohi2Id { get; private set; }

    public int Kohi3Id { get; private set; }

    public int Kohi4Id { get; private set; }

    public int HokenStartDate { get; private set; }

    public int Kohi1StartDate { get; private set; }

    public int Kohi2StartDate { get; private set; }

    public int Kohi3StartDate { get; private set; }

    public int Kohi4StartDate { get; private set; }

    public int HokenEndDate { get; private set; }

    public int Kohi1EndDate { get; private set; }

    public int Kohi2EndDate { get; private set; }

    public int Kohi3EndDate { get; private set; }

    public int Kohi4EndDate { get; private set; }

    public bool IsHokenConfirmed { get; private set; }

    public bool IsKohi1Confirmed { get; private set; }

    public bool IsKohi2Confirmed { get; private set; }

    public bool IsKohi3Confirmed { get; private set; }

    public bool IsKohi4Confirmed { get; private set; }

    public int RousaiSyobyoDate { get; private set; }

    public bool IsRosai { get; private set; }

    public int IsTester { get; private set; }

    public int LatestHokenConfirmedDate { get; private set; }

    public int LatestKohi1ConfirmedDate { get; private set; }

    public int LatestKohi2ConfirmedDate { get; private set; }

    public int LatestKohi3ConfirmedDate { get; private set; }

    public int LatestKohi4ConfirmedDate { get; private set; }
    #endregion

    #region other param
    public int FirstDateOfThisMonth => (SinYm.ToString() + "01").AsInteger();

    public int LastDateOfThisMonth => (SinYm.ToString() + DateTime.DaysInMonth(SinYm / 100, SinYm % 100).ToString()).AsInteger();
    #endregion
}
