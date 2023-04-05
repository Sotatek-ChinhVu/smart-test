using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.TrailAccounting
{
    public class GetTrialAccountingOutputData : IOutputData
    {
        public GetTrialAccountingOutputData(string hokenPatternRate, List<SinMeiModel> sinMeis, List<SinHoModel> sinHos, List<SinGaiModel> sinGais, TrialAccountingInfDto accountingInf, List<WarningMemoDto> warningMemos, GetTrialAccountingStatus status)
        {
            HokenPatternRate = hokenPatternRate;
            SinMeis = sinMeis;
            SinHos = sinHos;
            SinGais = sinGais;
            AccountingInf = accountingInf;
            WarningMemos = warningMemos;
            Status = status;
        }

        public string HokenPatternRate { get; private set; }

        public List<SinMeiModel> SinMeis { get; private set; }

        public List<SinHoModel> SinHos { get; private set; }

        public List<SinGaiModel> SinGais { get; private set; }

        public TrialAccountingInfDto AccountingInf { get; private set; }

        public List<WarningMemoDto> WarningMemos { get; private set; }

        public GetTrialAccountingStatus Status { get; private set; }
    }
}
