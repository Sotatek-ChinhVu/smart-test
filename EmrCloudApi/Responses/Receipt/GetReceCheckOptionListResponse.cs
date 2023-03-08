using Domain.Models.Receipt.Recalculation;
using EmrCloudApi.Responses.Receipt.Dto;

namespace EmrCloudApi.Responses.Receipt;

public class GetReceCheckOptionListResponse
{
    public GetReceCheckOptionListResponse(Dictionary<string, ReceCheckOptModel> receCheckOptionDictionnary)
    {
        ReceCheckOptionList = receCheckOptionDictionnary.Select(item => new ReceCheckOptionDto(
                                                                            item.Key,
                                                                            item.Value.ErrCd,
                                                                            item.Value.CheckOpt,
                                                                            item.Value.IsInvalid == 0))
                                                        .ToList();
    }

    public List<ReceCheckOptionDto> ReceCheckOptionList { get; private set; }
}
