using Domain.Models.PtCmtInf;
using Domain.Models.SpecialNote.PatientInfo;

namespace EmrCloudApi.Tenant.Requests.SpecialNote
{
    public class PatientInfoRequest
    {
        public PtPregnancyRequest PregnancyItems { get; set; } = new PtPregnancyRequest();

        public PtCmtInfRequest PtCmtInfItems { get; set; } = new PtCmtInfRequest();

        public SeikaturekiInfRequest SeikatureInfItems { get; set; } = new SeikaturekiInfRequest();

        public List<PhysicalInfoRequest> PhysicalInfItems { get; set; } = new List<PhysicalInfoRequest>();
        public PatientInfoModel Map()
        {
            return new PatientInfoModel(PregnancyItems.Map(), PtCmtInfItems.Map(), SeikatureInfItems.Map(), PhysicalInfItems.Select(x => x.Map()).ToList());
        }
    }
    public class PtPregnancyRequest
    {
        public long Id { get; set; }

        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeqNo { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public int PeriodDate { get; set; }

        public int PeriodDueDate { get; set; }

        public int OvulationDate { get; set; }

        public int OvulationDueDate { get; set; }

        public int IsDeleted { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdateId { get; set; }

        public string UpdateMachine { get; set; } = String.Empty;
        public PtPregnancyModel Map()
        {
            return new PtPregnancyModel(Id,
            HpId,
            PtId,
            SeqNo,
            StartDate,
            EndDate,
            PeriodDate,
            PeriodDueDate,
            OvulationDate,
            OvulationDueDate,
            IsDeleted,
            UpdateDate,
            UpdateId,
            UpdateMachine);
        }

    }
    public class PtCmtInfRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SeqNo { get; set; }

        public string Text { get; set; } = String.Empty;

        public int IsDeleted { get; set; }

        public long Id { get; set; }
        public PtCmtInfModel Map()
        {
            return new PtCmtInfModel(HpId, PtId, SeqNo, Text, IsDeleted, Id);
        }
    }
    public class SeikaturekiInfRequest
    {
        public long Id { get; set; }

        public int HpId { get; set; }

        public long PtId { get; set; }

        public long SeqNo { get; set; }

        public string Text { get; set; } = String.Empty;
        public SeikaturekiInfModel Map()
        {
            return new SeikaturekiInfModel(Id, HpId, PtId, SeqNo, Text);
        }
    }
    public class PhysicalInfoRequest
    {
        public int HpId { get; set; }

        public string KensaItemCd { get; set; } = String.Empty;

        public int KensaItemSeqNo { get; set; }

        public string CenterCd { get; set; } = String.Empty;

        public string KensaName { get; set; } = String.Empty;

        public string KensaKana { get; set; } = String.Empty;

        public string Unit { get; set; } = String.Empty;

        public int MaterialCd { get; set; }

        public int ContainerCd { get; set; }

        public string MaleStd { get; set; } = String.Empty;

        public string MaleStdLow { get; set; } = String.Empty;

        public string MaleStdHigh { get; set; } = String.Empty;

        public string FemaleStd { get; set; } = String.Empty;

        public string FemaleStdLow { get; set; } = String.Empty;

        public string FemaleStdHigh { get; set; } = String.Empty;

        public string Formula { get; set; } = String.Empty;

        public string OyaItemCd { get; set; } = String.Empty;

        public int OyaItemSeqNo { get; set; }

        public long SortNo { get; set; }

        public string CenterItemCd1 { get; set; } = String.Empty;

        public string CenterItemCd2 { get; set; } = String.Empty;

        public int IsDelete { get; set; }

        public int Digit { get; set; }

        public List<KensaInfDetailRequest> KensaInfDetailModels { get; set; } = new List<KensaInfDetailRequest>();
        public PhysicalInfoModel Map()
        {
            return new PhysicalInfoModel(HpId, KensaItemCd, KensaItemSeqNo, CenterCd, KensaName, KensaKana, Unit, MaterialCd, ContainerCd, MaleStd, MaleStdLow, MaleStdHigh, FemaleStd, FemaleStdLow, FemaleStdHigh, Formula, OyaItemCd, OyaItemSeqNo, SortNo, CenterItemCd1, CenterItemCd2, IsDelete, Digit, KensaInfDetailModels.Select(x => x.Map()).ToList());
        }
    }
    public class KensaInfDetailRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public long IraiCd { get; set; }

        public long SeqNo { get; set; }

        public int IraiDate { get; set; }

        public long RaiinNo { get; set; }

        public string KensaItemCd { get; set; } = String.Empty;

        public string ResultVal { get; set; } = String.Empty;

        public string ResultType { get; set; } = String.Empty;

        public string AbnormalKbn { get; set; } = String.Empty;

        public int IsDeleted { get; set; }

        public string CmtCd1 { get; set; } = String.Empty;

        public string CmtCd2 { get; set; } = String.Empty;

        public DateTime UpdateDate { get; set; }
        public KensaInfDetailModel Map()
        {
            return new KensaInfDetailModel(HpId, PtId, IraiCd, SeqNo, IraiDate, RaiinNo, KensaItemCd, ResultVal, ResultType, AbnormalKbn, IsDeleted, CmtCd1, CmtCd2, UpdateDate);
        }
    }
}
