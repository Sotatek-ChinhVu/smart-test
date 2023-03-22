using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.TrailAccounting
{
    public class GetTrialAccountingMeiHoGaiOutputData : IOutputData
    {
        public GetTrialAccountingMeiHoGaiOutputData(List<SinMeiModel> sinMeis, List<SinHoModel> sinHos, List<SinGaiModel> sinGais, TrialAccountingInfDto accountingInf, string hokenPatternRate, GetTrialAccountingMeiHoGaiStatus status)
        {
            SinMeis = sinMeis;
            SinHos = sinHos;
            SinGais = sinGais;
            Status = status;
            AccountingInf = accountingInf;
            HokenPatternRate = hokenPatternRate;
        }

        public List<SinMeiModel> SinMeis { get; private set; }

        public List<SinHoModel> SinHos { get; private set; }

        public List<SinGaiModel> SinGais { get; private set; }

        public TrialAccountingInfDto AccountingInf { get; private set; }

        public string HokenPatternRate { get; private set; }

        public GetTrialAccountingMeiHoGaiStatus Status { get; private set; }
    }
}
