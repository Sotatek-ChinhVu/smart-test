using Domain.Constant;
using Entity.Tenant;

namespace Reporting.Karte1.Model
{
    public class CoPtByomeiModel
    {
        public PtByomei PtByomei { get; } = null;

        public CoPtByomeiModel(PtByomei ptByomei)
        {
            PtByomei = ptByomei;
        }

        /// <summary>
        /// 患者病名
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtByomei.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return PtByomei.PtId; }
        }

        /// <summary>
        /// 基本病名コード
        ///     コードを使用しない場合、「0000999」をセット
        /// </summary>
        public string ByomeiCd
        {
            get { return PtByomei.ByomeiCd; }
        }

        /// <summary>
        /// 連番
        ///     患者の病名を識別するためのシステム固有の番号
        /// </summary>
        public long SeqNo
        {
            get { return PtByomei.SeqNo; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return PtByomei.SortNo; }
        }

        /// <summary>
        /// 修飾語コード１
        /// </summary>
        public string SyusyokuCd1
        {
            get { return PtByomei.SyusyokuCd1; }
        }

        /// <summary>
        /// 修飾語コード２
        /// </summary>
        public string SyusyokuCd2
        {
            get { return PtByomei.SyusyokuCd2; }
        }

        /// <summary>
        /// 修飾語コード３
        /// </summary>
        public string SyusyokuCd3
        {
            get { return PtByomei.SyusyokuCd3; }
        }

        /// <summary>
        /// 修飾語コード４
        /// </summary>
        public string SyusyokuCd4
        {
            get { return PtByomei.SyusyokuCd4; }
        }

        /// <summary>
        /// 修飾語コード５
        /// </summary>
        public string SyusyokuCd5
        {
            get { return PtByomei.SyusyokuCd5; }
        }

        /// <summary>
        /// 修飾語コード６
        /// </summary>
        public string SyusyokuCd6
        {
            get { return PtByomei.SyusyokuCd6; }
        }

        /// <summary>
        /// 修飾語コード７
        /// </summary>
        public string SyusyokuCd7
        {
            get { return PtByomei.SyusyokuCd7; }
        }

        /// <summary>
        /// 修飾語コード８
        /// </summary>
        public string SyusyokuCd8
        {
            get { return PtByomei.SyusyokuCd8; }
        }

        /// <summary>
        /// 修飾語コード９
        /// </summary>
        public string SyusyokuCd9
        {
            get { return PtByomei.SyusyokuCd9; }
        }

        /// <summary>
        /// 修飾語コード１０
        /// </summary>
        public string SyusyokuCd10
        {
            get { return PtByomei.SyusyokuCd10; }
        }

        /// <summary>
        /// 修飾語コード１１
        /// </summary>
        public string SyusyokuCd11
        {
            get { return PtByomei.SyusyokuCd11; }
        }

        /// <summary>
        /// 修飾語コード１２
        /// </summary>
        public string SyusyokuCd12
        {
            get { return PtByomei.SyusyokuCd12; }
        }

        /// <summary>
        /// 修飾語コード１３
        /// </summary>
        public string SyusyokuCd13
        {
            get { return PtByomei.SyusyokuCd13; }
        }

        /// <summary>
        /// 修飾語コード１４
        /// </summary>
        public string SyusyokuCd14
        {
            get { return PtByomei.SyusyokuCd14; }
        }

        /// <summary>
        /// 修飾語コード１５
        /// </summary>
        public string SyusyokuCd15
        {
            get { return PtByomei.SyusyokuCd15; }
        }

        /// <summary>
        /// 修飾語コード１６
        /// </summary>
        public string SyusyokuCd16
        {
            get { return PtByomei.SyusyokuCd16; }
        }

        /// <summary>
        /// 修飾語コード１７
        /// </summary>
        public string SyusyokuCd17
        {
            get { return PtByomei.SyusyokuCd17; }
        }

        /// <summary>
        /// 修飾語コード１８
        /// </summary>
        public string SyusyokuCd18
        {
            get { return PtByomei.SyusyokuCd18; }
        }

        /// <summary>
        /// 修飾語コード１９
        /// </summary>
        public string SyusyokuCd19
        {
            get { return PtByomei.SyusyokuCd19; }
        }

        /// <summary>
        /// 修飾語コード２０
        /// </summary>
        public string SyusyokuCd20
        {
            get { return PtByomei.SyusyokuCd20; }
        }

        /// <summary>
        /// 修飾語コード２１
        /// </summary>
        public string SyusyokuCd21
        {
            get { return PtByomei.SyusyokuCd21; }
        }

        /// <summary>
        /// 病名
        ///     病名コードが「0000999」のときのみセット
        /// </summary>
        public string Byomei
        {
            get { return PtByomei.Byomei; }
        }

        public string ReceByomei
        {
            get
            {
                string ret = "";

                ret = PtByomei.Byomei;

                //ToDo: Duong.Le
                //if (PtByomei.SyobyoKbn == 1)
                //{
                //    ret = "（主）" + ret;
                //}

                return ret;
            }
        }

        /// <summary>
        /// 開始日
        ///     1:休診日
        /// </summary>
        public int StartDate
        {
            get { return PtByomei.StartDate; }
        }

        /// <summary>
        /// 転帰区分
        /// 転帰区分を表す。
        ///      0: 下記以外
        ///      1: 治ゆ
        ///      2: 中止
        ///      3: 死亡        
        ///      9: その他
        /// </summary>
        public int TenkiKbn
        {
            get { return PtByomei.TenkiKbn; }
        }
        /// <summary>
        /// 転帰
        /// </summary>
        public string Tenki
        {
            get
            {
                string ret = "";

                switch (PtByomei.TenkiKbn)
                {
                    case TenkiKbnConst.Cured:
                        ret = "治ゆ";
                        break;
                    case TenkiKbnConst.Dead:
                        ret = "死亡";
                        break;
                    case TenkiKbnConst.Canceled:
                        ret = "中止";
                        break;
                }

                return ret;
            }
        }
        /// <summary>
        /// 転帰日
        /// </summary>
        public int TenkiDate
        {
            get { return PtByomei.TenkiDate; }
        }

        /// <summary>
        /// 主病名区分
        ///     0: 主病名以外
        ///     1: 主病名
        /// </summary>
        //ToDo: Duong.Le
        //public int SyobyoKbn
        //{
        //    get { return PtByomei.SyobyoKbn; }
        //}

        /// <summary>
        /// 慢性疾患区分
        ///     特定疾患療養指導料等の算定対象であるか否かを表す
        ///     00: 対象外
        ///     03: 皮膚科特定疾患指導管理料（１）算定対象
        ///     04: 皮膚科特定疾患指導管理料（２）算定対象
        ///     05: 特定疾患療養指導料／老人慢性疾患生活指導料算定対象
        ///     07: てんかん指導料算定対象 
        ///     08: 特定疾患療養管理料又はてんかん指導料算
        ///     定対象 
        /// </summary>
        public int SikkanKbn
        {
            get { return PtByomei.SikkanKbn; }
        }

        /// <summary>
        /// 難病外来コード
        ///     当該傷病名が難病外来指導管理料の算定対象であるか否かを表す。
        ///     00: 算定対象外
        ///     09: 難病外来指導管理料算定対象
        /// </summary>
        public int NanByoCd
        {
            get { return PtByomei.NanByoCd; }
        }

        /// <summary>
        /// 補足コメント
        /// </summary>
        public string HosokuCmt
        {
            get { return PtByomei.HosokuCmt ?? ""; }
        }

        /// <summary>
        /// 保険組み合わせ番号
        ///     0: 共通病名
        /// </summary>
        public int HokenPid
        {
            get { return PtByomei.HokenPid; }
        }

        /// <summary>
        /// 当月病名区分
        ///     1: 当月病名
        /// </summary>
        public int TogetuByomei
        {
            get { return PtByomei.TogetuByomei; }
        }

        /// <summary>
        /// レセプト非表示区分
        ///     1: 非表示
        /// </summary>
        public int IsNodspRece
        {
            get { return PtByomei.IsNodspRece; }
        }

        /// <summary>
        /// カルテ非表示区分
        ///     1: 非表示
        /// </summary>
        public int IsNodspKarte
        {
            get { return PtByomei.IsNodspKarte; }
        }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtByomei.IsDeleted; }
        }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime CreateDate
        {
            get => PtByomei.CreateDate;
        }
        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime UpdateDate
        {
            get => PtByomei.UpdateDate;
        }

    }

}
