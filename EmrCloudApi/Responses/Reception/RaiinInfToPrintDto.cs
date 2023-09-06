using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception;

public class RaiinInfToPrintDto
{
    public RaiinInfToPrintDto(RaiinInfToPrintModel model)
    {

        NameBinding = model.NameBinding;
        TantoIdDisplay = model.TantoIdDisplay;
        PtNum = model.PtNum;
        PtId = model.PtId;
        HoubetuForPrintPrescription = model.HoubetuForPrintPrescription;
        HokenKbnForPrintPrescription = model.HokenKbnForPrintPrescription;
        HokensyaNo = model.HokensyaNo;
        UketukeNo = model.UketukeNo;
        RaiinInfSinDate = model.RaiinInfSinDate;
        HokenPid = model.HokenPid;
        KaDisplay = model.KaDisplay;
        UketukeSbt = model.UketukeSbt;
        UketsukeDisplay = model.UketsukeDisplay;
        HokenKbnName = model.HokenKbnName;
        SinDate = model.SinDate;
        SinDateBinding = model.SinDateBinding;
        SinDateYMBinding = model.SinDateYMBinding;
        TantoId = model.TantoId;
        KaId = model.KaId;
        UketsukeSbt = model.UketsukeSbt;
        HokenName = model.HokenName;
        HokenKbn = model.HokenKbn;
        RaiinInfStatus = model.RaiinInfStatus;
        Kohi1Id = model.Kohi1Id;
        Kohi2Id = model.Kohi2Id;
        Kohi3Id = model.Kohi3Id;
        Kohi4Id = model.Kohi4Id;
        Kohi1HokenSbtKbn = model.Kohi1HokenSbtKbn;
        Kohi2HokenSbtKbn = model.Kohi2HokenSbtKbn;
        Kohi3HokenSbtKbn = model.Kohi3HokenSbtKbn;
        Kohi4HokenSbtKbn = model.Kohi4HokenSbtKbn;
        Kohi1Houbetu = model.Kohi1Houbetu;
        Kohi2Houbetu = model.Kohi2Houbetu;
        Kohi3Houbetu = model.Kohi3Houbetu;
        Kohi4Houbetu = model.Kohi4Houbetu;
        ReceInfHoubetu = model.ReceInfHoubetu;
        HokenId = model.HokenId;
        HokenSbtStr = model.HokenSbtStr;
        HokenSbtCd = model.HokenSbtCd;
        RaiinNo = model.RaiinNo;
    }

    public string SinDateBinding { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string NameBinding { get; private set; }

    public int UketukeSbt { get; private set; }

    public string UketsukeDisplay { get; private set; }

    public int UketukeNo { get; private set; }

    public string KaDisplay { get; private set; }

    public string TantoIdDisplay { get; private set; }

    public int HokenPid { get; private set; }

    public string HokenKbnName { get; private set; }

    public string HoubetuForPrintPrescription { get; private set; }

    public int HokenKbnForPrintPrescription { get; private set; }

    public string HokensyaNo { get; private set; }

    public int RaiinInfSinDate { get; private set; }

    public int SinDate { get; private set; }

    public string SinDateYMBinding { get; private set; }

    public int TantoId { get; private set; }

    public int KaId { get; private set; }

    public int UketsukeSbt { get; private set; }

    public string HokenName { get; private set; }

    public int HokenKbn { get; private set; }

    public int RaiinInfStatus { get; private set; }

    public int Kohi1Id { get; private set; }

    public int Kohi2Id { get; private set; }

    public int Kohi3Id { get; private set; }

    public int Kohi4Id { get; private set; }

    public int Kohi1HokenSbtKbn { get; private set; }

    public int Kohi2HokenSbtKbn { get; private set; }

    public int Kohi3HokenSbtKbn { get; private set; }

    public int Kohi4HokenSbtKbn { get; private set; }

    public string Kohi1Houbetu { get; private set; }

    public string Kohi2Houbetu { get; private set; }

    public string Kohi3Houbetu { get; private set; }

    public string Kohi4Houbetu { get; private set; }

    public string ReceInfHoubetu { get; private set; }

    public int HokenId { get; private set; }

    public string HokenSbtStr { get; private set; }

    public int HokenSbtCd { get; private set; }

    public long RaiinNo { get; private set; }
}
