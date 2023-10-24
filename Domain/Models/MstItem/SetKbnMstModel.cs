namespace Domain.Models.MstItem
{
    public class SetKbnMstModel
    {
        public SetKbnMstModel(int hpId, int setKbn, int setKbnEdaNo, int generationId, string setKbnName, int kaCd, int docCd)
        {
            HpId = hpId;
            SetKbn = setKbn;
            SetKbnEdaNo = setKbnEdaNo;
            GenerationId = generationId;
            SetKbnName = setKbnName;
            KaCd = kaCd;
            DocCd = docCd;
        }


        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get; private set; }

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
        /// セット区分名称
        ///    
        /// </summary>
        public string SetKbnName { get; private set; }

        /// <summary>
        /// 診療科コード
        ///     0: 共通
        /// </summary>
        public int KaCd { get; private set; }

        /// <summary>
        /// 医師コード
        ///      0: 共通
        /// </summary>
        public int DocCd { get; private set; }
    }
}
