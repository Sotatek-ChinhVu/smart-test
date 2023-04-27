using Helper.Common;
using Helper.Enum;

namespace Domain.Models.MstItem
{
    public class DensiHaihanModel
    {
        public DensiHaihanModel(int initModelType, int modelType, int id, int hpId, string itemCd1, string originItemCd2, string itemCd2, string prevItemCd2, string name, int haihanKbn, int spJyoken, int startDate, int endDate, long seqNo, int userSetting, int targetKbn, int termCnt, int termSbt, int isInvalid, bool isReadOnly, bool isModified, bool isDeleted)
        {
            InitModelType = initModelType;
            Id = id;
            HpId = hpId;
            ItemCd1 = itemCd1;
            OriginItemCd2 = originItemCd2;
            ItemCd2 = itemCd2;
            PrevItemCd2 = prevItemCd2;
            Name = name;
            HaihanKbn = haihanKbn;
            SpJyoken = spJyoken;
            StartDate = startDate;
            EndDate = endDate;
            SeqNo = seqNo;
            UserSetting = userSetting;
            TargetKbn = targetKbn;
            TermCnt = termCnt;
            TermSbt = termSbt;
            IsInvalid = isInvalid;
            ModelType = modelType;
            IsReadOnly = isReadOnly;
            IsModified = isModified;
            IsDeleted = isDeleted;
        }

        public DensiHaihanModel()
        {
            ItemCd1 = string.Empty;
            ItemCd2 = string.Empty;
            OriginItemCd2 = string.Empty;
            PrevItemCd2 = string.Empty;
            Name = string.Empty;
        }


        public int InitModelType { get; private set; }

        public int Id { get; private set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 項目コード１
        /// 
        /// </summary>
        public string ItemCd1 { get; private set; }

        public string OriginItemCd2 { get; private set; }

        public bool IsAddNew
        {
            get => Id == 0 && IsDeleted == false;
        }

        public string ItemCd2 { get; private set; }


        public string PrevItemCd2 { get; private set; }


        public string Name { get; private set; }

        /// <summary>
        /// 背反区分
        /// "背反の条件を表す。 
        /// 1: 診療行為コード①を算定する。 
        /// 2: 診療行為コード②を算定する。 
        /// 3: 何れか一方を算定する。"
        /// </summary>
        public int HaihanKbn { get; private set; }

        
        public int SpJyoken { get; private set; }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate { get; private set; }

        public string StartDateBinding
        {
            get => CIUtil.SDateToShowSDate(StartDate);
        }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate { get; private set; }

        public string EndDateBinding
        {
            get => EndDate == 99999999 ? (CheckDefaultValue() ? "" : "9999/99/99") : CIUtil.SDateToShowSDate(EndDate);
        }

        public void SetEndDate(int value)
        {
            if (value == 0)
                EndDate = 99999999;
            else
                EndDate = value;
        }

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
        /// チェック期間数
        /// "TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録"
        /// </summary>
        public int TermCnt { get; private set; }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        public int TermSbt { get; private set; }

        public string TermSbtName
        {
            get => _termSbtDict[TermSbt];
        }

        private Dictionary<int, string> _termSbtDict = new Dictionary<int, string>()
        {
            { 0, "未指定" },
            { 1, "来院" },
            { 2, "日" },
            { 3, "暦週" },
            { 4, "暦月" },
            { 5, "週" },
            { 6, "月" },
            { 9, "患者当たり" }
        };


        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid { get; private set; }

        private int _modelType = (int)HaiHanModelType.DENSI_HAIHAN_DAY;

        public int ModelType
        {
            get => _modelType;
            set
            {
                if (IsReadOnly) return;
                _modelType = value;
                if (ModelType == (int)HaiHanModelType.DENSI_HAIHAN_CUSTOM)
                {
                    TermSbt = 1;
                }
                else
                {
                    TermSbt = 0;
                }
                IsModified = true;
            }
        }

        /// <summary>
        /// USER_SETTING in (0, 1)の場合、変更不可
        /// </summary>
        public bool IsReadOnly { get; private set; }

        /// <summary>
        /// Check Model Modified
        /// </summary>
        public bool IsModified { get; private set; }

        
        public bool IsDeleted { get; private set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd2) && string.IsNullOrEmpty(Name) && StartDate == 0 && EndDate == 99999999;
        }

        public bool IsValidValue()
        {
            return !string.IsNullOrEmpty(ItemCd2) && !string.IsNullOrEmpty(Name) && ItemCd2 == PrevItemCd2;
        }
    }
}
