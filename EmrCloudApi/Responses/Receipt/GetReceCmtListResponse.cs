using EmrCloudApi.Responses.Receipt.Dto;
using Helper.Constants;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetReceCmtListResponse
{
    public GetReceCmtListResponse(List<ReceCmtItem> receCmtList)
    {
        HeaderItemCmtList = receCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Header && item.CmtSbt == ReceCmtSbt.ItemCmt)
                                       .Select(item => new ReceCmtDto(item))
                                       .OrderBy(item => item.SeqNo)
                                       .ToList();
        FooterItemCmtList = receCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Footer && item.CmtSbt == ReceCmtSbt.ItemCmt)
                                       .Select(item => new ReceCmtDto(item))
                                       .OrderBy(item => item.SeqNo)
                                       .ToList();
        HeaderFreeCmtList = receCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Header && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                       .Select(item => new ReceCmtDto(item))
                                       .OrderBy(item => item.SeqNo)
                                       .ToList();
        FooterFreeCmtList = receCmtList.Where(item => item.CmtKbn == ReceCmtKbn.Footer && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                       .Select(item => new ReceCmtDto(item))
                                       .OrderBy(item => item.SeqNo)
                                       .ToList();
    }

    public List<ReceCmtDto> HeaderItemCmtList { get; private set; }

    public List<ReceCmtDto> FooterItemCmtList { get; private set; }

    public List<ReceCmtDto> HeaderFreeCmtList { get; private set; }

    public List<ReceCmtDto> FooterFreeCmtList { get; private set; }
}
