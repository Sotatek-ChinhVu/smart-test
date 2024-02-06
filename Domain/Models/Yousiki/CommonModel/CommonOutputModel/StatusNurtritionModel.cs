namespace Domain.Models.Yousiki.CommonModel.CommonOutputModel;

public class StatusNurtritionModel
{
    public StatusNurtritionModel(string title, string pointValue)
    {
        Title = title;
        PointValue = pointValue;
    }

    public string Title { get; private set; }

    public string PointValue { get; private set; }
}
