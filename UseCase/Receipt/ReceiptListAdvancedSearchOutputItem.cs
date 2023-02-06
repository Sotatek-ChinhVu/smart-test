using Domain.Models.Receipt;

namespace UseCase.Receipt;

public class ReceiptListAdvancedSearchOutputItem
{
    public ReceiptListAdvancedSearchOutputItem(ReceiptListModel model)
    {
        SeikyuKbn = model.SeikyuKbn;
        SinYm = model.SinYm;
        IsReceInfDetailExist = model.IsReceInfDetailExist;
        IsPaperRece = model.IsPaperRece;
        HokenKbn = model.HokenKbn;
        Output = model.Output;
        FusenKbn = model.FusenKbn;
        StatusKbn = model.StatusKbn;
        IsPending = model.IsPending;
        PtNum = model.PtNum;
        KanaName = model.KanaName;
        Name = model.Name;
        Sex = model.Sex;
        Age = model.Age;
        LastSinDateByHokenId = model.LastSinDateByHokenId;
        BirthDay = model.BirthDay;
        ReceSbt = model.ReceSbt;
        HokensyaNo = model.HokensyaNo;
        Tensu = model.Tensu;
        HokenSbtCd = model.HokenSbtCd;
        Kohi1Nissu = model.Kohi1Nissu;
        IsSyoukiInfExist = model.IsSyoukiInfExist;
        IsReceCmtExist = model.IsReceCmtExist;
        IsSyobyoKeikaExist = model.IsSyobyoKeikaExist;
        ReceSeikyuCmt = model.ReceSeikyuCmt;
        LastVisitDate = model.LastVisitDate;
        KaName = model.KaName;
        SName = model.SName;
        IsPtKyuseiExist = model.IsPtKyuseiExist;
        FutansyaNoKohi1 = model.FutansyaNoKohi1;
        FutansyaNoKohi2 = model.FutansyaNoKohi2;
        FutansyaNoKohi3 = model.FutansyaNoKohi3;
        FutansyaNoKohi4 = model.FutansyaNoKohi4;
        IsPtTest = model.IsPtTest;
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
