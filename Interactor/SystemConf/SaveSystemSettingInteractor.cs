using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.Santei;
using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SystemConf.SaveSystemSetting;
using static Helper.Constants.StatusConstant;

namespace Interactor.SystemConf
{
    public class SaveSystemSettingInteractor : ISaveSystemSettingInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly ISanteiInfRepository _santeiInfRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveSystemSettingInteractor(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository, IHpInfRepository hpInfRepository, ISanteiInfRepository santeiInfRepository, IMstItemRepository mstItemRepository)
        {
            _systemConfRepository = systemConfRepository;
            _hpInfRepository = hpInfRepository;
            _santeiInfRepository = santeiInfRepository;
            _mstItemRepository = mstItemRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveSystemSettingOutputData Handle(SaveSystemSettingInputData inputData)
        {
            try
            {
                if (inputData.HpInfs.Any())
                {
                    var hpInfs = ConvertItemToHpInfModel(inputData.HpInfs);
                    _hpInfRepository.SaveHpInf(inputData.UserId, hpInfs);
                }

                if (inputData.SystemConfMenus.Any())
                {
                    var systemConfMenu = ConvertItemToSystemConfItemModel(inputData.SystemConfMenus);
                    _systemConfRepository.SaveSystemGenerationConf(inputData.UserId, systemConfMenu);
                    _systemConfRepository.SaveSystemSetting(inputData.HpId, inputData.UserId, systemConfMenu);
                }

                if (inputData.SanteiInfs.Any())
                {
                    var santeiInfs = ConvertItemToSanteiInfModel(inputData.SanteiInfs);
                    _santeiInfRepository.SaveAutoSanteiMst(inputData.HpId, inputData.UserId, santeiInfs);
                }

                if (inputData.KensaCenters.Any())
                {
                    var kensaCenters = ConvertItemToKensaCenterMstModel(inputData.KensaCenters);
                    _mstItemRepository.SaveKensaCenterMst(inputData.UserId, kensaCenters);
                }

                return new SaveSystemSettingOutputData(SaveSystemSettingStatus.Successed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _hpInfRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _santeiInfRepository.ReleaseResource();
                _mstItemRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }

        private List<HpInfModel> ConvertItemToHpInfModel(List<HpInfItem> hpInfs)
        {
            var result = new List<HpInfModel>();

            foreach (var item in hpInfs)
            {
                var validationStatus = item.Validation();
                if (validationStatus != ValidationHpInfStatus.None)
                {
                    return new();
                }

                result.Add(new HpInfModel(
                    item.HpId,
                    item.StartDate,
                    item.HpCd,
                    item.RousaiHpCd,
                    item.HpName,
                    item.ReceHpName,
                    item.KaisetuName,
                    item.PostCd,
                    item.PrefNo,
                    item.Address1,
                    item.Address2,
                    item.Tel,
                    item.FaxNo,
                    item.OtherContacts,
                    item.UpdateId,
                    item.HpInfModelStatus));
            }
            return result;
        }

        private List<SystemConfMenuModel> ConvertItemToSystemConfItemModel(List<SystemConfMenuItem> systemConfs)
        {
            var result = new List<SystemConfMenuModel>();

            foreach (var item in systemConfs)
            {
                result.Add(new SystemConfMenuModel(
                        !item.SystemGenerationConfs.Any() ? new() :
                        item.SystemGenerationConfs.Select(x => new SystemGenerationConfModel(
                            x.Id,
                            x.HpId,
                            x.GrpCd,
                            x.GrpEdaNo,
                            x.StartDate,
                            x.EndDate,
                            x.Val,
                            x.Param,
                            x.Biko,
                            x.SystemGenerationConfStatus)).ToList(),

                        item.SystemConf == null ? new() :
                            new SystemConfModel(
                                item.SystemConf.HpId,
                                item.SystemConf.GrpCd,
                                item.SystemConf.GrpEdaNo,
                                item.SystemConf.Val,
                                item.SystemConf.Param,
                                item.SystemConf.Biko,
                                item.SystemConf.IsUpdatePtRyosyo,
                                item.SystemConf.SystemSettingModelStatus
                            )
                        ));
            }

            return result;
        }

        private List<SanteiInfDetailModel> ConvertItemToSanteiInfModel(List<SanteiInfDetailItem> santeiInfs)
        {
            var result = new List<SanteiInfDetailModel>();

            foreach (var item in santeiInfs)
            {
                result.Add(new SanteiInfDetailModel(
                    item.Id,
                    item.PtId,
                    item.ItemCd,
                    item.StartDate,
                    item.EndDate,
                    item.KisanSbt,
                    item.KisanDate,
                    item.Byomei,
                    item.HosokuComment,
                    item.Comment,
                    item.IsDeleted,
                    item.AutoSanteiMstModelStatus));
            }

            return result;
        }

        private List<KensaCenterMstModel> ConvertItemToKensaCenterMstModel(List<KensaCenterMstItem> kensaCenterMsts)
        {
            var result = new List<KensaCenterMstModel>();
            foreach (var item in kensaCenterMsts)
            {
                result.Add(new KensaCenterMstModel(
                                                    item.Id,
                                                    item.HpId,
                                                    item.CenterCd,
                                                    item.CenterName,
                                                    item.PrimaryKbn,
                                                    item.SortNo,
                                                    item.KensaCenterMstModelStatus));
            }

            return result;
        }
    }
}
