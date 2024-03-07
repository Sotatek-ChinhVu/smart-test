using CalculateService.Futan.Models;
using CalculateService.ReceFutan.Models;

namespace CalculateService.Interface
{
    public interface IReceFutanViewModel
    {
        void ReceFutanCalculateMain(
            int hpId,
            List<long> ptIds,
            int seikyuYm,
            string uniqueKey
        );

        List<ReceInfModel> KaikeiTotalCalculate(
            int hpId,
            long ptId,
            int sinYm
        );

        void ReceCalculate(int hpId, int seikyuYm);

        void Dispose();
    }
}
