using Domain.Models.PtFamilyReki;
using Helper.Constants;
using Helper.Extendsions;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtFamilyRekiItem : ObservableObject
    {
        public PtFamilyRekiModel PtFamilyReki { get; }
        private const string FREE_WORD = "0000999";

        public PtFamilyRekiItem(PtFamilyRekiModel ptFamilyReki)
        {
            PtFamilyReki = ptFamilyReki;
            if (string.IsNullOrEmpty(_textBinding) && !string.IsNullOrEmpty(Byomei) && string.IsNullOrEmpty(OldText))
            {
                ByomeiMstInfo = new Dictionary<string, string>() { { PtFamilyReki.ByomeiCd, PtFamilyReki.Byomei } };
                _textBinding = PtFamilyReki.ByomeiCd;
                OldText = PtFamilyReki.Byomei;
            }
        }

        public bool CheckDefaultValue()
        {
            if (Status == ModelStatus.None || Status == ModelStatus.Modified && !IsFreeText)
            {
                return string.IsNullOrEmpty(Byomei) && string.IsNullOrEmpty(Cmt);
            }
            else
            {
                return string.IsNullOrEmpty(TextBinding) && string.IsNullOrEmpty(CmtBinding);
            }
        }

        /// <summary>
        /// 家族歴
        /// </summary>
        /// <summary>
        /// 連番
        /// </summary>
        public long Id
        {
            get { return PtFamilyReki.Id; }
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtFamilyReki.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get { return PtFamilyReki.PtId; }
        }

        /// <summary>
        /// 家族ID
        ///     PT_FAMILY.家族ID
        /// </summary>
        public long FamilyId
        {
            get { return PtFamilyReki.FamilyId; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long SeqNo
        {
            get { return PtFamilyReki.SeqNo; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return PtFamilyReki.SortNo; }
        }

        /// <summary>
        /// 病名コード
        /// </summary>
        public string ByomeiCd
        {
            get { return PtFamilyReki.ByomeiCd; }
        }

        public Dictionary<string, string> ByomeiMstInfo { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 病態コード
        /// </summary>
        public string ByotaiCd
        {
            get { return PtFamilyReki.ByotaiCd; }
        }

        /// <summary>
        /// 病名
        /// </summary>
        public string Byomei
        {
            get { return PtFamilyReki.Byomei; }
        }

        /// <summary>
        /// コメント
        /// </summary>
        public string Cmt
        {
            get { return PtFamilyReki.Cmt; }
        }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtFamilyReki.IsDeleted; }
        }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime CreateDate
        {
            get { return PtFamilyReki.CreateDate; }
        }

        /// <summary>
        /// 作成者
        /// </summary>
        public int CreateId
        {
            get { return PtFamilyReki.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// </summary>
        public string CreateMachine
        {
            get { return PtFamilyReki.CreateMachine; }
        }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime UpdateDate
        {
            get { return PtFamilyReki.UpdateDate; }
        }

        /// <summary>
        /// 更新者
        /// </summary>
        public int UpdateId
        {
            get { return PtFamilyReki.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// </summary>
        public string UpdateMachine
        {
            get { return PtFamilyReki.UpdateMachine; }
        }

        private string _textBinding = string.Empty;
        public string TextBinding
        {
            get
            {
                if (string.IsNullOrEmpty(_textBinding) && !string.IsNullOrEmpty(Byomei) && string.IsNullOrEmpty(OldText))
                {
                    ByomeiMstInfo = new Dictionary<string, string>() { { ByomeiCd, Byomei } };
                    _textBinding = Byomei;
                    OldText = Byomei;
                }
                return _textBinding;
            }
            set
            {
                if (value.StartsWith("//") || value.StartsWith("／／") ||
                   value.StartsWith("･･") || value.StartsWith("・・"))
                {
                    TextComment = value;
                    ByomeiMstInfo = new Dictionary<string, string>() { { "0000999", TextComment } };
                    RaisePropertyChanged(() => IsFreeText);
                    Set(ref _textBinding, value.Substring(2));
                }
                else if (Set(ref _textBinding, value))
                {
                    TextComment = value;
                }
            }
        }

        private string _cmtBinding = string.Empty;
        public string CmtBinding
        {
            get
            {
                if (string.IsNullOrEmpty(_cmtBinding) && !string.IsNullOrEmpty(Cmt) && string.IsNullOrEmpty(OldCmt))
                {
                    _cmtBinding = Cmt;
                    OldCmt = Cmt;
                }
                return _cmtBinding;
            }
            set
            {
                Set(ref _cmtBinding, value);
            }
        }

        public string OldCmt { get; private set; } = string.Empty;
        public string OldText { get; private set; } = string.Empty;

        public bool IsFreeText
        {
            get
            {
                if (ByomeiMstInfo != null)
                {
                    return FREE_WORD == ByomeiMstInfo.Keys.FirstOrDefault();
                }
                return false;
            }
        }

        public string TextComment { get; private set; } = string.Empty;

        public bool CheckComment()
        {
            if (string.IsNullOrEmpty(TextComment))
            {
                return false;
            }
            return TextComment.StartsWith("//") || TextComment.StartsWith("／／") ||
                   TextComment.StartsWith("･･") || TextComment.StartsWith("・・");
        }

        private ModelStatus _status;
        public ModelStatus Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
    }
}
