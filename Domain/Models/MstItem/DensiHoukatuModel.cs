using Helper.Common;

namespace Domain.Models.MstItem
{
    public class DensiHoukatuModel
    {
        public DensiHoukatuModel(int hpId, string itemCd, int startDate, int endDate, int targetKbn, long seqNo, int houkatuTerm, string houkatuGrpNo, int userSetting, int isInvalid, bool isInvalidBinding, string name, string houkatuGrpItemCd, int spJyoken, bool isModified, bool isDeleted)
        {
            HpId = hpId;
            ItemCd = itemCd;
            StartDate = startDate;
            EndDate = endDate;
            TargetKbn = targetKbn;
            SeqNo = seqNo;
            HoukatuTerm = houkatuTerm;
            HoukatuGrpNo = houkatuGrpNo;
            UserSetting = userSetting;
            IsInvalid = isInvalid;
            IsInvalidBinding = isInvalidBinding;
            Name = name;
            HoukatuGrpItemCd = houkatuGrpItemCd;
            SpJyoken = spJyoken;
            IsModified = isModified;
            IsDeleted = isDeleted;
        }

        public DensiHoukatuModel()
        {
            ItemCd = string.Empty;
            Name = string.Empty;
            HoukatuGrpNo = string.Empty;
            HoukatuGrpItemCd = string.Empty;
        }

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
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        public int TargetKbn { get; private set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo { get; private set; }

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

        /// <summary>
        /// 包括グループ番号
        /// 0 "包括・被包括グループ番号を表す。 
        /// 包括・被包括テーブルの参照先グループを表す。"
        /// </summary>
        public string HoukatuGrpNo { get; private set; }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        public int UserSetting { get; private set; }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid { get; private set; }

        public bool IsInvalidBinding { get; private set; }

        public void SetIsInvalid(int value) => IsInvalid = value;

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

        public string HoukatuGrpItemCd { get; private set; }

        public int SpJyoken { get; private set; }

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

        public bool IsModified { get; private set; }

        public bool IsDeleted { get; private set; }

        public bool CanEditItem
        {
            get => !CheckDefaultValue() && IsInvalid == 0;
        }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd);
        }
    }
}
