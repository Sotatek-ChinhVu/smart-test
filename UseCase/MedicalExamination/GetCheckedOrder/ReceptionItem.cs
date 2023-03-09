using Domain.Models.Reception;
using Entity.Tenant;

namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class ReceptionItem
    {
        public ReceptionItem(ReceptionModel receptionModel)
        {
            HpId = receptionModel.HpId;
            PtId = receptionModel.PtId;
            SinDate = receptionModel.SinDate;
            RaiinNo = receptionModel.RaiinNo;
            HokenPid = receptionModel.HokenPid;
            OyaRaiinNo = receptionModel.OyaRaiinNo;
            Status = receptionModel.Status;
            IsYoyaku = receptionModel.IsYoyaku;
            YoyakuTime = receptionModel.YoyakuTime;
            YoyakuId = receptionModel.YoyakuId;
            UketukeSbt = receptionModel.UketukeSbt;
            UketukeTime = receptionModel.UketukeTime;
            UketukeId = receptionModel.UketukeId;
            UketukeNo = receptionModel.UketukeNo;
            SinStartTime = receptionModel.SinStartTime;
            SinEndTime = receptionModel.SinEndTime;
            KaikeiTime = receptionModel.KaikeiTime;
            KaikeiId = receptionModel.KaikeiId;
            KaId = receptionModel.KaId;
            TantoId = receptionModel.TantoId;
            SyosaisinKbn = receptionModel.SyosaisinKbn;
            JikanKbn = receptionModel.JikanKbn;
            CreateDate = DateTime.MinValue;
            UpdateDate = DateTime.MinValue;
            CreateMachine = string.Empty;
            UpdateMachine = string.Empty;
        }

        public int HpId{ get; private set; }

        public long PtId{ get; private set; }

        public int SinDate{ get; private set; }

        public long RaiinNo{ get; private set; }

        public int HokenPid{ get; private set; }

        public long OyaRaiinNo{ get; private set; }

        public int Status{ get; private set; }

        public int IsYoyaku{ get; private set; }

        public string YoyakuTime{ get; private set; }

        public int YoyakuId{ get; private set; }

        public int UketukeSbt{ get; private set; }

        public string UketukeTime{ get; private set; }

        public int UketukeId{ get; private set; }

        public int UketukeNo{ get; private set; }

        public string SinStartTime{ get; private set; }

        public string SinEndTime{ get; private set; }

        public string KaikeiTime{ get; private set; }

        public int KaikeiId{ get; private set; }

        public int KaId{ get; private set; }

        public int TantoId{ get; private set; }

        public int SyosaisinKbn{ get; private set; }

        public int JikanKbn{ get; private set; }

        public int CreateId{ get; private set; }

        public int UpdateId { get; private set; }

        public DateTime CreateDate{ get; private set; }

        public DateTime UpdateDate{ get; private set; }

        public string CreateMachine{ get; private set; }

        public string UpdateMachine{ get; private set;}
    }
}
