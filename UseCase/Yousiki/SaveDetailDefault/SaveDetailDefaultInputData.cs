using Domain.Models.Yousiki;
using Domain.Models.Yousiki.CommonModel.CommonOutputModel;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.SaveDetailDefault;

public class SaveDetailDefaultInputData : IInputData<SaveDetailDefaultOutputData>
{
    public SaveDetailDefaultInputData(int hpId, int mode, int dataType, List<PatientStatusModel> barthelIndexList, List<PatientStatusModel> fIMList, List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        HpId = hpId;
        Mode = mode;
        DataType = dataType;
        BarthelIndexList = barthelIndexList;
        FIMList = fIMList;
        Yousiki1InfDetailList = yousiki1InfDetailList;
    }

    public int HpId { get; private set; }

    /// <summary>
    /// 1 - BarthelIndexList
    /// 2 - FIMList
    /// 3 - Yousiki1InfDetailList
    /// </summary>
    public int Mode { get; private set; }

    public int DataType { get; private set; }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public List<PatientStatusModel> BarthelIndexList { get; private set; }

    public List<PatientStatusModel> FIMList { get; private set; }
}
