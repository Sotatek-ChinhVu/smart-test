using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using Domain.Models.PtFamily;
using Helper.Common;
using Helper.Extendsions;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtFamilyItem : ObservableObject
    {
        /// <summary>
        /// Return Entity PtFamily
        /// </summary>
        public PtFamilyModel PtFamily { get; }

        public int SinDay { get; set; }

        /// <summary>
        /// Return Entity PtInf
        /// </summary>
        public PatientInforModel PtInf { get; set; }
        
        public PtFamilyItem(PtFamilyModel ptFamily, PatientInforModel ptInf)
        {
            PtFamily = ptFamily;
            PtInf = ptInf;
            Initialize();
        }

        private void Initialize()
        {
            FamilyPtNumBuf = FamilyPtNum;
            SinDay = DateTime.Now.ToString("yyyyMMdd").AsInteger();
        }

        /// <summary>
        /// 家族ID
        ///     患者の家族を識別するための番号
        /// </summary>
        public long FamilyId
        {
            get { return PtFamily.FamilyId; }
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtFamily.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get
            {
                if (PtInf != null && PtInf.PtId > 0)
                    return PtInf.PtId;
                else
                    return PtFamily.PtId;
            }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long SeqNo
        {
            get { return PtFamily.SeqNo; }
        }

        /// <summary>
        /// 続柄コード
        /// </summary>
        public string ZokugaraCd
        {
            get { return PtFamily.ZokugaraCd; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return PtFamily.SortNo; }
        }

        /// <summary>
        /// 親ID
        ///     孫の親の家族ID
        /// </summary>
        public int ParentId
        {
            get { return PtFamily.ParentId; }
        }

        /// <summary>
        /// 家族患者ID
        /// </summary>
        public long FamilyPtId
        {
            get { return PtFamily.FamilyPtId; }
        }

        public long FamilyPtNum
        {
            get
            {
                if (PtInf != null && FamilyPtId > 0)
                    return PtInf.PtNum;
                else
                    return 0;
            }
        }

        public long FamilyPtNumBuf { get; set; }

        public string FamilyPtNumBinding
        {
            get
            {
                string ptValue = FormatType.FormatViewLongToString(FamilyPtNum);
                if (string.IsNullOrEmpty(ptValue))
                {
                    return FamilyPtNumBuf == 0 ? string.Empty : FamilyPtNumBuf.AsString();
                }
                return ptValue;
            }
            set => FamilyPtNumBuf = value.AsLong();
        }

        /// <summary>
        /// カナ氏名
        /// </summary>
        public string KanaName
        {
            get
            {
                if (PtInf != null && PtInf.PtId > 0)
                    return PtInf.KanaName;
                else
                    return PtFamily.KanaName;
            }
        }

        /// <summary>
        /// 氏名
        /// </summary>
        public string Name
        {
            get
            {
                if (PtInf != null && PtInf.PtId > 0)
                    return PtInf.Name;
                else
                    return PtFamily.Name;
            }
        }

        /// <summary>
        /// 性別
        ///     1:男
        ///     2:女
        /// </summary>
        public int Sex
        {
            get
            {
                if (PtInf != null && PtInf.PtId > 0)
                    return PtInf.Sex;
                else
                    return PtFamily.Sex;
            }
        }

        /// <summary>
        /// 生年月日
        ///     yyyymmdd
        /// </summary>
        public int Birthday
        {
            get
            {
                if (PtInf != null && PtInf.PtId > 0)
                    return PtInf.Birthday;
                else
                    return PtFamily.Birthday;
            }
        }

        public string BirthdayBinding
        {
            get => CIUtil.SDateToShowWDate(Birthday);
        }

        public int Age => CIUtil.SDateToAge(Birthday, SinDay);
        public string AgeBinding
        {
            get
            {
                if (Age > 0 && Birthday > 0)
                    return Age.AsString();
                else if (Age == 0 && CheckDefaultValue() == false)
                    return "0";
                else
                    return "";
            }
        }

        /// <summary>
        /// 死亡区分
        ///     0:生存
        ///     1:死亡
        ///     2:消息不明
        /// </summary>
        public int IsDead
        {
            get
            {
                if (PtInf != null && PtInf.PtId > 0)
                {
                    if (string.IsNullOrEmpty(_oldDeadInfo))
                    {
                        _oldDeadInfo = PtInf.IsDead.AsString();
                    }
                    return PtFamily.IsDead;
                }
                else
                {
                    if (string.IsNullOrEmpty(_oldDeadInfo))
                    {
                        _oldDeadInfo = PtFamily.IsDead.AsString();
                    }
                    return PtFamily.IsDead;
                }
            }
        }

        private string _oldDeadInfo = string.Empty;

        public bool CheckIsModifiedDeadInfo()
        {
            if (PtInf != null && PtInf.PtId > 0)
            {
                return IsDead.AsString() != _oldDeadInfo;
            }
            return false;
        }

        /// <summary>
        /// 別居フラグ
        ///     1:別居
        /// </summary>
        public int IsSeparated
        {
            get { return PtFamily.IsSeparated; }
        }

        public string IsSeparatedBinding
        {
            get
            {
                if (IsSeparated == 1)
                    return "✓";
                else
                    return "";
            }
        }

        /// <summary>
        /// 備考
        /// </summary>
        public string Biko
        {
            get { return PtFamily.Biko; }
        }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtFamily.IsDeleted; }
        }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime CreateDate
        {
            get { return PtFamily.CreateDate; }
        }

        /// <summary>
        /// 作成者
        /// </summary>
        public int CreateId
        {
            get { return PtFamily.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// </summary>
        public string CreateMachine
        {
            get { return PtFamily.CreateMachine; }
        }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime UpdateDate
        {
            get { return PtFamily.UpdateDate; }
        }

        /// <summary>
        /// 更新者
        /// </summary>
        public int UpdateId
        {
            get { return PtFamily.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// </summary>
        public string UpdateMachine
        {
            get { return PtFamily.UpdateMachine; }
        }

        private bool _modelModified;

        /// <summary>
        /// Check Model Modified
        /// </summary>
        public bool ModelModified
        {
            get => _modelModified;
            set { Set(ref _modelModified, value); }
        }

        public bool CheckDefaultValue()
        {
            return (string.IsNullOrEmpty(ZokugaraCd) &&
                    FamilyPtId <= 0 &&
                    string.IsNullOrEmpty(KanaName) &&
                    string.IsNullOrEmpty(Name) &&
                    Sex <= 0 &&
                    Birthday <= 0 &&
                    IsDead != 1 &&
                    IsSeparated != 1 &&
                    string.IsNullOrEmpty(Biko) &&
                    string.IsNullOrEmpty(DiseaseName));
        }

        private string _diseaseName = string.Empty;
        public string DiseaseName
        {
            get => _diseaseName;
            set => Set(ref _diseaseName, value);
        }

        public List<PtFamilyRekiItem>? ListPtFamilyRekiModel { get; set; }
    }
}
