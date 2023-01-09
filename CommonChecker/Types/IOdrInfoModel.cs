using Helper.Extension;

namespace CommonChecker.Types
{
    public interface IOdrInfoModel<TOdrInfoDetailModel>
     where TOdrInfoDetailModel : class, IOdrInfoDetailModel
    {
        int OdrKouiKbn { get; }

        int SanteiKbn { get; }

        #region Exposed
        List<TOdrInfoDetailModel> OrdInfDetails { get; }
        List<TOdrInfoDetailModel> OdrInfDetailModelsIgnoreEmpty { get; }
        // 処方 - Drug
        bool IsDrug { get; }

        // 注射 - Injection
        bool IsInjection { get; }
        #endregion

        double SumBunkatu(string bunkatu)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bunkatu))
                    return 0;

                var nums = bunkatu.Split('+');

                return nums.Sum(n => n.AsDouble());
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
