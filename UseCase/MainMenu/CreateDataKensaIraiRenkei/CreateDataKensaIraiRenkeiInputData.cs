using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.CreateDataKensaIraiRenkei;

public class CreateDataKensaIraiRenkeiInputData : IInputData<CreateDataKensaIraiRenkeiOutputData>
{
    public CreateDataKensaIraiRenkeiInputData(int hpId, int userId, List<KensaIraiModel> kensaIraiList, string centerCd, int systemDate, bool reCreateDataKensaIraiRenkei)
    {
        HpId = hpId;
        UserId = userId;
        KensaIraiList = kensaIraiList;
        CenterCd = centerCd;
        SystemDate = systemDate;
        ReCreateDataKensaIraiRenkei = reCreateDataKensaIraiRenkei;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<KensaIraiModel> KensaIraiList { get; private set; }

    public string CenterCd { get; private set; }

    public int SystemDate { get; private set; }

    public bool ReCreateDataKensaIraiRenkei { get; private set; }
}
