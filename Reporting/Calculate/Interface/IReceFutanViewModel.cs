using Reporting.Calculate.Futan.Models;
using Reporting.Calculate.ReceFutan.Models;

namespace Reporting.Calculate.Interface
{
    public interface IReceFutanViewModel
    {
        void ReceFutanCalculateMain(
            List<long> ptIds,
            int seikyuYm
        );

        List<ReceInfModel> KaikeiTotalCalculate(
            long ptId,
            int sinYm
        );

        void ReceCalculate(int seikyuYm);
    }
}
