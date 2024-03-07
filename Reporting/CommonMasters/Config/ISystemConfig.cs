using Domain.Common;

namespace Reporting.CommonMasters.Config;

public interface ISystemConfig : IRepositoryBase
{
    int OrderLabelKaPrint(int hpId);
    int OrderLabelSyosaiPrint(int hpId);
    int OrderLabelCreateNamePrint(int hpId);
    int OrderLabelHeaderPrint(int hpId);
    int OrderLabelKensaDsp(int hpId);
    int OrderLabelYoyakuDateDsp(int hpId);
    int OrderLabelSanteiGaiDsp(int hpId);
    string JyusinHyoRaiinKbn(int hpId);
    int SijisenRpName(int hpId);
    int JyusinHyoRpName(int hpId);
    int SijisenAlrgy(int hpId);
    int JyusinHyoAlrgy(int hpId);
    int SijisenPtCmt(int hpId);
    int JyusinHyoPtCmt(int hpId);
    int SijisenKensaYokiZairyo(int hpId);
    int JyusinHyoKensaYokiZairyo(int hpId);
    string WebIdQrCode(int hpId);
    string MedicalInstitutionCode(int hpId);
    string WebIdUrlForPc(int hpId);
    int SyohosenQRVersion(int hpId);
    int SyohosenChiikiHoukatu(int hpId);
    int SyohosenRinjiKisai(int hpId);
    int SyohosenTani(int hpId);
    int SyohosenHikae(int hpId);
    int SyohosenFutanRate(int hpId);
    string SyohosenRefillZero(int hpId);
    int SyohosenRefillStrikeLine(int hpId);
    int SyohosenQRKbn(int hpId);
    int RosaiReceden(int hpId);
    string RosaiRecedenTerm(int hpId);
    int AccountingDetailIncludeComment(int hpId);
    int AccountingDetailIncludeOutDrug(int hpId);
    int AccountingUseBackwardFields(int hpId);
    int AccountingTeikeibunPrint(int hpId);
    string AccountingTeikeibun1(int hpId);
    string AccountingTeikeibun2(int hpId);
    string AccountingTeikeibun3(int hpId);
    int AccountingFormType(int hpId);
    int AccountingDetailFormType(int hpId);
    int AccountingMonthFormType(int hpId);
    int AccountingDetailMonthFormType(int hpId);
    int PrintReceiptPay0Yen(int hpId);
    int PrintDetailPay0Yen(int hpId);
    string PlanetHostName(int hpId);
    string PlanetDatabase(int hpId);
    string PlanetUserName(int hpId);
    string PlanetPassword(int hpId);
    int PlanetType(int hpId);
    int YakutaiTaniDsp(int hpId);
    int YakutaiOnceAmount(int hpId);
    string YakutaiFukuyojiIppokaItemCd(int hpId);
    int YakutaiPrintUnit(int hpId);
    int YakutaiPaperSize(int hpId);
    int YakutaiNaifukuPaperSmallMinValue(int hpId);
    int YakutaiNaifukuPaperNormalMinValue(int hpId);
    int YakutaiNaifukuPaperBigMinValue(int hpId);
    int YakutaiTonpukuPaperSmallMinValue(int hpId);
    int YakutaiTonpukuPaperNormalMinValue(int hpId);
    int YakutaiTonpukuPaperBigMinValue(int hpId);
    int YakutaiGaiyoPaperSmallMinValue(int hpId);
    int YakutaiGaiyoPaperNormalMinValue(int hpId);
    int YakutaiGaiyoPaperBigMinValue(int hpId);
    string YakutaiNaifukuPaperSmallPrinter(int hpId);
    string YakutaiNaifukuPaperNormalPrinter(int hpId);
    string YakutaiNaifukuPaperBigPrinter(int hpId);
    string YakutaiTonpukuPaperSmallPrinter(int hpId);
    string YakutaiTonpukuPaperNormalPrinter(int hpId);
    string YakutaiTonpukuPaperBigPrinter(int hpId);
    string YakutaiGaiyoPaperSmallPrinter(int hpId);
    string YakutaiGaiyoPaperNormalPrinter(int hpId);
    string YakutaiGaiyoPaperBigPrinter(int hpId);
    int HikariDiskIsTotalCnt(int hpId);
    int PrintReceipt(int hpId);
    int PrintDetail(int hpId);
    int P13WelfareGreenSeikyuType(int hpId);
    int P13WelfareBlueSeikyuType(int hpId);
    int OdrKensaIraiKaCode(int hpId);
    int SyohosenKouiDivide(int hpId);
    /// <summary>
    /// 電子処方箋
    /// VAL in (0, 1)以外はライセンスなし
    /// </summary>
    int ElectronicPrescriptionLicense(int hpId);
    int JibaiJunkyo(int hpId);
}
