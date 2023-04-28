using Helper.Common;

namespace Domain.Models.MstItem
{
    public class TeikyoByomeiModel
    {
        public TeikyoByomeiModel(int sikkanCd, int hpId, string itemCd, string byomeiCd, int startYM, int endYM, int isInvalid, int isInvalidTokusyo, int editKbn, int systemData, string byomei, string kanaName, bool isDeleted, bool isModified)
        {
            SikkanCd = sikkanCd;
            HpId = hpId;
            ItemCd = itemCd;
            ByomeiCd = byomeiCd;
            StartYM = startYM;
            EndYM = endYM;
            IsInvalid = isInvalid;
            IsInvalidTokusyo = isInvalidTokusyo;
            EditKbn = editKbn;
            SystemData = systemData;
            Byomei = byomei;
            KanaName = kanaName;
            IsDeleted = isDeleted;
            IsModified = isModified;
        }

        public TeikyoByomeiModel()
        {
            ItemCd = string.Empty;
            ByomeiCd = string.Empty;
            Byomei = string.Empty;
            KanaName = string.Empty;
        }

        public int SikkanCd { get; private set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd { get; private set; }

        /// <summary>
        /// 病名コード
        /// 
        /// </summary>
        public string ByomeiCd { get; private set; }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartYM { get; private set; }

        public string StartYMDisplay
        {
            get => StartYM == 0 ? "0" : CIUtil.SMonthToShowSMonth(StartYM);
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndYM { get; private set; }

        public string EndYMDisplay
        {
            get => EndYM == 999999 ? "9999/99" : CIUtil.SMonthToShowSMonth(EndYM);
        }

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        public int IsInvalid { get; private set; }

        /// <summary>
        /// 特処無効区分
        /// 0:有効 1:無効
        /// </summary>
        public int IsInvalidTokusyo { get; private set; }

        /// <summary>
        /// 編集区分
        /// 1:ユーザーが編集したデータ
        /// </summary>
        public int EditKbn { get; private set; }

        /// <summary>
        /// システム管理データ
        /// 1:配信したマスタ
        /// </summary>
        public int SystemData { get; private set; }

        public string Byomei { get; private set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string KanaName { get; private set; }


        public bool IsDeleted { get; private set; }

        public bool IsModified { get; private set; }

        
        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(Byomei) && string.IsNullOrEmpty(ByomeiCd) && StartYM == 0 && EndYM == 999999;
        }
    }
}
