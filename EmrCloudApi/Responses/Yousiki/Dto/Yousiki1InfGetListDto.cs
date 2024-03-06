using Domain.Models.Yousiki;

namespace EmrCloudApi.Responses.Yousiki.Dto;

public class Yousiki1InfGetListDto : Yousiki1InfDto
{
    public Yousiki1InfGetListDto(Yousiki1InfModel model) : base(model)
    {
        DataTypeSeqNoDic = model.DataTypeSeqNoDic;
    }

    public Dictionary<int, int> DataTypeSeqNoDic { get; private set; }
}
