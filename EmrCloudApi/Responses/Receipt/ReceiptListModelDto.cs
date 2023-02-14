using UseCase.Receipt.ReceiptListAdvancedSearch;

namespace EmrCloudApi.Responses.Receipt;

public class ReceiptListModelDto
{
    public ReceiptListModelDto(ReceiptListAdvancedSearchOutputItem output)
    {
        SeikyuKbn = output.SeikyuKbn;
        SinYm = output.SinYm;
        IsReceInfDetailExist = output.IsReceInfDetailExist;
        IsPaperRece = output.IsPaperRece;
        HokenKbn = output.HokenKbn;
        Output = output.Output;
        FusenKbn = output.FusenKbn;
        StatusKbn = output.StatusKbn;
        IsPending = output.IsPending;
        PtNum = output.PtNum;
        KanaName = output.KanaName;
        Name = output.Name;
        Sex = output.Sex;
        Age = output.Age;
        LastSinDateByHokenId = output.LastSinDateByHokenId;
        BirthDay = output.BirthDay;
        ReceSbt = output.ReceSbt;
        HokensyaNo = output.HokensyaNo;
        Tensu = output.Tensu;
        HokenSbtCd = output.HokenSbtCd;
        Kohi1Nissu = output.Kohi1Nissu;
        IsSyoukiInfExist = output.IsSyoukiInfExist;
        IsReceCmtExist = output.IsReceCmtExist;
        IsSyobyoKeikaExist = output.IsSyobyoKeikaExist;
        ReceSeikyuCmt = output.ReceSeikyuCmt;
        LastVisitDate = output.LastVisitDate;
        KaName = output.KaName;
        SName = output.SName;
        IsPtKyuseiExist = output.IsPtKyuseiExist;
        FutansyaNoKohi1 = output.FutansyaNoKohi1;
        FutansyaNoKohi2 = output.FutansyaNoKohi2;
        FutansyaNoKohi3 = output.FutansyaNoKohi3;
        FutansyaNoKohi4 = output.FutansyaNoKohi4;
        IsPtTest = output.IsPtTest;
    }

    public int SeikyuKbn { get; private set; }

    public int SinYm { get; private set; }

    public int IsReceInfDetailExist { get; private set; }

    public int IsPaperRece { get; private set; }

    public int HokenKbn { get; private set; }

    public int Output { get; private set; }

    public int FusenKbn { get; private set; }

    public int StatusKbn { get; private set; }

    public int IsPending { get; private set; }

    public long PtNum { get; private set; }

    public string KanaName { get; private set; }

    public string Name { get; private set; }

    public int Sex { get; private set; }

    public int Age { get; private set; }

    public int LastSinDateByHokenId { get; private set; }

    public int BirthDay { get; private set; }

    public string ReceSbt { get; private set; }

    public string HokensyaNo { get; private set; }

    public int Tensu { get; private set; }

    public int HokenSbtCd { get; private set; }

    public int Kohi1Nissu { get; private set; }

    public int IsSyoukiInfExist { get; private set; }

    public int IsReceCmtExist { get; private set; }

    public int IsSyobyoKeikaExist { get; private set; }

    public string ReceSeikyuCmt { get; private set; }

    public int LastVisitDate { get; private set; }

    public string KaName { get; private set; }

    public string SName { get; private set; }

    public int IsPtKyuseiExist { get; private set; }

    public string FutansyaNoKohi1 { get; private set; }

    public string FutansyaNoKohi2 { get; private set; }

    public string FutansyaNoKohi3 { get; private set; }

    public string FutansyaNoKohi4 { get; private set; }

    public bool IsPtTest { get; private set; }
}
