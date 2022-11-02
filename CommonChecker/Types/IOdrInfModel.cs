using Domain.Types;

namespace CommonChecker.Types
{
    public interface IOdrInfModel<TOdrInfDetailModel>
     where TOdrInfDetailModel : class, IOdrInfDetailModel
    {
        List<TOdrInfDetailModel> OdrInfDetailModelsIgnoreEmpty { get; }
    }
}
