using EmrCalculateApi.Futan.Models;

namespace EmrCalculateApi.Interface
{
    public interface IFutanCalculate
    {
        void FutanCalculation(
            long ptId,
            int sinDate,
            List<SinKouiCountModel> sinKouiCounts,
            List<SinKouiModel> sinKouis,
            List<SinKouiDetailModel> sinKouiDetails,
            List<SinRpInfModel> sinRpInfs,
            int seikyuUp);

        List<KaikeiInfModel> TrialFutanCalculation(
            long ptId,
            int sinDate,
            long raiinNo,
            List<SinKouiCountModel> sinKouiCounts,
            List<SinKouiModel> sinKouis,
            List<SinKouiDetailModel> sinKouiDetails,
            List<SinRpInfModel> sinRpInfs,
            List<RaiinInfModel> raiinInfs
        );

        void DetailCalculate(bool raiinAdjust);
    }
}
