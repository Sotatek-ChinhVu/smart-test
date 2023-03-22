using Helper.Constants;
using UseCase.Receipt.ReceCmtHistory;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class ReceCmtHistoryDto
{
    public ReceCmtHistoryDto(ReceCmtHistoryOutputItem output)
    {
        SinYm = output.SinYm;
        SinYmDisplay = output.SinYmDisplay;
        HokenId = output.HokenId;
        HokenName = output.HokenName;
        HeaderItemCmtList = output.ReceCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Header && item.CmtSbt == ReceCmtSbt.ItemCmt)
                                              .Select(item => new ReceCmtDto(item))
                                              .OrderBy(item => item.SeqNo)
                                              .ToList();
        FooterItemCmtList = output.ReceCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Footer && item.CmtSbt == ReceCmtSbt.ItemCmt)
                                              .Select(item => new ReceCmtDto(item))
                                              .OrderBy(item => item.SeqNo)
                                              .ToList();
        HeaderFreeCmtList = output.ReceCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Header && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                              .Select(item => new ReceCmtDto(item))
                                              .OrderBy(item => item.SeqNo)
                                              .ToList();
        FooterFreeCmtList = output.ReceCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Footer && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                              .Select(item => new ReceCmtDto(item))
                                              .OrderBy(item => item.SeqNo)
                                              .ToList();
    }

    public int SinYm { get; private set; }

    public string SinYmDisplay { get; private set; }

    public int HokenId { get; private set; }

    public string HokenName { get; private set; }

    public List<ReceCmtDto> HeaderItemCmtList { get; private set; }

    public List<ReceCmtDto> FooterItemCmtList { get; private set; }

    public List<ReceCmtDto> HeaderFreeCmtList { get; private set; }

    public List<ReceCmtDto> FooterFreeCmtList { get; private set; }
}
