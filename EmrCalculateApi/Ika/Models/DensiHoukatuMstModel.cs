using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class DensiHoukatuMstModel 
    {
        public DensiHoukatuGrp DensiHoukatuGrp { get; } = null;
        public DensiHoukatu DensiHoukatu { get; } = null;

        public DensiHoukatuMstModel(DensiHoukatuGrp densiHoukatuGrp, DensiHoukatu densiHoukatu)
        {
            DensiHoukatuGrp = densiHoukatuGrp;
            DensiHoukatu = densiHoukatu;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return DensiHoukatu.HpId; }
        }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return DensiHoukatuGrp.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 他の項目を包括する項目の診療行為コード
        /// </summary>
        public string DelItemCd
        {
            get { return DensiHoukatu.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 包括単位
        /// "包括する期間を表す
        /// 00: 関連なし
        /// 01: 1日につき
        /// 02: 同一月内
        /// 03: 同時
        /// 05: 手術前1週間
        /// 06: 1手術につき
        /// ※05,06はチェックしない"
        /// </summary>
        public int HoukatuTerm
        {
            get { return DensiHoukatu.HoukatuTerm; }
        }

        /// <summary>
        /// 包括グループ番号
        /// 0 "包括・被包括グループ番号を表す。 
        /// 包括・被包括テーブルの参照先グループを表す。"
        /// </summary>
        public string HoukatuGrpNo
        {
            get { return DensiHoukatu.HoukatuGrpNo ?? string.Empty; }
        }

        /// <summary>
        /// 特例条件
        /// "包括・被包括の条件に特別な条件がある場合に設定する 
        /// 0: 条件なし
        /// 1: 条件あり "
        /// </summary>
        public int SpJyoken
        {
            get { return DensiHoukatuGrp.SpJyoken; }
        }
    }
}
