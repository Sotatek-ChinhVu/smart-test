using Helper.Constants;

namespace Domain.Models.MstItem
{
    public class SaveSetNameMntModel
    {
        public SaveSetNameMntModel(string itemNameTenMst, int cmtCol1, int cmtColKeta1, int cmtCol2, int cmtColKeta2, int cmtCol3, int cmtColKeta3, int cmtCol4, int cmtColKeta4, int setCd, int setKbn, int setKbnEdaNo, int generationId, string setName, int rowNo, string itemCd, string itemName, string cmtName, string cmtOpt, bool isSet, string itemNameTenMstBinding, string setFlag)
        {
            ItemNameTenMst = itemNameTenMst;
            CmtCol1 = cmtCol1;
            CmtColKeta1 = cmtColKeta1;
            CmtCol2 = cmtCol2;
            CmtColKeta2 = cmtColKeta2;
            CmtCol3 = cmtCol3;
            CmtColKeta3 = cmtColKeta3;
            CmtCol4 = cmtCol4;
            CmtColKeta4 = cmtColKeta4;
            SetCd = setCd;
            SetKbn = setKbn;
            SetKbnEdaNo = setKbnEdaNo;
            GenerationId = generationId;
            SetName = setName;
            RowNo = rowNo;
            ItemCd = itemCd;
            ItemName = itemName;
            CmtName = cmtName;
            CmtOpt = cmtOpt;
            IsSet = isSet;
            ItemNameTenMstBinding = itemNameTenMstBinding;
            SetFlag = setFlag;
        }

        public string ItemNameTenMst { get; private set; }

        public int CmtCol1 { get; private set; }

        public int CmtColKeta1 { get; private set; }

        public int CmtCol2 { get; private set; }

        public int CmtColKeta2 { get; private set; }

        public int CmtCol3 { get; private set; }

        public int CmtColKeta3 { get; private set; }

        public int CmtCol4 { get; private set; }

        public int CmtColKeta4 { get; private set; }

        /// <summary>
        /// セットコード
        /// </summary>
        public int SetCd { get; private set; }

        /// <summary>
        /// セット区分
        /// </summary>
        public int SetKbn { get; private set; }

        /// <summary>
        /// セット区分枝番
        /// </summary>
        public int SetKbnEdaNo { get; private set; }

        /// <summary>
        /// 世代ID
        /// </summary>
        public int GenerationId { get; private set; }

        /// <summary>
        /// セット名称
        /// </summary>
        public string SetName { get; private set; }

        /// <summary>
        /// 行番号
        /// </summary>
        public int RowNo { get; private set; } = 0;

        /// <summary>
        /// 項目コード
        ///     TEN_MST.ITEM_CD
        /// </summary>
        public string ItemCd { get; private set; } = string.Empty;

        /// <summary>
        /// 項目名称
        /// </summary>
        public string ItemName { get; private set; } = string.Empty;

        /// <summary>
        /// コメント名称
        ///    "コメントマスターの名称
        ///    ※当該項目がコメント項目の場合に使用"
        /// </summary>
        public string CmtName { get; private set; } = string.Empty;

        /// <summary>
        /// コメント文
        ///    コメントマスターの定型文に組み合わせる文字情報
        ///    ※当該項目がコメント項目の場合に使用
        /// </summary>
        public string CmtOpt { get; private set; } = string.Empty;

        /// <summary>
        /// 作成日時 
        /// </summary>
        public bool IsSet { get; private set; } = false;

        public string ItemNameTenMstBinding { get; private set; } = string.Empty;

        public string SetFlag { get; private set; } = string.Empty;

        public bool Is830Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment830Pattern);

        public bool Is831Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment831Pattern);

        public bool Is840Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment840Pattern) && ItemCd != ItemCdConst.GazoDensibaitaiHozon;

        public bool Is842Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment842Pattern);

        public bool Is850Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment850Pattern);

        public bool Is851Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment851Pattern);

        public bool Is852Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment852Pattern);

        public bool Is853Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment853Pattern);

        public bool Is880Cmt => !string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith(ItemCdConst.Comment880Pattern);

        public bool IsCommentMaster => Is830Cmt || Is831Cmt || Is840Cmt || Is842Cmt || Is850Cmt || Is851Cmt || Is852Cmt || Is853Cmt || Is880Cmt;

        public bool CheckDefaultValue()
        {
            return SetCd <= 0;
        }
    }
}
