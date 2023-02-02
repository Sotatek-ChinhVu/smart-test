namespace Domain.Models.Receipt;

public class ReceiptListModel
{
    public ReceiptListModel(int seikyuKbn, int sinYm, int isReceInfDetailExist, int isPaperRece, int hokenKbn, int output, int fusenKbn, int statusKbn, int isPending, long ptNum, string kanaName, string name, int sex, int age, int lastSinDateByHokenId, int birthDay, string receSbt, string hokensyaNo, int tensu, int hokenSbtCd, int kohi1Nissu, int isSyoukiInfExist, int isReceCmtExist, int isSyobyoKeikaExist, int receSeikyuCmt, int lastVisitDate, string kaName, string sName, int isPtKyuseiExist, string futansyaNoKohi1, string futansyaNoKohi2, string futansyaNoKohi3, string futansyaNoKohi4, string isPtTest)
    {
        SeikyuKbn = seikyuKbn;
        SinYm = sinYm;
        IsReceInfDetailExist = isReceInfDetailExist;
        IsPaperRece = isPaperRece;
        HokenKbn = hokenKbn;
        Output = output;
        FusenKbn = fusenKbn;
        StatusKbn = statusKbn;
        IsPending = isPending;
        PtNum = ptNum;
        KanaName = kanaName;
        Name = name;
        Sex = sex;
        Age = age;
        LastSinDateByHokenId = lastSinDateByHokenId;
        BirthDay = birthDay;
        ReceSbt = receSbt;
        HokensyaNo = hokensyaNo;
        Tensu = tensu;
        HokenSbtCd = hokenSbtCd;
        Kohi1Nissu = kohi1Nissu;
        IsSyoukiInfExist = isSyoukiInfExist;
        IsReceCmtExist = isReceCmtExist;
        IsSyobyoKeikaExist = isSyobyoKeikaExist;
        ReceSeikyuCmt = receSeikyuCmt;
        LastVisitDate = lastVisitDate;
        KaName = kaName;
        SName = sName;
        IsPtKyuseiExist = isPtKyuseiExist;
        FutansyaNoKohi1 = futansyaNoKohi1;
        FutansyaNoKohi2 = futansyaNoKohi2;
        FutansyaNoKohi3 = futansyaNoKohi3;
        FutansyaNoKohi4 = futansyaNoKohi4;
        IsPtTest = isPtTest;
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

    public int ReceSeikyuCmt { get; private set; }

    public int LastVisitDate { get; private set; }

    public string KaName { get; private set; }

    public string SName { get; private set; }

    public int IsPtKyuseiExist { get; private set; }

    public string FutansyaNoKohi1 { get; private set; }

    public string FutansyaNoKohi2 { get; private set; }

    public string FutansyaNoKohi3 { get; private set; }

    public string FutansyaNoKohi4 { get; private set; }

    public string IsPtTest { get; private set; }
}
