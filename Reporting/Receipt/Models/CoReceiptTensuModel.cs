using CalculateService.Constants;
using CalculateService.Receipt.ViewModels;

namespace Reporting.Receipt.Models
{
    public class CoReceiptTensuModel
    {
        private SinMeiViewModel _sinmeiViewModel;

        public CoReceiptTensuModel(SinMeiViewModel sinmeiViewModel)
        {
            _sinmeiViewModel = sinmeiViewModel;
        }

        /// <summary>
        /// 指定の集計先の点数の合計
        /// </summary>
        /// <param name="syukeiSaki"></param>
        /// <returns></returns>
        public double Tensu(string syukeiSaki)
        {
            List<double> tensu = _sinmeiViewModel.SinKoui.Where(p => p.SyukeiSaki == syukeiSaki).GroupBy(p => p.Ten).Select(p => p.Key).ToList();
            double ret = 0;
            if (tensu.Count() == 1)
            {
                ret = tensu[0];
            }
            return ret;
        }
        public double Tensu(List<string> syukeiSakis)
        {
            List<double> tensu = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).GroupBy(p => p.Ten).Select(p => p.Key).ToList();
            double ret = 0;
            if (tensu.Count() == 1)
            {
                ret = tensu[0];
            }
            return ret;
        }
        public double TensuSum(List<string> syukeiSakis)
        {
            double ret = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).Sum(p => p.Ten);

            return ret;
        }
        /// <summary>
        /// 指定の集計先の回数の合計
        /// </summary>
        /// <param name="syukeiSaki"></param>
        /// <returns></returns>
        public int TenColCount(string syukeiSaki, bool onlySI)
        {
            int ret = 0;

            if (onlySI)
            {
                ret = _sinmeiViewModel.SinKoui.Where(p => p.SyukeiSaki == syukeiSaki && p.RecId == "SI").Sum(p => p.TenColCount);
            }
            else
            {
                ret = _sinmeiViewModel.SinKoui.Where(p => p.SyukeiSaki == syukeiSaki).Sum(p => p.TenColCount);
            }

            return ret;
        }
        public int TenColCount(List<string> syukeiSakis, bool onlySI)
        {
            int ret = 0;

            if (onlySI)
            {
                ret = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki) && p.RecId == "SI").Sum(p => p.TenColCount);
            }
            else
            {
                ret = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).Sum(p => p.TenColCount);
            }

            return ret;
        }
        /// <summary>
        /// 指定の集計先の合計点数
        /// </summary>
        /// <param name="syukeiSaki"></param>
        /// <returns></returns>
        public double TotalTen(string syukeiSaki)
        {
            double ret = _sinmeiViewModel.SinKoui.Where(p => p.SyukeiSaki == syukeiSaki).Sum(p => p.TotalTen);

            return ret;
        }
        public double TotalTen(List<string> syukeiSakis)
        {
            double ret = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).Sum(p => p.TotalTen);

            return ret;
        }
        public double TotalKingaku(List<string> syukeiSakis)
        {
            double ret = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).Sum(p => p.Kingaku);

            return ret;
        }

        public double TensuKohi(List<string> syukeiSakis, int kohiIndex)
        {
            List<int> pIds = _sinmeiViewModel.GetKohiPids(kohiIndex);
            double ret = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki) && pIds.Contains(p.HokenPid)).Sum(p => p.Ten);

            return ret;
        }
        public int TenColCountKohi(List<string> syukeiSakis, int kohiIndex)
        {
            List<int> pIds = _sinmeiViewModel.GetKohiPids(kohiIndex);
            int ret = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki) && pIds.Contains(p.HokenPid)).Sum(p => p.TenColCount);

            return ret;
        }
        public double TotalTenKohi(List<string> syukeiSakis, int kohiIndex)
        {
            List<int> pIds = _sinmeiViewModel.GetKohiPids(kohiIndex);
            double ret = _sinmeiViewModel.SinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki) && pIds.Contains(p.HokenPid)).Sum(p => p.TotalTen);

            return ret;
        }

        public List<int> FutanKbnToKohiIndex(string futanKbn)
        {
            return _sinmeiViewModel.FutanKbnToKohiIndex(futanKbn);
        }
        public List<string> FutanKbns
        {
            get
            {
                return _sinmeiViewModel?.SinMei?.GroupBy(p => p.FutanKbn).Select(p => p.Key).ToList();
            }
        }
        public List<(int count, double kingaku)> TenColKingakuSonota(string syukeiSaki)
        {
            List<(int count, double kingaku)> ret = new List<(int count, double kingaku)>();

            _sinmeiViewModel.SinKoui.FindAll(p => p.SyukeiSaki == syukeiSaki).ForEach(p => ret.Add((p.Count, p.TotalTen)));

            return ret;
        }

        public string SyosinJikangai
        {
            get
            {
                string ret = "";

                if (_sinmeiViewModel.SinMei.Any(p => p.SyukeiSaki == ReceSyukeisaki.SyosinJikanGai))
                {
                    ret += "時間外";
                }

                if (_sinmeiViewModel.SinMei.Any(p => p.SyukeiSaki == ReceSyukeisaki.SyosinKyujitu))
                {
                    if (string.IsNullOrEmpty(ret) == false)
                    {
                        ret += "・";
                    }
                    ret += "休日";
                }

                if (_sinmeiViewModel.SinMei.Any(p => p.SyukeiSaki == ReceSyukeisaki.SyosinSinya))
                {
                    if (string.IsNullOrEmpty(ret) == false)
                    {
                        ret += "・";
                    }
                    ret += "深夜";
                }

                if (string.IsNullOrEmpty(ret) == false)
                {
                    ret = $"({ret})";
                }

                return ret;
            }

        }
    }
}
