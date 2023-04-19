namespace Reporting.CommonMasters.Config;

public interface ISystemConfig
{
    int OrderLabelKaPrint();
    int OrderLabelSyosaiPrint();
    int OrderLabelCreateNamePrint();
    int OrderLabelHeaderPrint();
    int OrderLabelKensaDsp();
    int OrderLabelYoyakuDateDsp();
    int OrderLabelSanteiGaiDsp();
    string JyusinHyoRaiinKbn();
    int SijisenRpName();
    int JyusinHyoRpName();
    int SijisenAlrgy();
    int JyusinHyoAlrgy();
    int SijisenPtCmt();
    int JyusinHyoPtCmt();
    int SijisenKensaYokiZairyo();
    int JyusinHyoKensaYokiZairyo();
    string WebIdQrCode();
    string MedicalInstitutionCode();
    string WebIdUrlForPc();
    int SyohosenQRVersion();
    int SyohosenChiikiHoukatu();
    int SyohosenRinjiKisai();
    int SyohosenTani();
    int SyohosenHikae();
    int SyohosenFutanRate();
    string SyohosenRefillZero();
    int SyohosenRefillStrikeLine();
    int SyohosenQRKbn();
    int AccountingDetailIncludeComment();
    int AccountingDetailIncludeOutDrug();
    int AccountingUseBackwardFields();
    int AccountingTeikeibunPrint();
    string AccountingTeikeibun1();
    string AccountingTeikeibun2();
    string AccountingTeikeibun3();
}
