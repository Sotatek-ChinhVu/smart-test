using UseCase.Santei.GetListSanteiInf;

namespace EmrCloudApi.Responses.Santei;

public class GetListSanteiInfResponse
{
    public GetListSanteiInfResponse(List<SanteiInfOutputItem> listSanteis, Dictionary<int, string> alertTermCombobox, Dictionary<int, string> kisanKbnCombobox, List<string> listByomeis)
    {
        ListSanteis = listSanteis.Select(item => new SanteiInfDto(item)).ToList();
        AlertTermCombobox = alertTermCombobox;
        KisanKbnCombobox = kisanKbnCombobox;
        ListByomeis = listByomeis;
    }

    public List<SanteiInfDto> ListSanteis { get; private set; }

    public Dictionary<int, string> AlertTermCombobox { get; private set; }

    public Dictionary<int, string> KisanKbnCombobox { get; private set; }

    public List<string> ListByomeis { get; private set; }
}
