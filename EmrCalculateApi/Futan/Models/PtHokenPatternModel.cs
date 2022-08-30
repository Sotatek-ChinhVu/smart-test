using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class PtHokenPatternModel
    {
        public PtHokenPattern PtHokenPattern { get; }

        public PtHokenPatternModel(PtHokenPattern ptHokenPattern)
        {
            PtHokenPattern = ptHokenPattern;
        }

        /// <summary>
        /// 保険ID
        ///  患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenPid
        {
            get { return PtHokenPattern.HokenPid; }
        }

        /// <summary>
        /// 保険区分
        ///  0:自費
        ///  1:社保
        ///  2:国保
        ///  11:労災(短期給付)
        ///  12:労災(傷病年金)
        ///  13:アフターケア
        ///  14:自賠責
        /// </summary>
        public int HokenKbn
        {
            get { return PtHokenPattern.HokenKbn; }
        }

        /// <summary>
        /// 保険種別コード
        ///  0:      下記以外
        ///  11..14: 社保単独～４併
        ///  21..24: 国保単独～４併
        ///  31..34: 後期単独～４併
        ///  41..44: 退職単独～４併
        ///  51..54: 公費単独～４併
        /// </summary>
        public int HokenSbtCd
        {
            get { return PtHokenPattern.HokenSbtCd; }
        }

        /// <summary>
        /// 主保険 保険ID
        /// </summary>
        public int HokenId
        {
            get { return PtHokenPattern.HokenId; }
        }

        /// <summary>
        /// 公費１ 保険ID
        /// </summary>
        public int Kohi1Id
        {
            get { return PtHokenPattern.Kohi1Id; }
        }

        /// <summary>
        /// 公費２ 保険ID
        /// </summary>
        public int Kohi2Id
        {
            get { return PtHokenPattern.Kohi2Id; }
        }

        /// <summary>
        /// 公費３ 保険ID
        /// </summary>
        public int Kohi3Id
        {
            get { return PtHokenPattern.Kohi3Id; }
        }

        /// <summary>
        /// 公費４ 保険ID
        /// </summary>
        public int Kohi4Id
        {
            get { return PtHokenPattern.Kohi4Id; }
        }

        /// <summary>
        /// 公費IDの取得
        /// </summary>
        /// <param name="kohiNo">公費番号[1..4]</param>
        /// <returns></returns>
        public int KohiNoToId(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return PtHokenPattern.Kohi1Id;
                case 2:
                    return PtHokenPattern.Kohi2Id;
                case 3:
                    return PtHokenPattern.Kohi3Id;
                case 4:
                    return PtHokenPattern.Kohi4Id;
                default:
                    return 0;
            }
        }
    }
}
