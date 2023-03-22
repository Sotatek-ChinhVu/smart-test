using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt.GetReceByomeiChecking;

namespace EmrCloudApi.Responses.Receipt;

public class GetReceByomeiCheckingResponse
{
    public GetReceByomeiCheckingResponse(GetReceByomeiCheckingOutputData output)
    {
        Data = output.Data.Select(item => new ReceByomeiCheckingDto(
                                              item.Key.DisplayItemName,
                                              item.Value.Select(byomei => new ByomeiCheckingDto(
                                                                              byomei.ItemCd,
                                                                              byomei.ByomeiCd,
                                                                              byomei.Byomei,
                                                                              byomei.FullByomei,
                                                                              byomei.SikkanKbn,
                                                                              byomei.SikkanCd,
                                                                              byomei.IsAdopted,
                                                                              byomei.NanbyoCd))
                                                        .ToList()
                                  )).ToList();
    }

    public List<ReceByomeiCheckingDto> Data { get; private set; }
}
