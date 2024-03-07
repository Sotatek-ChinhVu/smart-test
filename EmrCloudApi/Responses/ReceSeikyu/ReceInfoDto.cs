using Domain.Models.ReceSeikyu;

namespace EmrCloudApi.Responses.ReceSeikyu;

public class ReceInfoDto
{
    public ReceInfoDto(ReceInfo model)
    {
        PtId = model.PtId;
        HokenId = model.HokenId;
        SinYm = model.SinYm;
        SeikyuYm = model.SeikyuYm;
    }

    public long PtId { get; set; }

    public int HokenId { get; set; }

    public int SinYm { get; set; }

    public int SeikyuYm { get; set; }
}
