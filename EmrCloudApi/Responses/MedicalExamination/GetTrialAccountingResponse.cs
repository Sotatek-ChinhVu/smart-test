using Domain.Models.Accounting;
using UseCase.MedicalExamination.TrailAccounting;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetTrialAccountingResponse
    {
        public GetTrialAccountingResponse(string hokenPatternRate, List<SinMeiModel> sinMeis, List<SinHoModel> sinHos, List<SinGaiModel> sinGais, TrialAccountingInfDto accountingInf, List<WarningMemoDto> warningMemos)
        {
            HokenPatternRate = hokenPatternRate;
            SinMeis = sinMeis;
            SinHos = sinHos;
            SinGais = sinGais;
            AccountingInf = accountingInf;
            WarningMemos = warningMemos;
        }

        public string HokenPatternRate { get; private set; }

        public List<SinMeiModel> SinMeis { get; private set; }

        public List<SinHoModel> SinHos { get; private set; }

        public List<SinGaiModel> SinGais { get; private set; }

        public TrialAccountingInfDto AccountingInf { get; private set; }

        public List<WarningMemoDto> WarningMemos { get; private set; }

    }
}
