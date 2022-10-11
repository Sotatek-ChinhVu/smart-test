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
                var allOdrInfs = new List<OrdInfModel>();
                var inputDataList = inputDatas.ToList();

                if (inputDataList.Count == 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }
                if (inputDatas.HpId < 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }
                if (inputDatas.SinDate < 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }
                if (inputDataList.Count == 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }

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
                            DateTime.MinValue
                        );
                    if (item != null)
                    {
                        foreach (var itemDetail in item.OdrDetails)
                        {

                            var inputItem = itemDetail == null ? null : _mstItemRepository.GetTenMst(inputDatas?.HpId ?? 0, inputDatas?.SinDate ?? 0, itemDetail?.ItemCd ?? string.Empty);
                            var refillSetting = itemDetail == null ? 999 : _systemGenerationConfRepository.GetSettingValue(inputDatas?.HpId ?? 0, 2002, 0, inputDatas?.SinDate ?? 0, 999);
                            var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : _ordInfRepository.FindIpnMinYakkaMst(inputDatas?.HpId ?? 0, inputItem?.IpnNameCd ?? string.Empty, inputDatas?.SinDate ?? 0);
                            var isCheckIpnKasanExclude = _ordInfRepository.CheckIsGetYakkaPrice(inputDatas?.HpId ?? 0, inputItem ?? new TenItemModel(), inputDatas?.SinDate ?? 0);

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
    }
}
