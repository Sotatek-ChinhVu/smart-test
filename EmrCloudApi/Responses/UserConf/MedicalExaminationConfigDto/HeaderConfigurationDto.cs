using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class HeaderConfigurationDto
{
    public HeaderConfigurationDto(HeaderConfigurationOutputItem output)
    {
        ColorCode = output.ColorCode;
        Header1 = output.Header1;
        Header2 = output.Header2;
    }

    public Dictionary<int, string> ColorCode { get; private set; }

    public List<int> Header1 { get; private set; }

    public List<int> Header2 { get; private set; }
}
