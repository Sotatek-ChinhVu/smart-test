namespace Reporting.Sokatu
{
    public enum PrefKbn
    {
        PrefAll = 0,
        PrefIn = 1,
        PrefOut = 2
    }

    public enum KokhoKind
    {
        All = 0,      //国保+後期
        Kokho = 1,    //国保
        Kouki = 2,    //後期
        Tokuyohi = 3,      //特別療養費 国保+後期
        TokuyohiKokho = 4, //特別療養費 国保
        TokuyohiKouki = 5, //特別療養費 後期
    }

    /// <summary>
    /// 公費負担チェック
    ///     1:公費負担あり 2:一部負担相当額あり 3:公費負担あり(窓口)
    /// </summary>
    public enum FutanCheck
    {
        None = 0,
        KohiFutan = 1,
        IchibuFutan = 2,
        KohiFutan10en = 3,
    }

    /// <summary>
    /// 保険者番号
    ///     0:まとめなし
    ///     1:政令指定都市を代表番号にまとめる
    ///     2:政令指定都市を代表番号にまとめる（県内のみ）
    /// </summary>
    public enum HokensyaNoKbn
    {
        NoSum = 0,
        SumAll = 1,
        SumPrefIn = 2
    }

    public struct countData
    {
        public int Count;
        public int Nissu;
        public int Tensu;
        public int Futan;
        public int PtFutan;
        public int Choki;
        public int KohiCount;

        public void AddValue(countData countData)
        {
            Count += countData.Count;
            Nissu += countData.Nissu;
            Tensu += countData.Tensu;
            Futan += countData.Futan;
            PtFutan += countData.PtFutan;
            Choki += countData.Choki;
            KohiCount += countData.KohiCount;
        }

        public void Clear()
        {
            Count = 0;
            Nissu = 0;
            Tensu = 0;
            Futan = 0;
            PtFutan = 0;
            Choki = 0;
            KohiCount = 0;
        }
    }
}
