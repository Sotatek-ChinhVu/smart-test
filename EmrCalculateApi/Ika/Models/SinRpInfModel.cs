using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class SinRpInfModel
    {
        public SinRpInf SinRpInf { get; } = null;

        private int _isDeleted = 0;
        private long _keyNo = 0;

        public SinRpInfModel(SinRpInf sinRpInf)
        {
            SinRpInf = sinRpInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SinRpInf.HpId; }
            set
            {
                if (SinRpInf.HpId == value) return;
                SinRpInf.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SinRpInf.PtId; }
            set
            {
                if (SinRpInf.PtId == value) return;
                SinRpInf.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SinRpInf.SinYm; }
            set
            {
                if (SinRpInf.SinYm == value) return;
                SinRpInf.SinYm = value;
                //RaisePropertyChanged(() => SinYm);
            }
        }

        /// <summary>
        /// 剤番号
        /// SEQUENCE
        /// </summary>
        public int RpNo
        {
            get { return SinRpInf.RpNo; }
            set
            {
                if (SinRpInf.RpNo == value) return;
                SinRpInf.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 初回算定日
        /// </summary>
        public int FirstDay
        {
            get { return SinRpInf.FirstDay; }
            set
            {
                if (SinRpInf.FirstDay == value) return;
                SinRpInf.FirstDay = value;
                //RaisePropertyChanged(() => FirstDay);
            }
        }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int HokenKbn
        {
            get { return SinRpInf.HokenKbn; }
            set
            {
                if (SinRpInf.HokenKbn == value) return;
                SinRpInf.HokenKbn = value;
                //RaisePropertyChanged(() => HokenKbn);
            }
        }

        /// <summary>
        /// 診療行為区分
        /// 
        /// </summary>
        public int SinKouiKbn
        {
            get { return SinRpInf.SinKouiKbn; }
            set
            {
                if (SinRpInf.SinKouiKbn == value) return;
                SinRpInf.SinKouiKbn = value;
                //RaisePropertyChanged(() => SinKouiKbn);
            }
        }

        /// <summary>
        /// 診療識別
        /// レセプト電算に記録する診療識別
        /// </summary>
        public int SinId
        {
            get { return SinRpInf.SinId; }
            set
            {
                if (SinRpInf.SinId == value) return;
                SinRpInf.SinId = value;
                //RaisePropertyChanged(() => SinId);
            }
        }

        /// <summary>
        /// 代表コード表用番号
        /// 
        /// </summary>
        public string CdNo
        {
            get { return SinRpInf.CdNo ?? string.Empty; }
            set
            {
                if (SinRpInf.CdNo == value) return;
                SinRpInf.CdNo = value;
                //RaisePropertyChanged(() => CdNo);
            }
        }

        /// <summary>
        /// 算定区分
        /// 1:自費算定
        /// </summary>
        public int SanteiKbn
        {
            get { return SinRpInf.SanteiKbn; }
            set
            {
                if (SinRpInf.SanteiKbn == value) return;
                SinRpInf.SanteiKbn = value;
                //RaisePropertyChanged(() => SanteiKbn);
            }
        }

        /// <summary>
        /// 診療行為インデックス
        /// RP_NOに属する診療行為のKOUI_INDEXを結合したもの
        /// </summary>
        public string KouiData
        {
            get { return SinRpInf.KouiData ?? string.Empty; }
            set
            {
                if (SinRpInf.KouiData == value) return;
                SinRpInf.KouiData = value;
                //RaisePropertyChanged(() => KouiData);
            }
        }

        /// <summary>
        /// 削除区分
        /// </summary>
        public int IsDeleted
        {
            get { return SinRpInf.IsDeleted; }
            set
            {
                if (SinRpInf.IsDeleted == value) return;
                SinRpInf.IsDeleted = value;
                //RaisePropertyChanged(() => IsDeleted);
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return SinRpInf.CreateDate; }
            set
            {
                if (SinRpInf.CreateDate == value) return;
                SinRpInf.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return SinRpInf.CreateId; }
            set
            {
                if (SinRpInf.CreateId == value) return;
                SinRpInf.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return SinRpInf.CreateMachine ?? string.Empty; }
            set
            {
                if (SinRpInf.CreateMachine == value) return;
                SinRpInf.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return SinRpInf.UpdateDate; }
            set
            {
                if (SinRpInf.UpdateDate == value) return;
                SinRpInf.UpdateDate = value;
                //RaisePropertyChanged(() => UpdateDate);
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return SinRpInf.UpdateId; }
            set
            {
                if (SinRpInf.UpdateId == value) return;
                SinRpInf.UpdateId = value;
                //RaisePropertyChanged(() => UpdateId);
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return SinRpInf.UpdateMachine ?? string.Empty; }
            set
            {
                if (SinRpInf.UpdateMachine == value) return;
                SinRpInf.UpdateMachine = value;
                //RaisePropertyChanged(() => UpdateMachine);
            }
        }

        /// <summary>
        /// 更新情報
        ///     0: 変更なし
        ///     1: 追加
        ///     2: 削除
        /// </summary>
        public int UpdateState
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        /// <summary>
        /// キー番号
        /// </summary>
        public long KeyNo
        {
            get { return _keyNo; }
            set { _keyNo = value; }
        }

    }

}
