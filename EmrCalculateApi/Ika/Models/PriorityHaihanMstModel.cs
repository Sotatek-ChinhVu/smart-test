using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class PriorityHaihanMstModel
    {
        public PriorityHaihanMst PriorityHaihanMst { get; } = null;

        public PriorityHaihanMstModel(PriorityHaihanMst priorityHaihanMst)
        {
            PriorityHaihanMst = priorityHaihanMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return PriorityHaihanMst.HpId; }
        }

        /// <summary>
        /// 背反グループコード
        /// 
        /// </summary>
        public long HaihanGrp
        {
            get { return PriorityHaihanMst.HaihanGrp; }
        }

        /// <summary>
        /// 算定数
        /// 2～8
        /// </summary>
        public int Count
        {
            get { return PriorityHaihanMst.Count; }
        }

        /// <summary>
        /// 項目コード１
        /// 
        /// </summary>
        public string ItemCd1
        {
            get { return PriorityHaihanMst.ItemCd1 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード２
        /// 
        /// </summary>
        public string ItemCd2
        {
            get { return PriorityHaihanMst.ItemCd2 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード３
        /// 
        /// </summary>
        public string ItemCd3
        {
            get { return PriorityHaihanMst.ItemCd3 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード４
        /// 
        /// </summary>
        public string ItemCd4
        {
            get { return PriorityHaihanMst.ItemCd4 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード５
        /// 
        /// </summary>
        public string ItemCd5
        {
            get { return PriorityHaihanMst.ItemCd5 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード６
        /// 
        /// </summary>
        public string ItemCd6
        {
            get { return PriorityHaihanMst.ItemCd6 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード７
        /// 
        /// </summary>
        public string ItemCd7
        {
            get { return PriorityHaihanMst.ItemCd7 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード８
        /// 
        /// </summary>
        public string ItemCd8
        {
            get { return PriorityHaihanMst.ItemCd8 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード９
        /// 
        /// </summary>
        public string ItemCd9
        {
            get { return PriorityHaihanMst.ItemCd9 ?? string.Empty; }
        }

        public string ItemCd(int index)
        {
            string ret = "";

            switch(index)
            {
                case 1:
                    ret = ItemCd1;
                    break;
                case 2:
                    ret = ItemCd2;
                    break;
                case 3:
                    ret = ItemCd3;
                    break;
                case 4:
                    ret = ItemCd4;
                    break;
                case 5:
                    ret = ItemCd5;
                    break;
                case 6:
                    ret = ItemCd6;
                    break;
                case 7:
                    ret = ItemCd7;
                    break;
                case 8:
                    ret = ItemCd8;
                    break;
                case 9:
                    ret = ItemCd9;
                    break;
                default:
                    ret = "";
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 特例条件
        /// "背反条件に特別な条件がある場合に設定する 
        /// 0: 条件なし
        /// 1: 条件あり "
        /// </summary>
        public int SpJyoken
        {
            get { return PriorityHaihanMst.SpJyoken; }
        }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate
        {
            get { return PriorityHaihanMst.StartDate; }
        }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate
        {
            get { return PriorityHaihanMst.EndDate; }
        }

        /// <summary>
        /// チェック期間数
        /// "TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録"
        /// </summary>
        public int TermCnt
        {
            get { return PriorityHaihanMst.TermCnt; }
        }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        public int TermSbt
        {
            get { return PriorityHaihanMst.TermSbt; }
        }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        public int UserSetting
        {
            get { return PriorityHaihanMst.UserSetting; }
        }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        public int TargetKbn
        {
            get { return PriorityHaihanMst.TargetKbn; }
        }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid
        {
            get { return PriorityHaihanMst.IsInvalid; }
        }
    }

}
