using Domain.Models.Yousiki;

namespace EmrCloudApi.Responses.Yousiki.Dto;

public class Yousiki1InfGetListDto : Yousiki1InfDto
{
    public Yousiki1InfGetListDto(Yousiki1InfModel model, Dictionary<int, int> tabKeySeqNoDictionary) : base(model)
    {
        TabKeySeqNoDictionary = tabKeySeqNoDictionary;
    }

    public Dictionary<int, int> TabKeySeqNoDictionary { get; private set; }
}
