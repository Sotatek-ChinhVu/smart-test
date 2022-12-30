using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class DensiSanteiKaisuModel
    {
        public DensiSanteiKaisu DensiSanteiKaisu { get; } = null;

        public DensiSanteiKaisuModel(DensiSanteiKaisu densiSanteiKaisu)
        {
            DensiSanteiKaisu = densiSanteiKaisu;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return DensiSanteiKaisu.HpId; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return DensiSanteiKaisu.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 算定単位コード
        /// "チェック対象のコードは、
        /// 53:患者あたり, 121:日, 131:月,138:週, 
        /// 141:一連, 142:2週, 
        /// 143:2月, 144:3月, 145:4月, 146:6月, 147:12月, 
        /// 148:5年
        /// 997:初診から1ヶ月算定不可（休日除く）
        /// 998:初診から1ヶ月算定不可
        /// 999:カスタム(TERM_COUNT, TERM_SBTを使用)"
        /// </summary>
        public int UnitCd
        {
            get { return DensiSanteiKaisu.UnitCd; }
        }

        /// <summary>
        /// 算定回数
        /// 算定単位ごとの上限回数を表す。
        /// </summary>
        public int MaxCount
        {
            get { return DensiSanteiKaisu.MaxCount; }
        }

        /// <summary>
        /// 特例条件
        /// "算定条件に特別な条件がある場合に設定する。 
        /// 0: 条件なし 
        /// 1: 条件あり "
        /// </summary>
        public int SpJyoken
        {
            get { return DensiSanteiKaisu.SpJyoken; }
        }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate
        {
            get { return DensiSanteiKaisu.StartDate; }
        }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate
        {
            get { return DensiSanteiKaisu.EndDate; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo
        {
            get { return DensiSanteiKaisu.SeqNo; }
        }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        public int UserSetting
        {
            get { return DensiSanteiKaisu.UserSetting; }
        }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        public int TargetKbn
        {
            get { return DensiSanteiKaisu.TargetKbn; }
        }

        /// <summary>
        /// チェック期間数
        /// "TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_COUNT=2, TERM_SBT=1と登録"
        /// </summary>
        public int TermCount
        {
            get { return DensiSanteiKaisu.TermCount; }
        }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 2:日 3:暦週 4:暦月
        /// </summary>
        public int TermSbt
        {
            get { return DensiSanteiKaisu.TermSbt; }
        }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid
        {
            get { return DensiSanteiKaisu.IsInvalid; }
        }

        /// <summary>
        /// 項目グループコード
        /// ITEM_GRP_MST.ITEM_GRP_CD
        /// </summary>
        public int ItemGrpCd
        {
            get { return DensiSanteiKaisu.ItemGrpCd; }
        }
        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return DensiSanteiKaisu.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者ID
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return DensiSanteiKaisu.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return DensiSanteiKaisu.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return DensiSanteiKaisu.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者ID
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return DensiSanteiKaisu.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return DensiSanteiKaisu.UpdateMachine; }
        //}


    }

}
