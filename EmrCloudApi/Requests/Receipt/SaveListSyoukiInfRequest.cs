using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveListSyoukiInfRequest
{
    public SaveListSyoukiInfRequest(long ptId, int sinYm, int hokenId, List<SaveListSyoukiInfRequestItem> listSyoukiInf)
    {
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        ListSyoukiInf = listSyoukiInf;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public List<SaveListSyoukiInfRequestItem> ListSyoukiInf { get; private set; }
}
