using Domain.Common;
using Reporting.KensaLabel.Model;

namespace Reporting.KensaLabel.DB
{
    public interface IKensaLabelFinder : IRepositoryBase
    {
        PtInfModel GetPtInfModel(int hpId, long ptId);
    }
}
