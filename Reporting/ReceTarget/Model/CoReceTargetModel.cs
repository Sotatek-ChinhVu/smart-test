namespace Reporting.ReceTarget.Model;

public class CoReceTargetModel
{
    public List<CoReceInfModel> ReceInfModels { get; }

    public CoReceTargetModel(List<CoReceInfModel> receInfModels)
    {
        ReceInfModels = receInfModels;
    }

    public CoReceTargetModel()
    {
        ReceInfModels = new();
    }
}
