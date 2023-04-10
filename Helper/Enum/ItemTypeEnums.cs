namespace Helper.Enum
{
    /// <summary>
    /// WARNING: If adding a new item type in this file, please add it to Emr.CommonBase.CommonUserControl.Orders.Enums.ItemTypeEnums
    /// </summary>
    public enum ItemTypeEnums
    {
        None,
        /// <summary>
        /// 自費項目
        /// </summary>
        JihiItem,
        /// <summary>
        /// 用法
        /// </summary>
        UsageItem,
        /// <summary>
        /// 特定医療材料
        /// </summary>
        SpecificMedicalMeterialItem,
        /// <summary>
        /// COコメント
        /// </summary>
        COCommentItem,
        /// <summary>
        /// 特薬コメント
        /// </summary>
        SpecialMedicineCommentItem,
        /// <summary>
        /// コニカ連携コード
        /// </summary>
        KonikaItem,
        /// <summary>
        /// FCR連携コード
        /// </summary>
        FCRItem,
        /// <summary>
        /// 併用禁忌
        /// </summary>
        CombinedContraindicationItem,
        /// <summary>
        /// KN検査	
        /// </summary>
        KensaItem,
        /// <summary>
        /// IGE特異的
        /// </summary>
        TokuiTeki,
        /// <summary>
        /// S自賠責
        /// </summary>
        Jibaiseki,
        /// <summary>
        /// Xダミー
        /// </summary>
        Dami,
        /// <summary>
        /// 診療行為
        /// </summary>
        ShinryoKoi,
        /// <summary>
        /// 薬剤
        /// </summary>
        Yakuzai,
        /// <summary>
        /// 特材
        /// </summary>
        Tokuzai,
        /// <summary>
        /// 部位
        /// </summary>
        Bui,
        /// <summary>
        /// コメント
        /// </summary>
        CommentItem,
        /// <summary>
        /// その他
        /// </summary>
        Other,
        /// <summary>
        /// 公害
        /// </summary>
        Kogai,
        /// <summary>
        /// 島津連携コード
        /// </summary>
        Shimadzu,
    }
}
