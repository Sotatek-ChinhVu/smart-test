using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Common;

namespace EmrCalculateApi.Ika.Models
{
    public class PtInfModel
    {
        public PtInf PtInf { get; } = null;
        private int _ageKbn;
        private int _ageYear, _ageMonth, _ageDay;
        private bool _isStudent;
        private bool _isElder;

        public PtInfModel(PtInf ptInf, int sinDate)
        {
            PtInf = ptInf;

            _ageKbn = 9;

            _ageYear = 0;
            _ageMonth = 0;
            _ageDay = 0;

            CIUtil.SDateToDecodeAge(PtInf.Birthday, sinDate, ref _ageYear, ref _ageMonth, ref _ageDay);

            if ((_ageYear == 0) && (_ageMonth == 0) && (_ageDay < 28))
            {
                _ageKbn = 0;
            }
            else if ((_ageYear == 0) && (_ageMonth < 12))
            {
                // 乳児
                _ageKbn = 1;
            }
            else if (_ageYear < 3)
            {
                // 幼児
                _ageKbn = 2;
            }
            else if (_ageYear < 6)
            {
                // 幼児
                _ageKbn = 3;
            }

            _isStudent = CIUtil.IsStudent(PtInf.Birthday, sinDate);

            _isElder = CIUtil.AgeChk(PtInf.Birthday, sinDate, 70);
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号       
        /// </summary>
        public long PtId
        {
            get { return PtInf.PtId; }
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get { return PtInf.PtNum; }
        }
        ///// <summary>
        ///// 連番
        ///// </summary>
        //public long SeqNo
        //{
        //    get { return PtInf.SeqNo; }
        //    set
        //    {
        //        if (PtInf.SeqNo == value) return;
        //        PtInf.SeqNo = value;
        //        //RaisePropertyChanged(() => SeqNo);
        //    }
        //}

        ///// <summary>
        ///// 患者番号
        /////  医療機関が患者特定するための番号
        ///// </summary>
        //public long PtNum
        //{
        //    get { return PtInf.PtNum; }
        //    set
        //    {
        //        if (PtInf.PtNum == value) return;
        //        PtInf.PtNum = value;
        //        //RaisePropertyChanged(() => PtNum);
        //    }
        //}

        /// <summary>
        /// カナ氏名
        /// </summary>
        public string KanaName
        {
            get { return PtInf.KanaName ?? string.Empty; }
        }

        /// <summary>
        /// 氏名
        /// </summary>
        public string Name
        {
            get { return PtInf.Name ?? string.Empty; }
        }

        /// <summary>
        /// 性別
        ///  1:男 
        ///  2:女
        /// </summary>
        public int Sex
        {
            get { return PtInf.Sex; }
        }

        /// <summary>
        /// 生年月日
        ///  yyyymmdd 
        /// </summary>
        public int Birthday
        {
            get { return PtInf.Birthday; }
        }

        /// <summary>
        /// 死亡区分
        ///  0:生存 
        ///  1:死亡 
        ///  2:消息不明
        /// </summary>
        public int IsDead
        {
            get { return PtInf.IsDead; }
        }

        /// <summary>
        /// 死亡日
        ///  yyyymmdd  
        /// </summary>
        public int DeathDate
        {
            get { return PtInf.DeathDate; }
        }

        ///// <summary>
        ///// 自宅郵便番号
        /////  区切り文字("-") を除く   
        ///// </summary>
        //public string HomePost
        //{
        //    get { return PtInf.HomePost; }
        //    set
        //    {
        //        if (PtInf.HomePost == value) return;
        //        PtInf.HomePost = value;
        //        //RaisePropertyChanged(() => HomePost);
        //    }
        //}

        ///// <summary>
        ///// 自宅住所１
        ///// </summary>
        //public string HomeAddress1
        //{
        //    get { return PtInf.HomeAddress1; }
        //    set
        //    {
        //        if (PtInf.HomeAddress1 == value) return;
        //        PtInf.HomeAddress1 = value;
        //        //RaisePropertyChanged(() => HomeAddress1);
        //    }
        //}

        ///// <summary>
        ///// 自宅住所２
        ///// </summary>
        //public string HomeAddress2
        //{
        //    get { return PtInf.HomeAddress2; }
        //    set
        //    {
        //        if (PtInf.HomeAddress2 == value) return;
        //        PtInf.HomeAddress2 = value;
        //        //RaisePropertyChanged(() => HomeAddress2);
        //    }
        //}

        ///// <summary>
        ///// 電話番号１
        ///// </summary>
        //public string Tel1
        //{
        //    get { return PtInf.Tel1; }
        //    set
        //    {
        //        if (PtInf.Tel1 == value) return;
        //        PtInf.Tel1 = value;
        //        //RaisePropertyChanged(() => Tel1);
        //    }
        //}

        ///// <summary>
        ///// 電話番号２
        ///// </summary>
        //public string Tel2
        //{
        //    get { return PtInf.Tel2; }
        //    set
        //    {
        //        if (PtInf.Tel2 == value) return;
        //        PtInf.Tel2 = value;
        //        //RaisePropertyChanged(() => Tel2);
        //    }
        //}

        ///// <summary>
        ///// E-Mailアドレス
        ///// </summary>
        //public string Mail
        //{
        //    get { return PtInf.Mail; }
        //    set
        //    {
        //        if (PtInf.Mail == value) return;
        //        PtInf.Mail = value;
        //        //RaisePropertyChanged(() => Mail);
        //    }
        //}

        ///// <summary>
        ///// 世帯主名
        ///// </summary>
        //public string Setanusi
        //{
        //    get { return PtInf.Setanusi; }
        //    set
        //    {
        //        if (PtInf.Setanusi == value) return;
        //        PtInf.Setanusi = value;
        //        //RaisePropertyChanged(() => Setanusi);
        //    }
        //}

        ///// <summary>
        ///// 続柄
        ///// </summary>
        //public string Zokugara
        //{
        //    get { return PtInf.Zokugara; }
        //    set
        //    {
        //        if (PtInf.Zokugara == value) return;
        //        PtInf.Zokugara = value;
        //        //RaisePropertyChanged(() => Zokugara);
        //    }
        //}

        ///// <summary>
        ///// 職業
        ///// </summary>
        //public string Job
        //{
        //    get { return PtInf.Job; }
        //    set
        //    {
        //        if (PtInf.Job == value) return;
        //        PtInf.Job = value;
        //        //RaisePropertyChanged(() => Job);
        //    }
        //}

        ///// <summary>
        ///// 連絡先名称
        ///// </summary>
        //public string RenrakuName
        //{
        //    get { return PtInf.RenrakuName; }
        //    set
        //    {
        //        if (PtInf.RenrakuName == value) return;
        //        PtInf.RenrakuName = value;
        //        //RaisePropertyChanged(() => RenrakuName);
        //    }
        //}

        ///// <summary>
        ///// 連絡先郵便番号
        ///// </summary>
        //public string RenrakuPost
        //{
        //    get { return PtInf.RenrakuPost; }
        //    set
        //    {
        //        if (PtInf.RenrakuPost == value) return;
        //        PtInf.RenrakuPost = value;
        //        //RaisePropertyChanged(() => RenrakuPost);
        //    }
        //}

        ///// <summary>
        ///// 連絡先住所１
        ///// </summary>
        //public string RenrakuAddress1
        //{
        //    get { return PtInf.RenrakuAddress1; }
        //    set
        //    {
        //        if (PtInf.RenrakuAddress1 == value) return;
        //        PtInf.RenrakuAddress1 = value;
        //        //RaisePropertyChanged(() => RenrakuAddress1);
        //    }
        //}

        ///// <summary>
        ///// 連絡先住所２
        ///// </summary>
        //public string RenrakuAddress2
        //{
        //    get { return PtInf.RenrakuAddress2; }
        //    set
        //    {
        //        if (PtInf.RenrakuAddress2 == value) return;
        //        PtInf.RenrakuAddress2 = value;
        //        //RaisePropertyChanged(() => RenrakuAddress2);
        //    }
        //}

        ///// <summary>
        ///// 連絡先電話番号
        ///// </summary>
        //public string RenrakuTel
        //{
        //    get { return PtInf.RenrakuTel; }
        //    set
        //    {
        //        if (PtInf.RenrakuTel == value) return;
        //        PtInf.RenrakuTel = value;
        //        //RaisePropertyChanged(() => RenrakuTel);
        //    }
        //}

        ///// <summary>
        ///// 連絡先電話番号
        ///// </summary>
        //public string RenrakuMemo
        //{
        //    get { return PtInf.RenrakuMemo; }
        //    set
        //    {
        //        if (PtInf.RenrakuMemo == value) return;
        //        PtInf.RenrakuMemo = value;
        //        //RaisePropertyChanged(() => RenrakuMemo);
        //    }
        //}

        ///// <summary>
        ///// 勤務先名称
        ///// </summary>
        //public string OfficeName
        //{
        //    get { return PtInf.OfficeName; }
        //    set
        //    {
        //        if (PtInf.OfficeName == value) return;
        //        PtInf.OfficeName = value;
        //        //RaisePropertyChanged(() => OfficeName);
        //    }
        //}

        ///// <summary>
        ///// 勤務先郵便番号
        ///// </summary>
        //public string OfficePost
        //{
        //    get { return PtInf.OfficePost; }
        //    set
        //    {
        //        if (PtInf.OfficePost == value) return;
        //        PtInf.OfficePost = value;
        //        //RaisePropertyChanged(() => OfficePost);
        //    }
        //}

        ///// <summary>
        ///// 勤務先住所１
        ///// </summary>
        //public string OfficeAddress1
        //{
        //    get { return PtInf.OfficeAddress1; }
        //    set
        //    {
        //        if (PtInf.OfficeAddress1 == value) return;
        //        PtInf.OfficeAddress1 = value;
        //        //RaisePropertyChanged(() => OfficeAddress1);
        //    }
        //}

        ///// <summary>
        ///// 勤務先住所２
        ///// </summary>
        //public string OfficeAddress2
        //{
        //    get { return PtInf.OfficeAddress2; }
        //    set
        //    {
        //        if (PtInf.OfficeAddress2 == value) return;
        //        PtInf.OfficeAddress2 = value;
        //        //RaisePropertyChanged(() => OfficeAddress2);
        //    }
        //}

        ///// <summary>
        ///// 勤務先電話番号
        ///// </summary>
        //public string OfficeTel
        //{
        //    get { return PtInf.OfficeTel; }
        //    set
        //    {
        //        if (PtInf.OfficeTel == value) return;
        //        PtInf.OfficeTel = value;
        //        //RaisePropertyChanged(() => OfficeTel);
        //    }
        //}

        ///// <summary>
        ///// 勤務先備考
        ///// </summary>
        //public string OfficeMemo
        //{
        //    get { return PtInf.OfficeMemo; }
        //    set
        //    {
        //        if (PtInf.OfficeMemo == value) return;
        //        PtInf.OfficeMemo = value;
        //        //RaisePropertyChanged(() => OfficeMemo);
        //    }
        //}

        ///// <summary>
        ///// 領収証明細発行区分
        /////  0:不要 
        /////  1:要
        ///// </summary>
        //public int IsRyosyoDetail
        //{
        //    get { return PtInf.IsRyosyoDetail; }
        //    set
        //    {
        //        if (PtInf.IsRyosyoDetail == value) return;
        //        PtInf.IsRyosyoDetail = value;
        //        //RaisePropertyChanged(() => IsRyosyoDetail);
        //    }
        //}

        /// <summary>
        /// 主治医コード
        /// </summary>
        public int PrimaryDoctor
        {
            get { return PtInf.PrimaryDoctor; }
        }

        /// <summary>
        /// テスト患者区分
        ///  1:テスト患者
        /// </summary>
        public int IsTester
        {
            get { return PtInf.IsTester; }
        }

        ///// <summary>
        ///// 削除区分
        /////  1:削除
        ///// </summary>
        //public int IsDelete
        //{
        //    get { return PtInf.IsDelete; }
        //    set
        //    {
        //        if (PtInf.IsDelete == value) return;
        //        PtInf.IsDelete = value;
        //        //RaisePropertyChanged(() => IsDelete);
        //    }
        //}

        ///// <summary>
        ///// MAIN_HOKEN_PID
        ///// </summary>
        //public int MainHokenPid
        //{
        //    get { return PtInf.MainHokenPid; }
        //    set
        //    {
        //        if (PtInf.MainHokenPid == value) return;
        //        PtInf.MainHokenPid = value;
        //        //RaisePropertyChanged(() => MainHokenPid);
        //    }
        //}

        ///// <summary>
        ///// 作成日時 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return PtInf.CreateDate; }
        //    set
        //    {
        //        if (PtInf.CreateDate == value) return;
        //        PtInf.CreateDate = value;
        //        //RaisePropertyChanged(() => CreateDate);
        //    }
        //}

        ///// <summary>
        ///// 作成者  
        ///// </summary>
        //public int CreateId
        //{
        //    get { return PtInf.CreateId; }
        //    set
        //    {
        //        if (PtInf.CreateId == value) return;
        //        PtInf.CreateId = value;
        //        //RaisePropertyChanged(() => CreateId);
        //    }
        //}

        ///// <summary>
        ///// 作成端末   
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return PtInf.CreateMachine; }
        //    set
        //    {
        //        if (PtInf.CreateMachine == value) return;
        //        PtInf.CreateMachine = value;
        //        //RaisePropertyChanged(() => CreateMachine);
        //    }
        //}

        ///// <summary>
        ///// 更新日時   
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return PtInf.UpdateDate; }
        //    set
        //    {
        //        if (PtInf.UpdateDate == value) return;
        //        PtInf.UpdateDate = value;
        //        //RaisePropertyChanged(() => UpdateDate);
        //    }
        //}

        ///// <summary>
        ///// 更新者   
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return PtInf.UpdateId; }
        //    set
        //    {
        //        if (PtInf.UpdateId == value) return;
        //        PtInf.UpdateId = value;
        //        //RaisePropertyChanged(() => UpdateId);
        //    }
        //}

        ///// <summary>
        ///// 更新端末   
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return PtInf.UpdateMachine; }
        //    set
        //    {
        //        if (PtInf.UpdateMachine == value) return;
        //        PtInf.UpdateMachine = value;
        //        //RaisePropertyChanged(() => UpdateMachine);
        //    }
        //}

        /// <summary>
        /// 年齢区分
        /// 0: 新生児（生後28日未満）
        /// 1: 乳児（1歳未満）
        /// 2: 乳幼児（3歳未満）
        /// 3: 幼児（6歳未満）
        /// 9: 6歳以上
        /// </summary>
        public int AgeKbn()
        {
            return _ageKbn;
        }

        /// <summary>
        /// 年齢（年）
        /// </summary>
        public int Age
        {
            get { return _ageYear; }
        }

        /// <summary>
        /// 年齢（月）
        /// </summary>
        public int AgeMonth
        {
            get { return _ageMonth; }
        }

        /// <summary>
        /// 年齢（日）
        /// </summary>
        public int AgeDay
        {
            get { return _ageDay; }
        }

        /// <summary>
        /// 幼児（6歳未満）かどうか判定
        /// </summary>
        /// <returns>true: 幼児（6歳未満）</returns>
        public bool IsYoJi
        {
            get { return AgeKbn() < 9; }
        }

        /// <summary>
        /// 乳幼児（3歳未満）かどうか判定
        /// </summary>
        /// <returns>true: 乳幼児（3歳未満）</returns>
        public bool IsNyuyoJi
        {
            get { return AgeKbn() < 3; }
        }

        /// <summary>
        /// 乳児（1歳未満）かどうか判定
        /// </summary>
        /// <returns>true: 乳児（1歳未満）</returns>
        public bool IsNyuJi
        {
            get { return AgeKbn() < 2; }
        }

        /// <summary>
        /// 新生児（28日未満）かどうか判定
        /// </summary>
        /// <returns>true: 新生児（28日未満）</returns>
        public bool IsSinseiJi
        {
            get { return AgeKbn() < 1; }
        }

        /// <summary>
        /// 就学しているかどうか
        /// false: 未就学
        /// </summary>
        public bool IsStudent
        {
            get { return _isStudent; }
        }

        public bool IsElder
        {
            get { return _isElder; }
        }
    }

}
