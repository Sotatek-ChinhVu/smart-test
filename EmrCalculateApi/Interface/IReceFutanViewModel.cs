using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.ReceFutan.Models;

namespace EmrCalculateApi.Interface
{
    public interface IReceFutanViewModel
    {
        void ReceFutanCalculateMain(
            List<long> ptIds,
            int seikyuYm,
            string uniqueKey
        );

        List<ReceInfModel> KaikeiTotalCalculate(
            long ptId,
            int sinYm
        );

        void ReceCalculate(int seikyuYm);

        void Dispose();
    }
}
