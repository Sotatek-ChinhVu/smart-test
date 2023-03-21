using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Responses.MedicalExamination;

public class GetDataPrintKarte2Response
{
    public GetDataPrintKarte2Response(List<HistoryKarteOdrRaiinItem>? karteOrdRaiins)
    {
        KarteOrdRaiins = karteOrdRaiins;
    }

    public List<HistoryKarteOdrRaiinItem>? KarteOrdRaiins { get; private set; }
}
