using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Futan.Models
{
    public class FutancalcListModel
    {
        private readonly KaikeiDetailModel _kaikeiDetail;
        private readonly List<KaikeiDetailModel> _kaikeiDetails;
        private readonly List<OdrInfModel> _odrInfs;

        public FutancalcListModel(
            KaikeiDetailModel kaikeiDetailModel, List<KaikeiDetailModel> kaikeiDetailModels,
            List<OdrInfModel> odrInfModels)
        {
            _kaikeiDetail = kaikeiDetailModel;
            _kaikeiDetails = kaikeiDetailModels;
            _odrInfs = odrInfModels;
        }

        /// <summary>
        /// 公費使用回数取得
        /// </summary>
        public int GetKohiUsageCount(int kohiId, CountType countType, int countKbn, int LimitKbn, bool isGroupDay)
        {
            int fromDate = FromDate(countType);
            int toDate = _kaikeiDetail.SinDate;

            var wrkQuery = _kaikeiDetails.Where(k =>
                k.SinDate >= fromDate && k.SinDate <= toDate &&
                k.TotalIryohi > 0 &&
                k.OyaRaiinNo != _kaikeiDetail.OyaRaiinNo
            );

            //回数集計区分
            switch (countKbn)
            {
                case 1:
                    //上限超のみ
                    wrkQuery = wrkQuery.Where(k =>
                        (
                            (k.Kohi1Id == kohiId && k.Kohi1Futan > 0) ||
                            (k.Kohi2Id == kohiId && k.Kohi2Futan > 0) ||
                            (k.Kohi3Id == kohiId && k.Kohi3Futan > 0) ||
                            (k.Kohi4Id == kohiId && k.Kohi4Futan > 0)
                        )
                    );
                    break;
                case 2:
                    //上限以下含む
                    wrkQuery = wrkQuery.Where(k =>
                        (
                            (k.Kohi1Id == kohiId && (k.Kohi1Futan > 0 || k.IchibuFutan + k.Kohi2Futan + k.Kohi3Futan + k.Kohi4Futan > 0)) ||
                            (k.Kohi2Id == kohiId && (k.Kohi2Futan > 0 || k.IchibuFutan + k.Kohi3Futan + k.Kohi4Futan > 0)) ||
                            (k.Kohi3Id == kohiId && (k.Kohi3Futan > 0 || k.IchibuFutan + k.Kohi4Futan > 0)) ||
                            (k.Kohi4Id == kohiId && (k.Kohi4Futan > 0 || k.IchibuFutan > 0))
                        )
                    );
                    break;
                case 3:
                    //一部負担ありのみ
                    wrkQuery = wrkQuery.Where(k =>
                        (
                            (k.Kohi1Id == kohiId && (k.IchibuFutan + k.Kohi2Futan + k.Kohi3Futan + k.Kohi4Futan > 0)) ||
                            (k.Kohi2Id == kohiId && (k.IchibuFutan + k.Kohi3Futan + k.Kohi4Futan > 0)) ||
                            (k.Kohi3Id == kohiId && (k.IchibuFutan + k.Kohi4Futan > 0)) ||
                            (k.Kohi4Id == kohiId && (k.IchibuFutan > 0))
                        )
                    );
                    break;
                default:
                    //すべて
                    wrkQuery = wrkQuery.Where(k =>
                        (
                            (k.Kohi1Id == kohiId && k.TotalIryohi > 0) ||
                            (k.Kohi2Id == kohiId && k.TotalIryohi > 0) ||
                            (k.Kohi3Id == kohiId && k.TotalIryohi > 0) ||
                            (k.Kohi4Id == kohiId && k.TotalIryohi > 0)
                        )
                    );
                    break;
            };

            //上限区分
            switch (LimitKbn)
            {
                case 1:
                    //レセプト単位
                    wrkQuery = wrkQuery.Where(k =>
                        k.HokenId == _kaikeiDetail.HokenId
                    );
                    break;
                default:
                    //医療機関単位
                    break;
            }

            if (isGroupDay)
            {
                wrkQuery = wrkQuery.Where(k => k.SinDate != toDate);
                return wrkQuery.GroupBy(k => k.SinDate).ToList().Count;
            }
            return wrkQuery.GroupBy(k => k.OyaRaiinNo).ToList().Count;
        }

        /// <summary>
        /// 期間内の公費に係る一部負担金相当額合計を取得
        /// </summary>
        public int GetKohiIchibuFutan(int kohiId, CountType countType, int LimitKbn)
        {
            int fromDate = FromDate(countType);
            int toDate = _kaikeiDetail.SinDate;

            var wrkQuery = _kaikeiDetails.Where(k =>
                k.SinDate >= fromDate && k.SinDate <= toDate &&
                (
                    (k.Kohi1Id == kohiId) ||
                    (k.Kohi2Id == kohiId) ||
                    (k.Kohi3Id == kohiId) ||
                    (k.Kohi4Id == kohiId)
                )
            );

            //同一来院内の負担を取得
            if (countType == CountType.Times)
            {
                wrkQuery = wrkQuery.Where(k =>
                    k.OyaRaiinNo == _kaikeiDetail.OyaRaiinNo
                );
            }

            //上限区分
            switch (LimitKbn)
            {
                case 1:
                    //レセプト単位
                    wrkQuery = wrkQuery.Where(k =>
                        k.HokenId == _kaikeiDetail.HokenId
                    );
                    break;
                default:
                    //医療機関単位
                    break;
            }

#pragma warning disable S3358 // Ternary operators should not be nested
            int result = wrkQuery.Sum(
                k =>
                    k.Kohi1Id == kohiId ? k.IchibuFutan + k.Kohi2Futan + k.Kohi3Futan + k.Kohi4Futan :
                    k.Kohi2Id == kohiId ? k.IchibuFutan + k.Kohi3Futan + k.Kohi4Futan :
                    k.Kohi3Id == kohiId ? k.IchibuFutan + k.Kohi4Futan :
                    k.IchibuFutan
            );
#pragma warning restore S3358 // Ternary operators should not be nested

#pragma warning disable S3358 // Ternary operators should not be nested
            string kohiHoubetu =
                _kaikeiDetail.Kohi1Id == kohiId ? _kaikeiDetail.Kohi1Houbetu :
                _kaikeiDetail.Kohi2Id == kohiId ? _kaikeiDetail.Kohi2Houbetu :
                _kaikeiDetail.Kohi3Id == kohiId ? _kaikeiDetail.Kohi3Houbetu :
                _kaikeiDetail.Kohi4Id == kohiId ? _kaikeiDetail.Kohi4Houbetu : "";
#pragma warning restore S3358 // Ternary operators should not be nested

            //自立支援減免の患者で第２公費以降の計算時は減免額を除いて計算する
            if (_kaikeiDetail.GenmenKbn == GenmenKbn.Jiritusien &&
                _kaikeiDetail.HokenKbn == HokenKbn.Kokho &&
                !(new string[] { "15", "16", "21" }.Contains(kohiHoubetu)))
            {
                result -= wrkQuery.Sum(k => k.GenmenGaku);
            }

            return result;
        }

        /// <summary>
        /// 期間内の公費負担合計を取得
        /// </summary>
        public int GetKohiFutan(int kohiId, CountType countType, int LimitKbn, int hokenPid = 0)
        {
            int fromDate = FromDate(countType);
            int toDate = _kaikeiDetail.SinDate;

            var wrkQuery = _kaikeiDetails.Where(k =>
                k.SinDate >= fromDate && k.SinDate <= toDate &&
                (
                    (k.Kohi1Id == kohiId) ||
                    (k.Kohi2Id == kohiId) ||
                    (k.Kohi3Id == kohiId) ||
                    (k.Kohi4Id == kohiId)
                )
            );

            //保険組合せの指定
            if (hokenPid > 0)
            {
                wrkQuery = wrkQuery.Where(k => k.HokenPid == hokenPid);
            }

            //同一来院内の負担を取得
            if (countType == CountType.Times)
            {
                wrkQuery = wrkQuery.Where(k =>
                    k.OyaRaiinNo == _kaikeiDetail.OyaRaiinNo
                );
            }

            //上限区分
            switch (LimitKbn)
            {
                case 1:
                    //レセプト単位
                    wrkQuery = wrkQuery.Where(k =>
                        k.HokenId == _kaikeiDetail.HokenId
                    );
                    break;
                default:
                    //医療機関単位
                    break;
            }

#pragma warning disable S3358 // Ternary operators should not be nested
            int result = wrkQuery.Sum(
                k =>
                    k.Kohi1Id == kohiId ? k.Kohi1Futan :
                    k.Kohi2Id == kohiId ? k.Kohi2Futan :
                    k.Kohi3Id == kohiId ? k.Kohi3Futan :
                    k.Kohi4Id == kohiId ? k.Kohi4Futan :
                    0
            );
#pragma warning restore S3358 // Ternary operators should not be nested

            return result;
        }

        /// <summary>
        /// 期間内の公費に係る点数合計を取得
        /// </summary>
        public int GetKohiTensu(int kohiId, CountType countType, int LimitKbn)
        {
            int fromDate = FromDate(countType);
            int toDate = _kaikeiDetail.SinDate;

            var wrkQuery = _kaikeiDetails.Where(k =>
                k.SinDate >= fromDate && k.SinDate <= toDate &&
                (
                    (k.Kohi1Id == kohiId) ||
                    (k.Kohi2Id == kohiId) ||
                    (k.Kohi3Id == kohiId) ||
                    (k.Kohi4Id == kohiId)
                )
            );

            //同一来院内の負担を取得
            if (countType == CountType.Times)
            {
                wrkQuery = wrkQuery.Where(k =>
                    k.OyaRaiinNo == _kaikeiDetail.OyaRaiinNo
                );
            }

            //上限区分
            switch (LimitKbn)
            {
                case 1:
                    //レセプト単位
                    wrkQuery = wrkQuery.Where(k =>
                        k.HokenId == _kaikeiDetail.HokenId
                    );
                    break;
                default:
                    //医療機関単位
                    break;
            }

            return wrkQuery.Sum(k => k.Tensu);
        }

        /// <summary>
        /// 前回までの高額療養費限度額を取得
        /// </summary>
        public int GetPreKogakuLimit()
        {
            int fromDate = FromDate(CountType.Month);
            int toDate = _kaikeiDetail.SinDate;

            var wrkQuery = _kaikeiDetails.Where(k =>
                k.SinDate >= fromDate && k.SinDate <= toDate &&
                (
                    k.HokenPid == _kaikeiDetail.HokenPid ||
                    (
                        //後期は月中で保険が変わっても通算で上限をかける
                        _kaikeiDetail.IsKouki && k.IsKouki &&
                        k.Kohi1Id == _kaikeiDetail.Kohi1Id && k.Kohi2Id == _kaikeiDetail.Kohi2Id &&
                        k.Kohi3Id == _kaikeiDetail.Kohi3Id && k.Kohi4Id == _kaikeiDetail.Kohi4Id
                    )
                ) &&
                k.SortKey.CompareTo(_kaikeiDetail.SortKey) == -1
            );

            int result = wrkQuery.Any() ? wrkQuery.Max(k => k.KogakuLimit) : 0;

            return result;
        }

        /// <summary>
        /// 期間内の一部負担金相当額合計を取得
        /// </summary>
        public int GetKogakuFutan(int hokenPid)
        {
            int fromDate = FromDate(CountType.Month);
            int toDate = _kaikeiDetail.SinDate;

            var wrkQuery = _kaikeiDetails.Where(k =>
                k.SinDate >= fromDate && k.SinDate <= toDate &&
                (
                    k.HokenPid == hokenPid ||
                    (
                        //後期は月中で保険が変わっても通算で上限をかける
                        _kaikeiDetail.IsKouki && k.IsKouki &&
                        k.Kohi1Id == _kaikeiDetail.Kohi1Id && k.Kohi2Id == _kaikeiDetail.Kohi2Id &&
                        k.Kohi3Id == _kaikeiDetail.Kohi3Id && k.Kohi4Id == _kaikeiDetail.Kohi4Id
                    )
                ) &&
                k.AdjustPid == 0
            );

            int result = wrkQuery.Sum(
                k => k.IchibuFutan + k.Kohi1Futan + k.Kohi2Futan + k.Kohi3Futan + k.Kohi4Futan
            );

            return result;
        }

        /// <summary>
        /// 期間内の一部負担金額合計を取得
        /// </summary>
        public int GetKogakuIchibuFutan(int hokenPid)
        {
            int fromDate = FromDate(CountType.Month);
            int toDate = _kaikeiDetail.SinDate;

            var wrkQuery = _kaikeiDetails.Where(k =>
                k.SinDate >= fromDate && k.SinDate <= toDate &&
                (
                    k.HokenPid == hokenPid ||
                    (
                        //後期は月中で保険が変わっても通算で上限をかける
                        _kaikeiDetail.IsKouki && k.IsKouki &&
                        k.Kohi1Id == _kaikeiDetail.Kohi1Id && k.Kohi2Id == _kaikeiDetail.Kohi2Id &&
                        k.Kohi3Id == _kaikeiDetail.Kohi3Id && k.Kohi4Id == _kaikeiDetail.Kohi4Id
                    )
                )
            );

            int result = wrkQuery.Sum(k => k.IchibuFutan);

            return result;
        }

        /// <summary>
        /// 期間内の患者負担額合計を取得
        /// </summary>
        public int GetTotalPtFutan(int hokenId)
        {
            int fromDate = FromDate(CountType.Month);
            int toDate = _kaikeiDetail.SinDate;

            var wrkQuery = _kaikeiDetails.Where(k =>
                k.SinDate >= fromDate && k.SinDate <= toDate &&
                (
                    k.HokenId == hokenId ||
                    (
                        //後期は月中で保険が変わっても通算で上限をかける
                        _kaikeiDetail.IsKouki && k.IsKouki &&
                        k.Kohi1Id == _kaikeiDetail.Kohi1Id && k.Kohi2Id == _kaikeiDetail.Kohi2Id &&
                        k.Kohi3Id == _kaikeiDetail.Kohi3Id && k.Kohi4Id == _kaikeiDetail.Kohi4Id
                    )
                )
            );

            int result = wrkQuery.Sum(k => k.PtFutan + k.AdjustRound);

            return result;
        }

        /// <summary>
        /// 期間内の総医療費合計を取得
        /// </summary>
        public int GetTotalIryohi(int hokenPid, int hokenId)
        {
            List<KaikeiDetailModel> totalDetails = new List<KaikeiDetailModel>(_kaikeiDetails);
            totalDetails.Add(_kaikeiDetail);

            var wrkQuery = totalDetails.Where(k =>
                k.HokenId == hokenId
            );

            if (hokenPid > 0)
            {
                wrkQuery = wrkQuery.Where(k =>
                    k.HokenPid == hokenPid
                );

                return wrkQuery.Sum(k => k.TotalIryohi);
            }
            else
            {
                var wrkDetails = wrkQuery.GroupBy(k => k.HokenPid).ToList();

                int wrkIryohi = 0;

                foreach (var wrkGroup in wrkDetails)
                {
                    //一部負担相当額を取得
                    int wrkIchibu = wrkGroup.Sum(d =>
                        d.IchibuFutan + d.Kohi1Futan + d.Kohi2Futan + d.Kohi3Futan + d.Kohi4Futan
                    );

                    //公費併用と保険単独の療養が併せて行われている場合、
                    //70歳未満では一部負担金等がそれぞれ21,000円以上で公費の費用徴収額があることが条件
                    if (_kaikeiDetail.AgeKbn == AgeKbn.Elder || wrkIchibu >= KogakuIchibu.BorderVal ||
                        _kaikeiDetail.KogakuTotalKbn == KogakuTotalKbn.IncludeKogakuTotal)
                    {
                        wrkIryohi = wrkIryohi + wrkGroup.Sum(d => d.TotalIryohi);
                    }
                }

                return wrkIryohi;
            }

        }




        /// <summary>
        /// 初診料の算定有無を取得
        /// </summary>
        public bool IsOdrSyoshin(int kohiId, int LimitKbn, bool dojituNika)
        {
            var wrkQuery = _odrInfs.Where(x =>
                x.SinDate == _kaikeiDetail.SinDate &&
                x.ItemCd == ItemCdConst.SyosaiKihon &&
                (
                    x.Kohi1Id == kohiId ||
                    x.Kohi2Id == kohiId ||
                    x.Kohi3Id == kohiId ||
                    x.Kohi4Id == kohiId
                )
            );

            //同一日２科目
            if (dojituNika)
            {
                wrkQuery = wrkQuery.Where(x => x.Suryo == SyosaiConst.Syosin || x.Suryo == SyosaiConst.Syosin2 || x.Suryo == SyosaiConst.SyosinCorona);
            }
            else
            {
                wrkQuery = wrkQuery.Where(x => x.Suryo == SyosaiConst.Syosin || x.Suryo == SyosaiConst.SyosinCorona);
            }

            //上限区分
            switch (LimitKbn)
            {
                case 1:
                    //レセプト単位
                    wrkQuery = wrkQuery.Where(x =>
                        x.HokenId == _kaikeiDetail.HokenId
                    );
                    break;
                default:
                    //医療機関単位
                    break;
            }

            return (wrkQuery.ToList().Count >= 1);
        }

        /// <summary>
        /// 集計区分に応じて期間Fromを取得
        /// </summary>
        private int FromDate(CountType countType)
        {
            if (countType == CountType.Month)
            {
                return _kaikeiDetail.SinDate / 100 * 100 + 1;
            }
            else
            {
                return _kaikeiDetail.SinDate;
            }
        }
    }

    /// <summary>
    /// 年齢区分
    /// </summary>
    public static class AgeKbn
    {
        public const int Ippan = 0;
        public const int Elder = 1;
    }

    /// <summary>
    /// 高額療養費の合算対象となる一部負担金
    /// </summary>
    public static class KogakuIchibu
    {
        public const int BorderVal = 21000;
    }

    /// <summary>
    /// 集計区分(0:回上限 1:日上限 2:月上限)
    /// </summary>
    public enum CountType { Times = 0, Day = 1, Month = 2 };

    /// <summary>
    /// 高額療養費合算対象
    ///     1:合算対象外
    ///     2:21,000未満合算対象
    /// </summary>
    public static class KogakuTotalKbn
    {
        public const int None = 0;
        public const int ExcludeKogakuTotal = 1;
        public const int IncludeKogakuTotal = 2;
    }
}
