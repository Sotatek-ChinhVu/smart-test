using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class SyoukiInfDto
{
    public SyoukiInfDto(SyoukiInfItem outputItem)
    {
        SeqNo = outputItem.SeqNo;
        SortNo = outputItem.SortNo;
        SyoukiKbn = outputItem.SyoukiKbn;
        Syouki = outputItem.Syouki;
    }

    public int SeqNo { get; private set; }

    public int SortNo { get; private set; }

    public int SyoukiKbn { get; private set; }

    public string Syouki { get; private set; }
}
