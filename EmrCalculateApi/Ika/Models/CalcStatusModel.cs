using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class CalcStatusModel 
    {
        public CalcStatus CalcStatus { get; } = null;

        private int _updateStatus;

        public CalcStatusModel(CalcStatus calcStatus)
        {
            CalcStatus = calcStatus;
            _updateStatus = 0;
        }

        /// <summary>
        /// 計算ID
        /// SEQUENCE
        /// </summary>
        public long CalcId
        {
            get { return CalcStatus.CalcId; }
            set
            {
                if (CalcStatus.CalcId == value) return;
                CalcStatus.CalcId = value;
                //RaisePropertyChanged(() => CalcId);
            }
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return CalcStatus.HpId; }
            set
            {
                if (CalcStatus.HpId == value) return;
                CalcStatus.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return CalcStatus.PtId; }
            set
            {
                if (CalcStatus.PtId == value) return;
                CalcStatus.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return CalcStatus.SinDate; }
            set
            {
                if (CalcStatus.SinDate == value) return;
                CalcStatus.SinDate = value;
                //RaisePropertyChanged(() => SinDate);
            }
        }

        /// <summary>
        /// 請求情報更新
        /// 0:反映しない　1:反映する　2:試算
        /// </summary>
        public int SeikyuUp
        {
            get { return CalcStatus.SeikyuUp; }
            set
            {
                if (CalcStatus.SeikyuUp == value) return;
                CalcStatus.SeikyuUp = value;
                //RaisePropertyChanged(() => SeikyuUp);
            }
        }
        /// <summary>
        /// 計算モード
        /// 0:通常計算　1:連続計算　2:試算
        /// </summary>
        public int CalcMode
        {
            get { return CalcStatus.CalcMode; }
            set
            {
                if (CalcStatus.CalcMode == value) return;
                CalcStatus.CalcMode = value;
                //RaisePropertyChanged(() => CalcMode);
            }
        }
        /// <summary>
        /// RECE_STATUS.STATUS_KBN=8のとき、0に戻すかどうか
        /// 0:クリアしない　1:クリアする
        /// </summary>
        public int ClearReceChk
        {
            get { return CalcStatus.ClearReceChk; }
            set
            {
                if (CalcStatus.ClearReceChk == value) return;
                CalcStatus.ClearReceChk = value;
                //RaisePropertyChanged(() => ClearReceChk);
            }
        }

        /// <summary>
        /// 状態
        /// 0:未済 1:計算中 8:異常終了 9:終了
        /// </summary>
        public int Status
        {
            get { return CalcStatus.Status; }
            set
            {
                if (CalcStatus.Status == value) return;
                CalcStatus.Status = value;
                //RaisePropertyChanged(() => Status);
            }
        }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        public string Biko
        {
            get { return CalcStatus.Biko ?? string.Empty; }
            set
            {
                if (CalcStatus.Biko == value) return;
                CalcStatus.Biko = value;
                //RaisePropertyChanged(() => Biko);
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return CalcStatus.CreateDate; }
            set
            {
                if (CalcStatus.CreateDate == value) return;
                CalcStatus.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return CalcStatus.CreateId; }
            set
            {
                if (CalcStatus.CreateId == value) return;
                CalcStatus.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return CalcStatus.CreateMachine ?? string.Empty; }
            set
            {
                if (CalcStatus.CreateMachine == value) return;
                CalcStatus.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return CalcStatus.UpdateDate; }
            set
            {
                if (CalcStatus.UpdateDate == value) return;
                CalcStatus.UpdateDate = value;
                //RaisePropertyChanged(() => UpdateDate);
            }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return CalcStatus.UpdateId; }
            set
            {
                if (CalcStatus.UpdateId == value) return;
                CalcStatus.UpdateId = value;
                //RaisePropertyChanged(() => UpdateId);
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return CalcStatus.UpdateMachine ?? string.Empty; }
            set
            {
                if (CalcStatus.UpdateMachine == value) return;
                CalcStatus.UpdateMachine = value;
                //RaisePropertyChanged(() => UpdateMachine);
            }
        }

        public int UpdateStatus
        {
            get { return _updateStatus; }
            set
            {
                if (_updateStatus == value) return;
                _updateStatus = value;
            }
        }
    }

}
