using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetInsuranceReceInfList;

public class GetInsuranceReceInfListInputData : IInputData<GetInsuranceReceInfListOutputData>
{
    public GetInsuranceReceInfListInputData(int hpId, int seikyuYm, int sinYm, long ptId, int hokenId)
    {
        HpId = hpId;
        SeikyuYm = seikyuYm;
        SinYm = sinYm;
        PtId = ptId;
        HokenId = hokenId;
    }

    public int HpId { get; private set; }

    public int SeikyuYm { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }
}
