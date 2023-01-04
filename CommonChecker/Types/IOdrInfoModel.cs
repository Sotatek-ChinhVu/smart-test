using Helper.Extension;

namespace CommonChecker.Types
{
    public interface IOdrInfoModel<TOdrInfoDetailModel>
     where TOdrInfoDetailModel : class, IOdrInfoDetailModel
    {
        long Id { get; }

        int HpId { get; }

        long RpNo { get; }

        long RpEdaNo { get; }

        int OdrKouiKbn { get; }

        string RpName { get; }

        int InoutKbn { get; }

        int SikyuKbn { get; }

        int SyohoSbt { get; }

        int SanteiKbn { get; }

        int TosekiKbn { get; }

        int DaysCnt { get; }

        int SortNo { get; }

        int IsDeleted { get; }

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
