using Domain.Models.Yousiki;

namespace Domain.Models.Yousiki.CommonModel.CommonOutputModel;

public class InputByomeiCommonModel
{
    public InputByomeiCommonModel(CommonForm1Model byomeiInf)
    {
        ByomeiInf = byomeiInf;
        DuringMonthMedicineInfModel = null;
        FinalMedicineDateModel = null;
    }

    public InputByomeiCommonModel(CommonForm1Model byomeiInf, Yousiki1InfDetailModel? duringMonthMedicineInfModel, Yousiki1InfDetailModel? finalMedicineDateModel)
    {
        ByomeiInf = byomeiInf;
        DuringMonthMedicineInfModel = duringMonthMedicineInfModel;
        FinalMedicineDateModel = finalMedicineDateModel;
    }

    public CommonForm1Model ByomeiInf { get; private set; }

    public Yousiki1InfDetailModel? DuringMonthMedicineInfModel { get; private set; } = null;

    public Yousiki1InfDetailModel? FinalMedicineDateModel { get; private set; } = null;
}
