using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;
using Helper.Common;

namespace EmrCalculateApi.Ika.ViewModels
{
    public class IkaCalculateUtilViewModel
    {

        /// <summary>
        /// 指定日が属する月の最終日を取得する
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <returns></returns>
        public static int GetLastDateOfMonth(int baseDate)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = CIUtil.SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                retDate = baseDate / 100 * 100 + DateTime.DaysInMonth(dt1.Year, dt1.Month);
            }

            return retDate;
        }

        ///<summary>
        ///指定の月数前の初日の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数</param>
        ///<returns>基準日の指定月数前の月の初日の日付</returns>
        public static int MonthsBefore(int baseDate, int term)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = CIUtil.SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddMonths(term * -1);
                retDate = CIUtil.DateTimeToInt(dt1);
                retDate = retDate / 100 * 100 + 1;
            }
            return retDate;
        }

        ///<summary>
        ///指定の月数後の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数</param>
        ///<returns>基準日の指定月数後の日付</returns>
        public static int MonthsAfter(int baseDate, int term)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = CIUtil.SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddMonths(term);
                retDate = CIUtil.DateTimeToInt(dt1);
            }
            return retDate;
        }

        ///<summary>
        ///指定の年数前の月の初日の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">年数</param>
        ///<returns>基準日の指定年数前の月の初日の日付</returns>
        public static int YearsBefore(int baseDate, int term)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = CIUtil.SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddYears(term * -1);
                retDate = CIUtil.DateTimeToInt(dt1);
                retDate = retDate / 100 * 100 + 1;
            }
            return retDate;
        }


    }
}
