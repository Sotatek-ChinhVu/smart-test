﻿using Helper.Constants;

namespace Domain.Models.MstItem
{
    public class SetNameMntModel
    {
        public SetNameMntModel(bool isSetOdrInfDetail, string itemNameTenMst, int cmtCol1, int cmtColKeta1, int cmtCol2, int cmtColKeta2, int cmtCol3, int cmtColKeta3,
        int cmtCol4, int cmtColKeta4, int setCd, int setKbn, int setKbnEdaNo, int generationId, int level1, int level2, int level3,
        string setName, int rowNo, string itemCd, string itemName, string cmtName, string cmtOpt, DateTime createDate, int createId,
        string createMachine, DateTime updateDate, int updateId, string updateMachine)
        {
            IsSetOdrInfDetail = isSetOdrInfDetail;
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
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            SetName = setName;
            RowNo = rowNo;
            ItemCd = itemCd;
            ItemName = itemName;
            CmtName = cmtName;
            CmtOpt = cmtOpt;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;

        }
        public SetNameMntModel(bool isSetOdrInfDetail, int setCd, int setKbn, int setKbnEdaNo, int generationId, int level1, int level2, int level3,
        string setName, DateTime createDate, int createId,
        string createMachine, DateTime updateDate, int updateId, string updateMachine, string itemNameTenMst, int cmtCol1, int cmtColKeta1, int cmtCol2, int cmtColKeta2, int cmtCol3, int cmtColKeta3,
        int cmtCol4, int cmtColKeta4)
        {
            IsSetOdrInfDetail = isSetOdrInfDetail;
            SetCd = setCd;
            SetKbn = setKbn;
            SetKbnEdaNo = setKbnEdaNo;
            GenerationId = generationId;
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            SetName = setName;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;           
            ItemNameTenMst = itemNameTenMst;
            CmtCol1 = cmtCol1;
            CmtColKeta1 = cmtColKeta1;
            CmtCol2 = cmtCol2;
            CmtColKeta2 = cmtColKeta2;
            CmtCol3 = cmtCol3;
            CmtColKeta3 = cmtColKeta3;
            CmtCol4 = cmtCol4;
            CmtColKeta4 = cmtColKeta4;
        }
        public bool IsSetOdrInfDetail { get; private set; }
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
        /// レベル１
        /// </summary>
        public int Level1 { get; private set; }

        /// <summary>
        /// レベル２
        /// </summary>
        public int Level2 { get; private set; }

        /// <summary>
        /// レベル３
        /// </summary>
        public int Level3 { get; private set; }

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
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// 作成者
        /// </summary>
        public int CreateId { get; private set; }

        /// <summary>
        /// 作成端末 
        /// </summary>
        public string CreateMachine { get; private set; }

        /// <summary>
        /// 更新日時 
        /// </summary>
        public DateTime UpdateDate { get; private set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public int UpdateId { get; private set; }

        /// <summary>
        /// 更新端末 
        /// </summary>
        public string UpdateMachine { get; private set; }

        public bool IsSet { get; set; }

        public string SetCategory { get; set; }

        public string SetKbnEdaNoBinding { get; set; }

        public string Level1Binding { get; set; }

        public string Level2Binding { get; set; }

        public string Level3Binding { get; set; }

        public string IsSetString { get; set; }

        public string ItemNameBinding { get; set; }

        public string ItemNameTenMstBinding { get; set; }

        public string SetFlag { get; set; }

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
