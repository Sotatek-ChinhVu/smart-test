using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetNextUketukeNoBySetting;

public class GetNextUketukeNoBySettingInputData : IInputData<GetNextUketukeNoBySettingOutputData>
{
    public GetNextUketukeNoBySettingInputData(int hpId, int sindate, int infKbn, int kaId, int uketukeMode, int defaultUkeNo)
    {
        HpId = hpId;
        Sindate = sindate;
        InfKbn = infKbn;
        KaId = kaId;
        UketukeMode = uketukeMode;
        DefaultUkeNo = defaultUkeNo;
    }

    public int HpId { get; private set; }

    public int Sindate { get; private set; }

    public int InfKbn { get; private set; }

    public int KaId { get; private set; }

    public int UketukeMode { get; private set; }

    public int DefaultUkeNo { get; private set; }
}
