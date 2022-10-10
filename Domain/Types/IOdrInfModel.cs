using Domain.Models.OrdInfs;
using Helper.Constants;
using Helper.Extension;
using static Helper.Constants.TodayOrderConst;

namespace Domain.Types
{
    public interface IOdrInfModel<TOdrInfDetailModel>
     where TOdrInfDetailModel : class, IOdrInfDetailModel
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

        GroupKoui GroupKoui { get; }

        int IsDeleted { get; }

        #region Exposed
        List<TOdrInfDetailModel> OrdInfDetails { get; }

        // 処方 - Drug
        bool IsDrug { get; }

        // 注射 - Injection
        bool IsInjection { get; }
        #endregion
    }
}
