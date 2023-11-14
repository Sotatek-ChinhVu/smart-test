using Helper.Common;

namespace Domain.Models.MstItem
{
    public class DensiHoukatuGrpModel
    {
        public DensiHoukatuGrpModel(int hpId, string houkatuGrpNo, string itemCd, int spJyoken, int startDate, int endDate, long seqNo, int userSetting, int targetKbn, int isInvalid, int houkatuTerm, string name, string houkatuItemCd, bool isUpdate, bool isDeleted)
        {
            HpId = hpId;
            HoukatuGrpNo = houkatuGrpNo;
            ItemCd = itemCd;
            SpJyoken = spJyoken;
            StartDate = startDate;
            EndDate = endDate;
            SeqNo = seqNo;
            UserSetting = userSetting;
            TargetKbn = targetKbn;
            IsInvalid = isInvalid;
            HoukatuTerm = houkatuTerm;
            Name = name;
            HoukatuItemCd = houkatuItemCd;
            IsUpdate = isUpdate;
            IsDeleted = isDeleted;
        }

        public DensiHoukatuGrpModel()
        {
            ItemCd = string.Empty;
            HoukatuGrpNo = string.Empty;
            HoukatuItemCd = string.Empty;
            Name = string.Empty;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 包括グループ番号
        /// 
        /// </summary>
        public string HoukatuGrpNo { get; private set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd { get; private set; }

        /// <summary>
        /// 特例条件
        /// "包括・被包括の条件に特別な条件がある場合に設定する 
        /// 0: 条件なし
        /// 1: 条件あり "
        /// </summary>
        public int SpJyoken { get; private set; }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate { get; private set; }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate { get; private set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo { get; private set; }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        public int UserSetting { get; private set; }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        public int TargetKbn { get; private set; }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid { get; private set; }

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
        public int HoukatuTerm { get; private set; }

        public string HoukatuTermDisplay
        {
            get
            {
                string termDisplay = string.Empty;
                switch (HoukatuTerm)
                {
                    case 1:
                        termDisplay = "日";
                        break;
                    case 2:
                        termDisplay = "月";
                        break;
                    case 3:
                        termDisplay = "同時";
                        break;
                    case 99:
                        termDisplay = "月（算定日以降）";
                        break;
                }
                return termDisplay;
            }
        }
        /// <summary>
        /// 漢字名称
        /// </summary>
        public string Name { get; private set; }

        public string HoukatuItemCd { get; private set; }

        public string StartDateBinding
        {
            get => CIUtil.SDateToShowSDate(StartDate);
        }

        public string EndDateBinding
        {
            get
            {
                if (EndDate == 99999999)
                {
                    if (!CheckDefaultValue())
                    {
                        return "9999/99/99";
                    }
                }
                else
                {
                    return CIUtil.SDateToShowSDate(EndDate);
                }
                return string.Empty;
            }
        }

        public bool CanEditItem
        {
            get => !CheckDefaultValue() && IsInvalid == 0;
        }

        public bool IsUpdate { get; private set; }

        public bool IsDeleted { get; private set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd);
        }
    }
}
