using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.Common.Utils
{
    public class SokatuUtil
    {

        ///公費の法別番号を取得
        public static List<string> GetKohiHoubetu(List<CoReceInfModel> coReceInfs, List<string> excludeHoubetu)
        {
            excludeHoubetu = excludeHoubetu ?? new List<string>();

            //公１法別
            var retNums = coReceInfs.Where(r =>
                !excludeHoubetu.Contains(r.Kohi1Houbetu) && r.Kohi1ReceKisai
            ).GroupBy(r => r.Kohi1Houbetu).Select(r => r.Key).ToList();
            //公２法別
            var wrkNums = coReceInfs.Where(r =>
                !excludeHoubetu.Contains(r.Kohi2Houbetu) && r.Kohi2ReceKisai
            ).GroupBy(r => r.Kohi2Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別
            retNums = retNums.Union(wrkNums).ToList();
            //公３法別
            wrkNums = coReceInfs.Where(r =>
                !excludeHoubetu.Contains(r.Kohi3Houbetu) && r.Kohi3ReceKisai
            ).GroupBy(r => r.Kohi3Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別 + 公３法別
            retNums = retNums.Union(wrkNums).ToList();
            //公４法別
            wrkNums = coReceInfs.Where(r =>
                !excludeHoubetu.Contains(r.Kohi4Houbetu) && r.Kohi4ReceKisai
            ).GroupBy(r => r.Kohi4Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別 + 公３法別 + 公４法別
            retNums = retNums.Union(wrkNums).ToList();

            //法別番号順にソート
            retNums.Sort();

            return retNums;
        }

        ///地単公費の法別番号を取得
        public static List<string> GetPrefInHoubetu(List<CoReceInfModel> coReceInfs, int prefNo)
        {
            //公１法別
            var retNums = coReceInfs.Where(r =>
                r.Kohi1ReceKisai && r.PtKohi1?.PrefNo == prefNo
            ).GroupBy(r => r.Kohi1Houbetu).Select(r => r.Key).ToList();
            //公２法別
            var wrkNums = coReceInfs.Where(r =>
                r.Kohi2ReceKisai && r.PtKohi2?.PrefNo == prefNo
            ).GroupBy(r => r.Kohi2Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別
            retNums = retNums.Union(wrkNums).ToList();
            //公３法別
            wrkNums = coReceInfs.Where(r =>
                r.Kohi3ReceKisai && r.PtKohi3?.PrefNo == prefNo
            ).GroupBy(r => r.Kohi3Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別 + 公３法別
            retNums = retNums.Union(wrkNums).ToList();
            //公４法別
            wrkNums = coReceInfs.Where(r =>
                r.Kohi4ReceKisai && r.PtKohi4?.PrefNo == prefNo
            ).GroupBy(r => r.Kohi4Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別 + 公３法別 + 公４法別
            retNums = retNums.Union(wrkNums).ToList();

            //法別番号順にソート
            retNums.Sort();

            return retNums;
        }

        /// <summary>
        /// 公費名称の取得
        /// </summary>
        /// <param name="coKohiHoubetus"></param>
        /// <param name="prefNo"></param>
        /// <param name="houbetu"></param>
        /// <returns></returns>
        public static string GetKohiName(List<CoKohiHoubetuMstModel> coKohiHoubetus, int prefNo, string houbetu)
        {
            return
                coKohiHoubetus.Find(k => k.PrefNo == 0 && k.Houbetu == houbetu)?.HokenNameCd ??
                coKohiHoubetus.Find(k => k.PrefNo == prefNo && k.Houbetu == houbetu)?.HokenNameCd;
        }
    }
}
