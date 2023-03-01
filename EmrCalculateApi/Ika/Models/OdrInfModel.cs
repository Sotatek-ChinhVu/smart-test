using Helper.Constants;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class OdrInfModel
    {
        //public OdrInf OdrInf { get; } = null;
        public OdrInfDataModel OdrInf { get; } = null;
        public PtHokenPatternModel PtHokenPattern { get; } = null;
        public RaiinInfModel RaiinInf { get; } = null;

        private long _rpNo = 0;
        private long _rpEdaNo = 0;

        public OdrInfModel(OdrInf odrInf, PtHokenPatternModel ptHokenPattern, RaiinInfModel raiinInf)
        {
            OdrInf = new OdrInfDataModel(odrInf);
            PtHokenPattern = ptHokenPattern;
            RaiinInf = raiinInf;

            _rpNo = odrInf.RpNo;
            _rpEdaNo = odrInf.RpEdaNo;
        }

        public OdrInfModel(OdrInfDataModel odrInf, PtHokenPatternModel ptHokenPattern, RaiinInfModel raiinInf)
        {
            OdrInf = odrInf;
            PtHokenPattern = ptHokenPattern;
            RaiinInf = raiinInf;

            _rpNo = odrInf.RpNo;
            _rpEdaNo = odrInf.RpEdaNo;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return OdrInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return OdrInf.PtId; }
        }

        /// <summary>
        /// 診療日
        ///     yyyymmdd
        /// </summary>
        public int SinDate
        {
            get { return OdrInf.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return OdrInf.RaiinNo; }
        }

        /// <summary>
        /// 剤番号
        /// </summary>
        public long RpNo
        {
            get { return _rpNo; }
            set { _rpNo = value; }
        }

        /// <summary>
        /// 剤枝番
        ///     剤に変更があった場合、カウントアップ
        /// </summary>
        public long RpEdaNo
        {
            get { return _rpEdaNo; }
            set { _rpEdaNo = value; }
        }

        /// <summary>
        /// 保険組合せID
        /// </summary>
        public int HokenPid
        {
            get { return OdrInf.HokenPid; }
        }

        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId
        {
            get { return PtHokenPattern.HokenId; }
        }

        /// <summary>
        /// オーダー行為区分
        /// </summary>
        public int OdrKouiKbn
        {
            get { return OdrInf.OdrKouiKbn; }
        }

        /// <summary>
        /// 院内院外区分
        ///     0: 院内
        ///     1: 院外
        /// </summary>
        public int InoutKbn
        {
            get { return OdrInf.InoutKbn; }
        }

        /// <summary>
        /// 至急区分
        ///     0: 通常 
        ///     1: 至急
        /// </summary>
        public int SikyuKbn
        {
            get { return OdrInf.SikyuKbn; }
        }

        /// <summary>
        /// 処方種別
        ///     0: 日数判断
        ///     1: 臨時
        ///     2: 常態
        /// </summary>
        public int SyohoSbt
        {
            get { return OdrInf.SyohoSbt; }
        }

        /// <summary>
        /// 算定区分
        ///     1: 算定外
        ///     2: 自費算定
        /// </summary>
        public int SanteiKbn
        {
            get { return OdrInf.SanteiKbn; }
        }

        /// <summary>
        /// 日数回数
        ///     処方日数
        /// </summary>
        public int DaysCnt
        {
            get { return OdrInf.DaysCnt; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return OdrInf.SortNo; }
        }

        /// <summary>
        /// 削除区分
        ///     1:削除
        ///     2:未確定削除
        /// </summary>
        public int IsDeleted
        {
            get { return OdrInf.IsDeleted; }
        }

        //-------------------------------------------------------------------------

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
        public int HokenSyu
        {
            get
            {
                if(new List<int> { 1, 2 }.Contains(PtHokenPattern.HokenKbn))
                {
                    return Domain.Constant.HokenSyu.Kenpo;
                }
                else if(new List<int> { 11,12}.Contains(PtHokenPattern.HokenKbn))
                {
                    return Domain.Constant.HokenSyu.Rosai;
                }
                else if(new List<int> { 13 }.Contains(PtHokenPattern.HokenKbn))
                {
                    return Domain.Constant.HokenSyu.After;
                }
                else if (new List<int> { 14 }.Contains(PtHokenPattern.HokenKbn))
                {
                    return Domain.Constant.HokenSyu.Jibai;
                }
                else
                {
                    return Domain.Constant.HokenSyu.Jihi;
                }
            }
        }

        public string SinStartTime
        {
            get { return RaiinInf.SinStartTime ?? string.Empty; }
        }
    }

}
