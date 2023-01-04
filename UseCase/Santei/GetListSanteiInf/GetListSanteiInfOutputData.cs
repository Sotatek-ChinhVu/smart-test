using UseCase.Core.Sync.Core;

namespace UseCase.Santei.GetListSanteiInf;

public class GetListSanteiInfOutputData : IOutputData
{
    public GetListSanteiInfOutputData(GetListSanteiInfStatus status)
    {
        Status = status;
        ListSanteiInfs = new();
        AlertTermCombobox = new();
        KisanKbnCombobox = new();
        ListByomeis = new();
    }

    public GetListSanteiInfOutputData(GetListSanteiInfStatus status, List<SanteiInfOutputItem> listSanteiInfs, Dictionary<int, string> alertTermCombobox, Dictionary<int, string> kisanKbnCombobox, List<string> listByomeis)
    {
        Status = status;
        ListSanteiInfs = listSanteiInfs;
        AlertTermCombobox = alertTermCombobox;
        KisanKbnCombobox = kisanKbnCombobox;
        ListByomeis = listByomeis;
    }

    public GetListSanteiInfStatus Status { get; private set; }

    public List<SanteiInfOutputItem> ListSanteiInfs { get; private set; }

    public Dictionary<int, string> AlertTermCombobox { get; private set; }

    public Dictionary<int, string> KisanKbnCombobox { get; private set; }

    public List<string> ListByomeis { get; private set; }
}
