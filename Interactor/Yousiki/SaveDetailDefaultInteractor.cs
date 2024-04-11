using Domain.Models.Yousiki;
using Domain.Models.Yousiki.CommonModel.CommonOutputModel;
using System.Text;
using UseCase.Yousiki.SaveDetailDefault;

namespace Interactor.Yousiki;

public class SaveDetailDefaultInteractor : ISaveDetailDefaultInputPort
{
    private readonly IYousikiRepository _yousikiRepository;
    private const string CodeNo_PatientStatus = "RPADL01";

    public SaveDetailDefaultInteractor(IYousikiRepository yousikiRepository)
    {
        _yousikiRepository = yousikiRepository;
    }

    public SaveDetailDefaultOutputData Handle(SaveDetailDefaultInputData inputData)
    {
        List<Yousiki1InfDetailModel> defaultList = new();

        /// <summary>
        /// 1 - BarthelIndexList
        /// 2 - FIMList
        /// 3 - Yousiki1InfDetailList
        /// </summary>
        switch (inputData.Mode)
        {
            case 1:
                var listBarthelIndexLabel = new List<string> { "＊食事", "＊移乗", "＊整容", "＊トイレ動作", "＊入浴", "＊平地歩行", "＊階段", "＊更衣", "＊排便管理", "＊排尿管理" };
                var value = GetDefaultParam(listBarthelIndexLabel, inputData.BarthelIndexList);
                var defaultItem = new Yousiki1InfDetailModel(0, 0, inputData.DataType, 1, CodeNo_PatientStatus, 0, inputData.Mode, value);
                defaultList.Add(defaultItem);
                break;
            case 2:
                var listFIMLabel = new List<string> { "＊食事", "＊整容", "＊清拭", "＊更衣（上半身）", "＊更衣（下半身）", "＊トイレ", "＊排尿コントロール", "＊排便コントロール",
                                                        "＊ベッド・車椅子移乗", "＊トイレ移乗", "＊浴槽・シャワー移乗", "＊歩行・車椅子移動", "＊階段移動", "＊理解", "＊表出", "＊社会的交流", "＊問題解決", "＊記憶"};
                value = GetDefaultParam(listFIMLabel, inputData.FIMList, "1");
                defaultItem = new Yousiki1InfDetailModel(0, 0, inputData.DataType, 1, CodeNo_PatientStatus, 0, inputData.Mode, value);
                defaultList.Add(defaultItem);
                break;
            case 3:
                foreach (var item in inputData.Yousiki1InfDetailList)
                {
                    defaultItem = new Yousiki1InfDetailModel(0, 0, inputData.DataType, 1, item.CodeNo, 0, item.Payload, item.Value);
                    defaultList.Add(defaultItem);
                }
                break;
            default:
                return new SaveDetailDefaultOutputData(SaveDetailDefaultStatus.InvalidMode);
        }

        if (_yousikiRepository.SaveYousiki1InfDetailDefault(inputData.HpId, inputData.DataType, defaultList))
        {
            return new SaveDetailDefaultOutputData(SaveDetailDefaultStatus.Successed);
        }
        return new SaveDetailDefaultOutputData(SaveDetailDefaultStatus.Failed);
    }

    private string GetDefaultParam(List<string> indexLabelList, List<PatientStatusModel> patientStatusList, string defaultParam = "0")
    {
        StringBuilder resultStringBuilder = new();
        foreach (var label in indexLabelList)
        {
            var defaultItem = patientStatusList.FirstOrDefault(item => item.StatusLabel == label);
            if (defaultItem != null)
            {
                resultStringBuilder.Append(defaultItem.StatusValue);
                continue;
            }
            resultStringBuilder.Append(defaultParam);
        }
        return resultStringBuilder.ToString();
    }
}
