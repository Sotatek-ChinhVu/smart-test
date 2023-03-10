using UseCase.Receipt.ReceCmtHistory;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class ReceCmtHistoryDto
{
    public ReceCmtHistoryDto(ReceCmtHistoryOutputItem output)
    {
        SinYm = output.SinYm;
        SinYmDisplay = output.SinYmDisplay;
        HokenName = output.HokenName;
        ReceCmtList = output.ReceCmtList.Select(item => new ReceCmtDto(item)).ToList();
    }

    public int SinYm { get; private set; }

    public string SinYmDisplay { get; private set; }

    public string HokenName { get; private set; }

    public List<ReceCmtDto> ReceCmtList { get; private set; }
}
