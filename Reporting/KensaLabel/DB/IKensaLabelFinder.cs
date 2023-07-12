using Reporting.KensaLabel.Model;

namespace Reporting.KensaLabel.DB
{
    public interface IKensaLabelFinder
    {
        PtInfModel GetPtInfModel(int hpId, long ptId);
    }
}
