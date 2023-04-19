using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Receipt.Models;

namespace Reporting.Receipt.DB
{
    public interface ICoReceiptFinder
    {
        HpInfModel FindHpInf(int hpId, int sinDate);

        List<ReceInfModel> FindReceInf(
            int hpId, int mode, int target, int seikyuYm, List<long> ptId, int sinYm, int hokenId,
            string receSbt, bool includeTester, bool paperOnly, List<int> seikyuKbns, int tantoId, int kaId, int grpId);

        List<ReceInfModel> FindReceInf(int hpId,
            EmrCalculateApi.ReceFutan.Models.ReceInfModel receInf);

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
    }
}
