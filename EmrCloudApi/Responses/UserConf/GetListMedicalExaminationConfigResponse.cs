using EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;
using UseCase.UserConf.GetListMedicalExaminationConfig;

namespace EmrCloudApi.Responses.UserConf;

public class GetListMedicalExaminationConfigResponse
{
    public GetListMedicalExaminationConfigResponse(GetListMedicalExaminationConfigOutputData output)
    {
        LayoutConfiguration = new LayoutConfigurationDto(output.LayoutConfiguration);
        OrderConfiguration = new OrderConfigurationDto(output.OrderConfiguration);
        KarteConfiguration = new KarteConfigurationDto(output.KarteConfiguration);
        HeaderConfiguration = new HeaderConfigurationDto(output.HeaderConfiguration);
        SummaryConfiguration = new SummaryConfigurationDto(output.SummaryConfiguration);
        FunctionConfiguration = new FunctionConfigurationDto(output.FunctionConfiguration);
        SaveConfirmationConfiguration = new SaveConfirmationConfigurationDto(output.SaveConfirmationConfiguration);
        SuperSetConfiguration = new SuperSetConfigurationDto(output.SuperSetConfiguration);
        OtherConfiguration = new OtherConfigurationDto(output.OtherConfiguration);
    }

    public LayoutConfigurationDto LayoutConfiguration { get; private set; }

    public OrderConfigurationDto OrderConfiguration { get; private set; }

    public KarteConfigurationDto KarteConfiguration { get; private set; }

    public HeaderConfigurationDto HeaderConfiguration { get; private set; }

    public SummaryConfigurationDto SummaryConfiguration { get; private set; }

    public FunctionConfigurationDto FunctionConfiguration { get; private set; }

    public SaveConfirmationConfigurationDto SaveConfirmationConfiguration { get; private set; }

    public SuperSetConfigurationDto SuperSetConfiguration { get; private set; }

    public OtherConfigurationDto OtherConfiguration { get; private set; }
}
