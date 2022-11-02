using Helper.Constants;

namespace CommonChecker.Types
{
    public interface IOdrInfDetailModel
    {
        string DisplayItemName { get; }

        bool IsUsage { get; }

        ReleasedDrugType ReleasedType { get; }
    }
}
