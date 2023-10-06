using Domain.Models.Accounting;
using Entity.Tenant;
using Reporting.Calculate.Ika.Models;
using Reporting.Calculate.Receipt.Models;
using Reporting.Receipt.Models;

namespace Reporting.Receipt.DB
{
    public interface ICoReceiptFinder
    {
        HpInfModel FindHpInf(int hpId, int sinDate);

        List<ReceInfModel> FindReceInf(
            int hpId, int mode, int target, int seikyuYm, List<long> ptId, int sinYm, int hokenId,
            string receSbt, bool includeTester, bool paperOnly, List<int> seikyuKbns, int tantoId, int kaId, int grpId);

        List<ReceInfModel> FindReceInf(int hpId,
            Reporting.Calculate.ReceFutan.Models.ReceInfModel receInf);

        List<ReceInfModel> FindReceInfFukuoka(
           int hpId, int mode, int target, int seikyuYm, List<long> ptId, int sinYm, int hokenId,
            string receSbt, bool includeTester, bool paperOnly, List<int> seikyuKbns, int tantoId, int kaId, int grpId);

        List<SinKouiCountModel> FindSinKouiCountDataForPreview(int hpId, List<long> ptId, int sinYm);

        List<SinKouiCountModel> FindSinKouiCountDataForRece(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId,
            int mode, bool includeTester, List<int> seikyuKbns, int tantoId, int kaId);

        List<SinKouiModel> FindSinKouiDataForPreview(int hpId, List<long> ptId, int sinYm, int hokenId, int hokenId2);

        List<SinKouiModel> FindSinKouiDataForRece(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId, int mode,
            bool includeTester, List<int> seikyuKbns, int tantoId, int kaId);

        List<SinKouiDetailModel> FindSinKouiDetailDataForPreview(int hpId, List<long> ptId, int sinYm);

        List<SinKouiDetailModel> FindSinKouiDetailDataForRece(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId,
            int mode, bool includeTester, List<int> seikyuKbns, int tantoId, int kaId);

        List<SinRpInfModel> FindSinRpInfDataForPreview(int hpId, List<long> ptId, int sinYm);


        List<SinRpInfModel> FindSinRpInfDataForRece(int hpId, int seikyuYm, List<long> ptId, int sinYm, int hokenId, int mode,
            bool includeTester, List<int> seikyuKbns, int tantoId, int kaId);

        PtInfModel FindPtInf(int hpId, long ptId, int sinDate);

        HokenDataModel FindHokenData(int hpId, long ptId, int hokenId);

        List<KohiDataModel> FindKohiData(int hpId, long ptId, int sinDate);

        List<SyobyoDataModel> FindSyobyoData(int hpId, long ptId, int sinYm, int hokenId, int outputYm);

        List<CoKaikeiDetailModel> FindKaikeiDetail(int hpId, long ptId, int sinYm, int hokenId);

        PtKyuseiModel FindPtKyusei(int hpId, long ptId, int lastDate);

        PtRousaiTenki FindPtRousaiTenki(int hpId, long ptId, int sinYm, int hokenId);

        SyobyoKeikaModel FindSyobyoKeika(int hpId, long ptId, int sinYm, int hokenId);

        List<int> FindTuuinDays(int hpId, long ptId, int sinYm, int hokenId);

        SyobyoKeikaModel FindSyobyoKeikaForAfter(int hpId, long ptId, int sinDate, int hokenId);

        int ZenkaiKensaDate(int hpId, long ptId, int sinDate, int hokenId);

        ReceInf? GetReceInf(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId);

        List<CoHokenMstModel> FindHokenMst(int hpId, int sinDate, int hokenNo, int hokenEdaNo, int prefNo);

        ReceSeikyu? GetReceSeikyu(int hpId, long ptId, int hokenId, int sinYm);

        List<RecePreviewModel> GetReceInf(int hpId, long ptId);
    }
}
