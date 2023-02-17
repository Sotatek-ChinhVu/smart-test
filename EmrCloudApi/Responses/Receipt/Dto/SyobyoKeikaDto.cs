using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class SyobyoKeikaDto
{
    public SyobyoKeikaDto(SyobyoKeikaItem output)
    {
        SinDay = output.SinDay;
        SeqNo = output.SeqNo;
        Keika = output.Keika;
    }

    public int SinDay { get; private set; }

    public int SeqNo { get; private set; }

    public string Keika { get; private set; }
}
