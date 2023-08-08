using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception;

public class RaiinInfToPrintDto
{
    public RaiinInfToPrintDto(RaiinInfToPrintModel model)
    {

        NameBinding = model.NameBinding;
        TantoIdDisplay = model.TantoIdDisplay;
        PtNum = model.PtNum;
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
    }

    public string SinDateBinding { get; private set; }

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
}
