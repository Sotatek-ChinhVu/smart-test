using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class SanteiDetailCountModel
    {
        public SinKouiDetail SinKouiDetail { get; } = null;
        public SinKouiCount SinKouiCount { get; } = null;

        public SanteiDetailCountModel(SinKouiDetail sinKouiDetail, SinKouiCount sinKouiCount)
        {
            SinKouiDetail = sinKouiDetail;
            SinKouiCount = SinKouiCount;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SinKouiDetail.HpId; }
            set
            {
                if (SinKouiDetail.HpId == value) return;
                SinKouiDetail.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SinKouiDetail.PtId; }
            set
            {
                if (SinKouiDetail.PtId == value) return;
                SinKouiDetail.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SinKouiDetail.SinYm; }
            set
            {
                if (SinKouiDetail.SinYm == value) return;
                SinKouiDetail.SinYm = value;
                //RaisePropertyChanged(() => SinYm);
            }
        }

        /// <summary>
        /// 剤番号
        /// SIN_KOUI.RP_NO
        /// </summary>
        public int RpNo
        {
            get { return SinKouiDetail.RpNo; }
            set
            {
                if (SinKouiDetail.RpNo == value) return;
                SinKouiDetail.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get { return SinKouiDetail.SeqNo; }
            set
            {
                if (SinKouiDetail.SeqNo == value) return;
                SinKouiDetail.SeqNo = value;
                //RaisePropertyChanged(() => SeqNo);
            }
        }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        public int RowNo
        {
            get { return SinKouiDetail.RowNo; }
            set
            {
                if (SinKouiDetail.RowNo == value) return;
                SinKouiDetail.RowNo = value;
                //RaisePropertyChanged(() => RowNo);
            }
        }

        /// <summary>
        /// 項目種別
        /// 1:コメント
        /// </summary>
        public int ItemSbt
        {
            get { return SinKouiDetail.ItemSbt; }
            set
            {
                if (SinKouiDetail.ItemSbt == value) return;
                SinKouiDetail.ItemSbt = value;
                //RaisePropertyChanged(() => ItemSbt);
            }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return SinKouiDetail.ItemCd ?? string.Empty; }
            set
            {
                if (SinKouiDetail.ItemCd == value) return;
                SinKouiDetail.ItemCd = value;
                //RaisePropertyChanged(() => ItemCd);
            }
        }

        /// <summary>
        /// 項目名称
        /// 
        /// </summary>
        public string ItemName
        {
            get { return SinKouiDetail.ItemName ?? string.Empty; }
            set
            {
                if (SinKouiDetail.ItemName == value) return;
                SinKouiDetail.ItemName = value;
                //RaisePropertyChanged(() => ItemName);
            }
        }

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        public double Suryo
        {
            get { return SinKouiDetail.Suryo; }
            set
            {
                if (SinKouiDetail.Suryo == value) return;
                SinKouiDetail.Suryo = value;
                //RaisePropertyChanged(() => Suryo);
            }
        }

        /// <summary>
        /// 数量２
        /// レセ電にのみ記載する数量（分、ｃｍ２など）
        /// </summary>
        public double Suryo2
        {
            get { return SinKouiDetail.Suryo2; }
            set
            {
                if (SinKouiDetail.Suryo2 == value) return;
                SinKouiDetail.Suryo2 = value;
                //RaisePropertyChanged(() => Suryo2);
            }
        }

        /// <summary>
        /// 書式区分
        /// 1: 列挙対象項目
        /// </summary>
        public int FmtKbn
        {
            get { return SinKouiDetail.FmtKbn; }
            set
            {
                if (SinKouiDetail.FmtKbn == value) return;
                SinKouiDetail.FmtKbn = value;
                //RaisePropertyChanged(() => FmtKbn);
            }
        }

        /// <summary>
        /// 単位コード
        /// 
        /// </summary>
        public int UnitCd
        {
            get { return SinKouiDetail.UnitCd; }
            set
            {
                if (SinKouiDetail.UnitCd == value) return;
                SinKouiDetail.UnitCd = value;
                //RaisePropertyChanged(() => UnitCd);
            }
        }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        public string UnitName
        {
            get { return SinKouiDetail.UnitName ?? string.Empty; }
            set
            {
                if (SinKouiDetail.UnitName == value) return;
                SinKouiDetail.UnitName = value;
                //RaisePropertyChanged(() => UnitName);
            }
        }

        /// <summary>
        /// 点数
        /// 当該項目の点数。金額項目の場合、10で割ったものを記録
        /// </summary>
        public double Ten
        {
            get { return SinKouiDetail.Ten; }
            set
            {
                if (SinKouiDetail.Ten == value) return;
                SinKouiDetail.Ten = value;
                //RaisePropertyChanged(() => Ten);
            }
        }

        /// <summary>
        /// 消費税
        /// "自費の場合の税金分
        /// TEN+ZEIになるようにする
        /// 内税項目の場合は、単価/(1+税率)*税率
        /// 単価-消費税をTENとする"
        /// </summary>
        public double Zei
        {
            get { return SinKouiDetail.Zei; }
            set
            {
                if (SinKouiDetail.Zei == value) return;
                SinKouiDetail.Zei = value;
                //RaisePropertyChanged(() => Zei);
            }
        }

        /// <summary>
        /// レセ非表示区分
        /// "1:非表示
        /// 2:電算のみ非表示"
        /// </summary>
        public int IsNodspRece
        {
            get { return SinKouiDetail.IsNodspRece; }
            set
            {
                if (SinKouiDetail.IsNodspRece == value) return;
                SinKouiDetail.IsNodspRece = value;
                //RaisePropertyChanged(() => IsNodspRece);
            }
        }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspPaperRece
        {
            get { return SinKouiDetail.IsNodspPaperRece; }
            set
            {
                if (SinKouiDetail.IsNodspPaperRece == value) return;
                SinKouiDetail.IsNodspPaperRece = value;
                //RaisePropertyChanged(() => IsNodspPaperRece);
            }
        }

        /// <summary>
        /// 領収証非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspRyosyu
        {
            get { return SinKouiDetail.IsNodspRyosyu; }
            set
            {
                if (SinKouiDetail.IsNodspRyosyu == value) return;
                SinKouiDetail.IsNodspRyosyu = value;
                //RaisePropertyChanged(() => IsNodspRyosyu);
            }
        }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        public string CmtOpt
        {
            get { return SinKouiDetail.CmtOpt ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtOpt == value) return;
                SinKouiDetail.CmtOpt = value;
                //RaisePropertyChanged(() => CmtOpt);
            }
        }

        /// <summary>
        /// コメント１
        /// 
        /// </summary>
        public string Cmt1
        {
            get { return SinKouiDetail.Cmt1 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.Cmt1 == value) return;
                SinKouiDetail.Cmt1 = value;
                //RaisePropertyChanged(() => Cmt1);
            }
        }

        /// <summary>
        /// コメントコード１
        /// 
        /// </summary>
        public string CmtCd1
        {
            get { return SinKouiDetail.CmtCd1 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtCd1 == value) return;
                SinKouiDetail.CmtCd1 = value;
                //RaisePropertyChanged(() => CmtCd1);
            }
        }

        /// <summary>
        /// コメント文１
        /// 
        /// </summary>
        public string CmtOpt1
        {
            get { return SinKouiDetail.CmtOpt1 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtOpt1 == value) return;
                SinKouiDetail.CmtOpt1 = value;
                //RaisePropertyChanged(() => CmtOpt1);
            }
        }

        /// <summary>
        /// コメント２
        /// 
        /// </summary>
        public string Cmt2
        {
            get { return SinKouiDetail.Cmt2 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.Cmt2 == value) return;
                SinKouiDetail.Cmt2 = value;
                //RaisePropertyChanged(() => Cmt2);
            }
        }

        /// <summary>
        /// コメントコード２
        /// 
        /// </summary>
        public string CmtCd2
        {
            get { return SinKouiDetail.CmtCd2 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtCd2 == value) return;
                SinKouiDetail.CmtCd2 = value;
                //RaisePropertyChanged(() => CmtCd2);
            }
        }

        /// <summary>
        /// コメント文２
        /// 
        /// </summary>
        public string CmtOpt2
        {
            get { return SinKouiDetail.CmtOpt2 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtOpt2 == value) return;
                SinKouiDetail.CmtOpt2 = value;
                //RaisePropertyChanged(() => CmtOpt2);
            }
        }

        /// <summary>
        /// コメント３
        /// 
        /// </summary>
        public string Cmt3
        {
            get { return SinKouiDetail.Cmt3 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.Cmt3 == value) return;
                SinKouiDetail.Cmt3 = value;
                //RaisePropertyChanged(() => Cmt3);
            }
        }

        /// <summary>
        /// コメントコード３
        /// 
        /// </summary>
        public string CmtCd3
        {
            get { return SinKouiDetail.CmtCd3 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtCd3 == value) return;
                SinKouiDetail.CmtCd3 = value;
                //RaisePropertyChanged(() => CmtCd3);
            }
        }

        /// <summary>
        /// コメント文３
        /// 
        /// </summary>
        public string CmtOpt3
        {
            get { return SinKouiDetail.CmtOpt3 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtOpt3 == value) return;
                SinKouiDetail.CmtOpt3 = value;
                //RaisePropertyChanged(() => CmtOpt3);
            }
        }

        // ------------------------------------------------


        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDay
        {
            get { return SinKouiCount.SinDay; }
            set
            {
                if (SinKouiCount.SinDay == value) return;
                SinKouiCount.SinDay = value;
                //RaisePropertyChanged(() => SinDay);
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return SinKouiCount.RaiinNo; }
            set
            {
                if (SinKouiCount.RaiinNo == value) return;
                SinKouiCount.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }

        /// <summary>
        /// 回数
        /// 来院ごとの回数
        /// </summary>
        public int Count
        {
            get { return SinKouiCount.Count; }
            set
            {
                if (SinKouiCount.Count == value) return;
                SinKouiCount.Count = value;
                //RaisePropertyChanged(() => Count);
            }
        }

        /// <summary>
        /// 診療日（YYYYMMDD形式）
        /// </summary>
        public int SinDate
        {
            get { return SinKouiDetail.SinYm * 100 + SinKouiCount.SinDay; }
        }

    }

}
