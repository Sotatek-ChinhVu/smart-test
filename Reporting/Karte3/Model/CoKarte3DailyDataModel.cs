using static Reporting.Karte3.Enum.CoKarte3Column;

namespace Reporting.Karte3.Model
{
    class CoKarte3DailyDataModel
    {
        public CoKarte3DailyDataModel()
        {
            ColumnDatas = new List<List<double>>();
            EnTenKbns = new List<List<int>>();

            for (int i = 0; i < System.Enum.GetNames(typeof(Karte3Column)).Length; i++)
            {
                ColumnDatas.Add(new List<double>());
                EnTenKbns.Add(new List<int>());
            }
        }

        /// <summary>
        /// すべてのリストの中で最大の要素数を持つリストの要素数を返す
        /// </summary>
        public int MaxCount
        {
            get
            {
                int ret = 0;

                for (int i = 0; i < ColumnDatas.Count(); i++)
                {
                    if (ret < ColumnDatas[i].Count())
                    {
                        ret = ColumnDatas[i].Count();
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 日付
        /// </summary>
        public int Date { get; set; }
        /// <summary>
        /// 診察
        /// </summary>
        public List<List<double>> ColumnDatas { get; set; }
        public List<List<int>> EnTenKbns { get; set; }
        /// <summary>
        /// 指定列の合計を取得する
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public double Sum(Karte3Column index)
        {
            return ColumnDatas[(int)index].Sum();
        }

        /// <summary>
        /// リストを取得する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<double> GetList(Karte3Column index)
        {
            return ColumnDatas[(int)index];
        }
        public List<int> GetEnTenList(Karte3Column index)
        {
            return EnTenKbns[(int)index];
        }
        public List<double> this[Karte3Column index]
        {
            get { return ColumnDatas[(int)index]; }
            set { ColumnDatas[(int)index] = value; }
        }
    }
}
