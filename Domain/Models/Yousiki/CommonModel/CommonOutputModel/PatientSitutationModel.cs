namespace Domain.Models.Yousiki.CommonModel.CommonOutputModel;

public class PatientSitutationModel
{
    public PatientSitutationModel(string title, string situationValue)
    {
        Title = title;
        SituationValue = situationValue;
    }

    public string Title { get; private set; }

    public string SituationValue { get; private set; }
}
