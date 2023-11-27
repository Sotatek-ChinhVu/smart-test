using Domain.Common;
using Reporting.ReceiptCheck.Model;

namespace Reporting.ReceiptCheck.DB;

public interface ICoReceiptCheckFinder : IRepositoryBase
{
    List<CoReceiptCheckModel> GetCoReceiptChecks(int hpId, List<long> ptIds, int sinYm);
}
