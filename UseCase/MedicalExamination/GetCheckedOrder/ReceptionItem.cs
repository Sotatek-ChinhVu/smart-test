using Domain.Models.Reception;
using Entity.Tenant;

namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class ReceptionItem
    {
        public RaiinInf RaiinInf { get; } = new();

        public ReceptionItem(ReceptionModel receptionModel)
        {
            RaiinInf.HpId = receptionModel.HpId;
            RaiinInf.PtId = receptionModel.PtId;
            RaiinInf.SinDate = receptionModel.SinDate;
            RaiinInf.RaiinNo = receptionModel.RaiinNo;
            RaiinInf.HokenPid = receptionModel.HokenPid;
            RaiinInf.OyaRaiinNo = receptionModel.OyaRaiinNo;
            RaiinInf.Status = receptionModel.Status;
            RaiinInf.IsYoyaku = receptionModel.IsYoyaku;
            RaiinInf.YoyakuTime = receptionModel.YoyakuTime;
            RaiinInf.YoyakuId = receptionModel.YoyakuId;
            RaiinInf.UketukeSbt = receptionModel.UketukeSbt;
            RaiinInf.UketukeTime = receptionModel.UketukeTime;
            RaiinInf.UketukeId = receptionModel.UketukeId;
            RaiinInf.UketukeNo = receptionModel.UketukeNo;
            RaiinInf.SinStartTime = receptionModel.SinStartTime;
            RaiinInf.SinEndTime = receptionModel.SinEndTime;
            RaiinInf.KaikeiTime = receptionModel.KaikeiTime;
            RaiinInf.KaikeiId = receptionModel.KaikeiId;
            RaiinInf.KaId = receptionModel.KaId;
            RaiinInf.TantoId = receptionModel.TantoId;
            RaiinInf.SanteiKbn = receptionModel.SanteiKbn;
            RaiinInf.SyosaisinKbn = receptionModel.SyosaisinKbn;
            RaiinInf.JikanKbn = receptionModel.JikanKbn;
            RaiinInf.ConfirmationResult = string.Empty;
            RaiinInf.ConfirmationState = 0;
            RaiinInf.IsDeleted = 0;
            RaiinInf.CreateDate = DateTime.MinValue;
            RaiinInf.UpdateDate = DateTime.MinValue;
            RaiinInf.CreateMachine = string.Empty;
            RaiinInf.UpdateMachine = string.Empty;
            RaiinInf.CreateId = 1;
            RaiinInf.UpdateId = 1;
        }

        public int HpId
        {
            get => RaiinInf.HpId;
            set
            {
                if (RaiinInf.HpId == value) return;
                RaiinInf.HpId = value;
            }
        }

        public long PtId
        {
            get => RaiinInf.PtId;
            set
            {
                if (RaiinInf.PtId == value) return;
                RaiinInf.PtId = value;
            }
        }

        public int SinDate
        {
            get => RaiinInf.SinDate;
            set
            {
                if (RaiinInf.SinDate == value) return;
                RaiinInf.SinDate = value;
            }
        }

        public long RaiinNo
        {
            get => RaiinInf.RaiinNo;
            set
            {
                if (RaiinInf.RaiinNo == value) return;
                RaiinInf.RaiinNo = value;
            }
        }

        public int HokenPid
        {
            get => RaiinInf.HokenPid;
            set
            {
                if (RaiinInf.HokenPid == value) return;
                RaiinInf.HokenPid = value;
            }
        }

        public long OyaRaiinNo
        {
            get => RaiinInf.OyaRaiinNo;
            set
            {
                if (RaiinInf.OyaRaiinNo == value) return;
                RaiinInf.OyaRaiinNo = value;
            }
        }

        public int Status
        {
            get => RaiinInf.Status;
            set
            {
                if (RaiinInf.Status == value) return;
                RaiinInf.Status = value;
            }
        }

        public int IsYoyaku
        {
            get => RaiinInf.IsYoyaku;
            set
            {
                if (RaiinInf.IsYoyaku == value) return;
                RaiinInf.IsYoyaku = value;
            }
        }

        public string YoyakuTime
        {
            get => RaiinInf.YoyakuTime ?? string.Empty;
            set
            {
                if (RaiinInf.YoyakuTime == value) return;
                RaiinInf.YoyakuTime = value;
            }
        }

        public int YoyakuId
        {
            get => RaiinInf.YoyakuId;
            set
            {
                if (RaiinInf.YoyakuId == value) return;
                RaiinInf.YoyakuId = value;
            }
        }

        public int UketukeSbt
        {
            get => RaiinInf.UketukeSbt;
            set
            {
                if (RaiinInf.UketukeSbt == value) return;
                RaiinInf.UketukeSbt = value;
            }
        }

        public string UketukeTime
        {
            get => RaiinInf.UketukeTime ?? string.Empty;
            set
            {
                if (RaiinInf.UketukeTime == value) return;
                RaiinInf.UketukeTime = value;
            }
        }

        public int UketukeId
        {
            get => RaiinInf.UketukeId;
            set
            {
                if (RaiinInf.UketukeId == value) return;
                RaiinInf.UketukeId = value;
            }
        }

        public int UketukeNo
        {
            get => RaiinInf.UketukeNo;
            set
            {
                if (RaiinInf.UketukeNo == value) return;
                RaiinInf.UketukeNo = value;
            }
        }

        public string SinStartTime
        {
            get => RaiinInf.SinStartTime ?? string.Empty;
            set
            {
                if (RaiinInf.SinStartTime == value) return;
                RaiinInf.SinStartTime = value;
            }
        }

        public string SinEndTime
        {
            get => RaiinInf.SinEndTime ?? string.Empty;
            set
            {
                if (RaiinInf.SinEndTime == value) return;
                RaiinInf.SinEndTime = value;
            }
        }

        public string KaikeiTime
        {
            get => RaiinInf.KaikeiTime ?? string.Empty;
            set
            {
                if (RaiinInf.KaikeiTime == value) return;
                RaiinInf.KaikeiTime = value;
            }
        }

        public int KaikeiId
        {
            get => RaiinInf.KaikeiId;
            set
            {
                if (RaiinInf.KaikeiId == value) return;
                RaiinInf.KaikeiId = value;
            }
        }

        public int KaId
        {
            get => RaiinInf.KaId;
            set
            {
                if (RaiinInf.KaId == value) return;
                RaiinInf.KaId = value;
            }
        }

        public int TantoId
        {
            get => RaiinInf.TantoId;
            set
            {
                if (RaiinInf.TantoId == value) return;
                RaiinInf.TantoId = value;
            }
        }

        public int SyosaisinKbn
        {
            get => RaiinInf.SyosaisinKbn;
            set
            {
                if (RaiinInf.SyosaisinKbn == value) return;
                RaiinInf.SyosaisinKbn = value;
            }
        }

        public int JikanKbn
        {
            get => RaiinInf.JikanKbn;
            set
            {
                if (RaiinInf.JikanKbn == value) return;
                RaiinInf.JikanKbn = value;
            }
        }

        public int CreateId
        {
            get => RaiinInf.CreateId;
            set
            {
                if (RaiinInf.CreateId == value) return;
                RaiinInf.CreateId = value;
            }
        }

        public int UpdateId
        {
            get => RaiinInf.UpdateId;
            set
            {
                if (RaiinInf.UpdateId == value) return;
                RaiinInf.UpdateId = value;
            }
        }

        public DateTime CreateDate
        {
            get => RaiinInf.CreateDate;
            set
            {
                if (RaiinInf.CreateDate == value) return;
                RaiinInf.CreateDate = value;
            }
        }

        public DateTime UpdateDate
        {
            get => RaiinInf.UpdateDate;
            set
            {
                if (RaiinInf.UpdateDate == value) return;
                RaiinInf.UpdateDate = value;
            }
        }

        public string CreateMachine
        {
            get => RaiinInf.CreateMachine ?? string.Empty;
            set
            {
                if (RaiinInf.CreateMachine == value) return;
                RaiinInf.CreateMachine = value;
            }
        }

        public string UpdateMachine
        {
            get => RaiinInf.UpdateMachine ?? string.Empty;
            set
            {
                if (RaiinInf.UpdateMachine == value) return;
                RaiinInf.UpdateMachine = value;
            }
        }
    }
}
