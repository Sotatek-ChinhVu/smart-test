using Domain.Models.Receipt;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class PtHokenInfKaikeiDto
{
    public PtHokenInfKaikeiDto(PtHokenInfKaikeiItem item)
    {
        HokenId = item.HokenId;
        PtId = item.PtId;
        HokenKbn = item.HokenKbn;
        Houbetu = item.Houbetu;
        HonkeKbn = item.HonkeKbn;
        HokensyaNo = item.HokensyaNo;
        HokenStartDate = item.HokenStartDate;
        HokenEndDate = item.HokenEndDate;
        HokenName = item.HokenName;
    }

    public int HokenId { get; private set; }

    public long PtId { get; private set; }

    public int HokenKbn { get; private set; }

    public string Houbetu { get; private set; }

    public int HonkeKbn { get; private set; }

    public string HokensyaNo { get; private set; }

    public int HokenStartDate { get; private set; }

    public int HokenEndDate { get; private set; }

    public string HokenName { get; private set; }
}
