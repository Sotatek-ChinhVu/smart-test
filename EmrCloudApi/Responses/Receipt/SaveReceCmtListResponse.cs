using EmrCloudApi.Responses.Receipt.Dto;
using Helper.Constants;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class SaveReceCmtListResponse
{
    public SaveReceCmtListResponse(List<ReceCmtItem> receCmtList, bool status)
    {
        HeaderItemCmtInvalidList = receCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Header && item.CmtSbt == ReceCmtSbt.ItemCmt)
                                              .Select(item => new ReceCmtDto(item))
                                              .OrderBy(item => item.SeqNo)
                                              .ToList();
        FooterItemCmtInvalidList = receCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Footer && item.CmtSbt == ReceCmtSbt.ItemCmt)
                                              .Select(item => new ReceCmtDto(item))
                                              .OrderBy(item => item.SeqNo)
                                              .ToList();
        HeaderFreeCmtInvalidList = receCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Header && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                              .Select(item => new ReceCmtDto(item))
                                              .OrderBy(item => item.SeqNo)
                                              .ToList();
        FooterFreeCmtInvalidList = receCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Footer && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                              .Select(item => new ReceCmtDto(item))
                                              .OrderBy(item => item.SeqNo)
                                              .ToList();
        Status = status;
    }

    public List<ReceCmtDto> HeaderItemCmtInvalidList { get; private set; }

    public List<ReceCmtDto> FooterItemCmtInvalidList { get; private set; }

    public List<ReceCmtDto> HeaderFreeCmtInvalidList { get; private set; }

    public List<ReceCmtDto> FooterFreeCmtInvalidList { get; private set; }

    public bool Status { get; private set; }
}
