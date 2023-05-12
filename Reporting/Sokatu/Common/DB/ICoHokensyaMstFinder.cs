using Domain.Common;
using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.Common.DB
{
    public interface ICoHokensyaMstFinder : IRepositoryBase
    {
        List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos);
    }
}
