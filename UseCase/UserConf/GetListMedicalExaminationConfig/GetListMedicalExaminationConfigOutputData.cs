using UseCase.Core.Sync.Core;
using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace UseCase.UserConf.GetListMedicalExaminationConfig;

public class GetListMedicalExaminationConfigOutputData : IOutputData
{
    public GetListMedicalExaminationConfigOutputData(GetListMedicalExaminationConfigStatus status)
    {
        Status = status;
        LayoutConfiguration = new();
        OrderConfiguration = new();
        KarteConfiguration = new();
        HeaderConfiguration = new();
        SummaryConfiguration = new();
        FunctionConfiguration = new();
        SaveConfirmationConfiguration = new();
        SuperSetConfiguration = new();
        OtherConfiguration = new();
    }

    public GetListMedicalExaminationConfigOutputData(
                                                        GetListMedicalExaminationConfigStatus status,
                                                        LayoutConfigurationOutputItem layoutConfiguration,
                                                        OrderConfigurationOutputItem orderConfiguration,
                                                        KarteConfigurationOutputItem karteConfiguration,
                                                        HeaderConfigurationOutputItem headerConfiguration,
                                                        SummaryConfigurationOutputItem summaryConfiguration,
                                                        FunctionConfigurationOutputItem functionConfiguration,
                                                        SaveConfirmationConfigurationOutputItem saveConfirmationConfiguration,
                                                        SuperSetConfigurationOutputItem superSetConfiguration,
                                                        OtherConfigurationOutputItem otherConfiguration
                                                    )
    {
        Status = status;
        LayoutConfiguration = layoutConfiguration;
        OrderConfiguration = orderConfiguration;
        KarteConfiguration = karteConfiguration;
        HeaderConfiguration = headerConfiguration;
        SummaryConfiguration = summaryConfiguration;
        FunctionConfiguration = functionConfiguration;
        SaveConfirmationConfiguration = saveConfirmationConfiguration;
        SuperSetConfiguration = superSetConfiguration;
        OtherConfiguration = otherConfiguration;
    }

    public GetListMedicalExaminationConfigStatus Status { get; private set; }

    public LayoutConfigurationOutputItem LayoutConfiguration { get; private set; }

    public OrderConfigurationOutputItem OrderConfiguration { get; private set; }

    public KarteConfigurationOutputItem KarteConfiguration { get; private set; }

    public HeaderConfigurationOutputItem HeaderConfiguration { get; private set; }

    public SummaryConfigurationOutputItem SummaryConfiguration { get; private set; }

    public FunctionConfigurationOutputItem FunctionConfiguration { get; private set; }

    public SaveConfirmationConfigurationOutputItem SaveConfirmationConfiguration { get; private set; }

    public SuperSetConfigurationOutputItem SuperSetConfiguration { get; private set; }

    public OtherConfigurationOutputItem OtherConfiguration { get; private set; }
}
