using CalculateService.Futan.Models;
using CalculateService.ReceFutan.Models;

namespace CalculateService.Interface
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
