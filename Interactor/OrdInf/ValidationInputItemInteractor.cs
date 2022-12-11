using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SystemGenerationConf;
using UseCase.OrdInfs.ValidationInputItem;
using static Helper.Constants.OrderInfConst;

namespace Interactor.OrdInfs
{
    public class ValidationInputitemInteractor : IValidationInputItemInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
        public ValidationInputitemInteractor(IOrdInfRepository ordInfRepository, IMstItemRepository mstItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository)
        {
            _ordInfRepository = ordInfRepository;
            _mstItemRepository = mstItemRepository;
            _systemGenerationConfRepository = systemGenerationConfRepository;
        }

        public ValidationInputItemOutputData Handle(ValidationInputItemInputData inputDatas)
        {
            try
            {
                var dicValidation = new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>();
                var inputDataList = inputDatas.ToList();

                if (inputDataList.Count == 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }
                if (inputDatas.HpId <= 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }
                if (inputDatas.SinDate <= 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }
                if (inputDataList.Count == 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }

                var allOdrInfs = ConvertInputDataToOrderInfs(inputDatas.HpId, inputDatas.SinDate, inputDataList);

                var count = 0;
                foreach (var item in allOdrInfs)
                {
                    var modelValidation = item.Validation(1);

                    if (modelValidation.Value != OrdInfValidationStatus.Valid && !dicValidation.ContainsKey(count.ToString()))
                    {
                        dicValidation.Add(count.ToString(), modelValidation);
                    }

                    count++;
                }

                return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Successed);
            }
            catch
            {
                return new ValidationInputItemOutputData(new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>(), ValidationInputItemStatus.Failed);
            }
        }

        private List<OrdInfModel> ConvertInputDataToOrderInfs(int hpId, int sinDate, List<ValidationInputItemItem> inputDataList)
        {
            var allOdrInfs = new List<OrdInfModel>();
            var itemCds = new List<string>();
            foreach (var item in inputDataList.Select(o => o.OdrDetails))
            {
                itemCds.AddRange(item?.Select(od => od.ItemCd).Distinct() ?? new List<string>());
            }
            itemCds = itemCds?.Distinct().ToList() ?? new List<string>();

            var tenMsts = _mstItemRepository.GetCheckTenItemModels(hpId, sinDate, itemCds);
            var ipnMinYakaMsts = _ordInfRepository.GetCheckIpnMinYakkaMsts(hpId, sinDate, tenMsts?.Select(t => t.IpnNameCd).ToList() ?? new List<string>());
            var refillSetting = _systemGenerationConfRepository.GetSettingValue(hpId, 2002, 0, sinDate, 999).Item1;
            var checkIsGetYakkaPrices = _ordInfRepository.CheckIsGetYakkaPrices(hpId, tenMsts ?? new List<TenItemModel>(), sinDate);

            foreach (var item in inputDataList)
            {
                var ordInf = new OrdInfModel(
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        item.OdrKouiKbn,
                        String.Empty,
                        item.InoutKbn,
                        0,
                        0,
                        0,
                        0,
                        item.DaysCnt,
                        0,
                        0,
                        0,
                        new List<OrdInfDetailModel>(),
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        ""
                    );
                if (item != null)
                {
                    foreach (var itemDetail in item.OdrDetails)
                    {

                        var inputItem = itemDetail == null ? null : tenMsts?.FirstOrDefault(t => t.ItemCd == itemDetail.ItemCd);
                        refillSetting = itemDetail == null ? 999 : refillSetting;
                        var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : ipnMinYakaMsts.FirstOrDefault(i => i.IpnNameCd == inputItem?.IpnNameCd);
                        var isCheckIpnKasanExclude = checkIsGetYakkaPrices.FirstOrDefault(y => y.Item1 == inputItem?.IpnNameCd && y.Item2 == inputItem?.ItemCd)?.Item3 == true;

                        if (itemDetail == null)
                        {
                            continue;
                        }

                        var ordInfDetail = new OrdInfDetailModel(
                                     0,
                                     0,
                                     0,
                                     0,
                                     itemDetail.RowNo,
                                     0,
                                     0,
                                    itemDetail.SinKouiKbn,
                                    itemDetail.ItemCd,
                                    itemDetail.ItemName,
                                    itemDetail.Suryo,
                                    itemDetail.UnitName,
                                    0,
                                    0,
                                     0,
                                    itemDetail.SyohoKbn,
                                    0,
                                    itemDetail.DrugKbn,
                                    itemDetail.YohoKbn,
                                    string.Empty,
                                    string.Empty,
                                    0,
                                    string.Empty,
                                    string.Empty,
                                    0,
                                    DateTime.MinValue,
                                    0,
                                    string.Empty,
                                    string.Empty,
                                    itemDetail.Bunkatu,
                                    itemDetail.CmtName,
                                    itemDetail.CmtOpt,
                                    string.Empty,
                                    0,
                                    inputItem?.MasterSbt ?? string.Empty,
                                    item?.InoutKbn ?? 0,
                                    ipnMinYakaMst?.Yakka ?? 0,
                                    isCheckIpnKasanExclude,
                                    refillSetting,
                                    inputItem?.CmtCol1 ?? 0,
                                    inputItem?.Ten ?? 0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    "",
                                    new List<YohoSetMstModel>(),
                                    0,
                                    0
                                );
                        ordInf.OrdInfDetails.Add(ordInfDetail);
                    }
                }

                allOdrInfs.Add(ordInf);
            }
            return allOdrInfs;
        }
    }
}
