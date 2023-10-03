namespace UseCase.MedicalExamination.SaveKensaIrai;

public class KensaIraiReportItem
{
    public KensaIraiReportItem(List<string> output, List<string> outputDummy, int systemConfigOdrKensaIraiSameFile, int raiinInfKaId, long kensaInfIraiCd, long ptInfPtNum)
    {
        Output = output;
        OutputDummy = outputDummy;
        SystemConfigOdrKensaIraiSameFile = systemConfigOdrKensaIraiSameFile;
        RaiinInfKaId = raiinInfKaId;
        KensaInfIraiCd = kensaInfIraiCd;
        PtInfPtNum = ptInfPtNum;
    }

    public List<string> Output { get; private set; }

    public List<string> OutputDummy { get; private set; }

    public int SystemConfigOdrKensaIraiSameFile { get; private set; }

    public int RaiinInfKaId { get; private set; }

    public long KensaInfIraiCd { get; private set; }

    public long PtInfPtNum { get; private set; }
}
