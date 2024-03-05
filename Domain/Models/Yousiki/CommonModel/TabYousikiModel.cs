namespace Domain.Models.Yousiki.CommonModel;

public class TabYousikiModel
{
    public TabYousikiModel(CommonModel commonModel, AtHomeModel atHomeModel, LivingHabitModel livingHabitModel, RehabilitationModel rehabilitationModel)
    {
        CommonModel = commonModel;
        AtHomeModel = atHomeModel;
        LivingHabitModel = livingHabitModel;
        RehabilitationModel = rehabilitationModel;
    }

    public CommonModel CommonModel { get; private set; }

    public AtHomeModel AtHomeModel { get; private set; }

    public LivingHabitModel LivingHabitModel { get; private set; }

    public RehabilitationModel RehabilitationModel { get; private set; }
}
